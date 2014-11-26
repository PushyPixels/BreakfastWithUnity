using UnityEngine;
using System.Collections;

public class GibOnCollide : MonoBehaviour
{
	public GameObject gib;

	void OnCollisionEnter()
	{
		Instantiate(gib,transform.position,transform.rotation);
		Destroy(gameObject);
	}
}
