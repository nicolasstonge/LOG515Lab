using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

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
    public Button activateBtn;
    public Button gameOverRestartBtn;
    public Button gameOverMainMenuBtn;
    public Button gameOverExitBtn;

    public InputField activationField;

    private GameController gameControler;

    int selectedDifficulty;

    // Use this for initialization
    void Start()
    {

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
        activateBtn.onClick.AddListener(ActivateOnClick);
        gameOverExitBtn.onClick.AddListener(ExitOnClick);
        gameOverMainMenuBtn.onClick.AddListener(DifficultyBackOnClick);
        gameOverRestartBtn.onClick.AddListener(GameOverRestartOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GameOverRestartOnClick()
    {
        gameControler.restart(selectedDifficulty);
    }

    void ExitOnClick()
    {
        Application.Quit();
    }

    void OptionOnClick()
    {
        DisplayOptionMenu();
    }

    void OptionBackOnClick()
    {
        DisplayMainMenu();
    }

    void DifficultyBackOnClick()
    {
        DisplayMainMenu();
        playMusic(true);
    }

    void ActivateOnClick()
    {
        if (activationField.text == "abc123")
        {
            DisplayMainMenu();
        }

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
        transform.Find("activation_panel").gameObject.SetActive(false);
        transform.Find("gameover_panel").gameObject.SetActive(false);
    }

    void DisplayDifficultyMenu()
    {
        transform.Find("difficulty_panel").gameObject.SetActive(true);
        transform.Find("main_panel").gameObject.SetActive(false);
    }

    public void DisplayGameOver()
    {
        transform.Find("main_panel").gameObject.SetActive(false);
        transform.Find("gameover_panel").gameObject.SetActive(true);
        playMusic(false);
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
        selectedDifficulty = 1;
        gameControler.play(5, 5);
    }

    public void DifficultyEasyOnClick()
    {
        selectedDifficulty = 2;
        gameControler.play(10, 10);
    }

    public void DifficultyNormalOnClick()
    {
        selectedDifficulty = 3;
        gameControler.play(20, 20);
    }

    public void DifficultyHardOnClick()
    {
        selectedDifficulty = 4;
        gameControler.play(30, 30);
    }

    public void DifficultyVeryHardOnClick()
    {
        selectedDifficulty = 5;
        gameControler.play(40, 40);
    }

    public void playMusic(bool music)
    {
        if (music)
        {
            transform.GetComponent<AudioSource>().Play();
        }
        else
        {
            transform.GetComponent<AudioSource>().Stop();
        }

    }
}
