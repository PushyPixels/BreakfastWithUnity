using UnityEngine;
using System.Collections;

public class AutoScrollTextureOffset : MonoBehaviour
{
	public Vector2 scrollSpeed = Vector2.one;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector2 finalTextureOffset = renderer.material.mainTextureOffset;
		finalTextureOffset += scrollSpeed*Time.deltaTime;

		while(finalTextureOffset.x >= 1.0f)
		{
			finalTextureOffset.x -= 1.0f;
		}
		while(finalTextureOffset.x < 0.0f)
		{
			finalTextureOffset.x += 1.0f;
		}
		while(finalTextureOffset.y >= 1.0f)
		{
			finalTextureOffset.y -= 1.0f;
		}
		while(finalTextureOffset.y < 0.0f)
		{
			finalTextureOffset.y += 1.0f;
		}

		renderer.material.mainTextureOffset = finalTextureOffset;
	}
}
