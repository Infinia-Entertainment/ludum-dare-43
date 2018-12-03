using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [SerializeField] private GameObject menuObject;
    [SerializeField] private GameObject achievementPanel;

    [SerializeField] private AudioListener audioListener;

    bool isMenuOpened = false;

    private void Start()
    {
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) //Input.GetKeyDown((KeyCode)PlayerControlKeys.MenuKey)
        {
            isMenuOpened = !isMenuOpened;
            if (isMenuOpened)
            {
                MenuClose();
            }
            else
            {
                MenuOpen();
            }
        }
    }

    public void MenuOpen()
    {
        menuObject.SetActive(true);
        Time.timeScale = 0;
        audioListener.enabled = false;
        //To add Input change (to disabled)
    }

    public void MenuClose()
    {
        menuObject.SetActive(false);
        Time.timeScale = 1;
        audioListener.enabled = true;
        //To add Input change (to disabled)
    }


    #region AchievementPanel
    public void OpenAchievementsMenu()
    {
        menuObject.SetActive(false);
        achievementPanel.SetActive(true);
    }

    public void CloseAchievementsMenu()
    {
        menuObject.SetActive(true);
        achievementPanel.SetActive(false);
    }
    #endregion

    public void OpenMainMenu()
    {
        /* This is only temporary code
         * This is what should be here:
         * 1. The data is saved to an appopriate slot (1,2,3)
         * 2. Persistent objects are destroyed 
         * 3. Scene is changed (With appropriate animation etc)
        */
        Time.timeScale = 1;
        audioListener.enabled = true;
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>()) Destroy(obj);
        SceneManager.LoadScene(0);
    }
}
