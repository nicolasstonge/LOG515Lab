using System.Collections;
using System.Collections.Generic;
using HardShellStudios.InputManagerV2;
using UnityEngine;

public static class hardInput
{

    /// <summary>
    /// Turns "true" when the key is pressed once.
    /// </summary>
    public static bool GetKeyDown(string KeyName)
    {
        return hardManager.Current().GetKeyDown(KeyName);
    }

    /// <summary>
    /// Turns "true" when the key is lifted.
    /// </summary>
    public static bool GetKeyUp(string KeyName)
    {
        return hardManager.Current().GetKeyUp(KeyName);
    }

    /// <summary>
    /// Turns "true" when the key is held down.
    /// </summary>
    public static bool GetKey(string KeyName)
    {
        return hardManager.Current().GetKey(KeyName);
    }

    /// <summary>
    /// Will either return the delta of an axis from KeyAixs, or will smooth a key between 0 (not held) and 1 (held down)
    /// </summary>
    public static float GetAxis(string KeyName)
    {
        return hardManager.Current().GetAxis(KeyName);
    }

    /// <summary>
    /// Will either return the delta of an axis from KeyAixs, or will smooth a key between 0 (not held) and 1 (held down)
    /// </summary>
    public static float GetAxis(string KeyName, string KeyName2)
    {
        return hardManager.Current().GetAxis(KeyName, KeyName2);
    }

    /// <summary>
    /// Set the binding for a key.
    /// </summary>
    public static void SetKey(string KeyName, object keyCode, bool UseThePrimaryKey = true)
    {
        if (UseThePrimaryKey)
            hardManager.Current().SetKeyPrimary(KeyName, keyCode);
        else
            hardManager.Current().SetKeySecondary(KeyName, keyCode);
    }

    /// <summary>
    /// Set the binding for a key.
    /// </summary>
    public static void SetKey(string KeyName, KeyAxis keyAxis, bool UseThePrimaryKey = true)
    {
        if (UseThePrimaryKey)
            hardManager.Current().SetKeyPrimary(KeyName, keyAxis);
        else
            hardManager.Current().SetKeySecondary(KeyName, keyAxis);
    }

    /// <summary>
    /// Set the binding for a key.
    /// </summary>
    public static void SetKey(string KeyName, KeyController keyAxis, bool UseThePrimaryKey = true)
    {
        if (UseThePrimaryKey)
            hardManager.Current().SetKeyPrimary(KeyName, keyAxis);
        else
            hardManager.Current().SetKeySecondary(KeyName, keyAxis);
    }

    /// <summary>
    /// Set a bindings sensitivity scale
    /// </summary>
    public static void Sensitivity(string KeyName, float Sensitivity)
    {
        hardManager.Current().SetKeySensitivity(KeyName, Sensitivity);
    }

    /// <summary>
    /// Get a bindings sensitivity
    /// </summary>
    public static float Sensitivity(string KeyName)
    {
        return hardManager.Current().Find(KeyName).Sensitivity;
    }

    /// <summary>
    /// Turns "true" when any key is held
    /// </summary>
    public static bool AnyKey()
    {
        return hardManager.Current().AnyKey();
    }

    /// <summary>
    /// Will return the KeyCode of any key that is pressed down
    /// </summary>
    public static KeyCode AnyKeyKEY()
    {
        return hardManager.Current().AnyKeyGET();
    }

    /// <summary>
    /// Will return the KeyController of any controler key or axis that is used
    /// </summary>
    public static KeyController AnyKeyCONTROLLER()
    {
        return hardManager.Current().AnyControllerGET();
    }

    /// <summary>
    /// Returns a prettier, formatted string for display in game to show what button a key is bound to.
    /// </summary>
    public static string GetKeyName(string KeyCode, bool GetPrimaryKey = true)
    {
        return hardUtility.MakeKeycodePretty(KeyCode, GetPrimaryKey);
    }

    /// <summary>
    /// This will format any keynames put in to the specified keys and return as a string.
    /// </summary>
    public static string FormatText(string str)
    {
        return hardUtility.FormatText(str);
    }

    /// <summary>
    /// Get all current bindings in the form of a list.
    /// </summary>
    public static HardKey[] GetAllBindings()
    {
        return hardManager.Current().GetAllBindings();
    }

    /// <summary>
    /// Reset's all the current bindings to their default values.
    /// </summary>
    public static void ResetAll()
    {
        hardManager.Current().ResetAll();
    }

    /// <summary>
    /// Reset's the given binding to it's default values.
    /// </summary>
    public static void ResetKey(string keyName)
    {
        hardManager.Current().ResetKey(keyName);
    }

    /// <summary>
    /// Get the mouse position in the world with relation to the main game camera.
    /// </summary>
    public static Vector3 MousePositionWorld()
    {
        return hardManager.Current().GetMouseWorldPosition();
    }

    /// <summary>
    /// Get the mouse position in the world with relation to the main game camera. Z = 0.
    /// </summary>
    public static Vector3 MousePositionWorld2D()
    {
        return hardManager.Current().GetMouseWorldPosition(true);
    }
}