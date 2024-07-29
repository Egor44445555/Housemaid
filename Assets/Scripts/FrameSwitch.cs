using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameSwitch : MonoBehaviour
{
    [SerializeField] public GameObject playerPositionStayEnter;
    [SerializeField] public GameObject playerPositionStayExit;
    [SerializeField] public Person person;

    public GameObject activeFrame;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && activeFrame)
        {

            print(activeFrame);
            activeFrame.SetActive(true);

            if (playerPositionStayEnter)
            {
                person.transform.position = playerPositionStayEnter.transform.position;
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
        }
    }
}
