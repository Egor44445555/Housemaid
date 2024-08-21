using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CheckList : MonoBehaviour
{
    public GameObject checkList;

    public void OpenCheckList()
    {
        if (!FindObjectOfType<Person>().stopRunning)
        {
            FindAnyObjectByType<AudioManager>().InteractionSound("ButtonTap", true);
            checkList.SetActive(true);
            FindObjectOfType<Person>().stopRunning = true;
            FindObjectOfType<Count>().ÑountChange();
        }        
    }

    public void CloseCheckList()
    {
        FindAnyObjectByType<AudioManager>().InteractionSound("ButtonTap", true);
        checkList.SetActive(false);
        FindObjectOfType<Person>().stopRunning = false;
    }
}