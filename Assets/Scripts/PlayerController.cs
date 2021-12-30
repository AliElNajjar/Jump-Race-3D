using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_jumpForce = 4;

    private Animator animator;
    private Rigidbody rb;

    private bool wasGrounded, isGrounded;
    private bool ismoving = false;
    private AudioSource jumpSound;
    private float jumpDistance = 0;
    private Vector3 firstPosition;
    private Vector3 secondPosition;

    private Touch touch;
    public float rotationSpeedModifier = 5.0f;
    private Quaternion rotationY;
    private Vector3 currentDirection;
    private float firstTouch;
    [SerializeField]
    private float forwardOffset = 40f;
    int counter;

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>(); 
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        jumpSound = gameObject.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void Update()
    {
        //animator.SetBool("Grounded", isGrounded);
        //if (GameController.SharedInstance.go == true)
        Movement();

        JumpingAndLanding();

        wasGrounded = isGrounded;

    }

    private void Movement()
    {
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

                    rb.velocity = new Vector3(0, rb.velocity.y, 0);

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
        //if (game is running)
        //{

        // To animate when reaching/leaving a platform
        if (!wasGrounded && isGrounded) //Land
        {
            animator.SetTrigger("Jump");

            jumpDistance = 0;
            firstPosition = transform.position;
            jumpSound.Play();
            counter++;
            rb.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);

        }

        if (!isGrounded && wasGrounded) //Jump
        {
            //animator.SetTrigger("Land");
            CalJumpDistance();
        }

        //To always instantly jump when on platform
        //if (isGrounded)
        //{
            
        //}
        // }

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
