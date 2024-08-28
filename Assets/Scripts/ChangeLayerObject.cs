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
        float playerPositionY = player.transform.position.y;
        int playerOrderLayer = player.GetComponent<SpriteRenderer>().sortingOrder;

        foreach (GameObject obj in gameObjects)
        {
            float objectPositionY = obj.transform.position.y + 1f;
            Transform[] objChild = obj.transform.GetComponentsInChildren<Transform>();

            if (playerPositionY > objectPositionY)
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
