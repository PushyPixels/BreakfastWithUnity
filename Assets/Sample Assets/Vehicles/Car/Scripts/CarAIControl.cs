using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarAIControl : MonoBehaviour
{
	// This script provides input to the car controller in the same way that the user control script does.
	// As such, it is really 'driving' the car, with no special physics or animation tricks to make the car behave properly.

	// "wandering" is used to give the cars a more human, less robotic feel. They can waver slightly
	// in speed and direction while driving towards their target.
	
	[SerializeField] [Range(0,1)] float cautiousSpeedFactor = 0.05f;            	// percentage of max speed to use when being maximally cautious
	[SerializeField] [Range(0,180)] float cautiousMaxAngle = 50f;               	// angle of approaching corner to treat as warranting maximum caution
	[SerializeField] float cautiousMaxDistance = 100f;		                		// distance at which distance-based cautiousness begins
	[SerializeField] float cautiousAngularVelocityFactor = 30f;                     // how cautious the AI should be when considering its own current angular velocity (i.e. easing off acceleration if spinning!)
	[SerializeField] float steerSensitivity = 0.05f;                           		// how sensitively the AI uses steering input to turn to the desired direction
	[SerializeField] float accelSensitivity = 0.04f;		    		            // How sensitively the AI uses the accelerator to reach the current desired speed
	[SerializeField] float brakeSensitivity = 1f;	                    			// How sensitively the AI uses the brake to reach the current desired speed
	[SerializeField] float lateralWanderDistance = 3f;                       		// how far the car will wander laterally towards its target
	[SerializeField] float lateralWanderSpeed = 0.1f;                       		// how fast the lateral wandering will fluctuate
	[SerializeField] [Range(0,1)] public float accelWanderAmount = 0.1f;            // how much the cars acceleration will wander
	[SerializeField] float accelWanderSpeed = 0.1f;                           		// how fast the cars acceleration wandering will fluctuate
	[SerializeField] BrakeCondition brakeCondition = BrakeCondition.TargetDistance; // what should the AI consider when accelerating/braking?
	[SerializeField] bool driving = false;							                // whether the AI is currently actively driving or stopped.
	[SerializeField] Transform target;	        									// 'target' the target object to aim for.	
	[SerializeField] bool stopWhenTargetReached;    								// should we stop driving when we reach the target?
	[SerializeField] float reachTargetThreshold = 2;    							// proximity to target to consider we 'reached' it, and stop driving.

	// for steering, the car will simply steer directly towards the target,
	// for acceleration and braking, the forward direction of the target is taken into
	// consideration, so the AI will brake (according to cautiousness settings) if
	// the direction of the target is not aligned with the direction of the car.

	
	public enum BrakeCondition {
		
		NeverBrake,
		// the car simply accelerates at full throttle all the time.
		
		TargetDirectionDifference,
		// the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.
		
		TargetDistance,
		// the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to
		// head for a stationary target and come to rest when it arrives there.
	}
	


	float randomPerlin;             // A random value for the car to base its wander on (so that AI cars don't all wander in the same pattern)
    CarController carController;    // Reference to actual car controller we are controlling

	// if this AI car collides with another car, it can take evasive action for a short duration:
	float avoidOtherCarTime;		// time until which to avoid the car we recently collided with
	float avoidOtherCarSlowdown;	// how much to slow down due to colliding with another car, whilst avoiding
	float avoidPathOffset;			// direction (-1 or 1) in which to offset path to avoid other car, whilst avoiding

    void Awake ()
    {
        // get the car controller reference
        carController = GetComponent<CarController>();

        // give the random perlin a random value
		randomPerlin = Random.value*100;
    }

    void FixedUpdate()
    {
		if (target == null || !driving)
		{
			// Car should not be moving,
			// (so use accel/brake to get to zero speed)
			float accel = Mathf.Clamp(-carController.CurrentSpeed,-1,1);
			carController.Move(0,accel);

        } else {
			// we're driving somewhere!

			Vector3 fwd = transform.forward;
			if (rigidbody.velocity.magnitude > carController.MaxSpeed*0.1f)
			{
				fwd = rigidbody.velocity;
			}

			float desiredSpeed = carController.MaxSpeed;

			// now it's time to decide if we should be slowing down...
			switch (brakeCondition)
			{

			case BrakeCondition.TargetDirectionDifference:
			{
				// the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.

				// check out the angle of our target compared to the current direction of the car
				float approachingCornerAngle = Vector3.Angle(target.forward,fwd);

				// also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
				float spinningAngle = rigidbody.angularVelocity.magnitude * cautiousAngularVelocityFactor;
		
				// if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
				float cautiousnessRequired = Mathf.InverseLerp(0, cautiousMaxAngle, Mathf.Max(spinningAngle, approachingCornerAngle));	
				desiredSpeed = Mathf.Lerp(carController.MaxSpeed, carController.MaxSpeed*cautiousSpeedFactor, cautiousnessRequired);
				break;
			}

			case BrakeCondition.TargetDistance:
			{
				// the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to
				// head for a stationary target and come to rest when it arrives there.

				// check out the distance to target
				Vector3 delta = target.position - transform.position;
				float distanceCautiousFactor = Mathf.InverseLerp( cautiousMaxDistance, 0, delta.magnitude );
				
				// also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
				float spinningAngle = rigidbody.angularVelocity.magnitude * cautiousAngularVelocityFactor;
				
				// if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
				float cautiousnessRequired = Mathf.Max ( Mathf.InverseLerp(0, cautiousMaxAngle, spinningAngle), distanceCautiousFactor);	
				desiredSpeed = Mathf.Lerp(carController.MaxSpeed, carController.MaxSpeed*cautiousSpeedFactor, cautiousnessRequired);
				break;
			}

			case BrakeCondition.NeverBrake:
				break; // blarg!
				

			}

			// Evasive action due to collision with other cars:

			// our target position starts off as the 'real' target position
			Vector3 offsetTargetPos = target.position;

			// if are we currently taking evasive action to prevent being stuck against another car:
			if (Time.time < avoidOtherCarTime)
			{
				// slow down if necessary (if we were behind the other car when collision occured)
				desiredSpeed *= avoidOtherCarSlowdown;

				// and veer towards the side of our path-to-target that is away from the other car
				offsetTargetPos += target.right * avoidPathOffset;
			} else {
				// no need for evasive action, we can just wander across the path-to-target in a random way,
				// which can help prevent AI from seeming too uniform and robotic in their driving
				offsetTargetPos += target.right * (Mathf.PerlinNoise( Time.time * lateralWanderSpeed, randomPerlin )*2-1) * lateralWanderDistance;
			}

			// use different sensitivity depending on whether accelerating or braking:
			float accelBrakeSensitivity = (desiredSpeed < carController.CurrentSpeed) ? brakeSensitivity : accelSensitivity;

			// decide the actual amount of accel/brake input to achieve desired speed.
			float accel = Mathf.Clamp((desiredSpeed-carController.CurrentSpeed) * accelBrakeSensitivity,-1,1);

	        // add acceleration 'wander', which also prevents AI from seeming too uniform and robotic in their driving
			// i.e. increasing the accel wander amount can introduce jostling and bumps between AI cars in a race
			accel *= (1-accelWanderAmount) + (Mathf.PerlinNoise( Time.time * accelWanderSpeed, randomPerlin ) * accelWanderAmount);
			
	        // calculate the local-relative position of the target, to steer towards
			Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);

	        // work out the local angle towards the target
			float targetAngle = Mathf.Atan2( localTarget.x, localTarget.z ) * Mathf.Rad2Deg;

	        // get the amount of steering needed to aim the car towards the target
	        float steer = Mathf.Clamp ( targetAngle * steerSensitivity, -1, 1 )  * Mathf.Sign(carController.CurrentSpeed);

	        // feed input to the car controller.
			carController.Move(steer,accel);

			// if appropriate, stop driving when we're close enough to the target.
			if (stopWhenTargetReached && localTarget.magnitude < reachTargetThreshold)
			{
				driving = false;
			}

		}

    }

	void OnCollisionStay(Collision col)
	{
		// detect collision against other cars, so that we can take evasive action
		if (col.rigidbody != null)
		{
			var otherAI = col.rigidbody.GetComponent<CarAIControl>();
			if (otherAI != null)
			{
				// we'll take evasive action for 1 second
				avoidOtherCarTime = Time.time + 1;

				// but who's in front?...
				if (Vector3.Angle(transform.forward, otherAI.transform.position-transform.position) < 90)
				{
					// the other ai is in front, so it is only good manners that we ought to brake...
					avoidOtherCarSlowdown = 0.5f;
				} else {
					// we're in front! ain't slowing down for anybody...
					avoidOtherCarSlowdown = 1;
				}

				// both cars should take evasive action by driving along an offset from the path centre,
				// away from the other car
				var otherCarLocalDelta = transform.InverseTransformPoint(otherAI.transform.position);
				float otherCarAngle = Mathf.Atan2(otherCarLocalDelta.x,otherCarLocalDelta.z);
				avoidPathOffset = lateralWanderDistance * -Mathf.Sign(otherCarAngle);

			}
		}
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
		driving = true;
	}

}
