namespace CPUMonitor
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
            this.startStopBtn = new System.Windows.Forms.Button();
            this.safeModeStartBtn = new System.Windows.Forms.Button();
            this.onTopCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // startStopBtn
            // 
            this.startStopBtn.Location = new System.Drawing.Point(111, 226);
            this.startStopBtn.Name = "startStopBtn";
            this.startStopBtn.Size = new System.Drawing.Size(75, 23);
            this.startStopBtn.TabIndex = 0;
            this.startStopBtn.Text = "Start";
            this.startStopBtn.UseVisualStyleBackColor = true;
            this.startStopBtn.Click += new System.EventHandler(this.startStopBtn_Click);
            // 
            // safeModeStartBtn
            // 
            this.safeModeStartBtn.Location = new System.Drawing.Point(192, 226);
            this.safeModeStartBtn.Name = "safeModeStartBtn";
            this.safeModeStartBtn.Size = new System.Drawing.Size(111, 23);
            this.safeModeStartBtn.TabIndex = 1;
            this.safeModeStartBtn.Text = "Start (safe mode)";
            this.safeModeStartBtn.UseVisualStyleBackColor = true;
            this.safeModeStartBtn.Click += new System.EventHandler(this.safeModeStartBtn_Click);
            // 
            // onTopCheckBox
            // 
            this.onTopCheckBox.AutoSize = true;
            this.onTopCheckBox.Location = new System.Drawing.Point(26, 297);
            this.onTopCheckBox.Name = "onTopCheckBox";
            this.onTopCheckBox.Size = new System.Drawing.Size(92, 17);
            this.onTopCheckBox.TabIndex = 2;
            this.onTopCheckBox.Text = "Always on top";
            this.onTopCheckBox.UseVisualStyleBackColor = true;
            this.onTopCheckBox.CheckedChanged += new System.EventHandler(this.onTopCheckBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 498);
            this.Controls.Add(this.onTopCheckBox);
            this.Controls.Add(this.safeModeStartBtn);
            this.Controls.Add(this.startStopBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Better CPU Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startStopBtn;
        private System.Windows.Forms.Button safeModeStartBtn;
        private System.Windows.Forms.CheckBox onTopCheckBox;
    }
}

