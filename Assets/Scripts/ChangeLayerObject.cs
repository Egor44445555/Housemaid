using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayerObject : MonoBehaviour
{
    public GameObject[] gameObjects;
    private Person player;

    public void ChangeOrderLayerObject()
    {
        player = FindObjectOfType<Person>();

        foreach (GameObject obj in gameObjects)
        {
            float playerPositionY = player.transform.position.y;
            float cartPositionY = obj.transform.position.y;
            int playerOrderLayer = player.GetComponent<SpriteRenderer>().sortingOrder;
            Transform[] objChild = obj.transform.GetComponentsInChildren<Transform>();

            if (playerPositionY > cartPositionY)
            {
                for (var j = 0; j < objChild.Length; j++)
                {
                    objChild[j].gameObject.GetComponent<SpriteRenderer>().sortingOrder = playerOrderLayer + 2;
                }

                obj.GetComponent<SpriteRenderer>().sortingOrder = playerOrderLayer + 1;
            }
            else
            {
                for (var j = 0; j < objChild.Length; j++)
                {
                    objChild[j].gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
                }

                obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
    }

    void Start()
    {
        ChangeOrderLayerObject();
    }
}
