using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameSwitch : MonoBehaviour
{
    [SerializeField] public GameObject playerPositionStayEnter;
    GameObject player;

    public GameObject activeFrame;
    private GameObject[] rooms;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rooms = GameObject.FindGameObjectsWithTag("Room");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            player.GetComponent<Person>().newFrame = activeFrame;
            player.GetComponent<Person>().doorEnterPoint = playerPositionStayEnter.transform.position;
        }
    }

    public void OpenDoor()
    {        
        rooms = GameObject.FindGameObjectsWithTag("Room");

        foreach (GameObject room in rooms)
        {
            room.SetActive(false);
        }

        player.GetComponent<Transform>().transform.position = new Vector3(0, 0, 0);
        player.GetComponent<Person>().newFrame.SetActive(true);
        player.GetComponent<Transform>().transform.position = player.GetComponent<Person>().doorEnterPoint;
    }
}
