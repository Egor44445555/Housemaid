using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMenu : MonoBehaviour
{
    private Inventory inventory;
    private Slot slot;

    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        print(GameObject.FindGameObjectWithTag("BroomInCartMenu"));
    }

    public void CloseCartMenu()
    {
        GameObject.FindGameObjectWithTag("CartMenu").SetActive(false);
        FindObjectOfType<Person>().stopRunning = false;
        FindObjectOfType<Person>().cartMenuIsOpen = false;
    }

    public void Pickup()
    {
        for (int i = 0; i < inventory.stuff.Length; i++)
        {
            print(inventory.stuff[i]);
        }
    }
}
