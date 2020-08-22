using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Threading;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    private float turnSpeedVertical;
    private float turnSpeedHorizontal;
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
    public float slideTime;

    private float xRotation = 0f;

    private float standingHeight;
    private float crouchHeight;
    private float crouchSpeed;
    private float standingSpeed;
    private Vector3 standingArmScale;
    private Vector3 crouchingArmScale;
    private float slidingSpeed;
    private float slidingTurnSpeed;

    private bool isSprinting;
    private bool isCrouching;
    private bool isSliding;

    private float currentTime;

    private float count;
    private Vector3 slideDirection = new Vector3();
    private Vector3 slideMovement = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.detectCollisions = false;
        turnSpeedVertical = turnSpeed;
        turnSpeedHorizontal = turnSpeed;
        standingHeight = transform.localScale.y;
        crouchHeight = transform.localScale.y / 2f;
        standingSpeed = moveSpeed;
        crouchSpeed = moveSpeed / 2f;
        standingArmScale = arm.localScale;
        crouchingArmScale = new Vector3(arm.localScale.x, arm.localScale.y * 2f, arm.localScale.z);
        slidingSpeed = moveSpeed * 3f;
        if(!canMultiJump)
        {
            timesCanMultiJump = 0;
        }
        count = 0;
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
        Slide();
        StopSlide();
        Jump();
        //Debug.Log(isSliding);
    }

    void CameraMovement()
    {
        float mouseY = Input.GetAxis("Mouse Y") * turnSpeedVertical * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        Vector3 direction = new Vector3(0, Input.GetAxis("Mouse X"), 0);
        Vector3 rotation2 = direction * turnSpeedHorizontal * Time.deltaTime;
        transform.eulerAngles += rotation2;
    }

    void PlayerMovement()
    {
        if (!rigidbody.useGravity && !isSliding)
        {
            forwardBackwards = Input.GetAxis("Vertical");

            Sprint();

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

    void Sprint()
    {
        if (forwardBackwards < 0)
        {
            isSprinting = false;
            forwardBackwards *= backPedal;
        }
        else if (forwardBackwards > 0 && Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            isSprinting = true;
            forwardBackwards *= sprintMultiplier;
        }
        else
        {
            isSprinting = false;
        }
    }

    void Crouch()
    {
        if(isGrounded)
        {
            if(Input.GetKey(KeyCode.LeftControl))
            {
                isCrouching = true;
                transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
                if (!isSliding)
                {
                    moveSpeed = crouchSpeed;
                }
                arm.localScale = crouchingArmScale;
            }
            else
            {
                isCrouching = false;
                transform.localScale = new Vector3(transform.localScale.x, standingHeight, transform.localScale.z);
                moveSpeed = standingSpeed;
                arm.localScale = standingArmScale;
            }
        }
    }

    void Slide()
    {
        if(isSprinting)
        {
            if(isCrouching && currentTime < slideTime + Time.time)
            {
                isSliding = true;
                if (count < 1)
                {
                    moveSpeed = slidingSpeed;
                    slideDirection = transform.forward;
                    count++;
                }
                slideMovement = slideDirection * moveSpeed * Time.deltaTime;
                controller.Move(slideMovement);
                currentTime = Time.time;
            }
        }
    }

    void StopSlide()
    {
        if(!isSliding || !isCrouching || Time.time > currentTime + slideTime)
        {
            isSliding = false;
            count = 0;
        }

        if(isSliding && moveSpeed > crouchSpeed)
        {
            moveSpeed *= .9875f;
            Debug.Log(moveSpeed);
            if (moveSpeed < crouchSpeed)
            {
                moveSpeed = crouchSpeed;
                isSliding = false;
                count = 0;
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
