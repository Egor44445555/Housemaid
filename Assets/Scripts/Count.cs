using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour
{
    private GameObject[] gameObjects;
    public Text countText;
    public int count = 3;

    public void countChange()
    {
        count = count - 1;
        countText.text = "Tasks: " + count.ToString();
        PlayerPrefs.SetInt("Count", count);
        PlayerPrefs.Save();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("Count"))
        {
            //print(PlayerPrefs.GetInt("Count", count));
        }
        
        gameObjects = GameObject.FindGameObjectsWithTag("Task");
        count = gameObjects.Length;
        countText.text = "Tasks: " + gameObjects.Length.ToString();
    }
}
