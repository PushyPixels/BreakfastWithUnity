using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	public static CameraShake Instance;

	private float _amplitude;
	private float _time;

	private Vector3 originalPosition;

	// Use this for initialization
	void Start ()
	{
		Instance = this;
		originalPosition = transform.localPosition;
	}

	public void Shake(float amplitude, float time)
	{
		_amplitude = amplitude;
		_time = time;
		StartCoroutine("ReallyShake");
	}

	IEnumerator ReallyShake()
	{
		float t = 0.0f;

		while(t < _time)
		{
			t += Time.deltaTime;
			transform.localPosition = originalPosition + Random.insideUnitSphere * _amplitude;
			yield return null;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	}


}
