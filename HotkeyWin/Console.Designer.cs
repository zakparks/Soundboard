namespace HotkeyWin
{
    partial class Console
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
            this.listBox_console = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox_console
            // 
            this.listBox_console.FormattingEnabled = true;
            this.listBox_console.Location = new System.Drawing.Point(16, 12);
            this.listBox_console.Name = "listBox_console";
            this.listBox_console.ScrollAlwaysVisible = true;
            this.listBox_console.Size = new System.Drawing.Size(834, 394);
            this.listBox_console.TabIndex = 1;
            // 
            // Console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 421);
            this.Controls.Add(this.listBox_console);
            this.Name = "Console";
            this.Text = "Console";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_console;
    }
}