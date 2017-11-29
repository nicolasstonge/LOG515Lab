﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilList : MonoBehaviour {

    public DbLoader db;
    public Button nextBtn;
    public GameObject profileBtnPrefab;

    List<GameObject> btnList = new List<GameObject>();
    int clickedIndex = 555;
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
	
	// Update is called once per frame
	void Update () {

        
	}

    void ButtonClicked (int btnIndex)
    {
        if (clickedIndex != 555)
        {
            Button button = btnList[clickedIndex].GetComponent<Button>();
            ColorBlock cb = button.colors;
            cb.normalColor = Color.white;
            button.colors = cb;
        }
        clickedIndex = btnIndex;
        GameObject.Find("GameController").GetComponent<GameController>().selectedProfile = btnList[btnIndex].transform.GetChild(0).GetComponent<Text>().text;

        Button button1 = btnList[clickedIndex].GetComponent<Button>();
        ColorBlock cb1 = button1.colors;
        cb1.normalColor = new Color(0.537f,0.537f,0.537f,1f);
        button1.colors = cb1;

        nextBtn.interactable = true;
    }
}
