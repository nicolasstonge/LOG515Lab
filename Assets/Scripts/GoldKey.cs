using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldKey : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
            GameObject key = GameObject.Find("Key");
            key.GetComponent<AudioSource>().Play();

            GameObject character = GameObject.Find("Character");
            character.GetComponent<Character>().key = true;
            transform.gameObject.SetActive(false);
          
    }
}
