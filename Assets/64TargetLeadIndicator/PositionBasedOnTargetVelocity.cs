using UnityEngine;
using System.Collections;

public class PositionBasedOnTargetVelocity : MonoBehaviour
{
	public Rigidbody target;
	public float time = 1.0f;
	
	// Update is called once per frame
	void FixedUpdate()
	{
		transform.position = target.position + target.velocity*time;
	}
}
