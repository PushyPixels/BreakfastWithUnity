using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RaycastPositionWithCallback : MonoBehaviour
{
	[System.Serializable]
	public class PositionCallback : UnityEvent<Vector3>
	{
	}

	public PositionCallback callback;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit hit;

		if(Physics.Raycast(transform.position, transform.forward, out hit))
		{
			callback.Invoke(hit.point);
		}
	}
}
