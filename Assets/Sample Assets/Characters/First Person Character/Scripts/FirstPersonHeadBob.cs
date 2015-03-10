using UnityEngine;
using System.Collections;

public class FirstPersonHeadBob : MonoBehaviour {

	// Head bobbing is controlled by a sine wave and the character's speed, modulated by a handful of values.
	// This script also controls sound effects for footsteps, landing and jumping
	// (because the footsteps are tied to the head bob cycle)
	// When jumping or landing, the head also moves and tilts based on some simple springy calculations.

	[SerializeField] Transform head;							// the object to which the head-bob movement should be applied

	// these modulate the head bob movement
	[SerializeField] float headBobFrequency = 1.5f;				// the base speed of the head bobbing (in cycles per metre)
	[SerializeField] float headBobHeight = 0.3f;				// the height range of the head bob
	[SerializeField] float headBobSwayAngle = 0.5f;				// the angle which the head tilts to left & right during the bob cycle
	[SerializeField] float headBobSideMovement = 0.05f;			// the distance the head moves to left & right during the bob cycle
	[SerializeField] float bobHeightSpeedMultiplier = 0.3f;		// the amount the bob height increases as the character's speed increases (for a good 'run' effect compared with walking)
	[SerializeField] float bobStrideSpeedLengthen = 0.3f;		// the amount the stride lengthens based on speed (so that running isn't like a silly speedwalk!)

	// these control the amount of movement applied to the head when the character jumps or lands
	[SerializeField] float jumpLandMove = 3;
	[SerializeField] float jumpLandTilt = 60;
	
	// audio clip references
	[SerializeField] AudioClip[] footstepSounds;		// an array of footstep sounds that will be randomly selected from.
	[SerializeField] AudioClip jumpSound;				// the sound played when character leaves the ground.
	[SerializeField] AudioClip landSound;				// the sound played when character touches back on ground.

	// private vars:
	FirstPersonCharacter character;						// a reference to the First Person Character component (on the parent gameobject)
	Vector3 originalLocalPos;							// the original local position of this gameobject at Start

	float nextStepTime = 0.5f;									// the time at which the next footstep sound is due to occur
	float headBobCycle = 0;								// the current position through the headbob cycle
	float headBobFade = 0;								// the current amount to which the head bob position is being applied or not (it is faded out when the character is not moving)


	// Fields for simple spring calculation:
	float springPos = 0;
	float springVelocity = 0;
	float springElastic = 1.1f;		
	float springDampen = 0.8f;
	float springVelocityThreshold = 0.05f;
	float springPositionThreshold = 0.05f;


	Vector3 prevPosition;								// the position from last frame
	Vector3 prevVelocity = Vector3.zero;				// the velocity from last frame
	bool prevGrounded = true;							// whether the character was grounded last frame


	// Use this for initialization
	void Start () {

		originalLocalPos = head.localPosition;
		character = GetComponent<FirstPersonCharacter>();
		if (GetComponent<AudioSource>() == null)
		{
			// we automatically add an audiosource, if one has not been manually added.
			// (if you want to control the rolloff or other audio settings, add an audiosource manually)
			gameObject.AddComponent<AudioSource>();
		}
		prevPosition = GetComponent<Rigidbody>().position;
	}


	
	// Update is called once per frame
	void FixedUpdate () {

		// we use the actual distance moved as the velocity since last frame, rather than reading
		//the rigidbody's velocity, because this prevents the 'running against a wall' effect.
		Vector3 velocity = (GetComponent<Rigidbody>().position - prevPosition) / Time.deltaTime;
		Vector3 velocityChange = velocity - prevVelocity;
		prevPosition = GetComponent<Rigidbody>().position;
		prevVelocity = velocity;

                  // vertical head position "spring simulation" for jumping/landing impacts
		springVelocity -= velocityChange.y;							// input to spring from change in character Y velocity
		springVelocity -= springPos*springElastic;					// elastic spring force towards zero position
		springVelocity *= springDampen;								// damping towards zero velocity
		springPos += springVelocity * Time.deltaTime;				// output to head Y position
		springPos = Mathf.Clamp( springPos, -.3f, .3f );			// clamp spring distance

		// snap spring values to zero if almost stopped:
		if (Mathf.Abs(springVelocity) < springVelocityThreshold && Mathf.Abs (springPos) < springPositionThreshold)
		{
			springVelocity = 0;
			springPos = 0;
		}

		// head bob cycle is based on "flat" velocity (i.e. excluding Y)
		float flatVelocity = new Vector3(velocity.x,0,velocity.z).magnitude;

		// lengthen stride based on speed (so run bobbing isn't lots of little steps)
		float strideLengthen = 1 + (flatVelocity * bobStrideSpeedLengthen);

		// increment cycle
		headBobCycle += (flatVelocity / strideLengthen) * (Time.deltaTime / headBobFrequency);

		// actual bobbing and swaying values calculated using Sine wave
		float bobFactor = Mathf.Sin(headBobCycle*Mathf.PI*2); 
		float bobSwayFactor = Mathf.Sin(headBobCycle*Mathf.PI*2 + Mathf.PI*.5f); // sway is offset along the sin curve by a quarter-turn in radians
		bobFactor = 1-(bobFactor*.5f+1); // bob value is brought into 0-1 range and inverted
		bobFactor *= bobFactor;	// bob value is biased towards 0

		// fade head bob effect to zero if not moving
		if (new Vector3(velocity.x,0,velocity.z).magnitude < 0.1f)
		{
			headBobFade = Mathf.Lerp(headBobFade,0,Time.deltaTime);
		} else {
			headBobFade = Mathf.Lerp(headBobFade,1,Time.deltaTime);
		}

		// height of bob is exaggerated based on speed
		float speedHeightFactor = 1 + (flatVelocity * bobHeightSpeedMultiplier);

		// finally, set the position and rotation values
		float xPos = -headBobSideMovement * bobSwayFactor;
		float yPos = springPos * jumpLandMove + bobFactor*headBobHeight*headBobFade*speedHeightFactor;
		float xTilt = -springPos*jumpLandTilt;
		float zTilt = bobSwayFactor*headBobSwayAngle*headBobFade;
		head.localPosition = originalLocalPos + new Vector3(xPos, yPos, 0);
		head.localRotation = Quaternion.Euler(xTilt,0,zTilt);


		// Play audio clips based on leaving ground/landing and head bob cycle
		if (character.grounded )
		{
			if (!prevGrounded)
			{
				GetComponent<AudioSource>().clip = landSound;
				GetComponent<AudioSource>().Play();
				nextStepTime = headBobCycle + .5f;
				
			} else {
				
				if ( headBobCycle > nextStepTime)
				{
					// time for next footstep sound:

					nextStepTime = headBobCycle + .5f;
					
					// pick & play a random footstep sound from the array,
					// excluding sound at index 0
					int n = Random.Range(1,footstepSounds.Length);
					GetComponent<AudioSource>().clip = footstepSounds[n];
					GetComponent<AudioSource>().Play();

					// move picked sound to index 0 so it's not picked next time
					footstepSounds[n] = footstepSounds[0];
					footstepSounds[0] = GetComponent<AudioSource>().clip;

				}
			}
			prevGrounded = true;
			
		} else {
			
			if (prevGrounded)
			{
				GetComponent<AudioSource>().clip = jumpSound;
				GetComponent<AudioSource>().Play();
			}
			prevGrounded = false;
		}
	}
}
