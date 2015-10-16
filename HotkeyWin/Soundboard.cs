using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hotkeys;

namespace HotkeyWin
{
    public partial class Soundboard : Form
    {
        // global hotkey variables
        private static Hotkeys.GlobalHotkey ghk1, ghk2, ghk3, ghk4, ghk5, ghk6, ghk7, ghk8, ghk9;
        private static Hotkeys.GlobalHotkey[] hotkeyPointer = new Hotkeys.GlobalHotkey[10];

        public Soundboard()
        {
            InitializeComponent();

            // assign hotkey values
            ghk1 = new Hotkeys.GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D1, this);
            ghk2 = new Hotkeys.GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D2, this);
            ghk3 = new Hotkeys.GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D3, this);
            ghk4 = new Hotkeys.GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D4, this);
            ghk5 = new Hotkeys.GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D5, this);
            ghk6 = new Hotkeys.GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D6, this);
            ghk7 = new Hotkeys.GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D7, this);
            ghk8 = new Hotkeys.GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D8, this);
            ghk9 = new Hotkeys.GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D9, this);

            // the hotkeyPointer array is used to iterate through all the keys in a loop
            // in other parts of the program, so there are chunks of code repeated 9 times everywhere.
            hotkeyPointer[0] = null; // i don't want to deal with zero based numbering for readability's sake
            hotkeyPointer[1] = ghk1;
            hotkeyPointer[2] = ghk2;
            hotkeyPointer[3] = ghk3;
            hotkeyPointer[4] = ghk4;
            hotkeyPointer[5] = ghk5;
            hotkeyPointer[6] = ghk6;
            hotkeyPointer[7] = ghk7;
            hotkeyPointer[8] = ghk8;
            hotkeyPointer[9] = ghk9;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // register the keys on load
            for (int i = 1; i <= 9; i++)
            {
                WriteLine("Trying to register CTRL+ALT+" + i);
                if (hotkeyPointer[i].Register()) WriteLine("Hotkey registered.");
                else WriteLine("Hotkey failed to register");
            }
        }

        private void HandleHotkey()
        {
            WriteLine("Hotkey pressed!");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Hotkeys.Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }

        // unregister the keys when the window is closed
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 1; i <= 9; i++)
            {
                WriteLine("Unregistering CTRL+ALT+" + i);
                if (!hotkeyPointer[i].Unregiser()) MessageBox.Show("Hotkey failed to unregister!");
            } 
        }

        private void WriteLine(string text)
        {
            textBox_console.Text += text + Environment.NewLine;
        }

        private void button_toggleConsole_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = (splitContainer1.Panel2Collapsed == false) ? splitContainer1.Panel2Collapsed = true : splitContainer1.Panel2Collapsed = false;
            
            
            this.Width = splitContainer1.Panel1.Width;
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
