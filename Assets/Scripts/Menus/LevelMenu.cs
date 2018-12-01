using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private GameObject WorldsPanel;
    [SerializeField] private GameObject World1Panel;
    [SerializeField] private GameObject World2Panel;
    [SerializeField] private GameObject World3Panel;
    [SerializeField] private GameObject World4Panel;

    #region World 1 Panel
    public void OpenWorld1()
    {
        WorldsPanel.SetActive(false);
        World1Panel.SetActive(true);
    }

    public void CloseWorld1()
    {
        WorldsPanel.SetActive(true);
        World1Panel.SetActive(false);
    }
    #endregion

    #region World 2 Panel
    public void OpenWorld2()
    {
        WorldsPanel.SetActive(false);
        World2Panel.SetActive(true);
    }

    public void CloseWorld2()
    {
        WorldsPanel.SetActive(true);
        World2Panel.SetActive(false);
    }
    #endregion

    #region World 3 Panel
    public void OpenWorld3()
    {
        WorldsPanel.SetActive(false);
        World3Panel.SetActive(true);
    }

    public void CloseWorld3()
    {
        WorldsPanel.SetActive(true);
        World3Panel.SetActive(false);
    }
    #endregion

    #region World 4 Panel
    public void OpenWorld4()
    {
        WorldsPanel.SetActive(false);
        World4Panel.SetActive(true);
    }

    public void CloseWorld4()
    {
        WorldsPanel.SetActive(true);
        World4Panel.SetActive(false);
    }
    #endregion

}
