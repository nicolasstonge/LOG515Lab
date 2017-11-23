using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HardShellStudios.InputManagerV2
{
    public class hardInputEditor : EditorWindow
    {
        string version = "1.05 - 13.07.2017";
        static HardKey[] keys;
        static bool[] opened;
        static KeyType[,] keyType;
        Vector2 scrollPos;
        Color32[] colours = new Color32[] { new Color32(255, 153, 0, 255), new Color32(251, 214, 4, 255) };
        static bool wantsAnalysis = true;
        static bool autoSave = true;

        enum KeyType
        {
            Keypress = 0,
            Axis = 1,
            Controller = 2
        }

        [MenuItem("Window/Edit Inputs")]
        public static void StartEditWindow()
        {
            hardInputEditor editor = (hardInputEditor)EditorWindow.GetWindow(typeof(hardInputEditor));
            editor.minSize = new Vector2(500, 750);
            editor.LoadKeys();
        }

        static bool CheckInputAsset()
        {
            FileInfo currentAsset = new FileInfo(Application.dataPath.Replace("Assets", "ProjectSettings/InputManager.asset"));
            FileInfo assetFixFile = new FileInfo(PathToAssetFix());

            return (currentAsset.Length == assetFixFile.Length);
        }

        static void FixInputAsset()
        {
            File.Delete(Application.dataPath.Replace("Assets", "ProjectSettings/InputManager.asset"));
            File.Copy(PathToAssetFix(), Application.dataPath.Replace("Assets", "ProjectSettings/InputManager.asset"));
            AssetDatabase.Refresh();
            Debug.Log("Hard Input Manger: Fixed Asset file.");
        }

        static string PathToAssetFix()
        {
            string[] results = AssetDatabase.FindAssets("InputManagerAsset");
            if (results.Length != 0)
            {
                return AssetDatabase.GUIDToAssetPath(results[0]);
            }
            return null;
        }

        string GetDefaultPath()
        {
            string[] results = AssetDatabase.FindAssets(hardUtility.DefaultBindings);
            if (results.Length != 0)
            {
                string found = "no";

                foreach (string s in results)
                {
                    if (AssetDatabase.GUIDToAssetPath(s).Contains("InputManager"))
                        found = AssetDatabase.GUIDToAssetPath(s);
                }
                if (found != "no")
                    return found;
                else
                {
                    CreateDefaults();
                    throw new Exception("No 'DefaultBindings.xml' can't be found in a resource file... Creating one for you in Assets/Resources/InputManager/");
                }

            }
            else
            {
                CreateDefaults();
                throw new Exception("No 'DefaultBindings.xml' can't be found in a resource file... Creating one for you in Assets/Resources/InputManager/");
            }
        }

        void CreateDefaults()
        {
            if (!Directory.Exists(Application.dataPath + "/Resources/InputManager/"))
                Directory.CreateDirectory(Application.dataPath + "/Resources/InputManager/");
            if (!File.Exists(Application.dataPath + "/Resources/InputManager/" + hardUtility.DefaultBindings + ".xml"))
                File.Create(Application.dataPath + "/Resources/InputManager/" + hardUtility.DefaultBindings + ".xml").Dispose();
            HardKey key = new HardKey();
            key.Name = "New Input";
            key.Sensitivity = 1;
            keys = new HardKey[] { key };
            hardUtility.SaveXML(keys, Application.dataPath + "/Resources/InputManager/" + hardUtility.DefaultBindings + ".xml");
            RefreshLengthVars();
            AssetDatabase.Refresh();
        }

        public void Update()
        {
            Repaint();
        }

        void RefreshLengthVars()
        {
            if (opened != null)
            {
                bool[] wasOpened = opened;
                opened = new bool[keys.Length];
                for (int i = 0; i < wasOpened.Length && i < opened.Length; i++)
                {
                    opened[i] = wasOpened[i];
                }
            }
            else
            {
                opened = new bool[keys.Length];
            }

            keyType = new KeyType[keys.Length, 2];

            for (int i = 0; i < keys.Length; i++)
            {
                keyType[i, 0] = keys[i].CheckPrimary(typeof(KeyCode)) ? KeyType.Keypress : keys[i].CheckPrimary(typeof(KeyAxis)) ? KeyType.Axis : KeyType.Controller;
                keyType[i, 1] = keys[i].CheckSecondary(typeof(KeyCode)) ? KeyType.Keypress : keys[i].CheckSecondary(typeof(KeyAxis)) ? KeyType.Axis : KeyType.Controller;
            }
        }

        void LoadKeys()
        {
            keys = hardUtility.ParseXML(GetDefaultPath()).ToArray();
            RefreshLengthVars();
        }

        static bool CompareKeys(HardKey key1, HardKey key2)
        {
            return (key1.Name == key2.Name) && key1.Primary.Equals(key2.Primary) && key1.Sensitivity == key2.Sensitivity;
        }

        public void OnGUI()
        {
            if (keys == null)
            {
                LoadKeys();
            }

            GUIStyle backstyle = new GUIStyle();
            EditorGUILayout.BeginVertical(backstyle);
            GUILayout.Label("Inputs", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            PlayerPrefs.SetInt("HARDINPUT-FORCEDEFAULT", EditorGUILayout.ToggleLeft(new GUIContent("Force Defaults", "Toggle 'TRUE' and it will always load the default bindings in the editor."), PlayerPrefs.GetInt("HARDINPUT-FORCEDEFAULT", 0) == 1) ? 1 : 0);
            wantsAnalysis = EditorGUILayout.ToggleLeft(new GUIContent("Realtime Inputs", "Toggle 'TRUE' and the inspector window will show keypresses in runtime."), wantsAnalysis);
            autoSave = EditorGUILayout.ToggleLeft(new GUIContent("Autosave", "Toggle 'TRUE' and all changes will automatically be saved."), autoSave);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.Height(500));
            bool shouldSave = false;
            List<HardKey> test = new List<HardKey>();

            for (int i = 0; i < keys.Length; i++)
            {
                HardKey newKey = keys[i];
                HardKey oldKey = hardUtility.CopyKey(newKey);
                test.Add(newKey);

                // Colours and foldout
                GUIStyle style = new GUIStyle();
                style.normal.background = MakeTex(colours[i % 2]);
                EditorGUILayout.BeginVertical(style);

                GUIStyle titleStyle = new GUIStyle();
                titleStyle.fontStyle = FontStyle.Bold;
                titleStyle.margin = new RectOffset(10, 10, 10, 10);

                EditorStyles.boldLabel.normal.textColor = new Color32(0, 0, 0, 255);

                if (Application.isPlaying && wantsAnalysis)
                {
                    if (hardManager.Current().GetKey(newKey.Name))
                    {
                        titleStyle.normal.textColor = new Color32(0, 0, 0, 255);
                    }
                    else
                        titleStyle.normal.textColor = new Color32(0, 0, 0, 150);
                }

                if (opened == null)
                    LoadKeys();

                opened[i] = EditorGUILayout.Foldout(opened[i], (opened[i] ? "▼ " : "► ") + newKey.Name, true, titleStyle);

                // End of foldout and colouring
                EditorGUI.indentLevel++;
                if (opened != null && opened[i])
                {
                    newKey.Name = EditorGUILayout.TextField(new GUIContent("Name", "The name used to be referenced in code. E.g. GetKeyDown('Forward')"), newKey.Name);
                    newKey.Sensitivity = EditorGUILayout.Slider(new GUIContent("Sensitivity", "This effects how quickly a key or axis scales when using 'GetAxis()' it will not effect other input methods like 'GeyKey()' or 'GetKeyDown()'"), newKey.Sensitivity, 0, 100);

                    EditorGUILayout.LabelField("Primary", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;

                    keyType[i, 0] = (KeyType)EditorGUILayout.EnumPopup(new GUIContent("Input Type", "Select whether you want to check a mouse axis, or a keyboard input."), keyType[i, 0]);

                    if (keyType[i, 0] == KeyType.Keypress)
                    {
                        newKey.Primary = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Primary", "The main option used for a users input."), newKey.PrimaryKey);
                    }
                    else if (keyType[i, 0] == KeyType.Axis)
                    {
                        newKey.Primary = (KeyAxis)EditorGUILayout.EnumPopup(new GUIContent("Primary", "The main option used for a users input."), newKey.PrimaryAxis);
                    }
                    else if (keyType[i, 0] == KeyType.Controller)
                    {
                        newKey.Primary = (KeyController)EditorGUILayout.EnumPopup(new GUIContent("Primary", "The main option used for a users input."), newKey.PrimaryCont);
                    }

                    EditorGUI.indentLevel--;



                    EditorGUILayout.LabelField("Secondary", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;

                    keyType[i, 1] = (KeyType)EditorGUILayout.EnumPopup(new GUIContent("Input Type", "Select whether you want to check a mouse axis, or a keyboard input."), keyType[i, 1]);

                    if (keyType[i, 1] == KeyType.Keypress)
                    {
                        newKey.Secondary = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Secondary", "The secondary option used for a users input."), newKey.SecondaryKey);
                    }
                    else if (keyType[i, 1] == KeyType.Axis)
                    {
                        newKey.Secondary = (KeyAxis)EditorGUILayout.EnumPopup(new GUIContent("Secondary", "The secondary option used for a users input."), newKey.SecondaryAxis);
                    }
                    else if (keyType[i, 1] == KeyType.Controller)
                    {
                        newKey.Secondary = (KeyController)EditorGUILayout.EnumPopup(new GUIContent("Secondary", "The secondary option used for a users input."), newKey.SecondaryCont);
                    }

                    EditorGUI.indentLevel--;

                    if (oldKey.Name != newKey.Name || oldKey.Sensitivity != newKey.Sensitivity || oldKey.Primary.ToString() != newKey.Primary.ToString() || oldKey.Secondary.ToString() != newKey.Secondary.ToString())
                        shouldSave = true;

                    GUILayout.Space(20);

                    if (GUILayout.Button("Remove Key", EditorStyles.toolbarButton))
                    {
                        RemoveInput(i);
                    }

                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            };

            if (shouldSave && autoSave)
                Save();

            if (GUILayout.Button("Add Input", EditorStyles.toolbarButton)) { AddInput(); }
            if (!CheckInputAsset() && EditorPrefs.GetBool("HARDSHELL_IgnoreAssetError", false) == false)
            {
                backstyle.normal.background = MakeTex(new Color32(255, 50, 50, 255));
                EditorGUILayout.BeginVertical(backstyle);
                EditorGUILayout.LabelField("Your default Unity file 'InputManager.asset' is incorrectly configured.", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Please click the button below to fix this.");
                EditorGUILayout.LabelField("If you are still having errors then contact below for support.");
                EditorGUILayout.LabelField("(This will overwrite your default Unity inputs, but is required for controller support)");
                EditorGUILayout.LabelField("(If you modifiy the default unity inputs for whatever reason then this error will persist,");
                EditorGUILayout.LabelField("if everything run's fine then just press the ignore button below)");
                if (GUILayout.Button("Fix InputManager.asset", EditorStyles.toolbarButton)) { FixInputAsset(); }
                if (GUILayout.Button("Ignore error.", EditorStyles.toolbarButton)) { EditorPrefs.SetBool("HARDSHELL_IgnoreAssetError", true); }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();


            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
            {
                LoadKeys();
            }

            if (GUILayout.Button(new GUIContent("Save", "Save the current bindings."), EditorStyles.toolbarButton)) { Save(); Debug.Log("Hard Shell Input: Saved bindings!"); }

            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Reimport InputManager.asset", EditorStyles.toolbarButton)) { FixInputAsset(); }


            GUILayout.Space(20);
            GUILayout.Label("Created by Haydn Comley (Version: " + version + ")", EditorStyles.boldLabel);

            GUILayout.Label("Find more of me at:", EditorStyles.boldLabel);
            GUILayout.Label("www.hardshellstudios.com + www.haydncomley.com");

            GUILayout.Label("Email for help at:", EditorStyles.boldLabel);
            GUILayout.Label("haydn@haydncomley.com or haydncomley@gmail.com");

            GUILayout.Label("Tutorials can be found on YouTube:", EditorStyles.boldLabel);
            if (GUILayout.Button("Click Here or Link https://www.youtube.com/channel/UCgqEGyFYByfBFCoIWQrhGEg", EditorStyles.toolbarButton))
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCgqEGyFYByfBFCoIWQrhGEg");
            }
            EditorGUILayout.EndHorizontal();
        }

        void Save()
        {
            hardUtility.SaveXML(keys, GetDefaultPath());
            AssetDatabase.Refresh();
        }

        void AddInput()
        {
            Save();
            HardKey key = new HardKey();
            key.Name = "New Input";
            key.Sensitivity = 3;    
            HardKey[] keyArrayOld = keys;
            keys = new HardKey[keys.Length + 1];
            for (int i = 0; i < keys.Length; i++)
            {
                if (i < keys.Length - 1)
                {
                    keys[i] = keyArrayOld[i];
                }
                else
                {
                    keys[i] = key;
                }
            }

            RefreshLengthVars();
            Save();
            AssetDatabase.Refresh();
        }

        void RemoveInput(int key)
        {
            bool wantsToRemove = EditorUtility.DisplayDialog("Remove Input.", "Are you sure you want to delete '" + keys[key].Name + "'?", "Yes", "No");
            if (wantsToRemove)
            {
                Save();
                HardKey[] keyArrayOld = keys;
                keys = new HardKey[keys.Length - 1];
                int count = 0;
                for (int i = 0; i < keyArrayOld.Length; i++)
                {
                    if (i != key)
                    {
                        keys[count] = keyArrayOld[i];
                        count++;
                    }
                }
                Save();
                RefreshLengthVars();
            }
        }

        private Texture2D MakeTex(Color col)
        {
            Color[] pix = new Color[50 * 50];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(50, 50);
            result.SetPixels(pix);
            result.Apply();

            return result;

        }

    }


}