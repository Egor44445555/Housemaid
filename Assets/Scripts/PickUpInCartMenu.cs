using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInCartMenu : MonoBehaviour
{
    private Inventory inventory;
    public GameObject slotButton;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void PickUpStuff()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.isFull[i] == false)
            {
                inventory.isFull[i] = true;
                Instantiate(slotButton, inventory.slots[i].transform);
                inventory.stuff[i] = slotButton;
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

                break;
            }
        }
    }
}
