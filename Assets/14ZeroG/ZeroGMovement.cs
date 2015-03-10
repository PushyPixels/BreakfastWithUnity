using UnityEngine;
using System.Collections;

public class ZeroGMovement : MonoBehaviour
{
	public string forwardAxisName = "Vertical";
	public string horizontalAxisName = "Horizontal";
	public float force = 10.0f;
	public ForceMode forceMode;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		Vector3 forceDirection = new Vector3(Input.GetAxis(horizontalAxisName),0.0f,Input.GetAxis(forwardAxisName)).normalized;
		GetComponent<Rigidbody>().AddForce(transform.rotation*forceDirection*force,forceMode);
	}
}
