using System;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using GlobalHotkeyConsole;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;
using NAudio.Wave;

namespace GlobalHotkey
{
    public static class AudioConfig
    {
        /// <summary>
        /// XML that stores the key to audio file associations
        /// </summary>
        private static XDocument _config;

        /// <summary>
        /// Class reference of the output console
        /// </summary>
        public static readonly HotkeyConsole _console = HotkeyConsole.GetInstance();

        /// <summary>
        /// All of the machine's installed input devices (microphones, etc)
        /// </summary>
        public static List<WaveInCapabilities> InputSources = new List<WaveInCapabilities>();

        /// <summary>
        /// All of the machine's installed output devices (speakers, etc)
        /// </summary>
        public static List<WaveOutCapabilities> OutputSources = new List<WaveOutCapabilities>();

        /// <summary>
        /// Writes audio assignments to the config file for future use
        /// </summary>
        public static void UpdateAudioConfig(int keyId, string fileNameWithExt)
        {
            XElement x = _config.Element("Soundboard").Elements("key").Single(key => (int)key.FirstAttribute == keyId);
            x.Value = fileNameWithExt;
            _config.Save(Path.Combine(Application.UserAppDataPath, "audioConfig.xml"));
        }

        /// <summary>
        /// Create a new audio config and initialize the xml structure
        /// </summary>
        private static void CreateAudioConfig()
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
        public static void LoadConfig()
        {
            foreach (XElement x in _config.Root.Nodes())
            {
                if (x.Value != string.Empty)
                {
                    int curKey = Convert.ToInt32(x.FirstAttribute.Value);
                    string audioFilePath = x.Value;
                    Soundboard.buttonLabels[curKey].Text = Soundboard.Hotkeys[curKey].assignAudio(curKey, audioFilePath);
                }
            }
        }

        /// <summary>
        /// If the config file exists, load it, if not make a new one
        /// </summary>
        public static void CheckLoadConfig()
        {
            if (File.Exists(Path.Combine(Application.UserAppDataPath, "audioConfig.xml")))
            {
                _config = XDocument.Load(Path.Combine(Application.UserAppDataPath, "audioConfig.xml"));
                LoadConfig();
            }
            else
            {
                CreateAudioConfig();
            }
        }

        /// <summary>
        /// Check if the appropriate audio devices are installed, enabled, and set up for use
        /// </summary>
        public static void GetAudioDevices()
        {
            // get all the sound in devices, (microphones, etc)
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                InputSources.Add(WaveIn.GetCapabilities(i));
            }

            // get all the sound out devices, (speakers and headphones, etc)
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                OutputSources.Add(WaveOut.GetCapabilities(i));
            }             
        }
    }
}
