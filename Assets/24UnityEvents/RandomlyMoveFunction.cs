using UnityEngine;
using System.Collections;

public class RandomlyMoveFunction : MonoBehaviour {
	public float radius = 1.0f;


	public void RandomMove()
	{
		transform.position += Random.insideUnitSphere*radius;
	}
}
