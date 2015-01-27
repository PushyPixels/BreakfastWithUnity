using UnityEngine;
using System.Collections;

public class ChangeScaleBasedOnAudioVolume : MonoBehaviour
{
	public float scaleBoost = 1.0f;

	float[] samples = new float[1];

	// Update is called once per frame
	void Update ()
	{
		AudioListener.GetOutputData(samples,0);

		float averageValue = 0.0f;
		foreach(float sample in samples)
		{
			averageValue += sample;
		}

		averageValue = averageValue/samples.Length;

		transform.localScale = Vector3.one*averageValue*scaleBoost;
	}
}
