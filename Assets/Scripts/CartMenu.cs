using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMenu : MonoBehaviour
{
    private Inventory inventory;

    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void CloseCartMenu()
    {        
        GameObject.FindGameObjectWithTag("CartMenu").SetActive(false);
        FindObjectOfType<Person>().stopRunning = false;
        FindObjectOfType<Person>().cartMenuIsOpen = false;
    }
}
