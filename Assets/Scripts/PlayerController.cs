using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public void Initialize(GameObject character)
    {
        m_animator = character.GetComponent<Animator>();
        m_rigidBody = character.GetComponent<Rigidbody>();
    }

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_jumpForce = 4;

    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;

    private bool m_wasGrounded;
    private bool m_isGrounded;
    private bool ismoving = false;
    private AudioSource jumpSound;
    private float jumpDistance = 0;
    private Vector3 firstPosition;
    private Vector3 secondPosition;

    private List<Collider> m_collisions = new List<Collider>();

    private Touch touch;
    public float rotationSpeedModifier = 5.0f;
    private Quaternion rotationY;
    private Vector3 currentDirection;
    private float firstTouch;
    [SerializeField]
    private float forwardOffset = 40f;

    private TouchControls touchControls;

    void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); }
        
        touchControls = new TouchControls();
    }

    private void Start()
    {
        jumpSound = gameObject.GetComponent<AudioSource>();
        touchControls.Touch.TouchInput;
        input.started += ReadInput;
        input.canceled += (ctx) => readInput = false;
    }

    private void ReadInput(InputAction.CallbackContext obj)
    {
        obj.ToString
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }

    }

    void Update()
    {
        m_animator.SetBool("Grounded", m_isGrounded);
        //if (GameController.SharedInstance.go == true)
        Movement();

        JumpingAndLanding();

        m_wasGrounded = m_isGrounded;

    }

    private void Movement()
    {
        if (!readInput) return;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                firstTouch = touch.position.y;
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                rotationY = Quaternion.Euler(0f, touch.deltaPosition.x * rotationSpeedModifier * Time.fixedDeltaTime, 0f);

                transform.rotation *= rotationY;

                float deltaPositionY = touch.position.y - firstTouch;

                if (deltaPositionY >= forwardOffset || ismoving)
                {
                    ismoving = true;

                    Vector3 direction = new Vector3(0f, 0f, m_moveSpeed * Time.fixedDeltaTime);

                    currentDirection = Vector3.Lerp(currentDirection, direction, 0.5f);

                    transform.Translate(currentDirection, Space.Self);

                }
                else
                {
                    ismoving = false;

                    m_rigidBody.velocity = new Vector3(0, m_rigidBody.velocity.y, 0);

                }

            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                ismoving = false;
            }
        }

    }

    private void JumpingAndLanding()
    {
        if (GameController.SharedInstance.levelComplete.activeSelf == false)
        {
            //To always instantly jump when on platform
            if (m_isGrounded)
            {
                m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            }


            // To animate when reaching/leaving a platform
            if (!m_wasGrounded && m_isGrounded) //Land
            {
                m_animator.SetTrigger("Land");
                CalJumpDistance();

            }

            if (!m_isGrounded && m_wasGrounded) //Jump
            {
                m_animator.SetTrigger("Jump");

                jumpDistance = 0;
                firstPosition = transform.position;

                //jump sound
                jumpSound.Play();

            }
        }

    }

    private void CalJumpDistance()
    {
        secondPosition = transform.position;
        jumpDistance = Vector3.Distance(firstPosition, secondPosition);

        if (jumpDistance >= 20)
        {
            GameController.SharedInstance.JumpComment("longJump");
        }

    }

}
