using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Labyrinth labyrinthPref;

    private Labyrinth labyrinthInstance;

	// Use this for initialization
	void Start ()
    {
        play();
	}
	
	// Update is called once per frame
	void Update ()
    {
		// to do restart new labyrinth
	}

    void play()
    {
        labyrinthInstance = Instantiate(labyrinthPref) as Labyrinth;
        labyrinthInstance.CreateLabyrinth();
    }
}
