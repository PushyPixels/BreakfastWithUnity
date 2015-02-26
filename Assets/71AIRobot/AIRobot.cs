using UnityEngine;
using System.Collections;

public class AIRobot : MonoBehaviour
{
	public float speed = 5.0f;
	public float rotationSpeed = 60.0f;

	private float perlinOffset;

	private float t = 0.0f;

	// Use this for initialization
	void Start ()
	{
		perlinOffset = Random.Range(-1000.0f,1000.0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		t += Time.deltaTime;

		//Forward backward movement
		transform.position += transform.forward*speed*Time.deltaTime;

		//Rotation
		transform.Rotate(0.0f,(Mathf.PerlinNoise(t,perlinOffset)-0.5f)*2.0f*rotationSpeed*Time.deltaTime,0.0f);
	}

	void OnCollisionEnter(Collision collision)
	{
		transform.forward = Vector3.Reflect(transform.forward,Vector3.ProjectOnPlane(collision.contacts[0].normal,Vector3.up));

		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}

	void OnCollisionExit()
	{
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}
}
