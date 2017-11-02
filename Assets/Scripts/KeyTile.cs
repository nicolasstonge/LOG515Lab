using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTile : MonoBehaviour {

    public GoldKey keyObject;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateKeyCollider()
    {
        keyObject.gameObject.SetActive(true);
    }
}
