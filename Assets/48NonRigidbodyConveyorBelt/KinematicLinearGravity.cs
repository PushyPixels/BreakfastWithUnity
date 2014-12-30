using UnityEngine;
using System.Collections;

public class KinematicLinearGravity : MonoBehaviour
{
	public LayerMask layerMask = -1;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		Vector3 gravityTime = Physics.gravity*Time.deltaTime;

		RaycastHit hit;
		if(Physics.Raycast(transform.position,Physics.gravity, out hit, gravityTime.magnitude,layerMask))
		{
			//Handle offset from surface problem here
			//transform.position = hit.point;
		}
		else
		{
			transform.position += gravityTime;
		}
	}
}
