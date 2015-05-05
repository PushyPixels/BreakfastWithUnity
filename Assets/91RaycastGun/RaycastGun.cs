using UnityEngine;
using System.Collections;

public class GunHit
{
	public float damage;
	public RaycastHit raycastHit;
}

public class RaycastGun : MonoBehaviour
{
	public float fireDelay = 0.1f;
	public float damage = 1.0f;
	public string buttonName = "Fire1";
	public LayerMask layerMask = -1;

	private bool readyToFire = true;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButtonDown(buttonName) && readyToFire)
		{
			RaycastHit hit;
			if(Physics.Raycast(transform.position,transform.forward,out hit,Mathf.Infinity,layerMask))
			{
				GunHit gunHit = new GunHit();
				gunHit.damage = damage;
				gunHit.raycastHit = hit;
				hit.collider.SendMessage("Damage",gunHit,SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
