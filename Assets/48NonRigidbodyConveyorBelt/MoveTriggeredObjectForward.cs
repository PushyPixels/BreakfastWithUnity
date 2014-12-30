using UnityEngine;
using System.Collections;

public class MoveTriggeredObjectForward : MonoBehaviour
{
	public float speed = 1.0f;

	void OnTriggerStay(Collider col)
	{
		col.transform.position += transform.forward*speed*Time.deltaTime;
	}
}
