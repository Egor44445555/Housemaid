using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour
{
    public string taskName;
    public string countTrash = "0";
    private string countPuddle = "0";
    private string countTask = "0";

    public void ÑountChange()
    {
        countTrash = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Trash"));
        countPuddle = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Puddle"));
        countTask = LayerMask.NameToLayer("TaskNextFloor").ToString();        

        foreach (var obj in FindObjectsOfType<Count>())
        {
            if (obj.taskName == "Trash")
            {
                obj.GetComponent<Text>().text = countTrash + " / 10";
            }

            if (obj.taskName == "Puddle")
            {
                obj.GetComponent<Text>().text = countPuddle + " / 3";
            }

            if (obj.taskName == "TaskNextFloor")
            {
                obj.GetComponent<Text>().text = countTask + " / 1";
            }
        }
    }

    void Start()
    {
        foreach (var obj in FindObjectsOfType<Count>())
        {
            if (obj.taskName == "Trash")
            {
                obj.GetComponent<Text>().text = countTrash + " / 10";
            }

            if (obj.taskName == "Puddle")
            {
                obj.GetComponent<Text>().text = countPuddle + " / 3";
            }

            if (obj.taskName == "TaskNextFloor")
            {
                obj.GetComponent<Text>().text = countTask + " / 1";
            }
        }     
    }
}
