using UnityEngine;
using System.Collections;

public class RandomOffset : MonoBehaviour
{
	public Material[] materials;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach(Material mat in materials)
		{
			mat.SetTextureOffset("_BumpMap",new Vector2(Random.value,Random.value));
		}
	}
}
