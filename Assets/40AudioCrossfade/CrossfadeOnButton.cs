using UnityEngine;
using System.Collections;

public class CrossfadeOnButton : MonoBehaviour
{
	public AudioClip[] tracks;

	public string buttonName = "Fire1";
	public float fadeTime = 1.0f;

	private int currentTrack = 0;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButtonDown(buttonName))
		{
			currentTrack++;
			if(currentTrack >= tracks.Length)
			{
				currentTrack = 0;
			}
			MusicManager.Crossfade(tracks[currentTrack], fadeTime);
		}
	}
}
