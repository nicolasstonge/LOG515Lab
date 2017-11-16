using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public float RotateSpeed = 1f;
    public float RotateRadius = 0.5f;
    private bool iFrame = false;

    private float _angle;
    Character player;

    public bool idle = true;
    public bool attacked = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Character").GetComponent<Character>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.LookAt(player.transform);

        if (idle & !attacked)
        {
            _angle += RotateSpeed * Time.deltaTime;

            var newPosition = new Vector3(transform.position.x + (Mathf.Sin(_angle) * RotateRadius), transform.position.y, transform.position.z + (Mathf.Cos(_angle) * RotateRadius));
            transform.position = newPosition;
        }
        if(!idle)
        {
            attacked = true;
            transform.Translate(Vector3.forward * 3 * Time.deltaTime);
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!iFrame)
        {
            if (other.tag == "Player")
            {
                iFrame = true;
                Invoke("resetInvulnerability", 3);
                GameObject.Find("Character").GetComponent<Character>().takeDamage();
            }
        }
    }

    private void resetInvulnerability()
    {
        iFrame = false;
    }
}
