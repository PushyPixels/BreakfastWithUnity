using UnityEngine;
using System.Collections;

public class ShakeOnStart : MonoBehaviour
{
	public float amplitude = 0.1f;
	public float time = 0.5f;

	// Use this for initialization
	void Start ()
	{
		CameraShake.Instance.Shake(amplitude,time);
	}
}
