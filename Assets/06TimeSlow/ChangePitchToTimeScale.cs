using UnityEngine;
using System.Collections;

public class ChangePitchToTimeScale : MonoBehaviour {
	// Update is called once per frame
	void Update ()
	{
		this.Audio().pitch = Time.timeScale;
	}
}
