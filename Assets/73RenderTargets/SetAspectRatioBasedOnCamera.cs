using UnityEngine;
using System.Collections;

public class SetAspectRatioBasedOnCamera : MonoBehaviour
{
	public Camera target;

	// Use this for initialization
	void Start ()
	{
		GetComponent<Camera>().aspect = target.aspect;
	}
}
