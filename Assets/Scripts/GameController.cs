using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Labyrinth labyrinthPref;
    public Menu menuPref;

    private Labyrinth labyrinthInstance;
    private Menu menuInstance;

	// Use this for initialization
	void Start ()
    {
        menuInstance = Instantiate(menuPref, transform) as Menu;
        menuInstance.name = "Menu";
    }
	
	// Update is called once per frame
	void Update ()
    {
		// to do restart new labyrinth
	}

    public void play(int sizeX, int sizeZ)
    {
        menuInstance.HideMenu();
        labyrinthInstance = Instantiate(labyrinthPref) as Labyrinth;
        labyrinthInstance.name = "Labyrinth";
        labyrinthInstance.CreateLabyrinth(sizeX, sizeZ);
    }

    public void stop()
    {
        menuInstance.ShowMenu();
        Destroy(labyrinthInstance.gameObject);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
