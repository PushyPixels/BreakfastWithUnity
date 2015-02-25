using UnityEngine;
using System.Collections;

public class RemoteControlRobot : MonoBehaviour
{
	public float speed = 5.0f;
	public float rotationSpeed = 60.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Forward backward movement
		transform.position += transform.forward*Input.GetAxis("Vertical")*speed*Time.deltaTime;

		//Rotation
		transform.Rotate(0.0f,rotationSpeed*Input.GetAxis("Horizontal")*Time.deltaTime,0.0f);
	}
}
