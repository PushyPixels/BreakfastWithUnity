using UnityEngine;
using System.Collections;

public class RandomPitch : MonoBehaviour
{
	public float pitchOffset = 0.2f;
	
	void Awake()
	{
		//Unity 5.0 version: GetComponent<AudioSource>()
		audio.pitch = Random.Range(audio.pitch - pitchOffset,audio.pitch + pitchOffset);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
