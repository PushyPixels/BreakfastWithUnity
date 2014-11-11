using UnityEngine;
using System.Collections;

public class SpawnRandomly : MonoBehaviour
{
	public GameObject[] objectsToSpawn;
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
		Instantiate(objectsToSpawn[Random.Range(0,objectsToSpawn.Length)], transform.position+Random.insideUnitSphere*distance, Quaternion.identity);
		Invoke("Spawn",Random.Range(minDelay,maxDelay));
	}
}
