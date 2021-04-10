using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void PlayRandomizedLevels()
    {
        SceneManager.LoadScene("RandomLevelSelect");
    }

    public void PlaySandbox()
    {
        SceneManager.LoadScene("Sandbox");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
