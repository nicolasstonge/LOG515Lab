using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(GetComponent<Rigidbody2D>().velocity.magnitude < 10)
            GetComponent<Rigidbody2D>().velocity *= 1.05f;

        if (collision.transform.tag.Contains("End"))
        {
            if (collision.transform.tag.Contains("1"))
                FindObjectOfType<GameControllerr>().EndGame("Player 2 Wins");
            else
                FindObjectOfType<GameControllerr>().EndGame("Player 1 Wins");

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = Vector3.zero;
            gameObject.SetActive(false);

        }
    }
}
