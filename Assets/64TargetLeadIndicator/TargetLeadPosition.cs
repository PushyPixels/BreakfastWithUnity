using UnityEngine;
using System.Collections;

public class TargetLeadPosition : MonoBehaviour
{
	public Rigidbody target;
	public float projectileVelocity = 1.0f;
	
	// Update is called once per frame
	void FixedUpdate()
	{
		Vector3 relativeVelocity = target.velocity - Camera.main.rigidbody.velocity;
		Vector3 delta = target.position - Camera.main.transform.position;

		float timeToTarget = AimAhead(delta, relativeVelocity, projectileVelocity);

		transform.position = target.position + target.velocity*timeToTarget;
	}

	// Calculate the time when we can hit a target with a bullet
	// Return a negative time if there is no solution
	float AimAhead(Vector3 delta, Vector3 vr, float muzzleV)
	{
		// Quadratic equation coefficients a*t^2 + b*t + c = 0
		float a = Vector3.Dot(vr, vr) - muzzleV*muzzleV;
		float b = 2f*Vector3.Dot(vr, delta);
		float c = Vector3.Dot(delta, delta);
		
		float det = b*b - 4f*a*c;
		
		// If the determinant is negative, then there is no solution
		if(det > 0f){
			return 2f*c/(Mathf.Sqrt(det) - b);
		} else {
			return -1f;
		}
	}
}
