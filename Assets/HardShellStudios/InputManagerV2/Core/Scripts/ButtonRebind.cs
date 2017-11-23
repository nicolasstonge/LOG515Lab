using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Hard Shell Studios/Input Manager/UI Button Rebind")]
public class ButtonRebind : MonoBehaviour {

    public enum RebindType {
        Rebind,
        Reset,
        ResetAll
    }

    public string KeyName;
    public bool UseSecondary = false;

    public RebindType rebindType = RebindType.Rebind; 

    public bool AutoText = true;
    public Text TextToFormat;
    string OriginalString;

    bool isBinding = false;

    private void Start()
    {
        if (!AutoText)
            OriginalString = TextToFormat.text;
        else
            OriginalString = string.Format("['{0}',{1}]", KeyName, !UseSecondary ? "P" : "S");
    }

    private void Update()
    {
        if (isBinding)
        {
            KeyCode key = hardInput.AnyKeyKEY();
            KeyController controller = hardInput.AnyKeyCONTROLLER();
            if (key != KeyCode.None)
            {
                if (key == KeyCode.Escape)
                {
                    hardInput.SetKey(KeyName, KeyCode.None, !UseSecondary);
                    StartCoroutine(waitToEnable());
                    isBinding = false;
                }
                else
                {
                    hardInput.SetKey(KeyName, key, !UseSecondary);
                    StartCoroutine(waitToEnable());
                    isBinding = false;
                }
            }
            else if (controller != KeyController.None)
            {
                hardInput.SetKey(KeyName, controller, !UseSecondary);
                StartCoroutine(waitToEnable());
                isBinding = false;
            }
            else if (Input.mouseScrollDelta.y > 0)
            {
                hardInput.SetKey(KeyName, KeyAxis.ScrollWheelUp, !UseSecondary);
                StartCoroutine(waitToEnable());
                isBinding = false;
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                hardInput.SetKey(KeyName, KeyAxis.ScrollWheelDown, !UseSecondary);
                StartCoroutine(waitToEnable());
                isBinding = false;
            }
        }
        else if(rebindType == RebindType.Rebind)
        {
            //GetComponent<Button>().interactable = true;
            TextToFormat.text = hardInput.FormatText(OriginalString);
        }
    }

    IEnumerator waitToEnable()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Button>().interactable = true;
    }

    public void RebindKey()
    {
        if (rebindType == RebindType.Rebind)
        {
            TextToFormat.text = "PRESS A KEY";
            isBinding = true;
            GetComponent<Button>().interactable = false;
        }
        else if (rebindType == RebindType.Reset)
            hardInput.ResetKey(KeyName);
        else if (rebindType == RebindType.ResetAll)
            hardInput.ResetAll();


    }

}
