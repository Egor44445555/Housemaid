using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] public GameObject popup;
    [SerializeField] public GameObject noteButton;

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

    public void OpenPopup()
    {
        FindAnyObjectByType<AudioManager>().InteractionSound("ButtonTap", true);

        if (GameObject.FindGameObjectWithTag("Popup"))
        {
            GameObject.FindGameObjectWithTag("Popup").SetActive(false);
        }

        if (popup.name == "Safe")
        {
            FindObjectOfType<Person>().safeFound = true;
        }

        if (popup.name == "NoteMenu")
        {
            FindAnyObjectByType<AudioManager>().InteractionSound("OpenNote", true);
        }

        popup.SetActive(true);
        FindObjectOfType<Person>().stopRunning = true;
        FindObjectOfType<Person>().popupIsOpen = popup.name;
    }
}
