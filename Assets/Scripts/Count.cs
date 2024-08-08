using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour
{
    private GameObject[] gameObjects;
    public Text countText;
    public int count = 0;

    public void countChange()
    {
        count = count + 1;
        countText.text = "Tasks: " + count.ToString() + " / 40";
        //PlayerPrefs.SetInt("Count", count);
        //PlayerPrefs.Save();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("Count"))
        {
            //print(PlayerPrefs.GetInt("Count", count));
        }

        countText.text = "Tasks: " + count.ToString() + " / 40";
    }
}
