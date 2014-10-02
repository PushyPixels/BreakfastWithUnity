using UnityEngine;
using System.Collections;

public class ProjectLaser : MonoBehaviour
{
	public GameObject laser;
	public float offset = 0.1f;

	private GameObject laserInstance;

	// Update is called once per frame
	void Update ()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position,transform.forward,out hit))
		{
			if(laserInstance == null)
			{
				laserInstance = Instantiate(laser,hit.point + hit.normal*offset,Quaternion.identity) as GameObject;
			}
			else
			{
				laserInstance.SetActive(true);	//Added after the original episode aired, we enable the laser object here
				laserInstance.transform.position = hit.point + hit.normal*offset;
			}
		}
		//Added after the original episode aired.  This makes it so the laser doesn't get "stuck"
		else
		{
			//If it exists, we disable the laser object here
			if(laserInstance != null)
			{
				laserInstance.SetActive(false);
			}
		}
	}
}
