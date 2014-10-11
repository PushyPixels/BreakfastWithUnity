using UnityEngine;
using System.Collections;

//From: http://answers.unity3d.com/questions/195698/stopping-a-rigidbody-at-target.html
[RequireComponent(typeof(Rigidbody))]
public class FollowPositionPhysics : MonoBehaviour
{
	public float toVel = 2.5f;
	public float maxVel = 15.0f;
	public float maxForce = 40.0f;
	public float gain = 5f;
	public Transform target;

	void FixedUpdate()
	{
		Vector3 targetPos = target.position;
		Vector3 dist = targetPos - transform.position;
		// calc a target vel proportional to distance (clamped to maxVel)
		Vector3 tgtVel = Vector3.ClampMagnitude(toVel * dist, maxVel);
		// calculate the velocity error
		Vector3 error = tgtVel - rigidbody.velocity;
		// calc a force proportional to the error (clamped to maxForce)
		Vector3 force = Vector3.ClampMagnitude(gain * error, maxForce);
		rigidbody.AddForce(force);
	}
}