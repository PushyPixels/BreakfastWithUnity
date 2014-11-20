using UnityEngine;
using System.Collections;

public class DestroyAndDetachChildrenAtStart : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		transform.DetachChildren();

		Destroy(gameObject);
	}
}
