
namespace ProcessThreadInfo
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.threadinfo_list = new System.Windows.Forms.ListView();
            this.tid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.startaddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.priority = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.get_process_thread_btn = new System.Windows.Forms.Button();
            this.proc_txt = new System.Windows.Forms.TextBox();
            this.procid_lbl = new System.Windows.Forms.Label();
            this.find_filtered_thread = new System.Windows.Forms.Button();
            this.filter_lbl = new System.Windows.Forms.Label();
            this.filter_txt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // threadinfo_list
            // 
            this.threadinfo_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.tid,
            this.startaddr,
            this.priority});
            this.threadinfo_list.FullRowSelect = true;
            this.threadinfo_list.HideSelection = false;
            this.threadinfo_list.Location = new System.Drawing.Point(12, 32);
            this.threadinfo_list.Name = "threadinfo_list";
            this.threadinfo_list.Size = new System.Drawing.Size(478, 406);
            this.threadinfo_list.TabIndex = 0;
            this.threadinfo_list.UseCompatibleStateImageBehavior = false;
            this.threadinfo_list.View = System.Windows.Forms.View.Details;
            // 
            // tid
            // 
            this.tid.Text = "Thread Id";
            // 
            // startaddr
            // 
            this.startaddr.Text = "Start Address";
            this.startaddr.Width = 300;
            // 
            // priority
            // 
            this.priority.Text = "Priority";
            this.priority.Width = 95;
            // 
            // get_process_thread_btn
            // 
            this.get_process_thread_btn.Location = new System.Drawing.Point(190, 4);
            this.get_process_thread_btn.Name = "get_process_thread_btn";
            this.get_process_thread_btn.Size = new System.Drawing.Size(75, 23);
            this.get_process_thread_btn.TabIndex = 1;
            this.get_process_thread_btn.Text = "Get Thread";
            this.get_process_thread_btn.UseVisualStyleBackColor = true;
            this.get_process_thread_btn.Click += new System.EventHandler(this.get_process_thread_Click);
            // 
            // proc_txt
            // 
            this.proc_txt.Location = new System.Drawing.Point(84, 6);
            this.proc_txt.Name = "proc_txt";
            this.proc_txt.Size = new System.Drawing.Size(100, 20);
            this.proc_txt.TabIndex = 2;
            // 
            // procid_lbl
            // 
            this.procid_lbl.AutoSize = true;
            this.procid_lbl.Location = new System.Drawing.Point(12, 9);
            this.procid_lbl.Name = "procid_lbl";
            this.procid_lbl.Size = new System.Drawing.Size(66, 13);
            this.procid_lbl.TabIndex = 3;
            this.procid_lbl.Text = "Process Id : ";
            // 
            // find_filtered_thread
            // 
            this.find_filtered_thread.Location = new System.Drawing.Point(415, 444);
            this.find_filtered_thread.Name = "find_filtered_thread";
            this.find_filtered_thread.Size = new System.Drawing.Size(75, 23);
            this.find_filtered_thread.TabIndex = 4;
            this.find_filtered_thread.Text = "Find";
            this.find_filtered_thread.UseVisualStyleBackColor = true;
            this.find_filtered_thread.Click += new System.EventHandler(this.find_filtered_thread_Click);
            // 
            // filter_lbl
            // 
            this.filter_lbl.AutoSize = true;
            this.filter_lbl.Location = new System.Drawing.Point(12, 449);
            this.filter_lbl.Name = "filter_lbl";
            this.filter_lbl.Size = new System.Drawing.Size(69, 13);
            this.filter_lbl.TabIndex = 5;
            this.filter_lbl.Text = "Filter Name : ";
            // 
            // filter_txt
            // 
            this.filter_txt.Location = new System.Drawing.Point(84, 446);
            this.filter_txt.Name = "filter_txt";
            this.filter_txt.Size = new System.Drawing.Size(325, 20);
            this.filter_txt.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 475);
            this.Controls.Add(this.filter_txt);
            this.Controls.Add(this.filter_lbl);
            this.Controls.Add(this.find_filtered_thread);
            this.Controls.Add(this.procid_lbl);
            this.Controls.Add(this.proc_txt);
            this.Controls.Add(this.get_process_thread_btn);
            this.Controls.Add(this.threadinfo_list);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Process Thread Info";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView threadinfo_list;
        private System.Windows.Forms.ColumnHeader tid;
        private System.Windows.Forms.ColumnHeader startaddr;
        private System.Windows.Forms.ColumnHeader priority;
        private System.Windows.Forms.Button get_process_thread_btn;
        private System.Windows.Forms.TextBox proc_txt;
        private System.Windows.Forms.Label procid_lbl;
        private System.Windows.Forms.Button find_filtered_thread;
        private System.Windows.Forms.Label filter_lbl;
        private System.Windows.Forms.TextBox filter_txt;
    }
}

