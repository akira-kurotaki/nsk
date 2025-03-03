namespace NskReportDesignDriver
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
            P1001 = new System.Windows.Forms.Button();
            viewer1 = new GrapeCity.ActiveReports.Viewer.Win.Viewer();
            numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            P1002 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // P1001
            // 
            P1001.Location = new System.Drawing.Point(12, 110);
            P1001.Name = "P1001";
            P1001.Size = new System.Drawing.Size(75, 23);
            P1001.TabIndex = 1;
            P1001.Text = "P1001";
            P1001.UseVisualStyleBackColor = true;
            P1001.Click += P1001_Click;
            // 
            // viewer1
            // 
            viewer1.CurrentPage = 0;
            viewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            viewer1.Font = new System.Drawing.Font("Segoe UI", 9F);
            viewer1.Location = new System.Drawing.Point(0, 0);
            viewer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            viewer1.Name = "viewer1";
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
            viewer1.Load += viewer1_Load;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new System.Drawing.Point(12, 64);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new System.Drawing.Size(120, 23);
            numericUpDown1.TabIndex = 2;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // P1002
            // 
            P1002.Location = new System.Drawing.Point(12, 151);
            P1002.Name = "P1002";
            P1002.Size = new System.Drawing.Size(75, 23);
            P1002.TabIndex = 3;
            P1002.Text = "P1002";
            P1002.UseVisualStyleBackColor = true;
            P1002.Click += P1002_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(751, 459);
            Controls.Add(P1002);
            Controls.Add(numericUpDown1);
            Controls.Add(P1001);
            Controls.Add(viewer1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GrapeCity.ActiveReports.Viewer.Win.Viewer viewer1;
        private System.Windows.Forms.Button P1001;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button P1002;
    }
}

