using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    public float blastDamage;
    public float blastRadius;
    public float timeAlive;
    public float blastForce;

    private float timeExisting;
    private SphereCollider sphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 radius = new Vector3(blastRadius, blastRadius, blastRadius);
        transform.localScale = radius;
        timeExisting = Time.time;
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        float newTime = Time.time;
        if(newTime > timeExisting + timeAlive)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.collider.tag == "SummonedWall")
        {
            collisionInfo.gameObject.GetComponent<Wall>().OnHit(blastDamage);
        }

        //For some reason it doesn't detect collision with the character controller even if another collider is attached to the object
        //But it does if a rigidbody is attached
        if (collisionInfo.collider.tag == "Player")
        {
            float rad = collisionInfo.gameObject.GetComponent<CharacterController>().radius; //Get the radius of the character body
            float distance = Vector3.Distance(collisionInfo.transform.position, transform.position); //Gets distance from center of character to center of explosion
            float lerp = 1 - Mathf.Abs((blastRadius * sphereCollider.radius + rad) - distance); //Gets a decimal value to interpolate from full damage to half damage
            float damage = Mathf.FloorToInt(blastDamage * lerp * rad) - 1f; //Fine tuning on damage value
            //Debug.Log(damage);
            collisionInfo.gameObject.GetComponent<PlayerStats>().selfDamage(damage); //Apply damage
        }
    }
}
