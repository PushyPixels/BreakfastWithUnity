using UnityEngine;
using System.Collections;

public class SleepOnStart : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		GetComponent<Rigidbody>().Sleep();
	}
}
