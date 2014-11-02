using UnityEngine;
using System.Collections;

public class ExterminateOnDistanceFromOrigin : MonoBehaviour
{
	public float distance = 20.0f;

	private float sqrDistance;
	private bool exterminated = false;

	// Use this for initialization
	void Start ()
	{
		sqrDistance = distance*distance;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!exterminated && transform.position.sqrMagnitude > sqrDistance)
		{
			particleSystem.loop = false;
			exterminated = true;
		}
		if(!particleSystem.IsAlive())
		{
			Destroy (gameObject);
		}
	}
}
