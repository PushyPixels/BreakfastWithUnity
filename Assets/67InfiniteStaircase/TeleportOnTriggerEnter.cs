using UnityEngine;
using System.Collections;

public class TeleportOnTriggerEnter : MonoBehaviour
{
	public Transform target;

	void OnTriggerEnter(Collider col)
	{
		Vector3 offset = col.transform.position - transform.position;
		col.transform.position = target.position + offset;
	}
}
