using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {
	public float delay = 1.0f;

	// Use this for initialization
	void Start () {
		Invoke ("DestroyNow",delay);
	}
	
	// Update is called once per frame
	void DestroyNow()
	{
		Destroy(gameObject);
	}
}
