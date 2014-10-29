using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class Wheel : MonoBehaviour
{
	public Transform wheelModel;
    public float Rpm { get; private set; }
    public float MaxRpm { get; private set; }
    public float SkidFactor { get; private set; }
    public bool OnGround { get; private set; }
    public Transform Hub { get; set; }
	public WheelCollider wheelCollider { get; private set; }
	public CarController car { get; private set; }
    public Transform skidTrailPrefab;
    public static Transform skidTrailsDetachedParent;
    public float loQualDist = 100;
    public bool steerable = false;
    public bool powered = false;
    [SerializeField]
    private float particleRate = 3;
    [SerializeField]
    private float slideThreshold = 10f;
  
	
	public float suspensionSpringPos { get; private set; }

    private float spinAngle;
    private float particleEmit;
    private float sidewaysStiffness;
    private float forwardStiffness;
    private float spinoutFactor;
    private float sideSlideFactor;
    private float springCompression;
    private ParticleSystem skidSmokeSystem;
    private Rigidbody rb;
    private WheelFrictionCurve sidewaysFriction;
    private WheelFrictionCurve forwardFriction;
    private Transform skidTrail;
    private bool leavingSkidTrail;
    private RaycastHit hit;
    private Vector3 relativeVelocity;
    private float sideSlideFactorTarget;
    private float spinoutFactorTarget;
    private float accelAmount;
    private float burnoutFactor;
    private float burnoutGrip;
    private float spinoutGrip;
    private float sideSlideGrip;
    private float minGrip;
    private float springCompressionGripModifier;
    private float burnoutRpm;
    private float skidFactorTarget;
    private bool ignore;
	private Vector3 originalWheelModelPosition;

    void Start()
    {
		car = transform.parent.GetComponent<CarController>();
		wheelCollider = collider as WheelCollider;
	
		if (wheelModel != null)
		{
			originalWheelModelPosition = wheelModel.localPosition;
			transform.position = wheelModel.position;// - wheelCollider.suspensionDistance*0.5f*transform.up;
		}

        // store initial starting values of wheelCollider
        sidewaysFriction = wheelCollider.sidewaysFriction;
        forwardFriction = wheelCollider.forwardFriction;
        sidewaysStiffness = wheelCollider.sidewaysFriction.stiffness;
        forwardStiffness = wheelCollider.forwardFriction.stiffness;

        // nominal max rpm at Car's top speed (can be more if Car is rolling downhill!)
        MaxRpm = (car.MaxSpeed / (Mathf.PI * wheelCollider.radius * 2)) * 60;

        // get a reference to the particle system for the tire smoke
        skidSmokeSystem = car.GetComponentInChildren<ParticleSystem>();
        rb = wheelCollider.attachedRigidbody;

        if (skidTrailsDetachedParent == null)
        {
            skidTrailsDetachedParent = new GameObject("Skid Trails - Detached").transform;
        }
    }


    // called in sync with the physics system
    void FixedUpdate()
    {

        // calculate if the wheel is sliding sideways
        relativeVelocity = transform.InverseTransformDirection(rb.velocity);
        
        // sideways slide is considered at maximum if 10% or more of the wheel's velocity is perpendicular to its forward direction:
        sideSlideFactorTarget = Mathf.Clamp01(Mathf.Abs(relativeVelocity.x * slideThreshold / car.MaxSpeed) * (car.SpeedFactor * .5f + .5f));
        sideSlideFactor = sideSlideFactorTarget > sideSlideFactor ? sideSlideFactorTarget : Mathf.Lerp(sideSlideFactor, sideSlideFactorTarget, Time.deltaTime);

        spinoutFactorTarget = Mathf.Clamp01((rb.angularVelocity.magnitude * Mathf.Rad2Deg * .01f) * (((1 - car.SpeedFactor) * .5f) + .5f));
        spinoutFactorTarget = Mathf.Lerp(0, spinoutFactorTarget, car.SpeedFactor + (powered ? car.AccelInput : 0));
        spinoutFactor = spinoutFactorTarget > spinoutFactor ? spinoutFactorTarget : Mathf.Lerp(spinoutFactor, spinoutFactorTarget, Time.deltaTime);
        
        // calculate if burnout slip should be occuring
        accelAmount = (wheelCollider.motorTorque / car.MaxTorque);
        burnoutFactor = 0;
        if (powered)
        {
            burnoutFactor = (accelAmount - (1 - car.BurnoutTendency)) / (1 - car.BurnoutTendency);
        }

        burnoutGrip = Mathf.Lerp(1, 1 - car.BurnoutSlipEffect, burnoutFactor);
        spinoutGrip = Mathf.Lerp(1, 1 - car.SpinoutSlipEffect, spinoutFactor);
        sideSlideGrip = Mathf.Lerp(1, 1 - car.SideSlideEffect, sideSlideFactor);

        // doing it this way stops the GC alloc
        minGrip = Mathf.Min(burnoutGrip, spinoutGrip);
        minGrip = Mathf.Min(sideSlideGrip, minGrip);

        springCompressionGripModifier = springCompression + 0.6f;
        springCompressionGripModifier *= springCompressionGripModifier;

        sidewaysFriction.stiffness = sidewaysStiffness * minGrip * springCompressionGripModifier;
        forwardFriction.stiffness = forwardStiffness * burnoutGrip * springCompressionGripModifier;

        wheelCollider.sidewaysFriction = sidewaysFriction;
        wheelCollider.forwardFriction = forwardFriction;

        burnoutRpm = (car.MaxSpeed * car.BurnoutTendency / (Mathf.PI * wheelCollider.radius * 2)) * 60;

        Rpm = burnoutRpm > wheelCollider.rpm ? Mathf.Lerp(wheelCollider.rpm, burnoutRpm, burnoutFactor) : wheelCollider.rpm;

        // if the car is on the ground deal with skid trails and ture smoke
        if (OnGround)
        {
            // overall skid factor, for drawing skid particles
            skidFactorTarget = Mathf.Max(burnoutFactor * 2, sideSlideFactor * rb.velocity.magnitude * .05f);
            skidFactorTarget = Mathf.Max(skidFactorTarget, spinoutFactor * rb.velocity.magnitude * .05f);
            skidFactorTarget = Mathf.Clamp01(-.1f + skidFactorTarget * 1.1f);
            SkidFactor = Mathf.MoveTowards(SkidFactor, skidFactorTarget, Time.deltaTime * 2);

            if (skidSmokeSystem != null)
            {
                particleEmit += SkidFactor * Time.deltaTime;
                if (particleEmit > particleRate)
                {
                    particleEmit = 0;
                    skidSmokeSystem.transform.position = transform.position - transform.up * wheelCollider.radius;
                    skidSmokeSystem.Emit(1);
                }
            }
        }


        if (skidTrailPrefab != null)
        {
            if (SkidFactor > 0.5f && OnGround)
            {
                if (!leavingSkidTrail)
                {
                    skidTrail = Instantiate(skidTrailPrefab) as Transform;
                    if (skidTrail != null)
                    {
                        skidTrail.parent = transform;
                        skidTrail.localPosition = -Vector3.up * (wheelCollider.radius - 0.1f);
                    }
                    leavingSkidTrail = true;
                }

            }
            else
            {
                if (leavingSkidTrail)
                {
                    skidTrail.parent = skidTrailsDetachedParent;
                    Destroy(skidTrail.gameObject, 10);
                    leavingSkidTrail = false;
                }
            }
        }

        // *6 converts RPM to Degrees per second (i.e. *360 and /60 )
        spinAngle += Rpm * 6 * Time.deltaTime;


        var distToCamSq = (Camera.main.transform.position - transform.position).sqrMagnitude;

        var checkThisFrame = true;

        if (distToCamSq > loQualDist * loQualDist)
        {
            // far from camera - use infrequent ground detection, once every five frames
            var checkProbability = Mathf.Lerp(1, 0.2f, Mathf.InverseLerp(loQualDist * loQualDist, loQualDist * loQualDist * 4, distToCamSq));
            checkThisFrame = Random.value < checkProbability;
        }

        if (!checkThisFrame)
            return;

        if (Physics.Raycast(transform.position, -transform.up, out hit, wheelCollider.suspensionDistance + wheelCollider.radius))
        {
            suspensionSpringPos = -(hit.distance - wheelCollider.radius);
            springCompression = Mathf.InverseLerp(-wheelCollider.suspensionDistance, wheelCollider.suspensionDistance, suspensionSpringPos);
            OnGround = true;
        }
        else
        {
			suspensionSpringPos = -(wheelCollider.suspensionDistance);
            OnGround = false;
            springCompression = 0;
            SkidFactor = 0;
        }

		// update wheel model position and rotation
		if (wheelModel != null)
		{
			wheelModel.localPosition = originalWheelModelPosition + Vector3.up * suspensionSpringPos;
			wheelModel.localRotation = Quaternion.AngleAxis(wheelCollider.steerAngle, Vector3.up) * Quaternion.Euler(spinAngle, 0, 0);
		}
	}
}



