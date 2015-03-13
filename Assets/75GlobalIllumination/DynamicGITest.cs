using UnityEngine;
using System.Collections;

public class DynamicGITest : MonoBehaviour
{
	[Range(0,1)]
	public float hue = 0.0f;
	[Range(0,1)]
	public float saturation = 1.0f;
	public float brightness = 1.0f;

	private Renderer renderer;
	private Color color;

	// Use this for initialization
	void Start ()
	{
		renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		color = new HSBColor(hue,saturation,brightness).ToColor(); 
		renderer.sharedMaterial.SetColor("_EmissionColor",color);
		DynamicGI.SetEmissive(renderer, color);
	}
}
