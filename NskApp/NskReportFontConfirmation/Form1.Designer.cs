namespace NskReportFontConfirmation
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
            viewer1 = new GrapeCity.ActiveReports.Viewer.Win.Viewer();
            label1 = new System.Windows.Forms.Label();
            TxtFilePath = new System.Windows.Forms.TextBox();
            btnSelect = new System.Windows.Forms.Button();
            btnExcute = new System.Windows.Forms.Button();
            TxtLog = new System.Windows.Forms.TextBox();
            checkBox1 = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // viewer1
            // 
            viewer1.CurrentPage = 0;
            viewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            viewer1.Font = new System.Drawing.Font("Segoe UI", 9F);
            viewer1.Location = new System.Drawing.Point(0, 0);
            viewer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            viewer1.Name = "viewer1";
            viewer1.PagesBackColor = System.Drawing.Color.FromArgb(128, 128, 128, 128);
            viewer1.PreviewPages = 0;
            // 
            // 
            // 
            // 
            // 
            // 
            viewer1.Sidebar.ParametersPanel.ContextMenu = null;
            viewer1.Sidebar.ParametersPanel.Width = 200;
            // 
            // 
            // 
            viewer1.Sidebar.SearchPanel.ContextMenu = null;
            viewer1.Sidebar.SearchPanel.Width = 200;
            // 
            // 
            // 
            viewer1.Sidebar.ThumbnailsPanel.ContextMenu = null;
            viewer1.Sidebar.ThumbnailsPanel.Width = 200;
            viewer1.Sidebar.ThumbnailsPanel.Zoom = 0.1D;
            // 
            // 
            // 
            viewer1.Sidebar.TocPanel.ContextMenu = null;
            viewer1.Sidebar.TocPanel.Expanded = true;
            viewer1.Sidebar.TocPanel.Width = 200;
            viewer1.Sidebar.Width = 200;
            viewer1.Size = new System.Drawing.Size(751, 459);
            viewer1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            label1.Location = new System.Drawing.Point(12, 67);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(121, 19);
            label1.TabIndex = 1;
            label1.Text = "帳票デザインファイル";
            // 
            // TxtFilePath
            // 
            TxtFilePath.Location = new System.Drawing.Point(139, 67);
            TxtFilePath.Name = "TxtFilePath";
            TxtFilePath.Size = new System.Drawing.Size(499, 23);
            TxtFilePath.TabIndex = 2;
            // 
            // btnSelect
            // 
            btnSelect.Location = new System.Drawing.Point(644, 67);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new System.Drawing.Size(75, 23);
            btnSelect.TabIndex = 3;
            btnSelect.Text = "選択";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnExcute
            // 
            btnExcute.Location = new System.Drawing.Point(644, 112);
            btnExcute.Name = "btnExcute";
            btnExcute.Size = new System.Drawing.Size(75, 50);
            btnExcute.TabIndex = 4;
            btnExcute.Text = "実行";
            btnExcute.UseVisualStyleBackColor = true;
            btnExcute.Click += btnExcute_Click;
            // 
            // TxtLog
            // 
            TxtLog.Location = new System.Drawing.Point(12, 182);
            TxtLog.Multiline = true;
            TxtLog.Name = "TxtLog";
            TxtLog.Size = new System.Drawing.Size(707, 235);
            TxtLog.TabIndex = 5;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(12, 112);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(382, 19);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "法定帳票※法定帳票の場合、このチェックボックスをチェックONしてください。";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(751, 459);
            Controls.Add(checkBox1);
            Controls.Add(TxtLog);
            Controls.Add(btnExcute);
            Controls.Add(btnSelect);
            Controls.Add(TxtFilePath);
            Controls.Add(label1);
            Controls.Add(viewer1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GrapeCity.ActiveReports.Viewer.Win.Viewer viewer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtFilePath;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnExcute;
        private System.Windows.Forms.TextBox TxtLog;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

