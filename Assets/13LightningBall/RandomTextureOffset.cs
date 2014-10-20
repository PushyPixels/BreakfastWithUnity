using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomTextureOffset : MonoBehaviour 
{
	[System.Serializable]
	public class MaterialInfo
	{
		public Material material;
		public string textureName = "_MainTex";
	}

	public List<MaterialInfo> materialInfoList = new List<MaterialInfo>();

	// Update is called once per frame
	void Update ()
	{
		foreach(MaterialInfo info in materialInfoList)
		{
			info.material.SetTextureOffset(info.textureName,new Vector2(Random.value,Random.value));
		}
	}
}
