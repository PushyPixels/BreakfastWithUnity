using UnityEngine;
using System.Collections;

public class PlaceTargetWithMouse : MonoBehaviour {

	public float surfaceOffset = 1.5f;
	public GameObject setTargetOn;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				transform.position = hit.point + hit.normal * surfaceOffset;
				if (setTargetOn != null)
				{
					setTargetOn.SendMessage("SetTarget",this.transform);
				}
			}
		}
	}
}
