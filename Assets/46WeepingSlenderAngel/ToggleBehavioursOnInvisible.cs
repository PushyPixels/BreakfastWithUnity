using UnityEngine;
using System.Collections;

public class ToggleBehavioursOnInvisible : MonoBehaviour
{
	public Behaviour[] behavioursEnabledOnInvisible;
	public Behaviour[] behavioursDisabledOnInvisible;
	private bool visible = false;
	private bool raycastable = false;

	void Update()
	{
		RaycastHit hit;

		if(Physics.Raycast(transform.position, Camera.main.transform.position - transform.position, out hit))
		{
			if(hit.collider.tag == "MainCamera")
			{
				raycastable = true;
			}
			else
			{
				raycastable = false;
			}
		}
		else
		{
			raycastable = false;
		}

		if(raycastable && visible)
		{
			foreach(Behaviour bev in behavioursEnabledOnInvisible)
			{
				bev.enabled = false;
			}
			foreach(Behaviour bev in behavioursDisabledOnInvisible)
			{
				bev.enabled = true;
			}
		}
		else
		{
			foreach(Behaviour bev in behavioursEnabledOnInvisible)
			{
				bev.enabled = true;
			}
			foreach(Behaviour bev in behavioursDisabledOnInvisible)
			{
				bev.enabled = false;
			}
		}
	}

	void OnBecameInvisible()
	{
		visible = false;
	}

	void OnBecameVisible()
	{
		visible = true;
	}
}
