using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private bool iFrame = false;
    public bool idle = true;

    Character player;
    NavMeshAgent agent;
    GameObject[] wayPoints;

    System.Random rnd = new System.Random();

    int idleDestination;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Character").GetComponent<Character>();
        agent = GetComponent<NavMeshAgent>();
        wayPoints = GameObject.FindGameObjectsWithTag("Waypoint");
        idleDestination = GetRandomWayPoint();
        agent.SetDestination(wayPoints[idleDestination].transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        if (idle)
        {
            if (agent.remainingDistance <= 1)
            {
                idleDestination = GetRandomWayPoint();
                agent.SetDestination(wayPoints[idleDestination].transform.position);
            }

        }
        if (!idle)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private int GetRandomWayPoint()
    {
        
        int r = rnd.Next(wayPoints.Length);
        return r;

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
