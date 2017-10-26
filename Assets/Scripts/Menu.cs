using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public Button exitButton;
    public Button playButton;
    public Button optionButton;
    public Button optionBackButton;

    private GameController gameControler;



    // Use this for initialization
    void Start () {

        gameControler = GameObject.Find("GameController").GetComponent<GameController>();
        exitButton.onClick.AddListener(ExitOnClick);
        playButton.onClick.AddListener(PlayOnClick);
        optionButton.onClick.AddListener(OptionOnClick);
        optionBackButton.onClick.AddListener(OptionBackOnClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ExitOnClick()
    {
        Debug.Log("exit");
        Application.Quit();
    }

    void OptionOnClick()
    {
        DisplayOptionMenu();
        Debug.Log("option");
    }

    void OptionBackOnClick()
    {
        DisplayMainMenu();
    }

    void PlayOnClick()
    {
        gameControler.play();
        Debug.Log("play");
    }

    void DisplayOptionMenu()
    {
        transform.Find("main_panel").gameObject.SetActive(false);
        transform.Find("options_panel").gameObject.SetActive(true);
    }

    void DisplayMainMenu()
    {
        transform.Find("main_panel").gameObject.SetActive(true);
        transform.Find("options_panel").gameObject.SetActive(false);
    }

    public void HideMenu()
    {
        transform.gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        transform.gameObject.SetActive(true);
    }
}
