using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Image heartConteiner1;
    public Image heartConteiner2;
    public Image heartConteiner3;
    public Image keySymbol;
    private bool blink = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void displayKeySymbol()
    {
        keySymbol.gameObject.SetActive(true);
    }

    public void setNumberHeart(int numberH)
    {
        switch (numberH)
        {
            case 0:
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break;
            case 1:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break;
            case 2:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(false);
                break;
            case 3:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    IEnumerator Blink()
    {
        Invoke("BlinkCountDown", 3);
        blink = true;
        int i = 0;

        while (blink)
        {
            if (i == 0)
            {
                heartConteiner1.color = new Color(255, 255, 255, 1);
                heartConteiner2.color = new Color(255, 255, 255, 1);
                heartConteiner3.color = new Color(255, 255, 255, 1);
                yield return new WaitForSeconds(0.25f);
                i = 1;

            }
            else
            {
                heartConteiner1.color = new Color(255, 0, 0, 1);
                heartConteiner2.color = new Color(255, 0, 0, 1);
                heartConteiner3.color = new Color(255, 0, 0, 1);

                yield return new WaitForSeconds(0.25f);
                i = 0;
            }
            
        }
        heartConteiner1.color = new Color(255, 255, 255, 1);
        heartConteiner2.color = new Color(255, 255, 255, 1);
        heartConteiner3.color = new Color(255, 255, 255, 1);


        StopAllCoroutines();
    }

    private void BlinkCountDown()
    {
        blink = false;
    }

    public void StartBlinking()
    {
        StopAllCoroutines();
        StartCoroutine("Blink");
    }

}
