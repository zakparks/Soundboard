using GlobalHotkeyConsole;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace GlobalHotkey
{
    /// <summary>
    /// Object that is used to track the data for each _key
    /// </summary>
    public class GlobalHotkey
    {
        /// <summary>
        /// Modifyer keys that prepend the hotkey (control, alt, etc.)
        /// </summary>
        private readonly int _modifier;

        /// <summary>
        /// Hotkey value as defined at https://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx
        /// </summary>
        private readonly int _key;

        /// <summary>
        /// Not quite sure yet, i'm hacking this apart from other OSS and it wasn't documented
        /// </summary>
        private IntPtr _hWnd;

        /// <summary>
        /// Hash object ID used to track the object
        /// </summary>
        private readonly int _id;

        /// <summary>
        /// string path to the audio file for the hotkey
        /// </summary>
        private string _audioPath;

        private NAudio.Wave.WaveFileReader AudioFilePlaying;

        private NAudio.Wave.DirectSoundOut LocalOutput = new NAudio.Wave.DirectSoundOut();

        /// <summary>
        /// Class reference of the output console
        /// </summary>
        private readonly HotkeyConsole _console = HotkeyConsole.GetInstance();

        /// <summary>
        /// Constructs a basic hotkey
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="key"></param>
        /// <param name="form"></param>
        public GlobalHotkey(int modifier, Keys key, Form form)
        {
            this._modifier = modifier;
            this._key = (int)key;
            _hWnd = form.Handle;
            _id = GetHashCode();
            _audioPath = string.Empty;
        }

        /// <summary>
        /// Checks if the given _key already has an audio file associated with it
        /// </summary>
        /// <returns>True if there is assigned audio, false otherwise</returns>
        private bool HasAudio()
        {
            return _audioPath != string.Empty;
        }

        /// <summary>
        /// Determines if there is audio to be played, and plays it through the speakers/headphones and microphone
        /// </summary>
        /// <param name="_keyId">A number 1 - 9 denoting which _key was pressed</param>
        public void PlayAudio(int keyId)
        {
            // stop playback if the key is pressed while already playing its sound
            if (LocalOutput != null && LocalOutput.PlaybackState == NAudio.Wave.PlaybackState.Playing)
            {
                LocalOutput.Stop();
                return;
            }

            if (HasAudio())
            {
                // play the audio locally
                AudioFilePlaying = new NAudio.Wave.WaveFileReader(_audioPath);
                LocalOutput = new NAudio.Wave.DirectSoundOut();
                LocalOutput.Init(new NAudio.Wave.WaveChannel32(AudioFilePlaying));
                LocalOutput.Play();
                _console.WriteLine("Audio played locally");

                // pipe the audio into the microphone

            }
            else
            {
                _console.WriteLine("Hotkey " + keyId + " has no audio assigned.");
                assignAudio(keyId);
                // rename the appropriate label to the audio file name
            }
        }

        /// <summary>
        /// Gets audio for the specified unassigned _key and assigns it.
        /// </summary>
        /// <param name="keyId">A number 1 - 9 denoting which _key was pressed</param>
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
                AudioConfig.UpdateAudioConfig(keyId, appdataAudioPath);
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
                _audioPath = appdataAudioPath;
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
        /// Gets audio for the specified unassigned _key and assigns it.
        /// </summary>
        /// <param name="keyId">A number 1 - 9 denoting which _key was pressed</param>
        /// <param name="audioFilePath">The audio path taken from the config file that this _key should be assigned to</param>
        /// <returns>String to set that button's label to, so the user can see what each button is set to</returns>
        public string assignAudio(int keyId, string audioFilePath)
        {
            string fileNameWithExt = string.Empty;

            try
            {
                _console.WriteLine("Assigning audio from config...");
                fileNameWithExt = Path.GetFileName(audioFilePath);

                // assign the audio
                _audioPath = Path.Combine(Application.UserAppDataPath, Path.GetFileName(audioFilePath)); ;
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
        /// Unassign an audio file from a _key
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
            return RegisterHotKey(_hWnd, _id, _modifier, _key);
        }

        /// <summary>
        /// Unregisters a hotkey with Windows
        /// </summary>
        /// <returns>Success or failure</returns>
        public bool Unregiser()
        {
            return UnregisterHotKey(_hWnd, _id);
        }

        /// <summary>
        /// Gets the hash code for the _key
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _modifier ^ _key ^ _hWnd.ToInt32();
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}