using UnityEngine;
using System.Collections;

public class RecursionTree : MonoBehaviour
{
	public RecursionTree twigReference;

	public float twigLength = 1.0f;
	public float spreadAngle = 30.0f;
	public float iterations = 3;
	public float splits = 3;


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
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
