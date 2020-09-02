using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateInteraction : MonoBehaviour
{
    public float interactiveDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(transform.parent.position, transform.forward); //creates a ray from the object's position going towards whatever is forward for the object
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, interactiveDistance)) //eventually add mask for interactable objects
            {
                if (hitInfo.collider.GetComponent<InteractableCheck>())
                {
                    hitInfo.collider.GetComponent<InteractableCheck>().hasBeenInteractedWith = true;
                    //Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
                }
                Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
            }
        }
        //Check if interact button is clicked
        //If clicked, raycast to the object
        //Get the object's interactable component if it has one
        //If it does have one, set the value equal to true
    }
}
