using UnityEngine;
using System.Collections;

public class DirectionVector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 directionVector = Camera.main.transform.position + Camera.main.rigidbody.velocity;

		if(Vector3.Dot(Camera.main.rigidbody.velocity, Camera.main.transform.forward) > 0 && Camera.main.rigidbody.velocity != Vector3.zero)
		{
			Vector3 newPosition = Camera.main.WorldToScreenPoint(directionVector);
			RectTransform parent = transform.GetComponent<RectTransform>();
			parent.anchoredPosition = (Vector3)newPosition;
		
			newPosition = parent.localPosition;
			newPosition.z = 0.0f;
			parent.localPosition = newPosition;
		}
		else
		{
			transform.position = Vector3.one*10000.0f;
		}
	}
}
