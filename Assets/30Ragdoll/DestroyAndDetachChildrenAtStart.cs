using UnityEngine;
using System.Collections;

public class DestroyAndDetachChildrenAtStart : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		foreach(Transform child in transform)
		{
			child.parent = null;
		}

		Destroy(gameObject,1.0f);
	}
}
