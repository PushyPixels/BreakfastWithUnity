using UnityEngine;

[RequireComponent(typeof(AeroplaneController))]
public class AeroplaneAIControl : MonoBehaviour {

	// This script represents an AI 'pilot' capable of flying the plane towards a designated target.
	// It sends the equivalent of the inputs that a user would send to the Aeroplane controller.

	[SerializeField] float rollSensitivity = .2f;                     // How sensitively the AI applies the roll controls
	[SerializeField] float pitchSensitivity = .5f;                    // How sensitively the AI applies the pitch controls
	[SerializeField] float lateralWanderDistance = 5;                 // The amount that the plane can wander by when heading for a target
	[SerializeField] float lateralWanderSpeed = 0.11f;                // The speed at which the plane will wander laterally
	[SerializeField] float maxClimbAngle = 45;                        // The maximum angle that the AI will attempt to make plane can climb at
	[SerializeField] float maxRollAngle = 45;                         // The maximum angle that the AI will attempt to u
	[SerializeField] float speedEffect = 0.01f;                       // This increases the effect of the controls based on the plane's speed.
	[SerializeField] float takeoffHeight = 20;						  // the AI will fly straight and only pitch upwards until reaching this height
	[SerializeField] Transform target;								  // the target to fly towards

	private AeroplaneController aeroplaneController;           // The aeroplane controller that is used to move the plane
	private float randomPerlin;                                // Used for generating random point on perlin noise so that the plane will wander off path slightly
	private bool takenOff;                               		// Has the plane taken off yet


    // setup script properties
	void Awake ()
	{
		// get the reference to the aeroplane controller, so we can send move input to it and read its current state.
		aeroplaneController = GetComponent<AeroplaneController>();

		// pick a random perlin starting point for lateral wandering
	    randomPerlin = Random.Range (0f, 100f);
	}
	

    // reset the object to sensible values
	public void Reset ()
	{
		takenOff = false;
	}

	
    // fixed update is called in time with the physics system update
	void FixedUpdate()
	{

		if (target != null)
		{

	        // make the plane wander from the path, useful for making the AI seem more human, less robotic.
			Vector3 targetPos = target.position + transform.right * (Mathf.PerlinNoise( Time.time * lateralWanderSpeed, randomPerlin )*2-1) * lateralWanderDistance;

	        // adjust the yaw and pitch towards the target
			Vector3 localTarget = transform.InverseTransformPoint(targetPos);
			float targetAngleYaw = Mathf.Atan2( localTarget.x, localTarget.z );
			float targetAnglePitch = -Mathf.Atan2( localTarget.y, localTarget.z );

		
	        // Set the target for the planes pitch, we check later that this has not passed the maximum threshold
			targetAnglePitch = Mathf.Clamp(targetAnglePitch,-maxClimbAngle* Mathf.Deg2Rad,maxClimbAngle* Mathf.Deg2Rad);

			// calculate the difference between current pitch and desired pitch
			float changePitch = targetAnglePitch - aeroplaneController.PitchAngle;

	       	// AI always applies gentle forward throttle
			float throttleInput = 0.5f; 

			// AI applies elevator control (pitch, rotation around x) to reach the target angle
			float pitchInput = changePitch * pitchSensitivity;

	        // clamp the planes roll
		    float desiredRoll = Mathf.Clamp (targetAngleYaw, -maxRollAngle*Mathf.Deg2Rad, maxRollAngle*Mathf.Deg2Rad);
			float yawInput = 0;
			float rollInput = 0;
	        if (!takenOff) {
				// if the plane hasn't taken off don't allow it to roll or yaw, only climb

				desiredRoll = 0;

				// If the planes altitude is above takeoffHeight we class this as taken off
				if (aeroplaneController.Altitude > takeoffHeight) {
					takenOff = true;
				}

			} else {

				// now we have taken off to a safe height, we can use the rudder and ailerons to yaw and roll
				yawInput = targetAngleYaw;
				rollInput = -(aeroplaneController.RollAngle - desiredRoll) * rollSensitivity;

			}

			// adjust how fast the AI is changing the controls based on the speed. Faster speed = faster on the controls.
			float currentSpeedEffect = 1 + (aeroplaneController.ForwardSpeed * speedEffect);
			rollInput *= currentSpeedEffect;
			pitchInput *= currentSpeedEffect;
			yawInput *= currentSpeedEffect;

	        // pass the current input to the plane (false = because AI never uses air brakes!)
			aeroplaneController.Move(rollInput, pitchInput, yawInput, throttleInput, false );

		} else {

			// no target set, send zeroed input to the planeW
			aeroplaneController.Move (0,0,0,0, false );

		}
	}

	// allows other scripts to set the plane's target
	public void SetTarget(Transform target)
	{
		this.target = target;
	}

}
