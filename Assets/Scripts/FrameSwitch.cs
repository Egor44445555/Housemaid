using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameSwitch : MonoBehaviour
{
    [SerializeField] public GameObject playerPositionCorridorStart;
    [SerializeField] public GameObject playerPositionStayEnter;
    [SerializeField] public GameObject playerPositionStayExit;
    [SerializeField] public Person person;

    public GameObject activeFrame;

    string startPos = "Corridor";

    void Start()
    {
        if (playerPositionCorridorStart && activeFrame.name == "Corridor")
        {
            person.transform.position = playerPositionCorridorStart.transform.position;            
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            activeFrame.SetActive(true);

            if (playerPositionStayEnter && activeFrame.name != startPos)
            {
                person.transform.position = playerPositionStayEnter.transform.position;

                startPos = "";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && activeFrame)
        {
            activeFrame.SetActive(false);

            if (playerPositionStayExit)
            {
                person.transform.position = playerPositionStayExit.transform.position;                
            }

            startPos = "";
        }
    }
}
