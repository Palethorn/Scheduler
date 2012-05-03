namespace SchedulerServer
{
    partial class MainWindow
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
            this.LogTxt = new System.Windows.Forms.RichTextBox();
            this.LogLbl = new System.Windows.Forms.Label();
            this.ServerSwitch = new System.Windows.Forms.Button();
            this.BindIpLbl = new System.Windows.Forms.Label();
            this.BindPortLbl = new System.Windows.Forms.Label();
            this.BindIpInput = new System.Windows.Forms.TextBox();
            this.BindPortInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LogTxt
            // 
            this.LogTxt.Location = new System.Drawing.Point(19, 89);
            this.LogTxt.Name = "LogTxt";
            this.LogTxt.Size = new System.Drawing.Size(438, 157);
            this.LogTxt.TabIndex = 0;
            this.LogTxt.Text = "";
            // 
            // LogLbl
            // 
            this.LogLbl.AutoSize = true;
            this.LogLbl.Location = new System.Drawing.Point(16, 73);
            this.LogLbl.Name = "LogLbl";
            this.LogLbl.Size = new System.Drawing.Size(28, 13);
            this.LogLbl.TabIndex = 1;
            this.LogLbl.Text = "Log:";
            // 
            // ServerSwitch
            // 
            this.ServerSwitch.Location = new System.Drawing.Point(382, 31);
            this.ServerSwitch.Name = "ServerSwitch";
            this.ServerSwitch.Size = new System.Drawing.Size(75, 23);
            this.ServerSwitch.TabIndex = 2;
            this.ServerSwitch.Text = "Start Server";
            this.ServerSwitch.UseVisualStyleBackColor = true;
            // 
            // BindIpLbl
            // 
            this.BindIpLbl.AutoSize = true;
            this.BindIpLbl.Location = new System.Drawing.Point(16, 12);
            this.BindIpLbl.Name = "BindIpLbl";
            this.BindIpLbl.Size = new System.Drawing.Size(43, 13);
            this.BindIpLbl.TabIndex = 4;
            this.BindIpLbl.Text = "Bind Ip:";
            // 
            // BindPortLbl
            // 
            this.BindPortLbl.AutoSize = true;
            this.BindPortLbl.Location = new System.Drawing.Point(16, 41);
            this.BindPortLbl.Name = "BindPortLbl";
            this.BindPortLbl.Size = new System.Drawing.Size(53, 13);
            this.BindPortLbl.TabIndex = 5;
            this.BindPortLbl.Text = "Bind Port:";
            // 
            // BindIpInput
            // 
            this.BindIpInput.Location = new System.Drawing.Point(93, 5);
            this.BindIpInput.Name = "BindIpInput";
            this.BindIpInput.Size = new System.Drawing.Size(141, 20);
            this.BindIpInput.TabIndex = 6;
            // 
            // BindPortInput
            // 
            this.BindPortInput.Location = new System.Drawing.Point(93, 34);
            this.BindPortInput.Name = "BindPortInput";
            this.BindPortInput.Size = new System.Drawing.Size(75, 20);
            this.BindPortInput.TabIndex = 7;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 252);
            this.Controls.Add(this.BindPortInput);
            this.Controls.Add(this.BindIpInput);
            this.Controls.Add(this.BindPortLbl);
            this.Controls.Add(this.BindIpLbl);
            this.Controls.Add(this.ServerSwitch);
            this.Controls.Add(this.LogLbl);
            this.Controls.Add(this.LogTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainWindow";
            this.Text = "Scheduler server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox LogTxt;
        private System.Windows.Forms.Label LogLbl;
        private System.Windows.Forms.Button ServerSwitch;
        private System.Windows.Forms.Label BindIpLbl;
        private System.Windows.Forms.Label BindPortLbl;
        private System.Windows.Forms.TextBox BindIpInput;
        private System.Windows.Forms.TextBox BindPortInput;
    }
}

