using UnityEngine;
using System.Collections;

public class MoveUpDown : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.up*Input.GetAxis("Vertical")*Time.deltaTime;
	}
}
