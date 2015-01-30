using UnityEngine;
using System.Collections;

public class LineOfSightRendering : MonoBehaviour
{
	public string viewerTag = "Player";
	public LayerMask layerMask = -1;

	private GameObject player;
	
	// Update is called once per frame
	void Update ()
	{
		if(player == null)
		{
			player = GameObject.FindGameObjectWithTag(viewerTag);
		}

		RaycastHit hit;

		if(Physics.Raycast(transform.position,player.transform.position - transform.position,out hit,Mathf.Infinity,layerMask))
		{
			if(hit.collider.gameObject == player)
			{
				renderer.enabled = true;
			}
			else
			{
				renderer.enabled = false;
			}
		}
		else
		{
			renderer.enabled = false;
		}
	
	}
}
