using UnityEngine;
using System.Collections;

public class SpawnRandomly : MonoBehaviour
{
	public GameObject objectToSpawn;
	public float distance = 3.0f;
	public float delay = 1.0f;

	// Use this for initialization
	void Start ()
	{
		Invoke("Spawn",delay);
	}

	void Spawn ()
	{
		GameObject instance = Instantiate(objectToSpawn, transform.position+Random.insideUnitSphere*distance, Quaternion.identity) as GameObject;
		if(instance.particleSystem != null)
		{
			instance.particleSystem.startColor = new Color(Random.value,Random.value,Random.value);
		}
		Invoke("Spawn",delay);
	}
}
