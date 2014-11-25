using UnityEngine;
using System.Collections;

public class MaintainOrientation : MonoBehaviour
{
	private Quaternion originalRotation;

	// Use this for initialization
	void Start ()
	{
		originalRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		transform.rotation = originalRotation;
	}
}
