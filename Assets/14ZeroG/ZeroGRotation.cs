using UnityEngine;
using System.Collections;

public class ZeroGRotation : MonoBehaviour
{
	public string verticalAxisName = "Mouse Y";
	public string horizontalAxisName = "Mouse X";
	public float force = 10.0f;
	public ForceMode forceMode;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		this.Rigidbody().AddTorque(transform.up*force*Input.GetAxis(horizontalAxisName),forceMode);
		this.Rigidbody().AddTorque(-transform.right*force*Input.GetAxis(verticalAxisName),forceMode);
	}
}
