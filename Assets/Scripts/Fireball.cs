using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float flightSpeed;
    public float directDamage;
    public float timeAlive;

    private float currentTime;

    public GameObject blast;

    public const float gravity = -.1962f;
    private bool isGrounded;
    public Vector3 velocity;
    public bool isGrenade;
    public float friction;

    public Transform groundCheck;
    public float groundDistance;
    public Rigidbody myRigidbody;

    public LayerMask groundMask;

    private GameObject arm;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        arm = GameObject.Find("PlayerArm");
        direction = arm.transform.forward;
        currentTime = Time.time;
        myRigidbody = GetComponent<Rigidbody>();
        if(isGrenade)
        {
            myRigidbody.useGravity = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrenade)
        {
            isOnGround();
        }

        if(isGrounded)
        {
            direction *= friction;
        }

        transform.Translate(direction * flightSpeed * Time.deltaTime + velocity, Space.Self);

        if(Time.time > currentTime + timeAlive || (isGrounded && !isGrenade))
        {
            Instantiate(blast, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void isOnGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "SummonedWall" && !isGrenade)
        {
            Instantiate(blast, transform.position, Quaternion.identity);
            Destroy(gameObject);
            collisionInfo.gameObject.GetComponent<Wall>().OnHit(directDamage);
        }
        else if (collisionInfo.collider.tag == "Enemy" && !isGrenade)
        {
            Instantiate(blast, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if(!isGrenade)
        {
            Instantiate(blast, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            float zDirAbs = Mathf.Abs(collisionInfo.GetContact(0).normal.z);
            float xDirAbs = Mathf.Abs(collisionInfo.GetContact(0).normal.x);
            
            if (zDirAbs > xDirAbs && !isGrounded)
            {
                direction = new Vector3(direction.x * friction * .6f, direction.y, direction.z * -friction * .6f);
            }
            else if (xDirAbs > zDirAbs && !isGrounded)
            {
                direction = new Vector3(direction.x * -friction * .6f, direction.y, direction.z * friction * .6f);
            }
            else
            {
                
            }
            //Debug.Log(collisionInfo.GetContact(0).normal);
        }
        //else if(collisionInfo.collider.tag == "Enemy")
        //{
        //    Destroy(gameObject);

        //}
    }
}
