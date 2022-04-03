using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public static GameMenuController Instance;

    public GameObject Menu;
    public GameObject WinScreen;
    public GameObject LoseScreen;

    private void Start()
    {
        Instance = this;
    }

    public void ToggleMenu()
    {
        Menu.SetActive(!Menu.activeInHierarchy);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Reset()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

    public void GoToMenu()
    {
        ShowMenu();
    }

    public void ShowWinScreen()
    {
        WinScreen.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        LoseScreen.SetActive(true);
    }

    internal void ShowMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
