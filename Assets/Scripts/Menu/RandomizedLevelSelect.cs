using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomizedLevelSelect : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void LoadEasyLevel()
    {
        SceneManager.LoadScene("EasyLevel");
    }

    public void LoadNormalLevel()
    {
        SceneManager.LoadScene("NormalLevel");
    }

    public void LoadHardLevel()
    {
        SceneManager.LoadScene("HardLevel");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
