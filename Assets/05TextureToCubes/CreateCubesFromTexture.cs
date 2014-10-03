using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateCubesFromTexture : MonoBehaviour
{
	public Renderer cube;
	public Texture2D texture;
	public float alphaThreshold = 50;

	public Dictionary<Color32,Material> pallette = new Dictionary<Color32,Material>();

	// Use this for initialization
	void Start ()
	{
		int x = 0;
		int y = 0;

		foreach(Color32 color in texture.GetPixels32())
		{
			if(x >= texture.width)
			{
				y++;
				x = 0;
			}

			if(color.a > alphaThreshold)
			{
				Renderer instance = Instantiate(cube,transform.position+transform.right*x+transform.up*y,Quaternion.identity) as Renderer;
				instance.material.color = color;
			}

			x++;
		}
	}
}
