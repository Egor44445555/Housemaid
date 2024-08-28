using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [SerializeField] GameObject noteInMenu;
    [SerializeField] GameObject noteButton;

    public void PickUpNote()
    {
        if (FindObjectOfType<Person>().safeFound)
        {
            FindAnyObjectByType<AudioManager>().InteractionSound("WriteOnNotebook", true);
            noteInMenu.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        }        
    }
}
