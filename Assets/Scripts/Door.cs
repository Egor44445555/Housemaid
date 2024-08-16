using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    GameObject[] doors;
    string countTrash;
    string countPuddle;
    string countTask;
    bool tasksComplete = true;

    public void OpenDoor(GameObject doorEnter)
    {
        var taskInfo = JsonHelper.GetJsonValue(doorEnter.GetComponent<FrameSwitch>().activeFrame.name);
        tasksComplete = true;
        countTrash = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Trash"));
        countPuddle = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Puddle"));
        countTask = LayerMask.NameToLayer("TaskNextFloor").ToString();

        if (taskInfo != null)
        {
            tasksComplete = int.Parse(countTrash) < taskInfo.collectTrash || int.Parse(countPuddle) < taskInfo.removePuddle ? false : true;
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

    public void CheckDoorAccess()
    {
        tasksComplete = true;
        countTrash = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Trash"));
        countPuddle = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Puddle"));
        countTask = LayerMask.NameToLayer("TaskNextFloor").ToString();

        if (doors != null && doors.Length > 0) {

            foreach (var door in doors)
            {
                var taskInfo = JsonHelper.GetJsonValue(door.GetComponent<FrameSwitch>().activeFrame.name);

                if (taskInfo != null)
                {
                    tasksComplete = int.Parse(countTrash) < taskInfo.collectTrash || int.Parse(countPuddle) < taskInfo.removePuddle ? false : true;
                }

                if (tasksComplete)
                {
                    door.GetComponent<Animator>().SetInteger("state", 0);
                }
                else
                {
                    door.GetComponent<Animator>().SetInteger("state", 2);
                }
            }
        }        
    }

    void Start()
    {
        var taskInfo = JsonHelper.GetJsonValue(this.gameObject.GetComponent<FrameSwitch>().activeFrame.name);
        countTrash = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Trash"));
        countPuddle = PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Puddle"));
        countTask = LayerMask.NameToLayer("TaskNextFloor").ToString();
        doors = GameObject.FindGameObjectsWithTag("Door");

        if (taskInfo != null)
        {
            tasksComplete = int.Parse(countTrash) < taskInfo.collectTrash || int.Parse(countPuddle) < taskInfo.removePuddle ? false : true;
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
