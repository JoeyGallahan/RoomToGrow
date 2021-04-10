using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public bool paused { get; set; } = false;
    [SerializeField] private GameObject container;
    [SerializeField] private bool canPause = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        container.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainMenu") ||
            SceneManager.GetActiveScene().name.Equals("RandomLevelSelect"))
        {
            canPause = false;
            paused = false;
            container.SetActive(false);
        }
        else
        {
            canPause = true;
        }

        if (canPause)
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            paused = !paused;
            container.SetActive(paused);
        }
    }
}
