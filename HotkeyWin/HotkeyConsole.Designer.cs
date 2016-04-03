namespace GlobalHotkeyConsole
{
    partial class HotkeyConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HotkeyConsole));
            this.listBox_console = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox_console
            // 
            this.listBox_console.BackColor = System.Drawing.SystemColors.ControlLight;
            this.listBox_console.FormattingEnabled = true;
            this.listBox_console.Location = new System.Drawing.Point(16, 12);
            this.listBox_console.Name = "listBox_console";
            this.listBox_console.ScrollAlwaysVisible = true;
            this.listBox_console.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox_console.Size = new System.Drawing.Size(834, 394);
            this.listBox_console.TabIndex = 1;
            // 
            // HotkeyConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 421);
            this.ControlBox = false;
            this.Controls.Add(this.listBox_console);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HotkeyConsole";
            this.Text = "HotkeyConsole";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_console;
    }
}