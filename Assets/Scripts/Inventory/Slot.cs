using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Inventory inventory;
    private Storage storage;
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
        int count = 0;
        int countToiletPaper = 0;
        int countTowels = 0;

        foreach (Transform child in transform)
        {
            bool destroySlot = true;

            if (FindObjectOfType<Person>().popupIsOpen == "")
            {
                child.GetComponent<Spawn>().SpawnDroppedItem();
            } else if (FindObjectOfType<Person>().popupIsOpen == "CartMenu")
            {
                string nameStuff = child.gameObject.name.Replace("(Clone)", "");
                char[] letters = nameStuff.ToCharArray();
                letters[0] = char.ToUpper(letters[0]);
                string newString = new string(letters);
                GameObject.FindGameObjectWithTag(newString + "InCartMenu").GetComponent<RectTransform>().sizeDelta = GameObject.FindGameObjectWithTag(newString + "InCartMenu").GetComponent<PickUpInCartMenu>().sizeStuff;
                inventory.ChangeStuffInCartOnScene(nameStuff, false);
            }
            else
            {
                string nameSlot = child.name.Replace("(Clone)", "");

                if (nameSlot == "toiletPaper")
                {
                    for (int i = 0; i < FindObjectOfType<Storage>().toiletPaper.Length; i++)
                    {
                        if (FindObjectOfType<Storage>().toiletPaper[i] == false)
                        {
                            FindObjectOfType<Storage>().toiletPaper[i] = true;
                            break;
                        }
                    }

                    foreach (bool item in FindObjectOfType<Storage>().toiletPaper)
                    {
                        if (item)
                        {
                            countToiletPaper++;
                            count++;
                        }
                    }

                    destroySlot = countToiletPaper == FindObjectOfType<Storage>().toiletPaper.Length ? false : true;
                } else if (nameSlot == "towels")
                {
                    for (int i = 0; i < FindObjectOfType<Storage>().towels.Length; i++)
                    {
                        if (FindObjectOfType<Storage>().towels[i] == false)
                        {
                            FindObjectOfType<Storage>().towels[i] = true;
                            break;
                        }
                    }

                    foreach(bool item in FindObjectOfType<Storage>().towels)
                    {
                        if (item)
                        {
                            countTowels++;
                            count++;
                        }
                    }

                    print(countTowels);
                    print(FindObjectOfType<Storage>().towels.Length);

                    destroySlot = countTowels == FindObjectOfType<Storage>().towels.Length ? false : true;
                }
            }

            if (destroySlot)
            {
                GameObject.Destroy(child.gameObject);
            }            

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
