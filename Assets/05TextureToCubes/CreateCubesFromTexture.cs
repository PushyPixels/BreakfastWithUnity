using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateCubesFromTexture : MonoBehaviour
{
	public Renderer cube;
	public Texture2D texture;
	public float alphaThreshold = 50;

	public Dictionary<Color32,Material> palette = new Dictionary<Color32,Material>();

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

				//This creates a new material if one does not exist for this color
				if(!palette.ContainsKey(color))
				{
					instance.material.color = color;
					palette.Add(color,instance.sharedMaterial);
				}
				//If we already have a material for this color, we use the one from the dictionary
				else
				{
					instance.sharedMaterial = palette[color];
				}
			}

			x++;
		}
	}
}
