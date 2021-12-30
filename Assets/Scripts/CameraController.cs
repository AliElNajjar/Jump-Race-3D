using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offsetPosition;

    private void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        //Read Rotation value from InputManager and rotate camera here.
        //player follow camera forward horizontally, camera follows player vertically
    }

    private void LateUpdate()
    {
        transform.parent.position = player.TransformPoint(offsetPosition);
    }
}
