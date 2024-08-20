using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        int countToiletPaper = 0;
        int countTowels = 0;

        foreach (Transform child in transform)
        {
            bool destroySlot = false;
            string nameStuff = child.gameObject.name.Replace("(Clone)", "");

            if (!FindObjectOfType<Storage>())
            {
                destroySlot = true;
            }

            if (FindObjectOfType<Person>().popupIsOpen == "")
            {
                child.GetComponent<Spawn>().SpawnDroppedItem();
            } else if (FindObjectOfType<Person>().popupIsOpen == "CartMenu")
            {                
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
                    foreach (bool item in FindObjectOfType<Storage>().toiletPaper)
                    {
                        if (item == false)
                        {
                            destroySlot = true;
                            countToiletPaper++;
                        }
                    }

                    for (int i = 0; i < FindObjectOfType<Storage>().toiletPaper.Length; i++)
                    {
                        if (FindObjectOfType<Storage>().toiletPaper[i] == false)
                        {
                            FindObjectOfType<Storage>().toiletPaper[i] = true;
                            break;
                        }
                    }
                } else if (nameSlot == "towels")
                {
                    foreach (bool item in FindObjectOfType<Storage>().towels)
                    {
                        if (item == false)
                        {
                            destroySlot = true;
                            countTowels++;
                        }
                    }

                    for (int i = 0; i < FindObjectOfType<Storage>().towels.Length; i++)
                    {
                        if (FindObjectOfType<Storage>().towels[i] == false)
                        {
                            FindObjectOfType<Storage>().towels[i] = true;
                            break;
                        }
                    }
                }
            }

            if (destroySlot)
            {
                FindAnyObjectByType<AudioManager>().InteractionSound("DropItem", true);
                GameObject.Destroy(child.gameObject);

                for (int i = 0; i < inventory.stuff.Length; i++)
                {
                    if (inventory.stuff[i] && inventory.stuff[i].name == child.gameObject.name.Replace("(Clone)", ""))
                    {
                        inventory.stuff[i] = null;
                    }
                }
            }

            if (FindObjectOfType<Storage>())
            {
                Transform[] objChild = FindObjectOfType<Storage>().transform.GetComponentsInChildren<Transform>();

                foreach (Transform item in objChild)
                {
                    if (item.name == "ToiletPaperGroup" && nameStuff == "toiletPaper")
                    {
                        for (int i = 0; i < item.transform.GetComponentsInChildren<Transform>().Length; i++)
                        {
                            if (item.transform.GetComponentsInChildren<Transform>()[i].tag == "ToiletPaperInCartMenu")
                            {
                                if (countToiletPaper <= i)
                                {
                                    Image image = item.transform.GetComponentsInChildren<Transform>()[i].GetComponent<Image>();
                                    Color newColor = image.color;
                                    newColor.a = 1;
                                    image.color = newColor;
                                }
                            }
                        }
                    }

                    if (item.name == "TowelsGroup" && nameStuff == "towels")
                    {
                        for (int i = 0; i < item.transform.GetComponentsInChildren<Transform>().Length; i++)
                        {
                            if (item.transform.GetComponentsInChildren<Transform>()[i].tag == "TowelsInCartMenu")
                            {
                                if (countTowels <= i)
                                {
                                    Image image = item.transform.GetComponentsInChildren<Transform>()[i].GetComponent<Image>();
                                    Color newColor = image.color;
                                    newColor.a = 1;
                                    image.color = newColor;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
