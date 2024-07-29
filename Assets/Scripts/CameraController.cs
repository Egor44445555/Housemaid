using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;

    void Start()
    {
        if (!player)
        {
            player = FindObjectOfType<Person>().transform;
        }
    }

    void Update()
    {
        pos = player.position;
        pos.z = -10f;
        pos.y = pos.y + 5f;

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}
