using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerr : MonoBehaviour {

    public Rigidbody2D PlayerLeft;
    public Rigidbody2D PlayerRight;
    public Rigidbody2D Ball;

    public GameObject Rebind;
    public Text EndText;

    public float PlayerSpeed = 1;

	// Use this for initialization
	void Start (){
        EndText.text = hardInput.FormatText("Press '['StartGame',P]' to start the game!");
    }
    

    // Update is called once per frame
    void Update ()
    {
        PlayerLeft.velocity = new Vector2(0, hardInput.GetAxis("Player1Up", "Player1Down") * PlayerSpeed);
        PlayerRight.velocity = new Vector2(0, hardInput.GetAxis("Player2Up", "Player2Down") * PlayerSpeed);

        if (hardInput.GetKeyDown("StartGame"))
        {
            Ball.gameObject.SetActive(true);
            Ball.velocity = new Vector2(5, 5);
            EndText.text = "";
            Rebind.SetActive(false);
        }
    }

    public void EndGame(string endCard)
    {
        EndText.text = endCard;
        Rebind.SetActive(true);
    }
}
