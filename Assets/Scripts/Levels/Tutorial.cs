using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject tut2;

    public void OpenTut2()
    {
        tut2.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
