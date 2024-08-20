using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] public GameObject popup;

    public void ClosePopup()
    {
        if (popup.name.Contains("Closet"))
        {
            FindAnyObjectByType<AudioManager>().InteractionSound("ClosetClose", true);
        }
        else
        {
            FindAnyObjectByType<AudioManager>().InteractionSound("ButtonTap", true);
        }

        GameObject.FindGameObjectWithTag("Popup").SetActive(false);
        FindObjectOfType<Person>().stopRunning = false;
        FindObjectOfType<Person>().popupIsOpen = "";
    }
}
