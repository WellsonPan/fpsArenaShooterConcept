using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour
{
    public float flightSpeed;

    private Vector3 direction;
    private Vector3 fireSpawnPos;
    private GameObject arm;
    private GameObject ground;
    private GameObject fakeGround;

    public GameObject fireSpread;

    public LayerMask groundMask;

    private bool isQuitting;

    // Start is called before the first frame update
    void Start()
    {
        arm = GameObject.Find("PlayerArm");
        ground = GameObject.Find("Real Ground");
        fakeGround = GameObject.Find("Visual Ground");
        direction = arm.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * flightSpeed * Time.fixedDeltaTime, Space.Self);
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if(!isQuitting)
        {
            fireSpawnPos = new Vector3(transform.position.x, fakeGround.transform.position.y, transform.position.z);
            Instantiate(fireSpread, fireSpawnPos, Quaternion.identity);
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
}
