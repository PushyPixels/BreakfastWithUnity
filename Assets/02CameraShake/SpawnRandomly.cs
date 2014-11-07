using UnityEngine;
using System.Collections;

public class SpawnRandomly : MonoBehaviour
{
	public GameObject objectToSpawn;
	public float distance = 3.0f;

	public float minDelay = 1.0f;
	public float maxDelay = 1.0f;

	// Use this for initialization
	void Start ()
	{
		Invoke("Spawn",Random.Range(minDelay,maxDelay));
	}

	void Spawn ()
	{
		Instantiate(objectToSpawn, transform.position+Random.insideUnitSphere*distance, Quaternion.identity);
		Invoke("Spawn",Random.Range(minDelay,maxDelay));
	}
}
