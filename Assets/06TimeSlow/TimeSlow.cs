using UnityEngine;
using System.Collections;

public class TimeSlow : MonoBehaviour
{
	public static TimeSlow Instance;

	public float defaultTimeScale = 0.5f;
	public float defaultDuration = 5.0f;
	public float defaultTransitionTime = 0.5f;

	// Use this for initialization
	void Start ()
	{
		Instance = this;
	}

	public void SlowTime()
	{
		StopAllCoroutines();
		StartCoroutine(ActuallyTimeSlow(defaultTimeScale, defaultDuration, defaultTransitionTime));
	}
	
	public void SlowTime(float timeScale, float duration, float transitionTime)
	{
		StopAllCoroutines();
		StartCoroutine(ActuallyTimeSlow(timeScale, duration, transitionTime));
	}

	IEnumerator ActuallyTimeSlow(float timeScale, float duration, float transitionTime)
	{
		float initialTime = Time.realtimeSinceStartup;

		//Transition in
		while(Time.realtimeSinceStartup - initialTime < transitionTime)
		{
			Time.timeScale = Mathf.Lerp(1.0f,timeScale,(Time.realtimeSinceStartup - initialTime)/transitionTime);
			yield return null;
		}

		Time.timeScale = timeScale;
		initialTime = Time.realtimeSinceStartup;

		//Hold for duration
		while(Time.realtimeSinceStartup - initialTime < duration)
		{
			yield return null;
		}

		initialTime = Time.realtimeSinceStartup;

		//Transition out
		while(Time.realtimeSinceStartup - initialTime < transitionTime)
		{
			Time.timeScale = Mathf.Lerp(timeScale,1.0f,(Time.realtimeSinceStartup - initialTime)/transitionTime);
			yield return null;
		}

		Time.timeScale = 1.0f;
	}
}
