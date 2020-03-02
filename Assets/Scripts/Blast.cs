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

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.tag == "SummonedWall")
        {
            collisionInfo.gameObject.GetComponent<Wall>().OnHit(blastDamage);
            //Debug.Log(collisionInfo.gameObject.GetComponent<Wall>().health);
        }

        //For some reason it doesn't detect collision with the character controller even if another collider is attached to the object
        //But it does if a rigidbody is attached
        if (collisionInfo.tag == "Player")
        {
            Vector3 closestPointOnPlayerToTheExplosion = collisionInfo.ClosestPoint(transform.position);

            //Get closest point on the player to the center of the explosion. Coincidentally also gives a direction from the center of the blast to the closest point on the player
            float distance = Vector3.Distance(transform.position, closestPointOnPlayerToTheExplosion);

            //Subtracts the distance from the radius to get a value between 0 and 1 with the help of clamp
            float lerp = Mathf.Clamp((blastRadius * sphereCollider.radius) - distance, .25f, 1f);

            //Fine tuning on the damage numbers along with the lerp to interpolate the damage from the edge to the center of the explosion
            float damage = Mathf.Floor(lerp * blastDamage * sphereCollider.radius);

            //Apply damage
            collisionInfo.GetComponent<PlayerStats>().selfDamage(damage);

            Debug.Log(damage);
        }
    }
}
