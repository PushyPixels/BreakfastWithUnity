using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LineVisualizerUsingVisualizationManager : MonoBehaviour
{
	public float size = 10.0f;
	public float amplitude = 1.0f;
	public int cutoffSample = 128; //MUST BE LOWER THAN SAMPLE SIZE

	private LineRenderer lineRenderer;
	private float stepSize;

	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(cutoffSample);
		stepSize = size/cutoffSample;
	}

	// Update is called once per frame
	void Update ()
	{

		int i = 0;

		for(i = 0; i < cutoffSample; i++)
		{
			Vector3 position = new Vector3(i*stepSize - size/2.0f,VisualizationManager.samples[i]*amplitude,0.0f);
			lineRenderer.SetPosition(i,position);
		}
	}
}
