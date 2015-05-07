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
	public float maxBulletSpreadAngle = 15.0f;
	public float timeTillMaxSpreadAngle = 1.0f;
	public AnimationCurve bulletSpreadCurve;
	public LayerMask layerMask = -1;

	private bool readyToFire = true;
	private float fireTime;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButton(buttonName)) //GetButtonDown for semi-auto, GetButton for automatic fire
		{
			fireTime += Time.deltaTime;

			if(readyToFire)
			{
				RaycastHit hit;

				Vector3 fireDirection = transform.forward;

				Quaternion fireRotation = Quaternion.LookRotation(fireDirection);

				Quaternion randomRotation = Random.rotation;

				float currentSpread = bulletSpreadCurve.Evaluate(fireTime/timeTillMaxSpreadAngle)*maxBulletSpreadAngle;

				//float currentSpread = Mathf.Lerp(0.0f, maxBulletSpreadAngle, fireTime/timeTillMaxSpreadAngle);

				fireRotation = Quaternion.RotateTowards(fireRotation,randomRotation,Random.Range(0.0f,currentSpread));

				if(Physics.Raycast(transform.position,fireRotation*Vector3.forward,out hit,Mathf.Infinity,layerMask))
				{
					GunHit gunHit = new GunHit();
					gunHit.damage = damage;
					gunHit.raycastHit = hit;
					hit.collider.SendMessage("Damage",gunHit,SendMessageOptions.DontRequireReceiver);
					readyToFire = false;
					Invoke("SetReadyToFire", fireDelay);
				}
			}
		}
		else
		{
			fireTime = 0.0f;
		}
	}

	void SetReadyToFire()
	{
		readyToFire = true;
	}
}
