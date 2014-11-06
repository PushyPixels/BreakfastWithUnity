using UnityEngine;
using System.Collections;

public class MoveToRaycastPositionFunction : MonoBehaviour
{
	public void MoveToPosition(RaycastHit hit)
	{
		transform.position = hit.point;
	}
}
