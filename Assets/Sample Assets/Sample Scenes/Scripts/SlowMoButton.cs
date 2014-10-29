using UnityEngine;
using System.Collections;

public class SlowMoButton : MonoBehaviour {

	public Texture FullSpeedTex;                       // the gui texture for full speed
	public Texture SlowSpeedTex;                       // the gui texture for slow motion mode
	public float fullSpeed = 1;
	public float slowSpeed = 0.3f;
	public new GUITexture guiTexture;               	// reference to the gui texture that will be changed
	public bool alsoScalePhysicsTimestep = true;
	bool slowMo;
	float targetTime;
	float lastRealTime;
	float fixedTimeRatio;

	void Start()
	{
		targetTime = fullSpeed;
		lastRealTime = Time.realtimeSinceStartup;
		fixedTimeRatio = Time.fixedDeltaTime / Time.timeScale;
	}

	void Update()
	{
		float realDeltaTime = Time.realtimeSinceStartup - lastRealTime;

		if (CrossPlatformInput.GetButtonDown ("Speed")) {

			// toggle slow motion state
			slowMo = !slowMo;

			// update button texture
			guiTexture.texture = slowMo ? SlowSpeedTex : FullSpeedTex;

			targetTime = slowMo ? slowSpeed : fullSpeed;
			
		}


		if (Time.timeScale != targetTime)
		{
			// lerp towards target time
			Time.timeScale = Mathf.Lerp (Time.timeScale, targetTime, realDeltaTime * 2);
			if (alsoScalePhysicsTimestep) {
				Time.fixedDeltaTime = fixedTimeRatio * Time.timeScale;
			}

			// snap if close enough:
			if (Mathf.Abs(Time.timeScale - targetTime) < 0.01f)
			{
				Time.timeScale = targetTime;
			}

		}


		lastRealTime = Time.realtimeSinceStartup;
	}

}



