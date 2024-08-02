using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;
    public GameObject[] stuff;

    public void ChangeStuffInCartOnScene(string name, bool hide)
    {
        Transform[] objChild = GameObject.FindGameObjectWithTag("Cart").transform.GetComponentsInChildren<Transform>();
        for (var j = 0; j < objChild.Length; j++)
        {
            if (!hide && objChild[j].gameObject.name.Contains(name))
            {
                objChild[j].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
            } else if (hide && objChild[j].gameObject.name.Contains(name))
            {
                objChild[j].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
            }
        }
    }
}
