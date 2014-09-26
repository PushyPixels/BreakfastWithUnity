using UnityEngine;
using System.Collections;

public class SpawnRandomly : MonoBehaviour {

	public GameObject spawnee;

	// Use this for initialization
	void Start ()
	{
		InvokeRepeating("Spawn",1.0f,1.0f);
	}

	void Spawn ()
	{
		Instantiate(spawnee, transform.position + Random.insideUnitSphere*10.0f,Quaternion.identity);
	}
}
