using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class InputManager : MonoBehaviour
{
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event StartTouch OnEndTouch;
    #endregion

    private TouchControls controls;
    private InputAction rotationAction;
    private Vector3 camRotation;
    public float rotationSpeedModifier = 5.0f;
    private Quaternion rotationY;

    private void Awake()
    {
        controls = new TouchControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        rotationAction = controls.Touch.Rotate;
        camRotation = Camera.main.transform.eulerAngles;
    }

    private void Update()
    {
        MoveCamera();
        
    }

    private void MoveCamera()
    {
        Vector2 dragDelta = rotationAction.ReadValue<Vector2>() * rotationSpeedModifier * Time.deltaTime;
        float rotY = -1 * dragDelta.x;
        float rotX = dragDelta.y;

        camRotation += new Vector3(rotX, rotY, 0f);
        camRotation.x.Clamp0360();
        camRotation.x.ClampRef(25f, 75f);
        camRotation.y.Clamp0360();
        Camera.main.transform.localEulerAngles = camRotation;
    }
}
