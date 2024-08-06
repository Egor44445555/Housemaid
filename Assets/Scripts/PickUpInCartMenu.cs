using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInCartMenu : MonoBehaviour
{
    private Inventory inventory;
    public GameObject slotButton;
    public Vector2 sizeStuff;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        sizeStuff = gameObject.GetComponent<RectTransform>().sizeDelta;
    }

    public void PickUpStuff()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.isFull[i] == false)
            {
                
                bool removeStuff = true;
                inventory.isFull[i] = true;
                Instantiate(slotButton, inventory.slots[i].transform);
                inventory.stuff[i] = slotButton;                
                inventory.ChangeStuffInCartOnScene(slotButton.name, true);                

                foreach(string item in inventory.endlessStuff)
                {
                    if (item == slotButton.name)
                    {
                        removeStuff = false;
                    }
                }

                if (removeStuff)
                {
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }

                break;
            }
        }
    }
}
