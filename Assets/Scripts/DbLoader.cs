using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbLoader : MonoBehaviour {

    string addUserUrl = "http://stongenicolas.zapto.org/PWNN/AddUser.php";
    string updateScoreUrl = "http://stongenicolas.zapto.org/PWNN/UpdateScore.php";
    string updateDeathUrl = "http://stongenicolas.zapto.org/PWNN/UpdateDeath.php";
    string getAllUsersUrl = "http://stongenicolas.zapto.org/PWNN/GetAllUsers.php";

    public string[] userList;

    // Use this for initialization
    IEnumerator Start ()
    {
        yield return GetAllUsers();
    }

    public IEnumerator GetAllUsers()
    {
        WWW data = new WWW(getAllUsersUrl);
        yield return data;
        string dataString = data.text;
        userList = new string[dataString.Split(';').Length - 1];

        for (int i = 0; i < dataString.Split(';').Length - 1; i++)
        {
            userList[i] = dataString.Split(';')[i];
        }  
    }

    public IEnumerator AddUser(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("scorePost", 0);
        form.AddField("deathPost", 0);
        form.AddField("labdonePost", 0);

        WWW www = new WWW(addUserUrl, form);
        yield return www;
    }

    public IEnumerator UpdateScore(string username, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("scorePost", score);

        WWW www = new WWW(updateScoreUrl, form);
        yield return www;
    }

    public IEnumerator UpdateDeath(string username, int nbrDeath)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("deathPost", nbrDeath);

        WWW www = new WWW(updateDeathUrl, form);
        yield return www;
    }
}
