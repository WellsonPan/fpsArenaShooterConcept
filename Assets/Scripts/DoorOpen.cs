using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private InteractableCheck doorCheck;
    private Rigidbody doorBody;
    public Vector3 doorOpenSpeed;
    public Vector3 doorCloseSpeed;
    public bool open;
    public float doorOpenThreshold;
    public float doorCloseThreshold;

    // Start is called before the first frame update
    void Start()
    {
        doorCheck = GetComponent<InteractableCheck>();
        doorBody = GetComponent<Rigidbody>();
        open = false;
        doorBody.isKinematic = true;
    }

    void DoorInteraction()
    {
        if (doorCheck.hasBeenInteractedWith)
        {
            doorBody.isKinematic = false;
            if (!open)
            {
                doorBody.AddForce(doorOpenSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
                if (transform.rotation.z > doorOpenThreshold)
                {
                    doorCheck.hasBeenInteractedWith = false;
                    doorBody.velocity = Vector3.zero;
                    doorBody.angularVelocity = Vector3.zero;
                    doorBody.isKinematic = true;
                    open = true;
                }
            }
            else if (open)
            {
                doorBody.AddForce(doorCloseSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
                if (transform.rotation.z < doorCloseThreshold)
                {
                    //Debug.Log("asmlkdfjkflsda");
                    doorCheck.hasBeenInteractedWith = false;
                    doorBody.velocity = Vector3.zero;
                    doorBody.angularVelocity = Vector3.zero;
                    doorBody.isKinematic = true;
                    open = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        DoorInteraction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
