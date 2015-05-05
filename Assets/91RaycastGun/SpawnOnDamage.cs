using UnityEngine;
using System.Collections;

public class SpawnOnDamage : MonoBehaviour
{
	public GameObject objectToSpawn;

	void Damage(GunHit gunHit)
	{
		Instantiate(objectToSpawn,gunHit.raycastHit.point,Quaternion.LookRotation(gunHit.raycastHit.normal));
	}
}
