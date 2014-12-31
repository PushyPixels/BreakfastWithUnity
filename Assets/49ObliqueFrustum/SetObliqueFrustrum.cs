using UnityEngine;
using System.Collections;

public class SetObliqueFrustrum : MonoBehaviour
{
	public float horizontalOblique = 0.0f;
	public float verticalOblique = 0.0f;

	public bool setEveryFrame = false;

	// Use this for initialization
	void Start ()
	{
		SetObliqueness(horizontalOblique, verticalOblique);
	}
	
	void Update ()
	{
		if(setEveryFrame)
		{
			SetObliqueness(horizontalOblique, verticalOblique);
		}
	}


	void SetObliqueness(float horizObl, float vertObl)
	{
		Matrix4x4 mat = camera.projectionMatrix;
		mat[0, 2] = horizObl;
		mat[1, 2] = vertObl;
		camera.projectionMatrix = mat;
	}
}
