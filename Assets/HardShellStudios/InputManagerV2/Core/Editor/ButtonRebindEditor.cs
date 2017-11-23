using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(ButtonRebind))]
public class ButtonRebindEditor : Editor
{

    ButtonRebind script;

    private void OnEnable()
    {
        script = (ButtonRebind)target;
    }

    public override void OnInspectorGUI()
    {
        script.KeyName = EditorGUILayout.TextField(new GUIContent("Key Name","The key you want this to target."), script.KeyName);
        script.UseSecondary = EditorGUILayout.Toggle(new GUIContent("Use Secondary Key?", "TRUE = Primary, FALSE = Secondary."), script.UseSecondary);
        script.rebindType = (ButtonRebind.RebindType)EditorGUILayout.EnumPopup(new GUIContent("Button Type", "Do you want this button to rebind or reset?"), script.rebindType);
        if (script.rebindType == ButtonRebind.RebindType.Rebind)
        {
            script.AutoText = EditorGUILayout.Toggle(new GUIContent("Auto Format Text?", "TRUE = Yes, FALSE = No"), script.AutoText);
            script.TextToFormat = (Text)EditorGUILayout.ObjectField("Text Element", script.TextToFormat, typeof(Text), true);
        }
    }
}