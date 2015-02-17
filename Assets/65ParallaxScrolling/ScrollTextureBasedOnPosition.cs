using UnityEngine;
using System.Collections;

public class ScrollTextureBasedOnPosition : MonoBehaviour
{
	public float scrollSpeedX = 1.0f;
	public float scrollSpeedY = 1.0f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 position = transform.position;
		position.x *= scrollSpeedX;
		position.y *= scrollSpeedY;
		renderer.material.mainTextureOffset = position;
	}
}
