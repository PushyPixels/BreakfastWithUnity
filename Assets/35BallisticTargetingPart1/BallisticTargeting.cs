using UnityEngine;
using System.Collections;

public class BallisticTargeting : MonoBehaviour
{
	public Transform target;
	public Transform targetIndicator;
	public float velocity = 10.0f;

	float CalculateTrajectoryAngles()
	{
		float v = velocity;

		Vector3 targetVector = target.position - transform.position;
		float height = -targetVector.y;
		targetVector.y = 0.0f;

		float x = targetVector.magnitude;
		float y = height;
		float g = Mathf.Abs(Physics.gravity.y);

		float top = v*v - Mathf.Sqrt(v*v*v*v - (g*(g*x*x + 2*y*v*v)));
		float bottom = g*x;

		float angle1 = Mathf.Atan2(bottom,top);

		return -angle1*Mathf.Rad2Deg;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		float angle1 = CalculateTrajectoryAngles();

		targetIndicator.localEulerAngles = new Vector3(angle1,0.0f,0.0f);
	}
}
