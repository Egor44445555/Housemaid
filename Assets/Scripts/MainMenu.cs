using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayCurrentLevel()
    {
        FindAnyObjectByType<AudioManager>().InteractionSound("ButtonTap", true);
        SceneManager.LoadScene(1);
    }

    public void OpenMenu()
    {
        FindAnyObjectByType<AudioManager>().InteractionSound("ButtonTap", true);
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        FindAnyObjectByType<AudioManager>().InteractionSound("ButtonTap", true);
        Application.Quit();
    }
}
