using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbLoader : MonoBehaviour {

    string addUserUrl = "http://stongenicolas.zapto.org:55020/PWNN/AddUser.php";
    string updateScoreUrl = "http://stongenicolas.zapto.org:55020/PWNN/UpdateScore.php";
    string updateDeathUrl = "http://stongenicolas.zapto.org:55020/PWNN/UpdateDeath.php";
    string updateLabDoneUrl = "http://stongenicolas.zapto.org:55020/PWNN/UpdateLabDone.php";
    string getAllUsersUrl = "http://stongenicolas.zapto.org:55020/PWNN/GetAllUsers.php";
    string getUserScoreUrl = "http://stongenicolas.zapto.org:55020/PWNN/GetUserScore.php";
    string getUserDeathUrl = "http://stongenicolas.zapto.org:55020/PWNN/GetUserDeath.php";
    string getUserLabdoneUrl = "http://stongenicolas.zapto.org:55020/PWNN/GetUserLabdone.php";

    public string[] userList;
    public int score;
    public int death;
    public int labdone;

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

    public IEnumerator GetUserScore(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);

        WWW data = new WWW(getUserScoreUrl, form);
        yield return data;
        string dataString = data.text;
        score = System.Int32.Parse(dataString.Split(';')[0]);
    }

    public IEnumerator GetUserDeath(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);

        WWW data = new WWW(getUserDeathUrl, form);
        yield return data;
        string dataString = data.text;
        death = System.Int32.Parse(dataString.Split(';')[0]);
    }

    public IEnumerator GetUserLabdone(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);

        WWW data = new WWW(getUserLabdoneUrl, form);
        yield return data;
        string dataString = data.text;
        labdone = System.Int32.Parse(dataString.Split(';')[0]);
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

    public IEnumerator UpdateScore(string username)
    {
        int newscore;
        yield return GetUserScore(username);
        newscore = score + 100;
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("scorePost", newscore);

        WWW www = new WWW(updateScoreUrl, form);
        yield return www;
    }

    public IEnumerator UpdateDeath(string username)
    {
        int nbrDeath;
        yield return GetUserDeath(username);
        nbrDeath = death + 1;
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("deathPost", nbrDeath);

        WWW www = new WWW(updateDeathUrl, form);
        yield return www;
    }

    public IEnumerator UpdateLabDone(string username)
    {
        int nbrLabdone;
        yield return GetUserLabdone(username);
        nbrLabdone = labdone + 1;

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("labdonePost", nbrLabdone);

        WWW www = new WWW(updateLabDoneUrl, form);
        yield return www;
    }
}
