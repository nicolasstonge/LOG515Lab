using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum KeyAxis
{
    None = 0,
    MouseX = 1,
    MouseY = 2,
    ScrollWheel = 3,
    ScrollWheelUp = 4,
    ScrollWheelDown = 5
};

public enum KeyController
{
    None = 0,
    LeftStickUp,
    LeftStickDown,
    LeftStickLeft,
    LeftStickRight,

    LeftStickX,
    LeftStickY,

    RightStickUp,
    RightStickDown,
    RightStickLeft,
    RightStickRight,

    RightStickX,
    RightStickY,

    DPadUp,
    DPadDown,
    DPadLeft,
    DPadRight,
    DPadX,
    DPadY,

    LeftTrigger,
    RightTrigger,

    A = 330,
    B = 331,
    X = 332,
    Y = 333,
    LeftBumper = 334,
    RightBumper = 335,
    Back = 336,
    Start = 337,
    LeftStickIn = 338,
    RightStickIn = 339,
};

namespace HardShellStudios.InputManagerV2
{

    /// <summary>
    /// The main struct which interfaces the XML to the code.
    /// </summary>
    [Serializable]
    public class HardKey
    {

        public int ID;
        public string Name;
        Type[] _Types = new Type[] { typeof(KeyCode), typeof(KeyCode) };
        KeyCode[] _Keys = new KeyCode[2];
        KeyAxis[] _Axis = new KeyAxis[2];
        KeyController[] _Controller = new KeyController[2];


        public object Primary
        {
            set
            {
                if (_Types[0] != null)
                {
                    if (_Types[0].Equals(typeof(KeyCode)))
                        _Keys[0] = KeyCode.None;
                    else if (_Types[0].Equals(typeof(KeyAxis)))
                        _Axis[0] = KeyAxis.None;
                    else if (_Types[0].Equals(typeof(KeyController)))
                        _Controller[0] = KeyController.None;
                }

                _Types[0] = value.GetType();

                if (_Types[0].Equals(typeof(KeyCode)))
                    _Keys[0] = (KeyCode)value;
                else if (_Types[0].Equals(typeof(KeyAxis)))
                    _Axis[0] = (KeyAxis)value;
                else if (_Types[0].Equals(typeof(KeyController)))
                    _Controller[0] = (KeyController)value;
            }
            get
            {
                if (_Types[0].Equals(typeof(KeyCode)))
                    return _Keys[0];
                else if (_Types[0].Equals(typeof(KeyAxis)))
                    return _Axis[0];
                else
                    return _Controller[0];
            }
        }

        public object Secondary
        {
            set
            {
                if (_Types[1] != null)
                {
                    if (_Types[1].Equals(typeof(KeyCode)))
                        _Keys[1] = KeyCode.None;
                    else if (_Types[0].Equals(typeof(KeyAxis)))
                        _Axis[1] = KeyAxis.None;
                    else if (_Types[0].Equals(typeof(KeyController)))
                        _Controller[1] = KeyController.None;
                }

                _Types[1] = value.GetType();

                if (_Types[1].Equals(typeof(KeyCode)))
                    _Keys[1] = (KeyCode)value;
                else if (_Types[1].Equals(typeof(KeyAxis)))
                    _Axis[1] = (KeyAxis)value;
                else if (_Types[1].Equals(typeof(KeyController)))
                    _Controller[1] = (KeyController)value;
            }
            get
            {
                if (_Types[1].Equals(typeof(KeyCode)))
                    return _Keys[1];
                else if (_Types[1].Equals(typeof(KeyAxis)))
                    return _Axis[1];
                else
                    return _Controller[1];
            }
        }

        public Type PrimaryType { get { return _Types[0]; } }
        public KeyCode PrimaryKey { get { return _Keys[0]; } }
        public KeyAxis PrimaryAxis { get { return _Axis[0]; } }
        public KeyController PrimaryCont { get { return _Controller[0]; } }

        public Type SecondaryType { get { return _Types[1]; } }
        public KeyCode SecondaryKey { get { return _Keys[1]; } }
        public KeyAxis SecondaryAxis { get { return _Axis[1]; } }
        public KeyController SecondaryCont { get { return _Controller[1]; } }

        public bool CheckPrimary(Type obj) { return _Types[0].Equals(obj); }
        public bool CheckSecondary(Type obj) { return _Types[1].Equals(obj); }

        public bool isPrimaryControllerAxis { get { return (int)_Controller[0] < 350; } }
        public bool isSecondaryControllerAxis { get { return (int)_Controller[1] < 350; } }

        public float Sensitivity;

    }
}