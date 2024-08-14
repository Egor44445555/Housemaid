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

        if (countText.name != "TaskNextFloor")
        {
            countText.text = "Дел для комнаты " + countText.name.Replace("CountTaskRoom", "") + ": " + count.ToString() + " / 40";
        }        
    }

    void Start()
    {
        if (countText.name != "TaskNextFloor")
        {
            countText.text = "Дел для комнаты " + countText.name.Replace("CountTaskRoom", "") + ": " + count.ToString() + " / 40";
        }        
    }
}
