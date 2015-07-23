using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecursionTree : MonoBehaviour
{
	public RecursionTree twigReference;

	public float twigLength = 1.0f;
	public float spreadAngle = 30.0f;
	public float iterations = 3;
	public float splits = 3;
	public float windAngle = 10.0f;
	public float minWindChangeTime = 1.0f;
	public float maxWindChangeTime = 5.0f;

	private List<Transform> childList = new List<Transform>();
	private bool doneParenting = false;
	private Quaternion originalRotation;

	[HideInInspector]
	public bool root = true;

	private static Quaternion goalRotation;


	// Use this for initialization
	void Start ()
	{
		if(iterations >= 1)
		{
			for(int i = 0; i < splits; i++)
			{
				Quaternion currentRotation = Quaternion.LookRotation(transform.forward);
				Quaternion randomRotation = Random.rotation;
				Quaternion twigRotation = Quaternion.RotateTowards(currentRotation,randomRotation,Random.Range(0.0f,spreadAngle));

				RecursionTree instance = Instantiate(twigReference,transform.position+transform.forward*twigLength,twigRotation) as RecursionTree;
				instance.iterations = iterations - 1;
				instance.root = false;

				childList.Add(instance.transform);
			}
		}

		originalRotation = transform.rotation;

		if(root)
		{
			WindChange();
			Invoke("WindChange",Random.Range(minWindChangeTime,maxWindChangeTime));
		}
	}

	void WindChange()
	{
		goalRotation = Random.rotation;
		Invoke("WindChange",Random.Range(minWindChangeTime,maxWindChangeTime));
	}

	// Update is called once per frame
	void Update ()
	{
		if(!doneParenting)
		{
			foreach(Transform child in childList)
			{
				child.parent = transform;
			}
			doneParenting = true;
		}
		else
		{
			//Quaternion randomRotation = Random.rotation;

			Quaternion maxRotation = Quaternion.RotateTowards(originalRotation,goalRotation,windAngle);

			transform.rotation = Quaternion.RotateTowards(transform.rotation,maxRotation,Random.Range(0.0f,windAngle*Time.deltaTime));
		}


		if(root)
		{
			Debug.DrawRay(transform.position,goalRotation*transform.forward);
		}
	}
}
