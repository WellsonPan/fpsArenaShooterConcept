using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSummon : MonoBehaviour
{
    public GameObject fireball;
    public GameObject arm;
    Vector3 spawnPos;

    private float cooldownTime;
    public float fireRate;

    // Start is called before the first frame update
    void Start()
    {
        cooldownTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        spawnPos = new Vector3(arm.transform.position.x, arm.transform.position.y, arm.transform.position.z);// + (arm.transform.localScale.z / 2.0f));
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time > cooldownTime + fireRate)
            {
                cooldownTime = Time.time;
                Instantiate(fireball, spawnPos, Quaternion.identity);
            }
        }
    }
}
