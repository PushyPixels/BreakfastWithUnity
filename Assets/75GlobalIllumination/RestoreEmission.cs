using UnityEngine;
using System.Collections;

public class RestoreEmission : MonoBehaviour {

	private Color originalColor;


	// Use this for initialization
	void Start ()
	{
		originalColor = GetComponent<Renderer>().sharedMaterial.GetColor("_EmissionColor");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnApplicationQuit()
	{
		GetComponent<Renderer>().sharedMaterial.SetColor("_EmissionColor",originalColor);
	}
}
