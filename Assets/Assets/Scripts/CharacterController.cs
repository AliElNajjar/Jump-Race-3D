using UnityEngine;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour
{
    public void Initialize(GameObject character)
    {
        m_animator = character.GetComponent<Animator>();
        m_rigidBody = character.GetComponent<Rigidbody>();
    }

    [SerializeField] private float m_moveSpeed = 2;
    //[SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;

    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    public float interpolation;
    private Vector3 m_Move;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    //private float m_jumpTimeStamp = 0;
    //private float m_minJumpInterval = 0.25f;

    private bool m_isGrounded;

    private AudioSource jumpSound;

    //private float jumpTimer = 0;
    private float jumpDistance = 0;
    private Vector3 firstPosition;
    private Vector3 secondPosition;

    private List<Collider> m_collisions = new List<Collider>();

    void Awake()
    {
        if(!m_animator) { gameObject.GetComponent<Animator>(); }
        if(!m_rigidBody) { gameObject.GetComponent<Animator>(); }
    }

    private void Start()
    {
        jumpSound = gameObject.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for(int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider)) {
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

        if(validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        } else
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
        if(m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }

    }

	void FixedUpdate ()
    {
        m_animator.SetBool("Grounded", m_isGrounded);
                
        if (GameController.SharedInstance.go == true)
        Movement();

        JumpingAndLanding();

        m_wasGrounded = m_isGrounded;

    }

    private void Movement()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        //no backwards
        v = Mathf.Clamp01(v);

        Transform camera = Camera.main.transform;

        //m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        //m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * v + camera.right * h;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized *directionLength;

        if(direction != Vector3.zero)
        {
            //m_currentDirection = Vector3.Lerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);
            m_currentDirection = Vector3.Lerp(m_currentDirection, direction, Time.deltaTime * interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            //transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;
            m_Move = (v * Vector3.forward) * m_moveSpeed * Time.deltaTime;
            
            /*direction = camera.forward * v;
            m_currentDirection = Vector3.Lerp(m_currentDirection, direction, Time.deltaTime * interpolation);
            m_Move = (m_currentDirection) * m_moveSpeed * Time.deltaTime;*/
            
            m_rigidBody.transform.Translate(m_Move);

            //m_animator.SetFloat("MoveSpeed", direction.magnitude);
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
