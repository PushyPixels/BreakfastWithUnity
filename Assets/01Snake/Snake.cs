using UnityEngine;
using System.Collections;

public class Snake : MonoBehaviour
{
	public GameObject snakeBit;
	public float spawnTime = 0.5f;

	// Use this for initialization
	void Start ()
	{
		Invoke("MoveSnake",spawnTime);
	}

	void MoveSnake()
	{
		Instantiate(snakeBit,transform.position,transform.rotation);
		int turnDirection = Random.Range(0,3);
		if(turnDirection == 1)
		{
			transform.Rotate(new Vector3(0,90,0));
		}
		else if(turnDirection == 2)
		{
			transform.Rotate(new Vector3(0,-90,0));
		}
		transform.position += transform.forward;
		Invoke("MoveSnake",spawnTime);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
