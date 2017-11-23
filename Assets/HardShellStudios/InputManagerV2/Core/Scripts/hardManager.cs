using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System;

namespace HardShellStudios.InputManagerV2
{

    public class hardManager {
        /// <summary>
        /// Singleton to allow access from anywhere.
        /// </summary>
		static hardManager current;
        static bool ForceDefaultBindings = false;

        /// <summary>
        /// Holds all the current bindings.
        /// </summary>
        public HardKey[] Bindings;
        public HardKey[] BackupBindings;

        /// <summary>
        /// This holds the realtime values of the input manager
        /// </summary>
        static int ControllerOffest = 0;
        static float[,] BindingValues;

        /// <summary>
        /// Singleton of the current input manager
        /// </summary>
        public static hardManager Current()
        {
            if (current == null)
            {
                current = new hardManager();
                current.Setup();
            }
            return current;
        }

        public void Setup()
        {
            if (PlayerPrefs.GetInt("HARDINPUT-FORCEDEFAULT", 0) == 1)
            {
                //Debug.Log("Hard Shell Input: Forcing default bindings.");
                ForceDefaultBindings = true;
            }

            if (File.Exists(Application.persistentDataPath + "/" + hardUtility.BindingsSave) && !ForceDefaultBindings)
            {
                try
                {
                    List<HardKey> keys = hardUtility.ParseXML(Application.persistentDataPath + "/" + hardUtility.BindingsSave);
                    List<HardKey> defaultKeys = hardUtility.LoadDefaults();
                    List<HardKey> returnKeys = new List<HardKey>();
                    foreach (HardKey keyDef in defaultKeys)
                    {
                        int state = 0;
                        foreach (HardKey key in keys)
                        {
                            if (key.Name.Equals(keyDef.Name)) { state = 1; returnKeys.Add(key); }
                        }

                        if (state == 0)
                        {
                            Debug.LogWarning("Hard Shell Input: User missing key '" + keyDef.Name + "'. Added to bindings.");
                            returnKeys.Add(keyDef);
                        }
                    }
                    Bindings = returnKeys.ToArray();
                }
                catch
                {
                    Debug.LogWarning("Hard Shell Input: Error loading saved bindings. Loading defaults.");
                    Bindings = hardUtility.LoadDefaults().ToArray();
                }
            }
            else
            {
                Bindings = hardUtility.LoadDefaults().ToArray();
            }
            BindingValues = new float[Bindings.Length, 3];
            BackupBindings = hardUtility.LoadDefaults().ToArray();
            SaveXML();

            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                if (Input.GetJoystickNames()[i].Contains("XBOX"))
                {
                    ControllerOffest = (20 * i) + 20;
                    break;
                }
            }
        }

        /// <summary>
        /// DOES NOTHING - Gets called when the script is init.
        /// </summary>
        public hardManager() {
            // Manager Created.
        }
        

        /// <summary>
        /// DONT USE UNLESS YOU KNOW WHAT YOU'RE DOING. MANAGER AUTO SAVES INPUTS DONT BOTHER WITH THIS OKEY..
        /// </summary>
        public void SaveXML()
        {
            hardUtility.SaveXML(Bindings, Application.persistentDataPath + "/" + hardUtility.BindingsSave);
        }

        public void ResetAll()
        {
            Debug.Log("Key: " + BackupBindings[0].PrimaryKey);
            Bindings = hardUtility.CopyKeys(BackupBindings);
            SaveXML();
        }

        public void ResetKey(string keyName)
        {
            for (int i = 0; i < Bindings.Length; i++)
            {
                if (Bindings[i].Name == keyName)
                {
                    Bindings[i] = hardUtility.CopyKey(FindBackup(keyName));
                }
            }
            SaveXML();
        }

        /// <summary>
        /// Finds a key with the given name
        /// </summary>
        public HardKey Find(string KeyName)
        {
            HardKey key = null;
            for (int i = 0; i < Bindings.Length; i++)
            {
                if (Bindings[i].Name == KeyName)
                {
                    return Bindings[i];
                }
            }

            if (key == null)
            {
                Debug.LogWarning(string.Format("Hard Shell Input: Key '{0}' was not found in the current dictionary.", KeyName));
                Bindings = hardUtility.LoadDefaults().ToArray();
                BindingValues = new float[Bindings.Length, 3];
                for (int i = 0; i < Bindings.Length; i++)
                {
                    if (Bindings[i].Name == KeyName)
                    {
                        return Bindings[i];
                    }
                }
                return null;
            }
            else
                throw new System.Exception(string.Format("Key '{0}' is not held within the dictionary", KeyName)); // Basically if this happens its becuase you probably mis-spelled a key name or didnt add it into the XML file...

        }

        public HardKey FindBackup(string KeyName)
        {
            for (int i = 0; i < BackupBindings.Length; i++)
            {
                if (BackupBindings[i].Name == KeyName)
                {
                    return BackupBindings[i];
                }
            }

            return BackupBindings[0];
        }

        /// <summary>
        /// Set a bindings primary key
        /// </summary>
        public void SetKeyPrimary(string KeyName, object keyCode)
        {
            HardKey key = Find(KeyName);
            if ((int)(KeyCode)keyCode >= 330)
                key.Primary = (KeyController)keyCode;
            else
                key.Primary = (KeyCode)keyCode;
            SaveXML();
        }

        /// <summary>
        /// Set a bindings primary key
        /// </summary>
        public void SetKeyPrimary(string KeyName, KeyController keyCode)
        {
            HardKey key = Find(KeyName);
            key.Primary = keyCode;
            SaveXML();
        }

        /// <summary>
        /// Set a bindings primary key
        /// </summary>
        public void SetKeyPrimary(string KeyName, KeyAxis keyAxis)
        {
            HardKey key = Find(KeyName);
            key.Primary = keyAxis;
            SaveXML();
        }

        /// <summary>
        /// Set a bindings secondary key
        /// </summary>
        public void SetKeySecondary(string KeyName, object keyCode)
        {
            HardKey key = Find(KeyName);
            if ((int)(KeyCode)keyCode >= 330)
                key.Secondary = (KeyController)keyCode;
            else
                key.Secondary = (KeyCode)keyCode;
            SaveXML();
        }

        /// <summary>
        /// Set a bindings primary key
        /// </summary>
        public void SetKeySecondary(string KeyName, KeyController keyAxis)
        {
            HardKey key = Find(KeyName);
            key.Secondary = keyAxis;
            SaveXML();
        }

        /// <summary>
        /// Set a bindings primary key
        /// </summary>
        public void SetKeySecondary(string KeyName, KeyAxis keyAxis)
        {
            HardKey key = Find(KeyName);
            key.Secondary = keyAxis;
            SaveXML();
        }

        /// <summary>
        /// Set a bindings sensitivity scaling
        /// </summary>
        public void SetKeySensitivity(string KeyName, float Sensitivity)
        {
            HardKey key = Find(KeyName);
            key.Sensitivity = Sensitivity;
            SaveXML();
        }

        /// <summary>
        /// Turns "true" when the key is pressed once.
        /// </summary>
        public bool GetKeyDown(string KeyName)
		{ 
            HardKey key = Find(KeyName);
            if (GetAxis(KeyName) > BindingValues[key.ID, 1])
            {
                BindingValues[key.ID, 2] = 1;
                return true;
            }
            else
               return false;
        }

        /// <summary>
        /// Turns "true" when the key is lifted.
        /// </summary>
        public bool GetKeyUp(string KeyName)
        {
            HardKey key = Find(KeyName);
            if (GetAxis(KeyName) < BindingValues[key.ID, 1])
            {
                BindingValues[key.ID, 2] = 0;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Turns "true" when the key is held down.
        /// </summary>
        public bool GetKey(string KeyName)
        {
            HardKey key = Find(KeyName);
            return (Input.GetKey(key.PrimaryKey) || Input.GetKey(key.SecondaryKey) || GetController(key) > 0 || GetAxis(key.PrimaryAxis) + GetAxis(key.SecondaryAxis) > 0);
        }

        float GetAxis(KeyAxis axis)
        {
            switch (axis)
            {
                default:
                    return 0;
                case KeyAxis.MouseX:
                    return Input.GetAxisRaw("Mouse X");
                case KeyAxis.MouseY:
                    return Input.GetAxisRaw("Mouse Y");
                case KeyAxis.ScrollWheel:
                    return Input.mouseScrollDelta.y;
                case KeyAxis.ScrollWheelUp:
                    return Mathf.Clamp(Input.mouseScrollDelta.y, 0, 10);
                case KeyAxis.ScrollWheelDown:
                    return Mathf.Clamp(0 - Input.mouseScrollDelta.y, 0, 10);
            }
        }

        float GetController(HardKey key)
        {
            return GetController(key.PrimaryCont) + GetController(key.SecondaryCont);
        }

        float GetController(KeyController axis)
        {
            switch (axis)
            {
                default:
                    return Input.GetKey((KeyCode)((int)axis + ControllerOffest)) ? 1 : 0;
                case KeyController.LeftStickX:
                    return Input.GetAxisRaw("HARD_JOY-LEFT_X");
                case KeyController.LeftStickY:
                    return Input.GetAxisRaw("HARD_JOY-LEFT_Y");
                case KeyController.RightStickX:
                    return Input.GetAxisRaw("HARD_JOY-RIGHT_X");
                case KeyController.RightStickY:
                    return Input.GetAxisRaw("HARD_JOY-RIGHT_Y");
                case KeyController.DPadX:
                    return Input.GetAxisRaw("HARD_JOY-DPAD_X");
                case KeyController.DPadY:
                    return Input.GetAxisRaw("HARD_JOY-DPAD_Y");
                case KeyController.LeftTrigger:
                    return Input.GetAxisRaw("HARD_JOY-TRIGG_L");
                case KeyController.RightTrigger:
                    return Input.GetAxisRaw("HARD_JOY-TRIGG_R");

                case KeyController.LeftStickUp:
                    return Mathf.Clamp(Input.GetAxisRaw("HARD_JOY-LEFT_Y"), 0, 1);
                case KeyController.LeftStickDown:
                    return Mathf.Clamp(0 - Input.GetAxisRaw("HARD_JOY-LEFT_Y"), 0, 1);
                case KeyController.LeftStickLeft:
                    return Mathf.Clamp(0 - Input.GetAxisRaw("HARD_JOY-LEFT_X"), 0, 1);
                case KeyController.LeftStickRight:
                    return Mathf.Clamp(Input.GetAxisRaw("HARD_JOY-LEFT_X"), 0, 1);

                case KeyController.RightStickUp:
                    return Mathf.Clamp(Input.GetAxisRaw("HARD_JOY-RIGHT_Y"), 0, 1);
                case KeyController.RightStickDown:
                    return Mathf.Clamp(0 - Input.GetAxisRaw("HARD_JOY-RIGHT_Y"), 0, 1);
                case KeyController.RightStickLeft:
                    return Mathf.Clamp(Input.GetAxisRaw("HARD_JOY-RIGHT_X"), 0, 1);
                case KeyController.RightStickRight:
                    return Mathf.Clamp(0 - Input.GetAxisRaw("HARD_JOY-RIGHT_X"), 0, 1);

                case KeyController.DPadUp:
                    return Mathf.Clamp(Input.GetAxisRaw("HARD_JOY-DPAD_Y"), 0, 1);
                case KeyController.DPadDown:
                    return Mathf.Clamp(0 - Input.GetAxisRaw("HARD_JOY-DPAD_Y"), 0, 1);
                case KeyController.DPadLeft:
                    return Mathf.Clamp(Input.GetAxisRaw("HARD_JOY-DPAD_X"), 0, 1);
                case KeyController.DPadRight:
                    return Mathf.Clamp(0 - Input.GetAxisRaw("HARD_JOY-DPAD_X"), 0, 1);
            }
        }

        int lastFrame = 0;
        float lastTime = 0;
        float difference = 0;
        float timeDifference
        {
            get
            {
                if (lastFrame < Time.frameCount)
                {
                    difference = Time.time - lastTime;
                    lastTime = Time.time;
                    lastFrame = Time.frameCount;
                    return difference;
                }
                else
                {
                    return difference;
                }

            }
            set
            {
                lastTime = value;
            }
        }

        /// <summary>
        /// Will either return the delta of an axis from KeyAixs, or will smooth a key between 0 (not held) and 1 (held down)
        /// </summary>
        public float GetAxis(string KeyName)
        {
            HardKey key = Find(KeyName);
            if (Time.frameCount > BindingValues[key.ID, 0])
            {
                BindingValues[key.ID, 1] = BindingValues[key.ID, 2];

                if (!key.CheckPrimary(typeof(KeyAxis)) && (KeyCode)key.Primary != KeyCode.None || !key.CheckSecondary(typeof(KeyAxis)) && (KeyCode)key.SecondaryAxis != KeyCode.None)
                {
                    BindingValues[key.ID, 2] = Mathf.Clamp(Mathf.MoveTowards(BindingValues[key.ID, 2], GetKey(KeyName) ? 1f : 0f, key.Sensitivity * timeDifference), -1, 1);
                }
                else
                {
                    BindingValues[key.ID, 2] = (GetAxis(key.PrimaryAxis) + GetAxis(key.SecondaryAxis) + (key.isPrimaryControllerAxis ? GetController(key.PrimaryCont) : 0f) + (key.isSecondaryControllerAxis ? GetController(key.SecondaryCont) : 0f)) * key.Sensitivity;
                }
            }
            else
            {
                return BindingValues[key.ID, 2];
            }

            BindingValues[key.ID, 0] = lastFrame;
            return BindingValues[key.ID, 2];
        }

        /// <summary>
        /// Will either return the delta of an axis from KeyAixs, or will smooth a key between 0 (not held) and 1 (held down)
        /// </summary>
        public float GetAxis(string KeyName, string KeyName2)
        {
             return GetAxis(KeyName) - GetAxis(KeyName2);
        }

        /// <summary>
        /// Turns "true" when any key is pressed.
        /// </summary>
        public bool AnyKeyDown()
        {
            return Input.anyKeyDown;
        }

        /// <summary>
        /// Turns "true" when any key is held
        /// </summary>
        public bool AnyKey()
        {
            return Input.anyKey;
        }

        /// <summary>
        /// Will return the KeyCode of any key that is held down
        /// </summary>
        public KeyCode AnyKeyGET()
        {
            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                    return kcode;
            }

            return KeyCode.None;
        }

        /// <summary>
        /// Will return the KeyCode of any key that is held down
        /// </summary>
        public KeyController AnyControllerGET()
        {
            foreach (KeyController kcode in System.Enum.GetValues(typeof(KeyController)))
            {
                if (GetController(kcode) > 0)
                    return kcode;
            }

            return KeyController.None;
        }

        /// <summary>
        /// Get all current bindings in the form of a list.
        /// </summary>
        public HardKey[] GetAllBindings()
        {
            return Bindings;
        }

        public Vector3 GetMouseWorldPosition(bool zToZero = false)
        {
            if (!zToZero)
                return Camera.main.ScreenToWorldPoint(Input.mousePosition);
            else
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                return new Vector3(position.x, position.y, 0);
            }
        }
    }
}
