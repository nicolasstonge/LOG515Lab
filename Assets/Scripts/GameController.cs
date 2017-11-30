using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Labyrinth labyrinthPref;
    public Menu menuPref;

    private Labyrinth labyrinthInstance;
    private Menu menuInstance;

    int seed = (int)System.DateTime.Now.Ticks;
    private System.Random rnd;

    public string selectedProfile;

    // Use this for initialization
    void Start()
    {
        rnd = new System.Random(seed);
        menuInstance = Instantiate(menuPref, transform) as Menu;
        menuInstance.name = "Menu";
    }

    // Update is called once per frame
    void Update()
    {
        // to do restart new labyrinth
    }

    public void Play(int sizeX, int sizeZ)
    {
        menuInstance.HideMenu();
        labyrinthInstance = Instantiate(labyrinthPref) as Labyrinth;
        labyrinthInstance.name = "Labyrinth";
        labyrinthInstance.CreateLabyrinth(sizeX, sizeZ);
    }

    public void Stop()
    {
        menuInstance.ShowMenu();
        Destroy(labyrinthInstance.gameObject);

        foreach (Transform child in GameObject.Find("LabyrinthObjects").transform)
        {
            Destroy(child.gameObject);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public IEnumerator GameOver()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        yield return GameObject.Find("DbLoader").GetComponent<DbLoader>().UpdateDeath(selectedProfile);

        menuInstance.ShowMenu();
        menuInstance.DisplayGameOver();
        Destroy(labyrinthInstance.gameObject);

        foreach (Transform child in GameObject.Find("LabyrinthObjects").transform)
        {
            Destroy(child.gameObject);
        }
 
    }

    public IEnumerator Win()
    {
        Debug.Log("Win!");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        yield return GameObject.Find("DbLoader").GetComponent<DbLoader>().UpdateScore(selectedProfile);
        yield return GameObject.Find("DbLoader").GetComponent<DbLoader>().UpdateLabDone(selectedProfile);

        menuInstance.ShowMenu();
        menuInstance.DisplayWin();
        Destroy(labyrinthInstance.gameObject);

        foreach (Transform child in GameObject.Find("LabyrinthObjects").transform)
        {
            Destroy(child.gameObject);
        }

    }

    public void Restart(int difficulty)
    {

        if (difficulty == 1)
        {
            menuInstance.DifficultyVeryEasyOnClick();
        }
        if (difficulty == 2)
        {
            menuInstance.DifficultyEasyOnClick();
        }
        if (difficulty == 3)
        {
            menuInstance.DifficultyNormalOnClick();
        }
        if (difficulty == 4)
        {
            menuInstance.DifficultyHardOnClick();
        }
        if (difficulty == 5)
        {
            menuInstance.DifficultyVeryHardOnClick();
        }
    }

    public int GetRandomInt(int min, int max)
    {

        int r = rnd.Next(min,max);
        return r;

    }
}
