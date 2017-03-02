using GlobalHotkeyConsole;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

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
        /// Class reference of the output console
        /// </summary>
        private HotkeyConsole _console = HotkeyConsole.GetInstance();

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
            hWnd = form.Handle;
            id = GetHashCode();
            audioPath = string.Empty;
        }

        /// <summary>
        /// Checks if the given key already has an audio file associated with it
        /// </summary>
        /// <param name="ghk"></param>
        /// <returns>True if there is assigned audio, false otherwise</returns>
        public bool HasAudio()
        {
            return (audioPath != string.Empty) ? true : false;
        }

        /// <summary>
        /// Determines if there is audio to be played, and plays it through the speakers/headphones and microphone
        /// </summary>
        /// <param name="_keyId">A number 1 - 9 denoting which key was pressed</param>
        public void PlayAudio(int keyId)
        {
            if (HasAudio())
            {
                // play the audio locally, maybe using System.Media.SoundPlayer
                // pipe the audio into the microphone
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.SoundLocation = audioPath;
                player.Load();
                player.Play();
                _console.WriteLine("Audio played locally");
            }
            else
            {
                _console.WriteLine("Hotkey " + keyId + " has no audio assigned.");
                assignAudio(keyId);
                // get an audio file using OpenFileDialog
                // copy audio file to AppData
                // assign new path to this.audioPath
                // add this new mapping to a config file
                // rename the appropriate label to the audio file name
            }
        }

        /// <summary>
        /// Gets audio for the specified unassigned key and assigns it.
        /// </summary>
        /// <param name="keyId">A number 1 - 9 denoting which key was pressed</param>
        /// <returns>String to set that button's label to, so the user can see what each button is set to</returns>
        public string assignAudio(int keyId)
        {
            OpenFileDialog dialogGetAudioFilePath = new OpenFileDialog();
            dialogGetAudioFilePath.InitialDirectory = @"C:\";
            dialogGetAudioFilePath.Filter = "Audio Files (.wav,.mp3)|*.wav;*.mp3";
            string fullFileName = string.Empty;
            string appdataAudioPath = string.Empty;
            string fileNameWithExt = string.Empty;

            //get the audio file
            try
            {
                _console.WriteLine("Assigning audio...");
                if (dialogGetAudioFilePath.ShowDialog() == DialogResult.OK)
                {
                    fullFileName = dialogGetAudioFilePath.FileName;
                    fileNameWithExt = Path.GetFileName(fullFileName);

                    //store a copy in AppData and rename it to <keynum>.wav
                    if (File.Exists(Path.Combine(Application.UserAppDataPath, fileNameWithExt)))
                    {
                        // TODO - if current config file already has this audio file in it, throw file exists error
                        appdataAudioPath = Path.Combine(Application.UserAppDataPath, fileNameWithExt);
                    }
                    else
                    {
                        appdataAudioPath = Path.Combine(Application.UserAppDataPath, fileNameWithExt);
                        File.Copy(fullFileName, appdataAudioPath);
                        _console.WriteLine("Copied audio to: " + appdataAudioPath);
                    }

                }
                else
                {
                    return Soundboard.buttonLabels[keyId].Text;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Audio assignment failed!!");
                fileNameWithExt = "Unassigned";
                _console.WriteLine("File copy failed! Exception Thown:\n");
                _console.WriteLine(e.Message);
            }

            //update the config file
            try
            {
                AudioConfig.updateAudioConfig(keyId, appdataAudioPath);
            }
            catch (Exception e)
            {
                _console.WriteLine("Config file update failed with exception:");
                _console.WriteLine(e.Message);
            }

            //assign the audio to the hotkey
            try
            {
                // assign the audio
                audioPath = appdataAudioPath;
                _console.WriteLine("Audio Assigned!");
            }
            catch (Exception e)
            {
                _console.WriteLine("Audio assignment failed with exception:");
                _console.WriteLine(e.Message);
            }
            

            return fileNameWithExt;
        }

        /// <summary>
        /// Gets audio for the specified unassigned key and assigns it.
        /// </summary>
        /// <param name="keyId">A number 1 - 9 denoting which key was pressed</param>
        /// <param name="audioFilePath">The audio path taken from the config file that this key should be assigned to</param>
        /// <returns>String to set that button's label to, so the user can see what each button is set to</returns>
        public string assignAudio(int keyId, string audioFilePath)
        {
            string fileNameWithExt = string.Empty;

            try
            {
                _console.WriteLine("Assigning audio from config...");
                fileNameWithExt = Path.GetFileName(audioFilePath);

                // assign the audio
                audioPath = Path.Combine(Application.UserAppDataPath, Path.GetFileName(audioFilePath)); ;
                _console.WriteLine("Audio Assigned!");
            }
            catch (Exception e)
            {
                MessageBox.Show("This audio file is already assigned!");
                fileNameWithExt = "Unassigned";
                _console.WriteLine("File copy failed! Exception Thown:\n");
                _console.WriteLine(e.Message);
            }

            return fileNameWithExt;
        }

        /// <summary>
        /// Unassign an audio file from a key
        /// </summary>
        /// <param name="keyId"></param>
        public void unassignAudio(int keyId)
        {
            // TODO - Unassignment of audio
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