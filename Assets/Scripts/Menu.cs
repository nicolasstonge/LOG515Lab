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
    public Button winRestartBtn;
    public Button winMainMenuBtn;
    public Button winExitBtn;
    public Button profileNextBtn;
    public Button statsButton;
    public Button statsBackBtn;
    public Button changeProfilBtn;

    public InputField activationField;

    private GameController gameControler;

    int selectedDifficulty;

    public Text mainMenuProfile;

    // stats texts
    public Text statsScore;
    public Text statsDeath;
    public Text statsLabdone;

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
        winRestartBtn.onClick.AddListener(GameOverRestartOnClick);
        winExitBtn.onClick.AddListener(ExitOnClick);
        winMainMenuBtn.onClick.AddListener(DifficultyBackOnClick);
        profileNextBtn.onClick.AddListener(OptionBackOnClick);
        statsButton.onClick.AddListener(delegate { StartCoroutine(DisplayStatsMenu()); });
        statsBackBtn.onClick.AddListener(DisplayMainMenu);
        changeProfilBtn.onClick.AddListener(DisplayProfileMenu);

}

// Update is called once per frame
void Update()
    {

    }

    void GameOverRestartOnClick()
    {
        gameControler.Restart(selectedDifficulty);
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
            DisplayProfileMenu();
        }

    }

    void PlayOnClick()
    {
        DisplayDifficultyMenu();
    }

    IEnumerator DisplayStatsMenu()
    {
        transform.Find("main_panel").gameObject.SetActive(false);
        
        transform.Find("stats_panel").gameObject.SetActive(true);
        yield return WaitForStats();
    }

    IEnumerator WaitForStats()
    {
        string username = GameObject.Find("GameController").GetComponent<GameController>().selectedProfile;
        DbLoader db = GameObject.Find("DbLoader").GetComponent<DbLoader>();
        yield return db.GetUserScore(username);
        yield return db.GetUserDeath(username);
        yield return db.GetUserLabdone(username);
        statsScore.text = db.score.ToString();
        statsDeath.text = db.death.ToString();
        statsLabdone.text = db.labdone.ToString();
    }

    void DisplayOptionMenu()
    {
        transform.Find("main_panel").gameObject.SetActive(false);
        transform.Find("options_panel").gameObject.SetActive(true);
    }

    void DisplayProfileMenu()
    {
        transform.Find("profils_panel").gameObject.SetActive(true);
        transform.Find("main_panel").gameObject.SetActive(false);
        transform.Find("activation_panel").gameObject.SetActive(false);
    }

    void DisplayMainMenu()
    {
        mainMenuProfile.text = gameControler.selectedProfile;
        transform.Find("main_panel").gameObject.SetActive(true);
        transform.Find("options_panel").gameObject.SetActive(false);
        transform.Find("difficulty_panel").gameObject.SetActive(false);
        transform.Find("activation_panel").gameObject.SetActive(false);
        transform.Find("gameover_panel").gameObject.SetActive(false);
        transform.Find("win_panel").gameObject.SetActive(false);
        transform.Find("profils_panel").gameObject.SetActive(false);
        transform.Find("stats_panel").gameObject.SetActive(false);
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

    public void DisplayWin()
    {
        transform.Find("main_panel").gameObject.SetActive(false);
        transform.Find("win_panel").gameObject.SetActive(true);
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
        gameControler.Play(5, 5);
    }

    public void DifficultyEasyOnClick()
    {
        selectedDifficulty = 2;
        gameControler.Play(10, 10);
    }

    public void DifficultyNormalOnClick()
    {
        selectedDifficulty = 3;
        gameControler.Play(20, 20);
    }

    public void DifficultyHardOnClick()
    {
        selectedDifficulty = 4;
        gameControler.Play(30, 30);
    }

    public void DifficultyVeryHardOnClick()
    {
        selectedDifficulty = 5;
        gameControler.Play(40, 40);
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
