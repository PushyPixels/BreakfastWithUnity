using UnityEngine;
using System.Collections;

public class MoveTowardsPlayer : MonoBehaviour
{
	public float speed = 1.0f;

	private GameObject player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += (player.transform.position - transform.position).normalized*speed*Time.deltaTime;
	}
}
