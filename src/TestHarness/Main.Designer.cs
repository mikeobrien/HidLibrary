namespace TestHarness
{
    partial class Main
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
            this.Devices = new System.Windows.Forms.ListBox();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.Scan = new System.Windows.Forms.Button();
            this.Read = new System.Windows.Forms.Button();
            this.SuccessBeep = new System.Windows.Forms.Button();
            this.ErrorBeep = new System.Windows.Forms.Button();
            this.Output = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Devices
            // 
            this.Devices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Devices.FormattingEnabled = true;
            this.Devices.Location = new System.Drawing.Point(0, 12);
            this.Devices.Name = "Devices";
            this.Devices.Size = new System.Drawing.Size(549, 147);
            this.Devices.TabIndex = 0;
            this.Devices.SelectedIndexChanged += new System.EventHandler(this.Devices_SelectedIndexChanged);
            this.Devices.DoubleClick += new System.EventHandler(this.Devices_DoubleClick);
            // 
            // Refresh
            // 
            this.RefreshBtn.Location = new System.Drawing.Point(350, 164);
            this.RefreshBtn.Name = "Refresh";
            this.RefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.RefreshBtn.TabIndex = 1;
            this.RefreshBtn.Text = "Refresh";
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // Scan
            // 
            this.Scan.Location = new System.Drawing.Point(93, 164);
            this.Scan.Name = "Scan";
            this.Scan.Size = new System.Drawing.Size(75, 23);
            this.Scan.TabIndex = 2;
            this.Scan.Text = "Scan";
            this.Scan.UseVisualStyleBackColor = true;
            this.Scan.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Scan_MouseDown);
            this.Scan.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Scan_MouseUp);
            // 
            // Read
            // 
            this.Read.Location = new System.Drawing.Point(12, 164);
            this.Read.Name = "Read";
            this.Read.Size = new System.Drawing.Size(75, 23);
            this.Read.TabIndex = 3;
            this.Read.Text = "Read";
            this.Read.UseVisualStyleBackColor = true;
            this.Read.Click += new System.EventHandler(this.Read_Click);
            // 
            // SuccessBeep
            // 
            this.SuccessBeep.Location = new System.Drawing.Point(255, 164);
            this.SuccessBeep.Name = "SuccessBeep";
            this.SuccessBeep.Size = new System.Drawing.Size(89, 23);
            this.SuccessBeep.TabIndex = 4;
            this.SuccessBeep.Text = "Success Beep";
            this.SuccessBeep.UseVisualStyleBackColor = true;
            this.SuccessBeep.Click += new System.EventHandler(this.SuccessBeep_Click);
            // 
            // ErrorBeep
            // 
            this.ErrorBeep.Location = new System.Drawing.Point(174, 164);
            this.ErrorBeep.Name = "ErrorBeep";
            this.ErrorBeep.Size = new System.Drawing.Size(75, 23);
            this.ErrorBeep.TabIndex = 5;
            this.ErrorBeep.Text = "Error Beep";
            this.ErrorBeep.UseVisualStyleBackColor = true;
            this.ErrorBeep.Click += new System.EventHandler(this.ErrorBeep_Click);
            // 
            // Output
            // 
            this.Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Output.Location = new System.Drawing.Point(0, 193);
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(549, 274);
            this.Output.TabIndex = 6;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 470);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.ErrorBeep);
            this.Controls.Add(this.SuccessBeep);
            this.Controls.Add(this.Read);
            this.Controls.Add(this.Scan);
            this.Controls.Add(this.RefreshBtn);
            this.Controls.Add(this.Devices);
            this.Name = "Main";
            this.Text = "Hid Test Harness";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox Devices;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.Button Scan;
        private System.Windows.Forms.Button Read;
        private System.Windows.Forms.Button SuccessBeep;
        private System.Windows.Forms.Button ErrorBeep;
        private System.Windows.Forms.TextBox Output;
    }
}

