using UnityEngine;
using System.Collections;

public class AnalogClock : MonoBehaviour
{
	public Transform hourHand;
	public Transform minuteHand;

	// Update is called once per frame
	void Update ()
	{
		System.DateTime time = System.DateTime.Now;

		Vector3 newRotation = hourHand.localEulerAngles;
		newRotation.z = 360.0f/12.0f*time.Hour + 360.0f/12.0f/60.0f*time.Minute;
		hourHand.localEulerAngles = newRotation;

		newRotation = minuteHand.localEulerAngles;
		newRotation.z = 360.0f/60.0f*time.Minute;
		minuteHand.localEulerAngles = newRotation;
	}
}
