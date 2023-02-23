using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject fadeOutPanel;
    public AudioSource shamisen;

    public void Start()
    {
        
    }

    public void PlayGame()
    {
        
        fadeOutPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShamisenSound()
    {
        shamisen.Play();
    }
}
