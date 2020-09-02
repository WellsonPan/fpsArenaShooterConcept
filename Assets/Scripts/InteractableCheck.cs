using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCheck : MonoBehaviour
{
    public bool hasBeenInteractedWith;
    public float delayBetweenInteractions;

    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenInteractedWith = false;
        currentTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //if(hasBeenInteractedWith && Time.time >= currentTime + delayBetweenInteractions)
        //{
        //    hasBeenInteractedWith = false;
        //    currentTime = Time.time;
        //}
    }
}
