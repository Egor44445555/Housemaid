using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] public GameObject popup;

    public void ClosePopup()
    {
        GameObject.FindGameObjectWithTag("Popup").SetActive(false);
        FindObjectOfType<Person>().stopRunning = false;
        FindObjectOfType<Person>().popupIsOpen = "";
    }
}
