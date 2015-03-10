using UnityEngine;
using System.Collections;

public class SetAspectRatio : MonoBehaviour
{
	public float aspectRatio = 1.0f;
	public bool realtimeUpdate = false;

	private Camera camera;

	// Use this for initialization
	void Start ()
	{
		camera = GetComponent<Camera>();
	}

	void Update()
	{
		if(realtimeUpdate)
		{
			camera.aspect = aspectRatio;
		}
	}
}
