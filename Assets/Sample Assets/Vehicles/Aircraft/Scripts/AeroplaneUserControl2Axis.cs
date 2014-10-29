using UnityEngine;

[RequireComponent(typeof(AeroplaneController))]
public class AeroplaneUserControl2Axis : MonoBehaviour
{

	// these max angles are only used on mobile, due to the way pitch and roll input are handled
	public float maxRollAngle = 80;
	public float maxPitchAngle = 80;

	// reference to the aeroplane that we're controlling
	private AeroplaneController aeroplane;

    void Awake ()
    {
        // Set up the reference to the aeroplane controller.
        aeroplane = GetComponent<AeroplaneController>();
    }

    void FixedUpdate()
    {
        // Read input for the pitch, yaw, roll and throttle of the aeroplane.
		#if CROSS_PLATFORM_INPUT
		float roll = CrossPlatformInput.GetAxis("Horizontal");
        float pitch = CrossPlatformInput.GetAxis("Vertical");
		bool airBrakes = CrossPlatformInput.GetButton("Fire1");
		#else
		float roll = Input.GetAxis("Horizontal");
		float pitch = Input.GetAxis("Vertical");
		bool airBrakes = Input.GetButton("Fire1");
		#endif

		// auto throttle up, or down if braking.
		float throttle = airBrakes ? -1 : 1;

		AdjustInputForMobileControls(ref roll, ref pitch, ref throttle);


        // Pass the input to the aeroplane
        aeroplane.Move(roll, pitch, 0, throttle, airBrakes);
    }

	private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
    {
		#if MOBILE_INPUT
		// because mobile tilt is used for roll and pitch, we help out by
		// assuming that a centered level device means the user
		// wants to fly straight and level! 

		// this means on mobile, the input represents the *desired* roll angle of the aeroplane,
		// and the roll input is calculated to achieve that.
		// whereas on non-mobile, the input directly controls the roll of the aeroplane.

		float intendedRollAngle = roll * maxRollAngle * Mathf.Deg2Rad;
		float intendedPitchAngle = pitch * maxPitchAngle * Mathf.Deg2Rad;
		roll = Mathf.Clamp( (intendedRollAngle - aeroplane.RollAngle) , -1, 1);
		pitch = Mathf.Clamp( (intendedPitchAngle - aeroplane.PitchAngle) , -1, 1);

		// similarly, the throttle axis input is considered to be the desired absolute value, not a relative change to current throttle.
		float intendedThrottle = throttle * 0.5f + 0.5f;
		throttle = Mathf.Clamp(intendedThrottle - aeroplane.Throttle, -1, 1);
		#endif
    }
}
