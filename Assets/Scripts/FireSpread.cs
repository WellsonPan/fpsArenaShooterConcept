using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class FireSpread : MonoBehaviour
{
    public float spreadSpeed;
    public float timeAlive;
    public float spreadSize;

    public float initialDamage;
    public float damageOverTime;

    private float currentTime;
    private Vector3 spreadCalculations;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < spreadSize)
        {
            //Change this so that it just eventually spreads as a whole sphere, but use shaders to make it a flat circle on the ground
            //Use particle system to create smoke and fire effects
            float spreadCalculation = spreadSpeed * Time.deltaTime;
            spreadCalculations = new Vector3(transform.localScale.x + spreadCalculation, transform.localScale.y, transform.localScale.z + spreadCalculation);
            transform.localScale = spreadCalculations;
        }

        if (Time.time > currentTime + timeAlive)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.tag == "SummonedWall")
        {
            collisionInfo.GetComponent<Wall>().OnHit(initialDamage);
        }

        if (collisionInfo.tag == "Player")
        {
            collisionInfo.GetComponent<PlayerStats>().selfDamage(initialDamage);
            //Debug.Log("initialDamage");
        }
    }

    private void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.tag == "SummonedWall")
        {
            collisionInfo.GetComponent<Wall>().OnHit(damageOverTime * Time.deltaTime);
        }

        if (collisionInfo.tag == "Player")
        {
            collisionInfo.GetComponent<PlayerStats>().selfDamage(damageOverTime * Time.deltaTime);
            Debug.Log(collisionInfo.GetComponent<PlayerStats>().playerHealth);
            //Debug.Log("sustainDamage");
        }
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log(initialDamage);   
    }
}
