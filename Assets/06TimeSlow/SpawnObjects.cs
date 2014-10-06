using UnityEngine;
using System.Collections;

public class SpawnObjects : MonoBehaviour {
	public GameObject objectToSpawn;
	public float delay = 1.0f;

	// Use this for initialization
	void Start () {
		Invoke("Spawn", delay);
	}
	
	// Update is called once per frame
	void Spawn () {
		Instantiate(objectToSpawn, transform.position,Quaternion.identity);
		Invoke("Spawn", delay);
	}
}
