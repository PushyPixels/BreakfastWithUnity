using UnityEngine;
using System.Collections;

public class PlayAfterDelay : MonoBehaviour {

	public float delay = 1.0f;

	// Use this for initialization
	void Start ()
	{
		Invoke("PlayNow",delay);
	}

	void PlayNow ()
	{
		GetComponent<AudioSource>().Play();
	}
}
