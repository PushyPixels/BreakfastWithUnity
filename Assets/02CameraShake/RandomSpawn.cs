using UnityEngine;
using System.Collections;

public class RandomSpawn : MonoBehaviour
{
	public GameObject objectToSpawn;
	public float distance = 5.0f;
	public float delay = 1.0f;

	// Use this for initialization
	void Start ()
	{
		Invoke("Spawn", delay);
	}

	void Spawn()
	{
		Instantiate(objectToSpawn,transform.position + Random.insideUnitSphere*distance, Quaternion.identity);
		Invoke("Spawn", delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
