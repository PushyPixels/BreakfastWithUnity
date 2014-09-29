using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	public static CameraShake Instance;

	private Vector3 initialPosition;
	private bool isShaking = false;

	public float _amplitude = 0.1f;

	// Use this for initialization
	void Start ()
	{
		Instance = this;
		initialPosition = transform.localPosition;
	}

	public void Shake(float amplitude, float duration)
	{
		_amplitude = amplitude;
		CancelInvoke();
		Invoke("StopShaking",duration);
		isShaking = true;
	}

	void StopShaking()
	{
		isShaking = false;
		transform.localPosition = initialPosition;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(isShaking)
		{
			transform.localPosition = initialPosition + Random.insideUnitSphere * _amplitude;
		}
	}
}
