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
    private string countToiletPaper = "0";
    private string countTowels = "0";

    public void ÑountChange()
    {        
        countTrash = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Trash"));
        countPuddle = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Puddle"));
        countToiletPaper = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("ToiletPaper"));
        countTowels = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Towels"));
        countTask = LayerMask.NameToLayer("TaskNextFloor") == -1 ? "0" : "1";

        PrintText();
    }

    void Start()
    {
        countToiletPaper = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("ToiletPaper"));
        countTowels = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Towels"));

        PrintText();
    }

    void PrintText()
    {
        var taskInfo = JsonHelper.GetJsonValue("Room3");
        var mainTaskInfo = JsonHelper.GetJsonValue("taskNextFloor");
        int mainTask = 0;

        foreach (Spawn child in FindObjectsOfType<Spawn>())
        {
            if (child.name.Contains("FlashCard"))
            {
                mainTask = 1;
            }
        }

        foreach (var obj in FindObjectsOfType<Count>())
        {
            if (obj.taskName == "Trash")
            {
                obj.GetComponent<Text>().text = countTrash + " / " + taskInfo.collectTrash;
            }

            if (obj.taskName == "Puddle")
            {
                obj.GetComponent<Text>().text = countPuddle + " / " + mainTaskInfo.removePuddle;
            }

            if (obj.taskName == "ToiletPaper")
            {
                obj.GetComponent<Text>().text = countToiletPaper + " / " + mainTaskInfo.toiletPaper;
            }

            if (obj.taskName == "Towels")
            {
                obj.GetComponent<Text>().text = countTowels + " / " + mainTaskInfo.towels;
            }

            if (obj.taskName == "NextFloor")
            {
                obj.GetComponent<Text>().text = mainTask + " / 1";
            }
        }
    }
}

[System.Serializable]
public class RoomInfo
{
    public int collectTrash;
    public int removePuddle;
    public int towels;
    public int toiletPaper;
    public string mainTask;
}
