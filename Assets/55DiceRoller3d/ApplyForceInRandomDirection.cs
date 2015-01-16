using UnityEngine;
using System.Collections;

public class ApplyForceInRandomDirection : MonoBehaviour
{
	public string buttonName = "Fire1";
	public float forceAmount = 10.0f;
	public float torqueAmount = 10.0f;
	public ForceMode forceMode;

	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButtonDown(buttonName))
		{
			rigidbody.AddForce(Random.onUnitSphere*forceAmount,forceMode);
			rigidbody.AddTorque(Random.onUnitSphere*torqueAmount,forceMode);
		}
	}
}
