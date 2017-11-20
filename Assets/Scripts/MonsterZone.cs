using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterZone : MonoBehaviour
{

    public Monster monster;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {

            monster.idle = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

            monster.idle = true;
        }
    }
}
