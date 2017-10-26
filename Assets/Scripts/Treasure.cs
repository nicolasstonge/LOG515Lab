using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    private GameController gameControler;

    // Use this for initialization
    void Start()
    {
        gameControler = GameObject.Find("GameController").GetComponent <GameController> ();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        gameControler.stop();
    }
}
