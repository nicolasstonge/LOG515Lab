using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilList : MonoBehaviour {

    public DbLoader db;
    public GameObject profileBtnPrefab;

    List<GameObject> btnList = new List<GameObject>();
    int nbrButton = 0;

	// Use this for initialization
	IEnumerator Start () {

        db = GameObject.Find("DbLoader").GetComponent<DbLoader>();
        yield return db.GetAllUsers();
        foreach (string item in db.userList)
        {
            int btnIndex = nbrButton;
            GameObject button;
            button = Instantiate(profileBtnPrefab, GameObject.Find("ProfileListContent").transform);
            button.transform.GetChild(0).GetComponent<Text>().text = item;
            button.GetComponent<Button>().onClick.AddListener(delegate { ButtonClicked(btnIndex); });
            btnList.Add(button);
            nbrButton++;
        }
    }

    public void UpdateList()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ButtonClicked (int btnIndex)
    {
        
        GameObject.Find("GameController").GetComponent<GameController>().selectedProfile = btnList[btnIndex].transform.GetChild(0).GetComponent<Text>().text;
    }
}
