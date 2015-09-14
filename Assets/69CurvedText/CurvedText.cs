using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CurvedText : Text
{
	public float radius = 0.5f;
	public float wrapAngle = 360.0f;
	public float scaleFactor = 100.0f;
	
	private float circumference
	{
		get
		{
			if(_radius != radius || _scaleFactor != scaleFactor)
			{
				_circumference = 2.0f*Mathf.PI*radius*scaleFactor;
				_radius = radius;
				_scaleFactor = scaleFactor;
			}

			return _circumference;
		}
	}
	private float _radius = -1;
	private float _scaleFactor = -1;
	private float _circumference = -1;

	protected override void OnValidate()
	{
		base.OnValidate();
		if(radius <= 0.0f)
		{
			radius = 0.001f;
		}
		if(scaleFactor <= 0.0f)
		{
			scaleFactor = 0.001f;
		}
	}
		
	protected override void OnPopulateMesh(Mesh outputMesh)
	{	
		base.OnPopulateMesh(outputMesh);

		Vector3[] verticies = outputMesh.vertices;

		for (int i = 0; i < outputMesh.vertices.Length; i++)
		{
			Debug.Log(outputMesh.vertices[i]);
			Vector3 v = outputMesh.vertices[i];

			float percentCircumference = v.x/circumference;
			Vector3 offset = Quaternion.Euler(0.0f,0.0f,-percentCircumference*360.0f)*Vector3.up;
			v = offset*radius*scaleFactor + offset*v.y;
			v += Vector3.down*radius*scaleFactor;

			verticies[i] = v;

			//outputMesh.vertices[i] = v;
			//Debug.Log(v);
			//Debug.Log(outputMesh.vertices[i]);
		}
		outputMesh.vertices = verticies;
	}

	void Update()
	{
		if(radius <= 0.0f)
		{
			radius = 0.001f;
		}
		if(scaleFactor <= 0.0f)
		{
			scaleFactor = 0.001f;
		}
		rectTransform.sizeDelta = new Vector2(circumference*wrapAngle/360.0f,rectTransform.sizeDelta.y);
	}
}
