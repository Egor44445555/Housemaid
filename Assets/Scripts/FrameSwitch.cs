using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameSwitch : MonoBehaviour
{
    [SerializeField] public GameObject playerPositionStayEnter;
    [SerializeField] public Person person;

    public GameObject activeFrame;
    private GameObject[] rooms;

    void Start()
    {
        rooms = GameObject.FindGameObjectsWithTag("Room");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            person.newFrame = activeFrame;
            person.doorEnterPoint = playerPositionStayEnter.transform.position;
        }
    }

    public void OpenDoor()
    {
        foreach (GameObject room in rooms)
        {
            room.SetActive(false);
        }

        person.transform.position = new Vector3(0, 0, 0);
        person.newFrame.SetActive(true);
        person.transform.position = person.doorEnterPoint;
    }
}
