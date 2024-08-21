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
        PlayerPrefs.SetString("task" + LayerMask.NameToLayer("Towels"), "0");
        PlayerPrefs.SetString("task" + LayerMask.NameToLayer("ToiletPaper"), "0");
        PlayerPrefs.Save();
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
            bool closetMenuIsOpen = false;
            bool destroySlot = false;
            string nameStuff = child.gameObject.name.Replace("(Clone)", "");

            if (!FindObjectOfType<Storage>())
            {
                destroySlot = true;
            }

            if (FindObjectOfType<Person>().popupIsOpen == "")
            {
                // Drop item on scene

                child.GetComponent<Spawn>().SpawnDroppedItem();
            } else if (FindObjectOfType<Person>().popupIsOpen == "CartMenu")
            {
                // Drop item in cart menu

                char[] letters = nameStuff.ToCharArray();
                letters[0] = char.ToUpper(letters[0]);
                string newString = new string(letters);
                GameObject.FindGameObjectWithTag(newString + "InCartMenu").GetComponent<RectTransform>().sizeDelta = GameObject.FindGameObjectWithTag(newString + "InCartMenu").GetComponent<PickUpInCartMenu>().sizeStuff;
                inventory.ChangeStuffInCartOnScene(nameStuff, false);
            }
            else
            {
                // Drop item in closet

                string nameSlot = child.name.Replace("(Clone)", "");

                closetMenuIsOpen = true;

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

                if (closetMenuIsOpen && nameStuff.Contains("toiletPaper"))
                {
                    PlayerPrefs.SetString("task" + LayerMask.NameToLayer("ToiletPaper"), (int.Parse(PlayerPrefs.GetString("task" + LayerMask.NameToLayer("ToiletPaper"))) + 1).ToString());
                    PlayerPrefs.Save();
                }

                if (closetMenuIsOpen && nameStuff.Contains("towels"))
                {
                    PlayerPrefs.SetString("task" + LayerMask.NameToLayer("Towels"), (int.Parse(PlayerPrefs.GetString("task" + LayerMask.NameToLayer("Towels"))) + 1).ToString());
                    PlayerPrefs.Save();
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
