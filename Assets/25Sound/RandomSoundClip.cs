using UnityEngine;
using System.Collections;

public class RandomSoundClip : MonoBehaviour
{
	public AudioClip[] soundClips;
	public bool playOnAwake = true;
	
	void Awake ()
	{
		GetComponent<AudioSource>().clip = soundClips[Random.Range(0,soundClips.Length)];

		if(playOnAwake)
		{
			GetComponent<AudioSource>().Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
