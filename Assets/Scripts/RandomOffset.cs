using UnityEngine;
using System.Collections;

public class RandomOffset : MonoBehaviour
{
	[System.Serializable]
	public class MaterialInfo
	{
		public string textureName;
		public Material material;
	}

	public MaterialInfo[] materialInfoList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach(MaterialInfo matInfo in materialInfoList)
		{
			matInfo.material.SetTextureOffset(matInfo.textureName,new Vector2(Random.value,Random.value));
		}
	}

	void OnDestroy()
	{
		foreach(MaterialInfo matInfo in materialInfoList)
		{
			matInfo.material.SetTextureOffset(matInfo.textureName,Vector2.zero);
		}
	}
}
