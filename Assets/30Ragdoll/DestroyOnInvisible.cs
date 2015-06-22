using UnityEngine;
using System.Collections;

public class DestroyOnInvisible : MonoBehaviour
{
	public GameObject objectToDestroy;

	void Start()
	{
		if(!objectToDestroy)
		{
			objectToDestroy = gameObject;
		}
	}

	void OnBecameInvisible()
	{
		Destroy (objectToDestroy);
	}
}
