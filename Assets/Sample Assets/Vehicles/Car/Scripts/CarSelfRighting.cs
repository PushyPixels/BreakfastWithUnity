using UnityEngine;
using System.Collections;

public class CarSelfRighting : MonoBehaviour
{

	// Automatically put the car the right way up, if it has come to rest upside-down.

    [SerializeField] private float waitTime = 3f;       	// time to wait before self righting
    [SerializeField] private float velocityThreshold = 1f;  // the velocity below which the car is considered stationary for self-righting
	private float lastOkTime;								// the last time that the car was in an OK state

    void Update ()
    {
        // is the car is the right way up
        if(transform.up.y > 0f || rigidbody.velocity.magnitude > velocityThreshold)
        {
			lastOkTime = Time.time;
        }

		if (Time.time > lastOkTime + waitTime)
		{
			RightCar ();
		}
    }

    // put the car back the right way up:
    void RightCar ()
    {
        // set the correct orientation for the car, and lift it off the ground a little
        transform.position += Vector3.up;
        transform.rotation = Quaternion.LookRotation(transform.forward);
    }
}
