using UnityEngine;

public class AeroplaneAudio : MonoBehaviour
{
	[SerializeField] AudioClip engineSound;                     		// Looped engine sound, whose pitch and volume are affected by the plane's throttle setting.
	[SerializeField] float engineMinThrottlePitch = 0.4f;				// Pitch of the engine sound when at minimum throttle.
	[SerializeField] float engineMaxThrottlePitch = 2f;					// Pitch of the engine sound when at maximum throttle.
	[SerializeField] float engineFwdSpeedMultiplier = 0.002f;       	// Additional multiplier for an increase in pitch of the engine from the plane's speed.

	[SerializeField] AudioClip windSound;                   			// Looped wind sound, whose pitch and volume are affected by the plane's velocity.
	[SerializeField] float windBasePitch = 0.2f;						// starting pitch for wind (when plane is at zero speed)
	[SerializeField] float windSpeedPitchFactor = 0.004f;       		// Relative increase in pitch of the wind from the plane's speed.
	[SerializeField] float windMaxSpeedVolume = 100;					// the speed the aircraft much reach before the wind sound reaches maximum volume.

    private AudioSource engineSoundSource;                              // Reference to the AudioSource for the engine.
    private AudioSource windSoundSource;                            	// Reference to the AudioSource for the wind.
    private AeroplaneController plane;                                  // Reference to the aeroplane controller.

	[SerializeField] private AdvancedSetttings advanced = new AdvancedSetttings(); // container to make advanced settings appear as rollout in inspector

    [System.Serializable]
    public class AdvancedSetttings                                      // A class for storing the advanced options.
    {
        public float engineMinDistance = 50f;                           // The min distance of the engine audio source.
        public float engineMaxDistance = 1000f;                         // The max distance of the engine audio source.
        public float engineDopplerLevel = 1f;                           // The doppler level of the engine audio source.
        [Range(0f, 1f)] public float engineMasterVolume = 0.5f;       	// An overall control of the engine sound volume.
        public float windMinDistance = 10f;                         	// The min distance of the wind audio source. 
        public float windMaxDistance = 100f;                       		// The max distance of the wind audio source.
        public float windDopplerLevel = 1f;                         	// The doppler level of the wind audio source.					
		[Range(0f, 1f)] public float windMasterVolume = 0.5f;       	// An overall control of the wind sound volume.
	}

    
	void Awake ()
	{
        // Set up the reference to the aeroplane controller.
		plane = GetComponent<AeroplaneController>();	
		
		// Add the audiosources and get the references.
		engineSoundSource = gameObject.AddComponent<AudioSource>();
		windSoundSource = gameObject.AddComponent<AudioSource>();
		
		// Assign clips to the audiosources.
		engineSoundSource.clip = engineSound;
		windSoundSource.clip = windSound;
		
		// Set the parameters of the audiosources.
		engineSoundSource.minDistance = advanced.engineMinDistance;
		engineSoundSource.maxDistance = advanced.engineMaxDistance;
		engineSoundSource.loop = true;
		engineSoundSource.dopplerLevel = advanced.engineDopplerLevel;
		
		windSoundSource.minDistance = advanced.windMinDistance;
		windSoundSource.maxDistance = advanced.windMaxDistance;
		windSoundSource.loop = true;
		windSoundSource.dopplerLevel = advanced.windDopplerLevel;
		
        // Start the sounds playing.
		engineSoundSource.Play();
		windSoundSource.Play();	
	}
	
	
	void Update ()
	{
        // Find what proportion of the engine's power is being used.
	    var enginePowerProportion = Mathf.InverseLerp(0, plane.MaxEnginePower, plane.EnginePower);

        // Set the engine's pitch to be proportional to the engine's current power.
		engineSoundSource.pitch = Mathf.Lerp(engineMinThrottlePitch, engineMaxThrottlePitch, enginePowerProportion);

        // Increase the engine's pitch by an amount proportional to the aeroplane's forward speed.
		// (this makes the pitch increase when going into a dive!)
        engineSoundSource.pitch += plane.ForwardSpeed * engineFwdSpeedMultiplier;

        // Set the engine's volume to be proportional to the engine's current power.
		engineSoundSource.volume = Mathf.InverseLerp(0,plane.MaxEnginePower * advanced.engineMasterVolume, plane.EnginePower);

        // Set the wind's pitch and volume to be proportional to the aeroplane's forward speed.
		float planeSpeed = rigidbody.velocity.magnitude;
		windSoundSource.pitch = windBasePitch + planeSpeed * windSpeedPitchFactor;	
		windSoundSource.volume = Mathf.InverseLerp( 0, windMaxSpeedVolume, planeSpeed ) * advanced.windMasterVolume;

	}
}
