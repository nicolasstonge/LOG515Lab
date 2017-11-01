using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public Button exitButton;
    public Button playButton;
    public Button optionButton;
    public Button optionBackButton;
    public Button difficultyBackButton;
    public Button difficultyVeryEasyBtn;
    public Button difficultyEasyBtn;
    public Button difficultyNormalBtn;
    public Button difficultyHardBtn;
    public Button difficultyVeryHardBtn;

    private GameController gameControler;



    // Use this for initialization
    void Start () {

        gameControler = GameObject.Find("GameController").GetComponent<GameController>();
        exitButton.onClick.AddListener(ExitOnClick);
        playButton.onClick.AddListener(PlayOnClick);
        optionButton.onClick.AddListener(OptionOnClick);
        optionBackButton.onClick.AddListener(OptionBackOnClick);
        difficultyBackButton.onClick.AddListener(DifficultyBackOnClick);
        difficultyVeryEasyBtn.onClick.AddListener(DifficultyVeryEasyOnClick);
        difficultyEasyBtn.onClick.AddListener(DifficultyEasyOnClick);
        difficultyNormalBtn.onClick.AddListener(DifficultyNormalOnClick);
        difficultyHardBtn.onClick.AddListener(DifficultyHardOnClick);
        difficultyVeryHardBtn.onClick.AddListener(DifficultyVeryHardOnClick);
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

    void DifficultyBackOnClick()
    {
        DisplayMainMenu();
    }

    void PlayOnClick()
    {
        DisplayDifficultyMenu();
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
        transform.Find("difficulty_panel").gameObject.SetActive(false);
    }

    void DisplayDifficultyMenu()
    {
        transform.Find("difficulty_panel").gameObject.SetActive(true);
        transform.Find("main_panel").gameObject.SetActive(false);
    }

    public void HideMenu()
    {
        transform.gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        DisplayMainMenu();
        transform.gameObject.SetActive(true);
    }

    public void DifficultyVeryEasyOnClick()
    {
        gameControler.play(5, 5);
    }

    public void DifficultyEasyOnClick()
    {
        gameControler.play(10, 10);
    }

    public void DifficultyNormalOnClick()
    {
        gameControler.play(20, 20);
    }

    public void DifficultyHardOnClick()
    {
        gameControler.play(30, 30);
    }

    public void DifficultyVeryHardOnClick()
    {
        gameControler.play(40, 40);
    }
}
