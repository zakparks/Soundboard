using GlobalHotkeyConsole;
using System;
using System.Windows.Forms;

namespace GlobalHotkey
{
    /// <summary>
    /// The form that lets the user set audio file assignments, and view console output
    /// </summary>
    public partial class Soundboard : Form
    {
        /// <summary>
        /// Global hotkeys representing the row of numbers on top of the keyboard
        /// </summary>
        private static GlobalHotkey ghk1, ghk2, ghk3, ghk4, ghk5, ghk6, ghk7, ghk8, ghk9;

        /// <summary>
        /// array that will hold the above hotkeys
        /// </summary>
        public static GlobalHotkey[] Hotkeys = new GlobalHotkey[10];

        /// <summary>
        /// Tracks the current hotkey by ID 1-9
        /// </summary>
        private static int _keyId;

        /// <summary>
        /// Class reference of the output console
        /// </summary>
        public readonly HotkeyConsole _console;

        /// <summary>
        /// Bool to toggle if buttons assign or unassign audio files
        /// </summary>
        private bool _unassign = false;

        /// <summary>
        /// Instance of the soundboard numberpad
        /// </summary>
        public Soundboard()
        {
            InitializeComponent();

            this._console = HotkeyConsole.GetInstance();

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
            Hotkeys[0] = null; // i don't want to deal with zero based numbering for readability's sake
            Hotkeys[1] = ghk1;
            Hotkeys[2] = ghk2;
            Hotkeys[3] = ghk3;
            Hotkeys[4] = ghk4;
            Hotkeys[5] = ghk5;
            Hotkeys[6] = ghk6;
            Hotkeys[7] = ghk7;
            Hotkeys[8] = ghk8;
            Hotkeys[9] = ghk9;

            // load the config if there is one from a previous session
            AudioConfig.checkLoadConfig();

            // set up or install the Virtual Audio Cable
            AudioConfig.CheckSetupAudio();
        }

        /// <summary>
        /// Registers all of the hotkeys declared at the beginning of the class automatically on startup
        /// Loads the config file and assigns keys with existing audio
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event args</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // register the keys on load
            for (int i = 1; i <= 9; i++)
            {
                _console.WriteLine("Trying to register CTRL+ALT+" + i);
                if (Hotkeys[i].Register()) _console.WriteLine("Hotkey registered.");
                else _console.WriteLine("Hotkey failed to register :(");
            }

            // load the config file and assign any keys with existing audio

        }

        /// <summary>
        /// Processes what to do when a hotkey press is detected
        /// </summary>
        /// <param name="m">Message with keypress paramaters</param>
        private void HandleHotkey(Message m)
        {
            string keyPressed = m.LParam.ToString();

            switch (keyPressed)
            {
                case Constants.HK_1:
                    _console.WriteLine("Hotkey 1 pressed!");
                    _keyId = 1;
                    Hotkeys[_keyId].PlayAudio(_keyId);
                    break;

                case Constants.HK_2:
                    _console.WriteLine("Hotkey 2 pressed!");
                    _keyId = 2;
                    Hotkeys[_keyId].PlayAudio(_keyId);
                    break;

                case Constants.HK_3:
                    _console.WriteLine("Hotkey 3 pressed!");
                    _keyId = 3;
                    Hotkeys[_keyId].PlayAudio(_keyId);
                    break;

                case Constants.HK_4:
                    _console.WriteLine("Hotkey 4 pressed!");
                    _keyId = 4;
                    Hotkeys[_keyId].PlayAudio(_keyId);
                    break;

                case Constants.HK_5:
                    _console.WriteLine("Hotkey 5 pressed!");
                    _keyId = 5;
                    Hotkeys[_keyId].PlayAudio(_keyId);
                    break;

                case Constants.HK_6:
                    _console.WriteLine("Hotkey 6 pressed!");
                    _keyId = 6;
                    Hotkeys[_keyId].PlayAudio(_keyId);
                    break;

                case Constants.HK_7:
                    _console.WriteLine("Hotkey 7 pressed!");
                    _keyId = 7;
                    Hotkeys[_keyId].PlayAudio(_keyId);
                    break;

                case Constants.HK_8:
                    _console.WriteLine("Hotkey 8 pressed!");
                    _keyId = 8;
                    Hotkeys[_keyId].PlayAudio(_keyId);
                    break;

                case Constants.HK_9:
                    _console.WriteLine("Hotkey 9 pressed!");
                    _keyId = 9;
                    Hotkeys[_keyId].PlayAudio(_keyId);
                    break;

                default:
                    break;
            }
            _keyId = 0;
        }

        /// <summary>
        /// Detects when a key is pressed
        /// </summary>
        /// <param name="m">Hotkey press paramaters</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey(m);
            base.WndProc(ref m);
        }

        /// <summary>
        /// Show or hide the console window, and change the button's text
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">event args</param>
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

        /// <summary>
        /// Unregister the keys when the window is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // change back the audio devices
            AudioConfig.ResetAudioDevices();

            // unregister keys
            for (int i = 1; i <= 9; i++)
            {
                _console.WriteLine("Unregistering CTRL+ALT+" + i);
                if (!Hotkeys[i].Unregiser()) MessageBox.Show("Hotkey failed to unregister!");
            }
        }

        /// <summary>
        /// Close the app when the exit button is clicked
        /// </summary>
        /// <param name="sender">Object Sender</param>
        /// <param name="e">Event args</param>
        private void button_exit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Toggles whether the buttons will assign or unassign audio files
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event args</param>
        private void button_unassign_Click(object sender, EventArgs e)
        {
            _unassign = (_unassign) ? false : true;
        }

        # region Audio Button Assignment Functions

        /// <summary>
        /// Set or unassign the audio file for hotkey 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_setSound1_Click(object sender, EventArgs e)
        {
            label_sound1.Text = ghk1.assignAudio(1);
        }

        /// <summary>
        /// Set or unassign the audio file for hotkey 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_setSound2_Click(object sender, EventArgs e)
        {
            label_sound2.Text = ghk2.assignAudio(2);
        }

        /// <summary>
        /// Set or unassign the audio file for hotkey 3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_setSound3_Click(object sender, EventArgs e)
        {
            label_sound3.Text = ghk3.assignAudio(3);
        }

        /// <summary>
        /// Set or unassign the audio file for hotkey 4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_setSound4_Click(object sender, EventArgs e)
        {
            label_sound4.Text = ghk4.assignAudio(4);
        }

        /// <summary>
        /// Set or unassign the audio file for hotkey 5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_setSound5_Click(object sender, EventArgs e)
        {
            label_sound5.Text = ghk5.assignAudio(5);
        }

        /// <summary>
        /// Set or unassign the audio file for hotkey 6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_setSound6_Click(object sender, EventArgs e)
        {
            label_sound6.Text = ghk6.assignAudio(6);
        }

        /// <summary>
        /// Set or unassign the audio file for hotkey 7
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_setSound7_Click(object sender, EventArgs e)
        {
            label_sound7.Text = ghk7.assignAudio(7);
        }

        /// <summary>
        /// Set or unassign the audio file for hotkey 8
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_setSound8_Click(object sender, EventArgs e)
        {
            label_sound8.Text = ghk8.assignAudio(8);
        }

        /// <summary>
        /// Set or unassign the audio file for hotkey 9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_setSound9_Click(object sender, EventArgs e)
        {
            label_sound9.Text = ghk9.assignAudio(9);
        }
        # endregion

    }
}