using UnityEngine;

public class Player : MonoBehaviour
{
    public AudioSource footSteps;

    public float speed = 5f;
    private CharacterController controller;
    private Animator anim;
    private bool isMoving;
    private bool canMove = true;

    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 direction;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove)
        {
            MovementCheck();
            AnimationCheck();
        }
    }

    private void MovementCheck()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontalMovement, 0f, verticalMovement);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothVelocity);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(direction * speed * Time.deltaTime);
        }
    }

    private void AnimationCheck()
    {
        if (direction != Vector3.zero && !isMoving)
        {
            isMoving = true;
            anim.SetBool("isRunning", isMoving);
        }
        else if (direction == Vector3.zero && isMoving)
        {
            isMoving = false;
            anim.SetBool("isRunning", isMoving);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bull"))
        {
            canMove = false;
            anim.SetBool("isDead", true);
            anim.enabled = false;
        }
    }

}
