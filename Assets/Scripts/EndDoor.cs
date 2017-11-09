using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour {

    public AudioClip doorLockedSound;
    public AudioClip doorOpenSound;
    private AudioSource source;


    // Use this for initialization
    void Start () {

        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (GameObject.Find("Character").GetComponent<Character>().key == true)
            {
                GameObject.Find("finish_door").GetComponent<Animation>().Play();
                source.clip = doorOpenSound;
                source.Play();
            }

            else
            {
                source.clip = doorLockedSound;
                source.Play();
            }
        }
    }
}
