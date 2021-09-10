﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ProcessThreadInfo
{
    sealed class DbgHelpNative
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct SYMBOL_INFO
        {
            public uint SizeOfStruct;
            public uint TypeIndex;          // Type Index of symbol
            ulong Reserved0;
            ulong Reserved1;
            public uint Index;
            public uint Size;
            public ulong ModBase;            // Base Address of module containing this symbol
            public uint Flags;
            public ulong Value;              // Value of symbol, ValuePresent should be 1
            public ulong Address;            // Address of symbol including base address of module
            public uint Register;           // Register holding value or pointer to value
            public uint Scope;              // Scope of the symbol
            public uint Tag;                // PDB classification
            public uint NameLen;            // Length of name stored in buffer
            public uint MaxNameLen;         // Buffer size
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string Name;               // Buffer
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct IMAGEHLP_LINE64
        {
            public uint SizeOfStruct;           // set to sizeof(IMAGEHLP_LINE64)
            public IntPtr Key;                    // internal
            public uint LineNumber;             // line number in file
            public IntPtr FileName;               // full filename
            public ulong Address;                // first instruction of line
        };

        [Flags]
        internal enum Options : uint
        {
            SYMOPT_ALLOW_ABSOLUTE_SYMBOLS = 0x00000800, // Enables the use of symbols that are stored with absolute addresses. Most symbols are stored as RVAs from the base of the module. DbgHelp translates them to absolute addresses. There are symbols that are stored as an absolute address. These have very specialized purposes and are typically not used.
            SYMOPT_ALLOW_ZERO_ADDRESS = 0x01000000, // Enables the use of symbols that do not have an address. By default, DbgHelp filters out symbols that do not have an address.
            SYMOPT_AUTO_PUBLICS = 0x00010000, // Do not search the public symbols when searching for symbols by address, or when enumerating symbols, unless they were not found in the global symbols or within the current scope. This option has no effect with SYMOPT_PUBLICS_ONLY.
            SYMOPT_CASE_INSENSITIVE = 0x00000001, // All symbol searches are insensitive to case.
            SYMOPT_DEBUG = 0x80000000, // Pass debug output through OutputDebugString or the SymRegisterCallbackProc64 callback function.
            SYMOPT_DEFERRED_LOADS = 0x00000004, // Symbols are not loaded until a reference is made requiring the symbols be loaded. This is the fastest, most efficient way to use the symbol handler.
            SYMOPT_DISABLE_SYMSRV_AUTODETECT = 0x02000000, // Disables the auto-detection of symbol server stores in the symbol path, even without the "SRV*" designation, maintaining compatibility with previous behavior. DbgHelp 6.6 and earlier:  This value is not supported.
            SYMOPT_EXACT_SYMBOLS = 0x00000400, // Do not load an unmatched .pdb file. Do not load export symbols if all else fails.
            SYMOPT_FAIL_CRITICAL_ERRORS = 0x00000200, // Do not display system dialog boxes when there is a media failure such as no media in a drive. Instead, the failure happens silently.
            SYMOPT_FAVOR_COMPRESSED = 0x00800000, // If there is both an uncompressed and a compressed file available, favor the compressed file. This option is good for slow connections.
            SYMOPT_FLAT_DIRECTORY = 0x00400000, // Symbols are stored in the root directory of the default downstream store. DbgHelp 6.1 and earlier:  This value is not supported.
            SYMOPT_IGNORE_CVREC = 0x00000080, // Ignore path information in the CodeView record of the image header when loading a .pdb file.
            SYMOPT_IGNORE_IMAGEDIR = 0x00200000, // Ignore the image directory. DbgHelp 6.1 and earlier:  This value is not supported.
            SYMOPT_IGNORE_NT_SYMPATH = 0x00001000, // Do not use the path specified by _NT_SYMBOL_PATH if the user calls SymSetSearchPath without a valid path. DbgHelp 5.1:  This value is not supported.
            SYMOPT_INCLUDE_32BIT_MODULES = 0x00002000, // When debugging on 64-bit Windows, include any 32-bit modules.
            SYMOPT_LOAD_ANYTHING = 0x00000040, // Disable checks to ensure a file (.exe, .dbg., or .pdb) is the correct file. Instead, load the first file located.
            SYMOPT_LOAD_LINES = 0x00000010, // Loads line number information.
            SYMOPT_NO_CPP = 0x00000008, // All C++ decorated symbols containing the symbol separator "::" are replaced by "__". This option exists for debuggers that cannot handle parsing real C++ symbol names.
            SYMOPT_NO_IMAGE_SEARCH = 0x00020000, // Do not search the image for the symbol path when loading the symbols for a module if the module header cannot be read. DbgHelp 5.1:  This value is not supported.
            SYMOPT_NO_PROMPTS = 0x00080000, // Prevents prompting for validation from the symbol server.
            SYMOPT_NO_PUBLICS = 0x00008000, // Do not search the publics table for symbols. This option should have little effect because there are copies of the public symbols in the globals table. DbgHelp 5.1:  This value is not supported.
            SYMOPT_NO_UNQUALIFIED_LOADS = 0x00000100, // Prevents symbols from being loaded when the caller examines symbols across multiple modules. Examine only the module whose symbols have already been loaded.
            SYMOPT_OVERWRITE = 0x00100000, // Overwrite the downlevel store from the symbol store. DbgHelp 6.1 and earlier:  This value is not supported.
            SYMOPT_PUBLICS_ONLY = 0x00004000, // Do not use private symbols. The version of DbgHelp that shipped with earlier Windows release supported only public symbols; this option provides compatibility with this limitation. DbgHelp 5.1:  This value is not supported.
            SYMOPT_SECURE = 0x00040000, // DbgHelp will not load any symbol server other than SymSrv. SymSrv will not use the downstream store specified in _NT_SYMBOL_PATH. After this flag has been set, it cannot be cleared. DbgHelp 6.0 and 6.1:  This flag can be cleared. DbgHelp 5.1:  This value is not supported.
            SYMOPT_UNDNAME = 0x00000002, // All symbols are presented in undecorated form. This option has no effect on global or local symbols because they are stored undecorated. This option applies only to public symbols.
        }

        [Flags]
        public enum SetErrorFlags : uint
        {
            /// <summary>
            /// Use the system default, which is to display all error dialog boxes.
            /// </summary>
            SEM_DEFAULT = 0,

            /// <summary>
            /// The system does not display the critical-error-handler message box. Instead, the system sends the error to the calling process.
            /// </summary>
            /// <remarks>
            /// Best practice is that all applications call the process-wide SetErrorMode function with a parameter of SEM_FAILCRITICALERRORS at startup. This is to prevent error mode dialogs from hanging the application.
            /// </remarks>
            SEM_FAILCRITICALERRORS = 0x0001,

            /// <summary>
            /// The system automatically fixes memory alignment faults and makes them invisible to the application. It does this for the calling process and any descendant processes. This feature is only supported by certain processor architectures. For more information, see the Remarks section.
            /// </summary>
            /// <remarks>
            /// After this value is set for a process, subsequent attempts to clear the value are ignored.
            /// </remarks>
            SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,

            /// <summary>
            /// The system does not display the Windows Error Reporting dialog.
            /// </summary>
            SEM_NOGPFAULTERRORBOX = 0x0002,

            /// <summary>
            /// The system does not display a message box when it fails to find a file. Instead, the error is returned to the calling process.
            /// </summary>
            SEM_NOOPENFILEERRORBOX = 0x8000
        }

        [DllImport("Kernel32.dll")]
        internal static extern SetErrorFlags SetErrorMode(SetErrorFlags flags);

        [DllImport("DbgHelp.dll", SetLastError = true)]
        internal static extern uint SymSetOptions(Options options);

        [DllImport("DbgHelp.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool SymInitialize(IntPtr handle, string user_search_path, bool invade_process);

        [DllImport("DbgHelp.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool SymCleanup(IntPtr handle);

        [DllImport("DbgHelp.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern ulong SymLoadModule64(IntPtr handle, IntPtr hFile, string ImageName, string ModuleName, ulong BaseOfDll, uint SizeOfDll);

        [DllImport("DbgHelp.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool SymFromAddr(IntPtr handle, ulong address, out ulong displacement, ref SYMBOL_INFO symbol);

        [DllImport("DbgHelp.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool SymGetLineFromAddr64(IntPtr handle, ulong address, out uint displacement, ref IMAGEHLP_LINE64 line);

        internal delegate bool SymRegisterCallbackProc64(IntPtr hProcess, DebugAction action_code, ulong callback_data, ulong user_context);

        [DllImport("DbgHelp.dll")]
        internal static extern bool SymRegisterCallback64(IntPtr hProcess, SymRegisterCallbackProc64 callback, ulong user_context);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }
        public enum DebugAction : uint
        {
            /// <summary>
            /// Display verbose information.
            /// </summary>
            /// <remarks>The CallbackData parameter is a pointer to a string.</remarks>
            CBA_DEBUG_INFO = 0x10000000,

            /// <summary>
            /// Deferred symbol loading has started. To cancel the symbol load, return TRUE.
            /// </summary>
            /// <remarks>The CallbackData parameter is a pointer to a IMAGEHLP_DEFERRED_SYMBOL_LOAD64 structure.</remarks>
            CBA_DEFERRED_SYMBOL_LOAD_CANCEL = 0x00000007,

            /// <summary>
            /// Deferred symbol load has completed.
            /// </summary>
            /// <remarks>The CallbackData parameter is a pointer to a IMAGEHLP_DEFERRED_SYMBOL_LOAD64 structure.</remarks>
            CBA_DEFERRED_SYMBOL_LOAD_COMPLETE = 0x00000002,

            /// <summary>
            /// Deferred symbol load has failed.
            /// </summary>
            /// <remarks>
            /// The CallbackData parameter is a pointer to a IMAGEHLP_DEFERRED_SYMBOL_LOAD64 structure. The symbol handler will attempt to load the symbols again if the callback function sets the FileName member of this structure.
            /// </remarks>
            CBA_DEFERRED_SYMBOL_LOAD_FAILURE = 0x00000003,

            /// <summary>
            /// Deferred symbol load has partially completed. The symbol loader is unable to read the image header from either the image file or the specified module.
            /// </summary>
            /// <remarks>
            /// The CallbackData parameter is a pointer to a IMAGEHLP_DEFERRED_SYMBOL_LOAD64 structure. The symbol handler will attempt to load the symbols again if the callback function sets the FileName member of this structure.
            /// DbgHelp 5.1:  This value is not supported.
            /// </remarks>
            CBA_DEFERRED_SYMBOL_LOAD_PARTIAL = 0x00000020,

            /// <summary>
            /// Deferred symbol load has started.
            /// </summary>
            /// <remarks>
            /// The CallbackData parameter is a pointer to a IMAGEHLP_DEFERRED_SYMBOL_LOAD64 structure.
            /// </remarks>
            CBA_DEFERRED_SYMBOL_LOAD_START = 0x00000001,

            /// <summary>
            /// Duplicate symbols were found. This reason is used only in COFF or CodeView format.
            /// </summary>
            /// <remarks>
            /// The CallbackData parameter is a pointer to a IMAGEHLP_DUPLICATE_SYMBOL64 structure. To specify which symbol to use, set the SelectedSymbol member of this structure.
            /// </remarks>
            CBA_DUPLICATE_SYMBOL = 0x00000005,

            /// <summary>
            /// Display verbose information. If you do not handle this event, the information is resent through the CBA_DEBUG_INFO event.
            /// </summary>
            /// <remarks>
            /// The CallbackData parameter is a pointer to a IMAGEHLP_CBA_EVENT structure. 
            /// </remarks>
            CBA_EVENT = 0x00000010,

            /// <summary>
            /// The loaded image has been read.
            /// </summary>
            /// <remarks>
            /// The CallbackData parameter is a pointer to a IMAGEHLP_CBA_READ_MEMORY structure. The callback function should read the number of bytes specified by the bytes member into the buffer specified by the buf member, and update the bytesread member accordingly.
            /// </remarks>
            CBA_READ_MEMORY = 0x00000006,

            /// <summary>
            /// Symbol options have been updated. To retrieve the current options, call the SymGetOptions function.
            /// </summary>
            /// <remarks>
            /// The CallbackData parameter should be ignored.
            /// </remarks>
            CBA_SET_OPTIONS = 0x00000008,

            /// <summary>
            /// Display verbose information for source server. If you do not handle this event, the information is resent through the CBA_DEBUG_INFO event.
            /// </summary>
            /// <remarks>
            /// The CallbackData parameter is a pointer to a IMAGEHLP_CBA_EVENT structure.
            /// DbgHelp 6.6 and earlier:  This value is not supported.
            /// </remarks>
            CBA_SRCSRV_EVENT = 0x40000000,

            /// <summary>
            /// Display verbose information for source server.
            /// </summary>
            /// <remarks>
            /// The CallbackData parameter is a pointer to a string.
            /// DbgHelp 6.6 and earlier:  This value is not supported.
            /// </remarks>
            CBA_SRCSRV_INFO = 0x20000000,

            /// <summary>
            /// Symbols have been unloaded.
            /// </summary>
            /// <remarks>
            /// The CallbackData parameter should be ignored.
            /// </remarks>
            CBA_SYMBOLS_UNLOADED = 0x00000004,
        }
    }

    public struct SymbolInfo
    {
        public string FileName;
        public int LineNumber;
        public string Symbol;
        public ulong Address;
    }


    public sealed class DbgHelp : IDisposable
    {
        private IntPtr m_Handle;
        private DbgHelpNative.SymRegisterCallbackProc64 m_CallbackGc;

        private static bool OnCallback(IntPtr hProcess, DbgHelpNative.DebugAction action_code, ulong callback_data, ulong user_context)
        {
            switch (action_code)
            {
                case DbgHelpNative.DebugAction.CBA_DEBUG_INFO:
                    Debug.Write(Marshal.PtrToStringAnsi((IntPtr)callback_data));
                    return true;

                default:
                    return false;
            }
        }

        public DbgHelp(ICollection<string> symbol_search_paths)
        {
            IntPtr handle = IntPtr.Add(IntPtr.Zero, 0x7ffffffe);

            DbgHelpNative.SetErrorMode(DbgHelpNative.SetErrorFlags.SEM_FAILCRITICALERRORS | DbgHelpNative.SetErrorFlags.SEM_NOOPENFILEERRORBOX);

            DbgHelpNative.SymSetOptions(DbgHelpNative.Options.SYMOPT_DEFERRED_LOADS | DbgHelpNative.Options.SYMOPT_DEBUG);

            string search_paths = null;
            if (symbol_search_paths.Count > 0)
            {
                var path = new StringBuilder(1024);
                foreach (var sym in symbol_search_paths)
                {
                    if (path.Length > 0)
                        path.Append(";");
                    path.Append(sym);
                }
                search_paths = path.ToString();
            }


            if (!DbgHelpNative.SymInitialize(handle, search_paths, false))
            {
                throw new ApplicationException("Failed to initialize DbgHelp library");
            }

            m_CallbackGc = new DbgHelpNative.SymRegisterCallbackProc64(OnCallback);

            DbgHelpNative.SymRegisterCallback64(handle, m_CallbackGc, 0);

            m_Handle = handle;
        }

        ~DbgHelp()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (m_Handle.ToInt64() == 0)
                return;

            DbgHelpNative.SymCleanup(m_Handle);

            m_Handle = IntPtr.Zero;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void LoadModule(string name, ulong base_address, ulong size)
        {
            if (0 == DbgHelpNative.SymLoadModule64(m_Handle, IntPtr.Zero, name, null, base_address, (uint)size))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to load " + name);
            }
        }

        public bool LookupSymbol(ulong address, out SymbolInfo sym)
        {
            sym.FileName = "";
            sym.LineNumber = 0;
            sym.Symbol = "";
            sym.Address = address;

            DbgHelpNative.SYMBOL_INFO data = new DbgHelpNative.SYMBOL_INFO();
            data.SizeOfStruct = (uint)Marshal.SizeOf(data) - 1024;
            data.MaxNameLen = 1024;

            ulong displacement;
            if (DbgHelpNative.SymFromAddr(m_Handle, address, out displacement, ref data))
            {
                sym.Symbol = data.Name;
            }
            else
            {
                return false;
            }

            DbgHelpNative.IMAGEHLP_LINE64 line = new DbgHelpNative.IMAGEHLP_LINE64();
            line.SizeOfStruct = (uint)Marshal.SizeOf(line);

            uint disp32;

            if (DbgHelpNative.SymGetLineFromAddr64(m_Handle, address, out disp32, ref line))
            {
                StringBuilder fn = new StringBuilder(128);
                for (int i = 0; ; ++i)
                {
                    byte b = Marshal.ReadByte(IntPtr.Add(line.FileName, i));
                    if (0 == b)
                        break;
                    fn.Append((char)b);
                }
                sym.FileName = fn.ToString();
                Console.WriteLine(sym.FileName); // added
                sym.LineNumber = (int)line.LineNumber;
                Console.WriteLine(sym.LineNumber); // added
            }
            else
            {
                Console.WriteLine(sym.FileName); // added
                sym.FileName = "(no source)";
            }

            return true;
        }

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern int NtQueryInformationThread(IntPtr threadhandle, ThreadInfomationClass threadinfoclass, IntPtr threadinformation, int threadinformationlength, IntPtr returnlengthptr);
        private enum ThreadInfomationClass : int
        {
            threadquerysetwin32startaddress = 9
        }

        public static String GetThreadStartAddress(IntPtr hProc, uint threadId)
        {
            IntPtr hThread = IntPtr.Zero;
            GCHandle handle = default(GCHandle);

            try
            {
                hThread = DbgHelpNative.OpenThread(DbgHelpNative.ThreadAccess.QUERY_INFORMATION, false, threadId);

                if (hThread == IntPtr.Zero)
                {
                    //throw new Win32Exception("OpenThread failed");
                }

                var threadAddress = new IntPtr[1];

                handle = GCHandle.Alloc(threadAddress, GCHandleType.Pinned);
                var result = NtQueryInformationThread(hThread, ThreadInfomationClass.threadquerysetwin32startaddress, handle.AddrOfPinnedObject(), IntPtr.Size, IntPtr.Zero);

                if (result != 0)
                {
                    //throw new Win32Exception(string.Format("NtQueryInformationThread failed; NTSTATUS = {0:X8}", result));
                }

                DbgHelpNative.SymSetOptions(DbgHelpNative.Options.SYMOPT_UNDNAME | DbgHelpNative.Options.SYMOPT_DEFERRED_LOADS);

                if (!DbgHelpNative.SymInitialize(hProc, null, true))
                {
                    //throw new Win32Exception("SymInitialize failed");
                }

                DbgHelpNative.SYMBOL_INFO symbolInfo = new DbgHelpNative.SYMBOL_INFO();

                symbolInfo.SizeOfStruct = (uint)Marshal.SizeOf(typeof(DbgHelpNative.SYMBOL_INFO)) - 1024;

                symbolInfo.MaxNameLen = 1024;

                ulong displacement;

                if (!DbgHelpNative.SymFromAddr(hProc, (ulong)threadAddress[0], out displacement, ref symbolInfo))
                {
                    //throw new Win32Exception("SymFromAddr failed");
                }
                return symbolInfo.Name;
                //return threadAddress[0];
            }
            finally
            {
                if (hThread != IntPtr.Zero)
                {
                    DbgHelpNative.CloseHandle(hThread);
                }

                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
        }

    }
}
