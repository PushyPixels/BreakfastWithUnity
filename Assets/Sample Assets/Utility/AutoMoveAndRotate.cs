using UnityEngine;
using System.Collections;

public class AutoMoveAndRotate : MonoBehaviour {

	public Vector3andSpace moveUnitsPerSecond;
	public Vector3andSpace rotateDegreesPerSecond;
	public bool ignoreTimescale;
	float lastRealTime;

	void Start()
	{
		lastRealTime = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update () {
		float deltaTime = Time.deltaTime;
		if (ignoreTimescale)
		{
			deltaTime = (Time.realtimeSinceStartup-lastRealTime);
			lastRealTime = Time.realtimeSinceStartup;
		}
		transform.Translate (moveUnitsPerSecond.value * deltaTime, moveUnitsPerSecond.space);
		transform.Rotate (rotateDegreesPerSecond.value * deltaTime, moveUnitsPerSecond.space);
	}

	[System.Serializable]
	public class Vector3andSpace
	{
		public Vector3 value;
		public Space space = Space.Self;
	}

}
