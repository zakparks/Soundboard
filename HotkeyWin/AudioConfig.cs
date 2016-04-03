using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace GlobalHotkey
{
    public class AudioConfig
    {
        /// <summary>
        /// XML that stores the key to audio file associations
        /// </summary>
        public static XDocument _config;

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
    }
}
