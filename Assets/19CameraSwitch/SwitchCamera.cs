using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour
{
	public Camera newCamera;
	public string escapeKey = "Fire1";

	private bool activeNow = false;

	// Use this for initialization
	void Start () {
	
	}

	void Update()
	{
		if(activeNow)
		{
			if(Input.GetButtonDown(escapeKey))
			{
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
				newCamera.enabled = false;
				activeNow = false;
			}
		}
	}

	void InteractEvent()
	{
		if(activeNow == false)
		{
			activeNow = true;
			newCamera.enabled = true;
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;
		}
	}
}
