using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace ProcessThreadInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void find_filtered_thread_Click(object sender, EventArgs e)
        {
            ListView tmp = new ListView();
            int idx = 0;
            foreach (ListViewItem items in threadinfo_list.Items)
            {
                if (items.SubItems[1].Text == filter_txt.Text)
                {
                    tmp.Items.Add(items.SubItems[0].Text);
                    tmp.Items[idx].SubItems.Add(items.SubItems[1].Text);
                    tmp.Items[idx].SubItems.Add(items.SubItems[2].Text);
                    idx++;
                }
            }
            threadinfo_list.Items.Clear();
            foreach (ListViewItem update in tmp.Items)
            {
                threadinfo_list.Items.Add((ListViewItem)update.Clone());
            }
            tmp.Clear();
        }

        private void get_process_thread_Click(object sender, EventArgs e)
        {
            Process proc = Process.GetProcessById(int.Parse(proc_txt.Text));
            ProcessThreadCollection procThread = proc.Threads;
            threadinfo_list.Items.Clear();
            int idx = 0;
            foreach (ProcessThread i in procThread)
            {
                threadinfo_list.Items.Add(i.Id.ToString());
                threadinfo_list.Items[idx].SubItems.Add(DbgHelp.GetThreadStartAddress(proc.Handle, (uint)i.Id));
                threadinfo_list.Items[idx].SubItems.Add(i.PriorityLevel.ToString());
                idx++;
            }
        }
    }
}
