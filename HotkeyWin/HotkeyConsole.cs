using System;
using System.Windows.Forms;

namespace GlobalHotkeyConsole
{
    /// <summary>
    /// This form shows the console output from all actions that take place anywhere in the program
    /// </summary>
    public partial class HotkeyConsole : Form
    {
        /// <summary>
        /// Instance of the class
        /// </summary>
        private static HotkeyConsole _hotkeyConsoleInstance;

        /// <summary>
        /// Initializes the form, and hides it by default.
        /// </summary>
        public HotkeyConsole()
        {
            InitializeComponent();
            this.Hide();
        }

        /// <summary>
        /// Returns an instance of the class
        /// </summary>
        /// <returns>Console window instance</returns>
        public static HotkeyConsole GetInstance()
        {
            if (_hotkeyConsoleInstance == null)
            {
                _hotkeyConsoleInstance = new HotkeyConsole();
            }

            return _hotkeyConsoleInstance;
        }

        /// <summary>
        /// A global write function that outputs the desired string with a timestamp
        /// </summary>
        /// <param name="text">The general text to be output</param>
        public void WriteLine(string text)
        {
            string time = DateTime.Now.ToString("HH:mm:ss.ffff");

            listBox_console.Items.Add(time + " - " + text + Environment.NewLine);

            // keep the textbox scrolled down to the last item output
            listBox_console.TopIndex = listBox_console.Items.Count - 1;
        }

        /// <summary>
        /// In case the form is closed, hide it instead and discard the command to close it.
        /// This prevents any currently written text from being lost.
        /// </summary>
        /// <param name="sender">Object that requested the action</param>
        /// <param name="e">Event arguments to close the form</param>
        private void Console_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}