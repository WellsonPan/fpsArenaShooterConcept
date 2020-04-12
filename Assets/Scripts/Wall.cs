using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wall : MonoBehaviour
{
    public float health;
    public float summonSpeed;

    private Vector3 aboveGround;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2) - .1f, transform.position.z);
        aboveGround = new Vector3(0, summonSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 aboveGround = new Vector3(0, summonSpeed, 0);
        //if (transform.position.y <= ((transform.localScale.y/2.0f) - (transform.localScale.y / 24.0f)))
        //{
        //    transform.Translate(aboveGround * Time.deltaTime, Space.Self);
        //}

        //Debug.Log("Wall: " + health);

        if(health <= 0)
        {
            Destroy(gameObject);
        }

    }

    void FixedUpdate()
    {
        if (transform.position.y <= ((transform.localScale.y / 2.0f) - (transform.localScale.y / 4.0f)))
        {
            transform.Translate(aboveGround * Time.deltaTime, Space.Self);
        }
    }

    public void OnHit(float damage)
    {
        health -= damage;
    }
}
