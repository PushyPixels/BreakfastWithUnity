using UnityEngine;
using System.Collections;

public class SetMaxAngularVelocity : MonoBehaviour
{
	public float maxAngularVelocity = 7.0f;

	// Use this for initialization
	void Start ()
	{
		GetComponent<Rigidbody>().maxAngularVelocity = maxAngularVelocity;
	}
}
