using UnityEngine;
using System.Collections;

public class VisualizationManager : MonoBehaviour
{
	public FFTWindow fftWindow;
	//public float debugValue;
	
	public static float[] samples = new float[1024]; //MUST BE A POWER OF TWO

	// Update is called once per frame
	void Update ()
	{
		AudioListener.GetSpectrumData(samples,0,fftWindow);
	}
}
