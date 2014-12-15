using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundEvent : MonoBehaviour
{
	void PlaySound()
	{
		audio.Play();
	}
}
