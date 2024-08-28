using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;
    public GameObject[] stuff;
    public string[] endlessStuff = { "toiletPaper", "towels" };

    public void ChangeStuffInCartOnScene(string name, bool hide)
    {
        bool removeStuff = true;

        if (GameObject.Find("Cart"))
        {
            Transform[] objChild = GameObject.Find("Cart").transform.GetComponentsInChildren<Transform>();

            for (var j = 0; j < objChild.Length; j++)
            {
                foreach (string item in endlessStuff)
                {
                    if (item == name)
                    {
                        removeStuff = false;
                    }
                }

                if (!hide && objChild[j].gameObject.name.Contains(name))
                {
                    objChild[j].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
                }
                else if (hide && objChild[j].gameObject.name.Contains(name) && removeStuff)
                {
                    objChild[j].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                }
            }
        }
    }
}
