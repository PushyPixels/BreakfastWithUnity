using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFillBasedOnVelocity : MonoBehaviour
{
	public Rigidbody objectToMeasure;
	public float maxVelocity = 10.0f;

	public bool disallowBackwards = false;

	private Image image;

	// Use this for initialization
	void Start ()
	{
		image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!disallowBackwards || Vector3.Dot(objectToMeasure.velocity, objectToMeasure.transform.forward) > 0.0f)
		{
			image.fillAmount = objectToMeasure.velocity.magnitude/maxVelocity;
		}
		else
		{
			image.fillAmount = 0.0f;
		}
	}
}
