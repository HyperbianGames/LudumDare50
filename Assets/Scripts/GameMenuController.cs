using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    public static GameMenuController Instance;

    public GameObject Menu;
    public GameObject WinScreen;
    public GameObject LoseScreen;
    public GameObject ElevatorSelect;
    public GameObject WarningTextObj;

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

    public void ResetGame()
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
        Time.timeScale = 0.1f;
        if (WinScreen != null)
            WinScreen.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        Time.timeScale = 0.1f;
        if (LoseScreen != null)
            LoseScreen.SetActive(true);
    }

    internal void ShowMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void ShowElevator(int numberOfTowersActive)
    {
        WarningTextObj.GetComponent<Text>().text = $"There are still {numberOfTowersActive} Pillars remaining. Defeat the enemies near the pillars to destroy them. Destroying Pillars will weaken Boss Name Here.\n\nAre you sure you want to go fight Boss Name Here ? ";

        ElevatorSelect.SetActive(true);
    }

    public void HideElevator()
    {
        ElevatorSelect.SetActive(false);
    }

    public void ElavtorYesClick()
    {
        ElavatorController.Instance.StartRaise();
        HideElevator();
    }

    public void ElavtorNoClick()
    {
        HideElevator();
        PlayerController.Instance.ReleasePlayer();
    }
}
