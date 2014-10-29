using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GUIText))]
public class FPSCounter : MonoBehaviour {
	
	float fpsMeasurePeriod = 0.5f;
	int fpsAccumulator = 0;
	float fpsNextPeriod = 0;
	int currentFps;
	string display = "{0} FPS";

	void Start()
	{
		fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
	}

	void Update()
	{

		// measure average frames per second
		fpsAccumulator++;
		if (Time.realtimeSinceStartup > fpsNextPeriod)
		{
			currentFps = (int)(fpsAccumulator / fpsMeasurePeriod);
			fpsAccumulator = 0;
			fpsNextPeriod += fpsMeasurePeriod;
			guiText.text = string.Format(display, currentFps);
		}


	}

}
