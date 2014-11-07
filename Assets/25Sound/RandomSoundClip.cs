using UnityEngine;
using System.Collections;

public class RandomSoundClip : MonoBehaviour
{
	public AudioClip[] soundClips;
	public bool playOnAwake = true;
	
	void Awake ()
	{
		audio.clip = soundClips[Random.Range(0,soundClips.Length)];

		if(playOnAwake)
		{
			audio.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
