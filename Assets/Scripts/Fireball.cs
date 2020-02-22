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

    private GameObject arm;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        arm = GameObject.Find("PlayerArm");
        direction = arm.transform.forward;
        currentTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * flightSpeed * Time.deltaTime, Space.Self);

        if(Time.time > currentTime + timeAlive)
        {
            Instantiate(blast, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "SummonedWall")
        {
            Instantiate(blast, transform.position, Quaternion.identity);
            Destroy(gameObject);
            collisionInfo.gameObject.GetComponent<Wall>().OnHit(directDamage);
        }
        else if (collisionInfo.collider.tag == "Player")
        {

        }
        else
        {
            Instantiate(blast, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        //else if(collisionInfo.collider.tag == "Enemy")
        //{
        //    Destroy(gameObject);

        //}
    }
}
