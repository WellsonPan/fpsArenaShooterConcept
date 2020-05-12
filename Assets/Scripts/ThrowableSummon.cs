using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableSummon : MonoBehaviour
{
    [Range(0, 1)]
    public int chooseThrowable;

    public GameObject fireball;
    public GameObject molotov;
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
        if (Input.GetMouseButtonDown(0))
        {

            if (Time.time > cooldownTime + fireRate)
            {
                spawnPos = new Vector3(arm.transform.position.x, arm.transform.position.y, arm.transform.position.z);// + (arm.transform.localScale.z / 2.0f));
                cooldownTime = Time.time;
                if (chooseThrowable == 0)
                {
                    Instantiate(fireball, spawnPos, Quaternion.identity);
                }
                else if(chooseThrowable == 1)
                {
                    Instantiate(molotov, spawnPos, Quaternion.identity);
                }
            }
        }
    }
}
