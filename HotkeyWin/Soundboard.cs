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
        private static Hotkeys.GlobalHotkey[] _hotkey = new Hotkeys.GlobalHotkey[10];

        public Console _console = new Console();

        public Soundboard()
        {
            InitializeComponent();
            _console.Hide();

            // assign hotkey values
            ghk1 = new GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D1, this);
            ghk2 = new GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D2, this);
            ghk3 = new GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D3, this);
            ghk4 = new GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D4, this);
            ghk5 = new GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D5, this);
            ghk6 = new GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D6, this);
            ghk7 = new GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D7, this);
            ghk8 = new GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D8, this);
            ghk9 = new GlobalHotkey(Constants.ALT + Constants.CTRL, Keys.D9, this);

            // the _hotkey array is used to iterate through all the keys in a loop
            // in other parts of the program, so there are chunks of code repeated 9 times everywhere.
            _hotkey[0] = null; // i don't want to deal with zero based numbering for readability's sake
            _hotkey[1] = ghk1;
            _hotkey[2] = ghk2;
            _hotkey[3] = ghk3;
            _hotkey[4] = ghk4;
            _hotkey[5] = ghk5;
            _hotkey[6] = ghk6;
            _hotkey[7] = ghk7;
            _hotkey[8] = ghk8;
            _hotkey[9] = ghk9;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // register the keys on load
            for (int i = 1; i <= 9; i++)
            {
                _console.WriteLine("Trying to register CTRL+ALT+" + i);
                if (_hotkey[i].Register()) _console.WriteLine("Hotkey registered.");
                else _console.WriteLine("Hotkey failed to register");
            }
        }

        private void HandleHotkey(Message m)
        {
            _console.WriteLine("Hotkey pressed for : " + m.ToString());
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey(m);
            base.WndProc(ref m);
        }

        // unregister the keys when the window is closed
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 1; i <= 9; i++)
            {
                _console.WriteLine("Unregistering CTRL+ALT+" + i);
                if (!_hotkey[i].Unregiser()) MessageBox.Show("Hotkey failed to unregister!");
            } 
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_toggleConsole_Click_1(object sender, EventArgs e)
        {
            if (!_console.Visible)
            {
                _console.Show();
                button_toggleConsole.Text = "Hide Console";
            }
            else
            {
                _console.Hide();
                button_toggleConsole.Text = "Show Console";
            }
        }
    }
}
