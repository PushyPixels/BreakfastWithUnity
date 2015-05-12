using UnityEngine;
using System.Collections;

public class ShotgunWeapon : WeaponBase
{
	public int shotFragments = 8;
	public float spreadAngle = 10.0f;

	protected override void PrimaryFire()
	{
		for(int i = 0; i < shotFragments; i++)
		{
			RaycastHit hit;
			
			Quaternion fireRotation = Quaternion.LookRotation(transform.forward);
			
			Quaternion randomRotation = Random.rotation;
			
			fireRotation = Quaternion.RotateTowards(fireRotation,randomRotation,Random.Range(0.0f,spreadAngle));

			if(Physics.Raycast(transform.position,fireRotation*Vector3.forward,out hit,Mathf.Infinity,layerMask))
			{
				GunHit gunHit = new GunHit();
				gunHit.damage = damage;
				gunHit.raycastHit = hit;
				hit.collider.SendMessage("Damage",gunHit,SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
