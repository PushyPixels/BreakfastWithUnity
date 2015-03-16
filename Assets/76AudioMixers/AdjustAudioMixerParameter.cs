using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AdjustAudioMixerParameter : MonoBehaviour
{
	public AudioMixer mixer;
	public float value; //This is mostly for testing
	public string parameterName;

	// Use this for initialization
	void Start () {
	
	}

	public void SetAudioParameter(float newValue)
	{
		value = newValue;
	}
	
	// Update is called once per frame
	void Update ()
	{
		mixer.SetFloat(parameterName,value);
	}
}
