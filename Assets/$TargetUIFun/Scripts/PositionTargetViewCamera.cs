using UnityEngine;
using System.Collections;

public class PositionTargetViewCamera : MonoBehaviour
{
	public float distanceFromTarget = 10.0f;

	// Update is called once per frame
	void Update ()
	{
		Transform target = TargetManager.Instance.target;

		if(TargetManager.Instance.target != null)
		{
			Vector3 targetToPlayerVector = Camera.main.transform.position - target.position;

			transform.position = target.position + (targetToPlayerVector).normalized*distanceFromTarget;

			transform.rotation = Quaternion.LookRotation(-targetToPlayerVector,Camera.main.transform.up);
		}
	}
}
