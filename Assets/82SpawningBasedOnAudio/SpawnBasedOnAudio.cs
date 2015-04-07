using UnityEngine;
using System.Collections;

public class SpawnBasedOnAudio : MonoBehaviour
{
	public GameObject objectPrefab;
	public float spawnThreshold = 0.5f;
	public int frequency = 0;
	public FFTWindow fftWindow;
	//public float debugValue;
	
	private float[] samples = new float[1024]; //MUST BE A POWER OF TWO
	
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		AudioListener.GetSpectrumData(samples,0,fftWindow);

		//debugValue = samples[frequency];

		if(samples[frequency] > spawnThreshold)
		{
			Instantiate(objectPrefab,transform.position,transform.rotation);
		}
	}
}
