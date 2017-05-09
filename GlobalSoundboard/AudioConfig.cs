using System;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using GlobalHotkeyConsole;
using Microsoft.Win32;
using System.Diagnostics;

namespace GlobalHotkey
{
    public class AudioConfig
    {
        /// <summary>
        /// XML that stores the key to audio file associations
        /// </summary>
        public static XDocument _config;

        /// <summary>
        /// Class reference of the output console
        /// </summary>
        public static readonly HotkeyConsole _console = HotkeyConsole.GetInstance();

        /// <summary>
        /// Writes audio assignments to the config file for future use
        /// </summary>
        public static void updateAudioConfig(int keyId, string fileNameWithExt)
        {
            XElement x = _config.Element("Soundboard").Elements("key").Single(key => (int)key.FirstAttribute == keyId);
            x.Value = fileNameWithExt;
            _config.Save(Path.Combine(Application.UserAppDataPath, "audioConfig.xml"));
        }

        /// <summary>
        /// Create a new audio config and initialize the xml structure
        /// </summary>
        public static void createAudioConfig()
        {
            _config = new XDocument(
                new XComment("Auto-generated configuration for Global Soundboard."),
                new XElement("Soundboard",
                    new XElement("key", new XAttribute("id", 1), string.Empty),
                    new XElement("key", new XAttribute("id", 2), string.Empty),
                    new XElement("key", new XAttribute("id", 3), string.Empty),
                    new XElement("key", new XAttribute("id", 4), string.Empty),
                    new XElement("key", new XAttribute("id", 5), string.Empty),
                    new XElement("key", new XAttribute("id", 6), string.Empty),
                    new XElement("key", new XAttribute("id", 7), string.Empty),
                    new XElement("key", new XAttribute("id", 8), string.Empty),
                    new XElement("key", new XAttribute("id", 9), string.Empty)
               )
            );
            // TODO - Test that this actually works
            _config.Save(Path.Combine(Application.UserAppDataPath, "audioConfig.xml"));
        }

        /// <summary>
        /// Load the existing configuration file and assign the saved values
        /// </summary>
        public static void loadConfig()
        {
            foreach (XElement x in _config.Root.Nodes())
            {
                if (x.Value != string.Empty)
                {
                    int curKey = System.Convert.ToInt32(x.FirstAttribute.Value);
                    string audioFilePath = x.Value;
                    Soundboard.buttonLabels[curKey].Text = Soundboard.Hotkeys[curKey].assignAudio(curKey, audioFilePath);
                }
            }
        }

        /// <summary>
        /// If the config file exists, load it, if not make a new one
        /// </summary>
        public static void checkLoadConfig()
        {
            if (File.Exists(Path.Combine(Application.UserAppDataPath, "audioConfig.xml")))
            {
                _config = XDocument.Load(Path.Combine(Application.UserAppDataPath, "audioConfig.xml"));
                loadConfig();
            }
            else
            {
                createAudioConfig();
            }
        }

        /// <summary>
        /// Check if the appropriate audio devices are installed, enabled, and set up for use
        /// </summary>
        public static void CheckSetupAudio()
        {
            // check if the VAC is installed
            bool virtualAudioInstalled = CheckInstalled();

            try
            {
                // install it if not
                if (!virtualAudioInstalled)
                {
                    var result = MessageBox.Show(@"A virtual audio cable must be installed to continue. Please follow the prompts on the following screens.", @"Install Virtual Audio", MessageBoxButtons.OKCancel);
                    _console.WriteLine("VAC not installed, promting for installation.");
                    if (result == DialogResult.OK)
                    {
                        // install
                        // TODO - make sure this acutally installs instead of cancelling the installer
                        _console.WriteLine("Launching VAC installer");
                        ProcessStartInfo start = new ProcessStartInfo
                        {
                            FileName = Path.Combine(Environment.CurrentDirectory, Environment.Is64BitOperatingSystem ?
                                @"../../Resources/VBCABLE_Driver_Pack43/VBCABLE_Setup_x64.exe" :
                                @"../../Resources/VBCABLE_Driver_Pack43/VBCABLE_Setup.exe")
                        };
                        int exitCode;

                        using (Process proc = Process.Start(start))
                        {
                            proc.WaitForExit();
                            exitCode = proc.ExitCode;
                        }

                        // double check installation was sucessfull 
                        virtualAudioInstalled = CheckInstalled();

                        // failure
                        if (!virtualAudioInstalled)
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        MessageBox.Show(@"No virtual audio cable is installed, the Soundboard cannot continue");
                        _console.WriteLine("User aborted installation of VAC, exiting.");
                        Application.Exit();
                    }
                }
            }
            catch (Exception e)
            {
                _console.WriteLine("VAC installer failed with exception: \n\n" + e.Message);
                MessageBox.Show(@"The virtual audio cable installation appears to have failed and the Soundboard cannot continue. If you are receiving this message in error, try rebooting your machine before launching the Soundboard again. Or, try manually installing the virtual audio cable from http://bit.ly/1puZds5", @"Error", MessageBoxButtons.OK);
                Application.Exit(); //TODO - this isn't exiting
            }
            

            // set up audio devices
            SetupAudioDevices();
            
        }

        /// <summary>
        /// Checks the registry to see if the provided VAC is installed
        /// </summary>
        /// <returns></returns>
        private static bool CheckInstalled()
        {
            var key = System.Environment.Is64BitOperatingSystem ? 
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall") :
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

            if (key == null)
                return false;

            return key.GetSubKeyNames()
                .Select(keyName => key.OpenSubKey(keyName))
                .Select(subkey => subkey.GetValue("DisplayName") as string)
                .Any(displayName => displayName != null && displayName.Contains("VB-Audio Virtual Cable"));
        }

        /// <summary>
        /// Set the default output and input device to CABLE input, to allow virtual audio cable mixing
        /// </summary>
        private static void SetupAudioDevices()
        {
            // save current input/output devices for later
            // set default output device CABLE Input
            // change default input device to match
            

        }

        /// <summary>
        /// Takes the audio devices that were in use before launch and resets the system to use those
        /// </summary>
        public static void ResetAudioDevices()
        {
            // take stored input/output devices and reset the sytem to those
        }
    }
}
