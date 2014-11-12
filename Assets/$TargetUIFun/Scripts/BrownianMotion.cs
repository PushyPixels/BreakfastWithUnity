using UnityEngine;
using System.Collections;

public class BrownianMotion : MonoBehaviour
{
	public float speed = 1.0f;
	public float turnDelay = 0.0f;
	public float rotateSpeed = 90.0f;

	private Quaternion goalRotation;

	void Start()
	{
		goalRotation = Random.rotation;
		Invoke ("Turn",turnDelay);
	}

	// Update is called once per frame
	void Update ()
	{
		transform.position += transform.forward*speed*Time.deltaTime;
		transform.rotation = Quaternion.RotateTowards(transform.rotation,goalRotation,rotateSpeed*Time.deltaTime);
	}

	void Turn()
	{
		Invoke ("Turn",turnDelay);
		goalRotation = Random.rotation;
	}
}
