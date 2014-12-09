using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
	public static MusicManager Instance;
	
	void Awake ()
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void Crossfade(AudioClip newTrack, float fadeTime = 1.0f)
	{
		Instance.StopAllCoroutines();

		if(Instance.GetComponents<AudioSource>().Length > 1)
		{
			Destroy(Instance.audio);
		}

		AudioSource newAudioSource = Instance.gameObject.AddComponent<AudioSource>();

		newAudioSource.volume = 0.0f;

		newAudioSource.clip = newTrack;

		newAudioSource.Play();

		Instance.StartCoroutine(Instance.ActuallyCrossfade(newAudioSource,fadeTime));
	}

	IEnumerator ActuallyCrossfade(AudioSource newSource, float fadeTime)
	{
		float t = 0.0f;

		float initialVolume = audio.volume;

		while(t < fadeTime)
		{
			audio.volume = Mathf.Lerp(initialVolume,0.0f,t/fadeTime);
			newSource.volume = Mathf.Lerp(0.0f,1.0f,t/fadeTime);

			t += Time.deltaTime;
			yield return null;
		}

		newSource.volume = 1.0f;

		Destroy(audio);
	}


}
