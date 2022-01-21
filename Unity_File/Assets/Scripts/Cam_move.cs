using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_move : MonoBehaviour
{
    public Transform player;
    public Vector3 distance;
    void Start()
    {
        distance = transform.position - player.position;
    }
    void LateUpdate()
    {
        transform.position = player.position + distance;
    }
}
