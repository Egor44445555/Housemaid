using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CheckList : MonoBehaviour
{
    public GameObject checkList;

    public void OpenCheckList()
    {
        checkList.SetActive(true);
        FindObjectOfType<Person>().stopRunning = true;
    }

    public void CloseCheckList()
    {
        checkList.SetActive(false);
        FindObjectOfType<Person>().stopRunning = false;
    }
}