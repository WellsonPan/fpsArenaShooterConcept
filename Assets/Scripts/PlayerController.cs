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

    private float xRotation = 0f;

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
        CameraMovement();
        Jump();
    }

    void CameraMovement()
    {
        float mouseY = Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        mainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        Vector3 direction = new Vector3(0, Input.GetAxis("Mouse X"), 0);
        Vector3 rotation2 = direction * turnSpeed * Time.deltaTime;
        transform.eulerAngles += rotation2;
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
