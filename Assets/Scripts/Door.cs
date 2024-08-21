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
            // Door enable open
            doorEnter.GetComponent<Animator>().SetInteger("state", 3);
        }
        else
        {
            // Door disable idle
            doorEnter.GetComponent<Animator>().SetInteger("state", 2);
        }
    }

    public void CheckDoorAccess()
    {
        string nameRoom = GameObject.FindGameObjectWithTag("Room").name.ToLower();
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
                    if (nameRoom.Contains("room"))
                    {
                        // Door idle
                        this.gameObject.GetComponent<Animator>().SetInteger("state", 0);
                    }
                    else
                    {
                        // Door enable idle
                        this.gameObject.GetComponent<Animator>().SetInteger("state", 4);
                    }
                }
                else
                {
                    // Door disable idle
                    door.GetComponent<Animator>().SetInteger("state", 2);
                }
            }
        }        
    }

    void Start()
    {
        var taskInfo = JsonHelper.GetJsonValue(this.gameObject.GetComponent<FrameSwitch>().activeFrame.name);
        string nameRoom = GameObject.FindGameObjectWithTag("Room").name.ToLower();
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
            if (nameRoom.Contains("room"))
            {
                // Door idle
                this.gameObject.GetComponent<Animator>().SetInteger("state", 0);
            } else
            {
                // Door enable idle
                this.gameObject.GetComponent<Animator>().SetInteger("state", 4);
            }
        } else
        {
            // Door disable idle
            this.gameObject.GetComponent<Animator>().SetInteger("state", 2);
        }
    }
}
