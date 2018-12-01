using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject levelsPanel;

    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject startNewButton;



    private void Start()
    {

    }

    #region Levels
    public void OpenLevels()
    {
        mainMenuPanel.SetActive(false);
        levelsPanel.SetActive(true);
    }

    public void CloseLevels()
    {
        mainMenuPanel.SetActive(true);
        levelsPanel.SetActive(false);
    }
    #endregion

    #region Options
    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    #endregion

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void LevelSelect(int index)
    {
        FindObjectOfType<SceneLoader>().LoadScene(sceneIndex: index);
    }
}
