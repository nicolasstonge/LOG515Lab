using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private bool iFrame = false;
    public bool idle = true;

    GameController gameController;
    Character player;
    NavMeshAgent agent;
    GameObject[] wayPoints;

    int idleDestination;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        player = GameObject.Find("Character").GetComponent<Character>();
        agent = GetComponent<NavMeshAgent>();
        wayPoints = GameObject.FindGameObjectsWithTag("Waypoint");
        idleDestination = gameController.GetRandomInt(0, wayPoints.Length);
        agent.SetDestination(wayPoints[idleDestination].transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        if (idle)
        {
            if (agent.remainingDistance <= 1)
            {
                idleDestination = gameController.GetRandomInt(0, wayPoints.Length);
                agent.SetDestination(wayPoints[idleDestination].transform.position);
            }

        }
        if (!idle)
        {
            agent.SetDestination(player.transform.position);
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
