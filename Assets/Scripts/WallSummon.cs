using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSummon : MonoBehaviour
{
    public float rayLength;
    public LayerMask mask;
    public GameObject wall;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.parent.position, transform.forward); //creates a ray from the object's position going towards whatever is forward for the object
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, rayLength, mask, QueryTriggerInteraction.Ignore))
        {
            //print(hitInfo.collider.gameObject.name);
            //Destroy(hitInfo.collider.gameObject);
            if(Input.GetMouseButtonDown(1))
            {
                Destroy(GameObject.FindGameObjectWithTag("SummonedWall"));
                Instantiate(wall, hitInfo.point, transform.parent.rotation);
            }
            Debug.DrawLine(ray.origin, hitInfo.point, Color.green); //green means hit something
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * rayLength, Color.red); //red means not hit something
        }
    }
}
