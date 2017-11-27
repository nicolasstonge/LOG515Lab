using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public bool key;
    public int life = 3;
    public Menu menu;

    // Use this for initialization
    void Start () {

        key = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage()
    {
        life --;
        Hud hud = GameObject.Find("HUD").GetComponent<Hud>();
        hud.setNumberHeart(life);
        transform.GetComponent<AudioSource>().Play();

        if (life == 0)
        {
            GameObject.Find("GameController").GetComponent<GameController>().GameOver();
        }

        hud.StartBlinking();
    }
}
