using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event StartTouch OnEndTouch;
    #endregion

    private TouchControls controls;
    private InputAction rotationDir;
    private Quaternion rot;

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
        //controls.Touch.PrimaryContact.started += StartTouchPrimary;
        //controls.Touch.PrimaryContact.canceled += EndTouchPrimary;
        rotationDir = controls.Touch.Rotate;
    }

    private void Update()
    {
        ////Camera.main.transform.position += (Vector3)controls.Touch.Rotate.ReadValue<Vector2>() * Time.deltaTime;
        //Vector3 dir = controls.Touch.Rotate.ReadValue<Vector2>();
        //dir.z = 0f;
        ////Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        //rot.SetFromToRotation(Camera.main.transform.forward, dir);
        //Camera.main.transform.LookAt(dir - Camera.main.transform.forward);
    }

    //private void StartTouchPrimary(InputAction.CallbackContext ctx)
    //{
    //    //if (OnStartTouch != null) OnStartTouch();
    //}

    //private void EndTouchPrimary(InputAction.CallbackContext ctx)
    //{
    //    throw new NotImplementedException();
    //}
}
