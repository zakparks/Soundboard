using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HotkeyWin
{
    public partial class Console : Form
    {
        public Console()
        {
            InitializeComponent();
        }

        public void WriteLine(string text)
        {
            listBox_console.Items.Add(text + Environment.NewLine);

            // the text box text doesn't scroll correctly, its pinned to the top fix plz
            listBox_console.TopIndex = listBox_console.Items.Count - 1;
        }

        private void Console_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
