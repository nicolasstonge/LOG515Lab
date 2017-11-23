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
        Cursor.lockState = CursorLockMode.None;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void takeDamage()
    {
        life --;
        Hud hud = GameObject.Find("HUD").GetComponent<Hud>();
        hud.setNumberHeart(life);
        transform.GetComponent<AudioSource>().Play();

        if (life == 0)
        {
            GameObject.Find("GameController").GetComponent<GameController>().gameOver();
        }

        hud.StartBlinking();
        
    }
}
