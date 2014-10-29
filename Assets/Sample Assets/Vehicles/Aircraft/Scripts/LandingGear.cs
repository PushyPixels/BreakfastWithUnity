using UnityEngine;
using System.Collections;

public class LandingGear : MonoBehaviour {

	// The landing gear can be raised and lowered at differing altitudes.
	// The gear is only lowered when descending, and only raised when climbing.

	// this script detects the raise/lower condition and sets a parameter on
	// the animator to actually play the animation to raise or lower the gear.

	public float raiseAtAltitude = 40;
	public float lowerAtAltitude = 40;

	GearState state = GearState.Lowered;
	Animator animator;

	enum GearState
	{
		Raised = -1,
		Lowered = 1
	}

	AeroplaneController plane;

	// Use this for initialization
	void Start () {
		plane = GetComponent<AeroplaneController>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == GearState.Lowered && plane.Altitude > raiseAtAltitude && rigidbody.velocity.y > 0)
		{
			state = GearState.Raised;
		}

		if (state == GearState.Raised && plane.Altitude < lowerAtAltitude && rigidbody.velocity.y < 0)
		{
			state = GearState.Lowered;
		}

		// set the parameter on the animator controller to trigger the appropriate animation
		animator.SetInteger("GearState",(int)state);

	}
}
