using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public GameObject Menu;

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
        SceneManager.LoadScene(2);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
