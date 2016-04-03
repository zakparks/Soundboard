using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Media;

namespace GlobalHotkey
{
    /// <summary>
    /// Object that is used to track the data for each key
    /// </summary>
    public class GlobalHotkey
    {
        /// <summary>
        /// Modifyer keys that prepend the hotkey (control, alt, etc.)
        /// </summary>
        private int modifier;

        /// <summary>
        /// Hotkey value as defined at https://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx
        /// </summary>
        private int key;

        /// <summary>
        /// Not quite sure yet, i'm hacking this apart from other OSS and it wasn't documented
        /// </summary>
        private IntPtr hWnd;

        /// <summary>
        /// Hash object ID used to track the object
        /// </summary>
        private int id;

        /// <summary>
        /// string path to the audio file for the hotkey
        /// </summary>
        private string audioPath;

        /// <summary>
        /// Constructs a basic hotkey
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="key"></param>
        /// <param name="form"></param>
        public GlobalHotkey(int modifier, Keys key, Form form)
        {
            this.modifier = modifier;
            this.key = (int)key;
            this.hWnd = form.Handle;
            id = this.GetHashCode();
            audioPath = string.Empty;
        }

        /// <summary>
        /// Checks if the given key already has an audio file associated with it
        /// </summary>
        /// <param name="ghk"></param>
        /// <returns>True if there is assigned audio, false otherwise</returns>
        public bool HasAudio()
        {
            return (audioPath != String.Empty) ? true : false;
        }

        public void PlayAudio(int keyId)
        {
            if (this.HasAudio())
            {
                Console._console.WriteLine("Audio Played!");
            }
            else
            {
                Console.WriteLine("Hotkey 1 has no audio assigned.");
            }
        }

        /// <summary>
        /// Register a hotkey with Windows
        /// </summary>
        /// <returns>Success or failure</returns>
        public bool Register()
        {
            return RegisterHotKey(hWnd, id, modifier, key);
        }

        /// <summary>
        /// Unregisters a hotkey with Windows
        /// </summary>
        /// <returns>Success or failure</returns>
        public bool Unregiser()
        {
            return UnregisterHotKey(hWnd, id);
        }

        /// <summary>
        /// Gets the hash code for the key
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return modifier ^ key ^ hWnd.ToInt32();
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}