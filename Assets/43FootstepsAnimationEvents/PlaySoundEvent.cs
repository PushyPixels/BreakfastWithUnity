using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundEvent : MonoBehaviour
{
	void PlaySound()
	{
		GetComponent<AudioSource>().Play();
	}
}
