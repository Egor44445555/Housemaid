using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    string countTrash;
    string countPuddle;
    string countTask;
    bool tasksComplete = true;

    public void OpenDoor(GameObject doorEnter)
    {
        var taskInfo = JsonHelper.GetJsonValue(doorEnter.GetComponent<FrameSwitch>().activeFrame.name);

        if (taskInfo != null)
        {
            tasksComplete = int.Parse(countTrash) < taskInfo.collectTrash ? false : true;
            tasksComplete = int.Parse(countPuddle) < taskInfo.removePuddle ? false : true;
        }

        if (tasksComplete)
        {
            doorEnter.GetComponent<Animator>().SetInteger("state", 1);
        }
        else
        {
            doorEnter.GetComponent<Animator>().SetInteger("state", 2);
        }
    }

    void Start()
    {
        var taskInfo = JsonHelper.GetJsonValue(this.gameObject.GetComponent<FrameSwitch>().activeFrame.name);
        countTrash = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Trash"));
        countPuddle = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Puddle"));
        countTask = LayerMask.NameToLayer("TaskNextFloor").ToString();

        if (taskInfo != null)
        {
            tasksComplete = int.Parse(countTrash) < taskInfo.collectTrash ? false : true;
            tasksComplete = int.Parse(countPuddle) < taskInfo.removePuddle ? false : true;
        }

        if (tasksComplete)
        {
            this.gameObject.GetComponent<Animator>().SetInteger("state", 0);
        } else
        {
            this.gameObject.GetComponent<Animator>().SetInteger("state", 2);
        }
    }
}
