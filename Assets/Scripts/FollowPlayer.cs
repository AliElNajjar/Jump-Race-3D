using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offsetPosition;

    private void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        transform.position = player.TransformPoint(offsetPosition);
        transform.LookAt(player);
    }
}
