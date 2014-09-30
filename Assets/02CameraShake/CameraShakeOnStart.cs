using UnityEngine;
using System.Collections;

public class CameraShakeOnStart : MonoBehaviour
{
	public float amplitude = 0.1f;
	public float duration = 0.5f;

	// Use this for initialization
	void Start ()
	{
		CameraShake.Instance.Shake(amplitude,duration);
	}
}
