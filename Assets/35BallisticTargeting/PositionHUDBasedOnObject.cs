using UnityEngine;
using System.Collections;

public class PositionHUDBasedOnObject : MonoBehaviour
{
	public Transform target;

	// Update is called once per frame
	void Update ()
	{
		transform.position = Camera.main.WorldToScreenPoint(target.position);
	}
}
