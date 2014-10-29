using UnityEngine;
using System.Collections;

public class HandHeldCam : LookatTarget {

	[SerializeField] float swaySpeed = .5f;
	[SerializeField] float baseSwayAmount = .5f;
	[SerializeField] float trackingSwayAmount = .5f;
	[Range(-1,1)][SerializeField] float trackingBias = 0;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	protected override void FollowTarget (float deltaTime)
	{
		base.FollowTarget(deltaTime);

		float bx = (Mathf.PerlinNoise(0,Time.time*swaySpeed)-0.5f);
		float by = (Mathf.PerlinNoise(0,(Time.time*swaySpeed)+100))-0.5f;

		bx *= baseSwayAmount;
		by *= baseSwayAmount;

		float tx = (Mathf.PerlinNoise(0,Time.time*swaySpeed)-0.5f)+trackingBias;
		float ty = ((Mathf.PerlinNoise(0,(Time.time*swaySpeed)+100))-0.5f)+trackingBias;

		tx *= -trackingSwayAmount * followVelocity.x;
		ty *= trackingSwayAmount * followVelocity.y;

		transform.Rotate( bx+tx, by+ty, 0 );

	}
}
