using UnityEngine;
using System.Collections;

public class ShootOnAxisInput : MonoBehaviour
{
	public GameObject bullet;

	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";

	public float shootDelay = 0.1f;

	private bool canShoot = true;

	
	void ResetShot ()
	{
		canShoot = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 shootDirection = Vector3.right*Input.GetAxis(horizontalAxis) + Vector3.forward*Input.GetAxis(verticalAxis);
		if(shootDirection.sqrMagnitude > 0.0f)
		{
			transform.rotation = Quaternion.LookRotation(shootDirection,Vector3.up);

			if(canShoot)
			{
				Instantiate(bullet,transform.position,transform.rotation);
				
				canShoot = false;
				Invoke("ResetShot",shootDelay);
			}
		}
	}
}
