using UnityEngine;
using System.Collections;

public class SmokeParticles : MonoBehaviour {
	
	public AudioClip[] extinguishSounds;
	
	void Start()
	{
		audio.clip = extinguishSounds[Random.Range(0,extinguishSounds.Length)];	
		audio.Play();
	}
	
}