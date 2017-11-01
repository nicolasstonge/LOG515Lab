using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldKey : MonoBehaviour {

    private GameController gameControler;
    private AudioSource audioSource;

    // Use this for initialization
    void Start () {

        gameControler = GameObject.Find("GameController").GetComponent<GameController>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        audioSource.Play();
        
    }
}
