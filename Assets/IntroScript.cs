using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    [SerializeField]
    private InputActionReference anyKey;

    // Start is called before the first frame update
    void Start()
    {
        anyKey.action.Enable();
    }

    private void OnDestroy()
    {
        anyKey.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (anyKey.action.triggered)
            SceneManager.LoadScene(1);
    }
}
