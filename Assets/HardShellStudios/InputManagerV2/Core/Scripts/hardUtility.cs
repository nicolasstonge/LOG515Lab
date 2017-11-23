using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

namespace HardShellStudios.InputManagerV2
{
    public static class hardUtility
    {
        private static string[] SavePrefixs = new string[] { "Key", "Axis", "Controller" };
        public static string BindingsSave = "KeyBindings.xml";
        public static string DefaultBindings = "DefaultBindings";

        /// <summary>
        /// DONT USE UNLESS YOU KNOW WHAT YOU'RE DOING. MANAGER AUTO SAVES INPUTS DONT BOTHER WITH THIS OKEY..
        /// </summary>
        public static List<HardKey> LoadDefaults()
        {
            TextAsset textAsset = (TextAsset)Resources.Load("InputManager/" + DefaultBindings);
            using (var tw = new StreamWriter(Application.persistentDataPath + "/" + BindingsSave, false))
            {
                tw.WriteLine(textAsset.text);
                tw.Close();
            }
            //File.Create(Application.persistentDataPath + "/" + BindingsSave);
            //File.WriteAllText(Application.persistentDataPath + "/" + BindingsSave, textAsset.text);
            return ParseXML(Application.persistentDataPath + "/" + BindingsSave);

        }

        public static object CopyObject(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, obj);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (object)binaryFormatter.Deserialize(memoryStream);
            }
        }

        public static HardKey[] CopyKeys(HardKey[] list)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, list);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (HardKey[])binaryFormatter.Deserialize(memoryStream);
            }
        }

        public static HardKey CopyKey(HardKey list)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, list);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (HardKey)binaryFormatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// Parses a text file to save and load bindings between sessions.
        /// </summary>
        public static List<HardKey> ParseXML(string xml)
        {
            List<HardKey> keys = new List<HardKey>();
            XDocument document = XDocument.Load(xml);
            var Inputs = document.Descendants("Input");
            foreach (var Input in Inputs)
            {
                HardKey addKey = new HardKey();
                addKey.ID = keys.Count;
                addKey.Name = Input.Attribute("Name").Value;
                addKey.Sensitivity = float.Parse(Input.Attribute("Sensitivity").Value);
                string[] Primary = Input.Attribute("Primary").Value.Split('.');
                string[] Secondary = Input.Attribute("Secondary").Value.Split('.');

                try
                {
                    if (Primary[0] == "Key")
                        addKey.Primary = (KeyCode)System.Enum.Parse(typeof(KeyCode), Primary[1]);
                    else if (Primary[0] == "Axis")
                        addKey.Primary = (KeyAxis)System.Enum.Parse(typeof(KeyAxis), Primary[1]);
                    else if (Primary[0] == "Controller")
                        addKey.Primary = (KeyController)System.Enum.Parse(typeof(KeyController), Primary[1]);
                }
                catch { Debug.LogWarningFormat("Hard Shell Input: Failed to bind '{0}' with '{1}'. Check spelling of KeyCode names.", addKey.Name, Input.Attribute("Primary").Value); }

                try
                {
                    if (Secondary[0] == "Key")
                        addKey.Secondary = (KeyCode)System.Enum.Parse(typeof(KeyCode), Secondary[1]);
                    else if (Secondary[0] == "Axis")
                        addKey.Secondary = (KeyAxis)System.Enum.Parse(typeof(KeyAxis), Secondary[1]);
                    else if (Secondary[0] == "Controller")
                        addKey.Secondary = (KeyController)System.Enum.Parse(typeof(KeyController), Secondary[1]);
                }
                catch { Debug.LogWarningFormat("Hard Shell Input: Failed to bind '{0}' with '{1}'. Check spelling of KeyCode names.", addKey.Name, Input.Attribute("Secondary").Value); }

                keys.Add(addKey);
            }

            return keys;
        }

        /// <summary>
        /// DONT USE UNLESS YOU KNOW WHAT YOU'RE DOING. MANAGER AUTO SAVES INPUTS DONT BOTHER WITH THIS OKEY..
        /// </summary>
        public static void SaveXML(HardKey[] keys, string location)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = false;
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(location, settings);
            writer.WriteStartElement("KeyBindings");
            for (int i = 0; i < keys.Length; i++)
            {
                HardKey key = keys[i];
                writer.WriteStartElement("Input");
                writer.WriteAttributeString("Name", key.Name);

                string prefixprime = key.CheckPrimary(typeof(KeyCode)) ? SavePrefixs[0] : key.CheckPrimary(typeof(KeyAxis)) ? SavePrefixs[1] : SavePrefixs[2];
                string prefixsecon = key.CheckSecondary(typeof(KeyCode)) ? SavePrefixs[0] : key.CheckSecondary(typeof(KeyAxis)) ? SavePrefixs[1] : SavePrefixs[2];

                writer.WriteAttributeString("Primary", prefixprime + "." + key.Primary.ToString());
                writer.WriteAttributeString("Secondary", prefixsecon + "." + key.Secondary.ToString());

                writer.WriteAttributeString("Sensitivity", key.Sensitivity.ToString());
                writer.WriteEndElement();
            }

            writer.WriteFullEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// Returns a prettier, formatted string for display in game to show what button a key is bound to.
        /// </summary>
        public static string MakeKeycodePretty(string KeyName, bool GetPrimary = true)
        {
            HardKey key = hardManager.Current().Find(KeyName);
            string name = key.Primary.ToString();

            KeyCode keycode = key.PrimaryKey;
            KeyController keycontroller = key.PrimaryCont;
            KeyAxis keyaxis = key.PrimaryAxis;

            if (GetPrimary)
            {
                name = key.Primary.ToString();
            }
            else
            {
                name = key.Secondary.ToString();
                keycode = key.SecondaryKey;
                keycontroller = key.SecondaryCont;
                keyaxis = key.SecondaryAxis;
            }

            if (key.CheckPrimary(typeof(KeyCode)) && GetPrimary || key.CheckSecondary(typeof(KeyCode)) && !GetPrimary)
            {
                if (name.Contains("Alpha"))
                    return name.Replace("Alpha", "");
                else if (name.Contains("Keypad"))
                    return name.Replace("Keypad", "Keypad ");
                else if (name.Contains("Left"))
                    return name.Replace("Left", "Left ");
                else if (name.Contains("Right"))
                    return name.Replace("Right", "Right ");
                else if (name.Contains("Up"))
                    return name.Replace("Up", "Up ");
                else if (name.Contains("Down"))
                    return name.Replace("Down", "Down ");
                else if (name.Contains("Mouse0"))
                    return "Left Mouse";
                else if (name.Contains("Mouse1"))
                    return "Right Mouse";
                else if (name.Contains("Mouse2"))
                    return "Middle Mouse";
                else if (name.Contains("Mouse"))
                    return "Mouse " + name.Replace("Mouse", "");
            }
            else if (key.CheckPrimary(typeof(KeyAxis)) && GetPrimary || key.CheckSecondary(typeof(KeyAxis)) && !GetPrimary)
            {
                if (keyaxis == KeyAxis.MouseX)
                    return "Mouse X";
                else if (keyaxis == KeyAxis.MouseY)
                    return "Mouse Y";
                else if (keyaxis == KeyAxis.ScrollWheel)
                    return "Scroll Wheel";
                else if (keyaxis == KeyAxis.ScrollWheelDown)
                    return "Scroll Wheel Down";
                else if (keyaxis == KeyAxis.ScrollWheelUp)
                    return "Scroll Wheel Up";
            }
            else if (key.CheckPrimary(typeof(KeyController)) && GetPrimary || key.CheckSecondary(typeof(KeyController)) && !GetPrimary)
            {
                switch (keycontroller)
                {
                    case KeyController.LeftStickX:
                        return "Left Stick X";
                    case KeyController.LeftStickY:
                        return "Left Stick Y";
                    case KeyController.RightStickX:
                        return "Right Stick X";
                    case KeyController.RightStickY:
                        return "Right Stick Y";
                    case KeyController.DPadX:
                        return "D-PAD X";
                    case KeyController.DPadY:
                        return "D-PAD Y";
                    case KeyController.LeftTrigger:
                        return "Left Trigger";
                    case KeyController.RightTrigger:
                        return "Right Trigger";

                    case KeyController.LeftStickUp:
                        return "Left Stick Up";
                    case KeyController.LeftStickDown:
                        return "Left Stick Down";
                    case KeyController.LeftStickLeft:
                        return "Left Stick Left";
                    case KeyController.LeftStickRight:
                        return "Left Stick Right";

                    case KeyController.RightStickUp:
                        return "Right Stick Up";
                    case KeyController.RightStickDown:
                        return "Right Stick Down";
                    case KeyController.RightStickLeft:
                        return "Right Stick Left";
                    case KeyController.RightStickRight:
                        return "Right Stick Right";

                    case KeyController.DPadUp:
                        return "D-PAD Up";
                    case KeyController.DPadDown:
                        return "D-PAD Down";
                    case KeyController.DPadLeft:
                        return "D-PAD Left";
                    case KeyController.DPadRight:
                        return "D-PAD Right";
                }
            }

            return name;
        }

        /// <summary>
        /// This will format any keynames put in to the specified keys.
        /// </summary>
        public static string FormatText(string str)
        {
            string newString = str;
            string[] FirstSplit = str.Split('[');
            List<string> getThese = new List<string>();
            for (int i = 1; i < FirstSplit.Length; i++)
            {
                if (FirstSplit[i].Contains("'"))
                {
                    getThese.Add(FirstSplit[i].Split(']')[0].Replace("'", ""));
                }
            }
            foreach (string s in getThese)
            {
                string[] LastSplit = s.Split(',');
                newString = newString.Replace("['" + LastSplit[0] + "'," + LastSplit[1] + "]", MakeKeycodePretty(LastSplit[0], LastSplit[1].Contains("P")));
            }
            //newString = newString.Replace(, "");
            //newString = newString.Replace("P]", "");
            //newString = newString.Replace("\",S]", "");

            return newString;
        }
    }

}
