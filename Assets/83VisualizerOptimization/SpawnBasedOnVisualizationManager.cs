using UnityEngine;
using System.Collections;

public class SpawnBasedOnVisualizationManager : MonoBehaviour
{
	public GameObject objectPrefab;
	public float spawnThreshold = 0.5f;
	public int frequency = 0;
	
	// Update is called once per frame
	void Update ()
	{
		if(VisualizationManager.samples[frequency] > spawnThreshold)
		{
			Instantiate(objectPrefab,transform.position,transform.rotation);
		}
	}
}
