using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void StartGameClick()
    {
        SceneManager.LoadSceneAsync(2);        
    }
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.SetActiveScene(arg0);
    }

    public void CreditSceneClick()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
