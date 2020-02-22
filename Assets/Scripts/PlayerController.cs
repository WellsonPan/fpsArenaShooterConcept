using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public Transform arm;
    public Transform mainCamera;
    public float jumpForce;

    public const float gravity = -9.81f;
    public CharacterController controller;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    public LayerMask environmentMask;

    private bool isGrounded;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        //These two should be in a UI class
        //Fix this stuff idiot
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        CanJump();
        ApplyGravity();
        PlayerMovement();
        CameraConstraintCheck(); //Work on this
        CameraMovement();
        Jump();
    }

    void CameraMovement()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Mouse Y") * -1f, 0, 0);
        Vector3 rotation = direction * turnSpeed * Time.deltaTime;
        mainCamera.eulerAngles += rotation;

        Vector3 direction2 = new Vector3(0, Input.GetAxis("Mouse X"), 0);
        Vector3 rotation2 = direction2 * turnSpeed * Time.deltaTime;
        transform.eulerAngles += rotation2;
    }

    void CameraConstraintCheck()
    {
        if (mainCamera.eulerAngles.x > 70 && !(mainCamera.eulerAngles.x > 90))
        {
            mainCamera.eulerAngles = new Vector3(70f, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);
        }

        //Debug.Log(transform.eulerAngles.x);
        if (mainCamera.eulerAngles.x < 290 && !(mainCamera.eulerAngles.x < 270))
        {
            mainCamera.eulerAngles = new Vector3(290f, mainCamera.eulerAngles.y, 0);
        }
    }

    void PlayerMovement()
    {
        Vector3 direction = Input.GetAxis("Horizontal") * transform.right +  Input.GetAxis("Vertical") * transform.forward;
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        //transform.Translate(movement);
        controller.Move(movement);
    }

    void CanJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask | environmentMask);
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
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
}
