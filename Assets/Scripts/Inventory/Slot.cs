using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Inventory inventory;
    public int i;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isFull[i] = false;
        }
    }

    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            if (!FindObjectOfType<Person>().cartMenuIsOpen)
            {
                child.GetComponent<Spawn>().SpawnDroppedItem();
            } else
            {
                string nameStuff = child.gameObject.name.Replace("(Clone)", "");
                char[] letters = nameStuff.ToCharArray();
                letters[0] = char.ToUpper(letters[0]);
                string newString = new string(letters);
                GameObject.FindGameObjectWithTag(newString + "InCartMenu").GetComponent<RectTransform>().sizeDelta = GameObject.FindGameObjectWithTag(newString + "InCartMenu").GetComponent<PickUpInCartMenu>().sizeStuff;
                inventory.ChangeStuffInCartOnScene(nameStuff, false);
            }

            GameObject.Destroy(child.gameObject);

            for (int i = 0; i < inventory.stuff.Length; i++)
            {
                if (inventory.stuff[i] && inventory.stuff[i].name == child.gameObject.name.Replace("(Clone)", ""))
                {
                    inventory.stuff[i] = null;
                }                
            }
        }
    }
}
