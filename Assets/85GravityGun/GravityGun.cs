using UnityEngine;
using System.Collections;

public class GravityGun : MonoBehaviour
{
	public string fireButton = "Fire1";
	public float grabDistance = 10.0f;
	public Transform holdPosition;
	public float throwForce = 100.0f;
	public ForceMode throwForceMode;
	public LayerMask layerMask = -1;

	private GameObject heldObject = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(heldObject == null)
		{
			if(Input.GetButtonDown(fireButton))
			{
				RaycastHit hit;
				if(Physics.Raycast(transform.position,transform.forward,out hit,grabDistance,layerMask))
				{
					heldObject = hit.collider.gameObject;
					heldObject.GetComponent<Rigidbody>().isKinematic = true;
					heldObject.GetComponent<Collider>().enabled = false;
				}
			}
		}
		else
		{
			heldObject.transform.position = holdPosition.position;
			heldObject.transform.rotation = holdPosition.rotation;

			if(Input.GetButtonDown(fireButton))
			{
				Rigidbody body = heldObject.GetComponent<Rigidbody>();
				body.isKinematic = false;
				heldObject.GetComponent<Collider>().enabled = true;
				body.AddForce(throwForce*transform.forward,throwForceMode);
				heldObject = null;
			}
		}
	}
}
