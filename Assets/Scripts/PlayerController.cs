using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public Transform arm;
    public GameObject mainCamera;
    public float jumpForce;

    public float sprintMultiplier;
    public float backPedal;
    private float forwardBackwards;
    private float strafe;
    public float airControl;

    public const float gravity = -9.81f;
    public CharacterController controller;
    public Rigidbody rigidbody;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    public LayerMask environmentMask;

    public bool canMultiJump;
    public int timesCanMultiJump;
    private int currentJump;
    private bool canStillJump;

    private bool isGrounded;
    public Vector3 velocity;

    private float xRotation = 0f;

    private float standingHeight;
    private float crouchHeight;
    private float crouchSpeed;
    private float standingSpeed;
    private Vector3 standingArmScale;
    private Vector3 crouchingArmScale;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.detectCollisions = false;
        standingHeight = transform.localScale.y;
        crouchHeight = transform.localScale.y / 2f;
        standingSpeed = moveSpeed;
        crouchSpeed = moveSpeed / 2f;
        standingArmScale = arm.localScale;
        crouchingArmScale = new Vector3(arm.localScale.x, arm.localScale.y * 2f, arm.localScale.z);
        if(!canMultiJump)
        {
            timesCanMultiJump = 0;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        ReEnableController();
        checkForJumpReset();
        CanJump();
        ApplyGravity();
        PlayerMovement();
        CameraMovement();
        Crouch();
        Jump();
        //Debug.Log(transform.forward);
    }

    void CameraMovement()
    {
        float mouseY = Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        Vector3 direction = new Vector3(0, Input.GetAxis("Mouse X"), 0);
        Vector3 rotation2 = direction * turnSpeed * Time.deltaTime;
        transform.eulerAngles += rotation2;
    }

    void PlayerMovement()
    {
        if (!rigidbody.useGravity)
        {
            forwardBackwards = Input.GetAxis("Vertical");

            if (forwardBackwards < 0)
            {
                forwardBackwards *= backPedal;
            }
            else if (forwardBackwards > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                forwardBackwards *= sprintMultiplier;
            }

            Vector3 direction = Input.GetAxis("Horizontal") * transform.right + forwardBackwards * transform.forward;
            Vector3 movement = direction * moveSpeed * Time.deltaTime;
            controller.Move(movement);
        }
        else
        {
            strafe = Input.GetAxis("Horizontal");
            Vector3 force = strafe * transform.right;
            //Vector3 velocityEdited = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
            Vector3 strafeMovement = force * moveSpeed * airControl *  Time.deltaTime + rigidbody.velocity;
            rigidbody.velocity = strafeMovement;
        }
    }

    void Crouch()
    {
        if(isGrounded)
        {
            if(Input.GetKey(KeyCode.LeftControl))
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
                moveSpeed = crouchSpeed;
                arm.localScale = crouchingArmScale;
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, standingHeight, transform.localScale.z);
                moveSpeed = standingSpeed;
                arm.localScale = standingArmScale;
            }
        }
    }

    void CanJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask | environmentMask);
        canStillJump = currentJump < timesCanMultiJump;
    }

    void checkForJumpReset()
    {
        if(isGrounded)
        {
            currentJump = 0;
        }
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && (isGrounded || canStillJump))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            currentJump++;
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = gravity;
        }
    }

    void ReEnableController()
    {
        if(isGrounded)
        {
            controller.enabled = true;
            rigidbody.useGravity = false;
            rigidbody.detectCollisions = false;
            rigidbody.velocity = Vector3.zero;
        }
    }

    private void OnDestroy()
    {
        mainCamera.SetActive(false);
    }
}
