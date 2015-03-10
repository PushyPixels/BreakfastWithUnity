using UnityEngine;
using System.Collections;

public class SmokeParticles : MonoBehaviour {
	
	public AudioClip[] extinguishSounds;
	
	void Start()
	{
		GetComponent<AudioSource>().clip = extinguishSounds[Random.Range(0,extinguishSounds.Length)];	
		GetComponent<AudioSource>().Play();
	}
	
}