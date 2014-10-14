using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleportToNearestFake : MonoBehaviour
{
	public string fakeTag = "Fake";

	private List<GameObject> fakeList = new List<GameObject>();
	private GameObject currentFake = null;
	private GameObject player = null;

	void Start()
	{
		//Cache fakes
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Fake"))
		{
			fakeList.Add(obj);
		}

		//Cache player
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update ()
	{
		//Determine the nearest fake
		GameObject nearestFake = FindNearestGameobject(fakeList, player);

		//If we need to move:
		if(nearestFake != currentFake)
		{
			//Move any cubes that are out of position back to their original positions
			foreach(Transform child in transform)
			{
				foreach(Transform grandChild in child)
				{
					grandChild.localPosition = Vector3.zero;
					grandChild.rotation = Quaternion.identity;

					//Reset rigidbodies
					grandChild.rigidbody.velocity = Vector3.zero;
					grandChild.rigidbody.angularVelocity = Vector3.zero;
				}
			}

			//Move the whole object to the fake's position
			transform.position = nearestFake.transform.position;

			//Disable the fake
			nearestFake.SetActive(false);

			//Reenbable any other fake that needs to be enabled
			if(currentFake != null)
			{
				currentFake.SetActive(true);
			}

			currentFake = nearestFake;
		}
	}

	GameObject FindNearestGameobject(List<GameObject> objectList, GameObject target)
	{
		float nearestDistance = Mathf.Infinity;
		GameObject nearestObject = null;

		foreach(GameObject obj in objectList)
		{
			float distance = (obj.transform.position - target.transform.position).sqrMagnitude;
			if(distance < nearestDistance)
			{
				nearestDistance = distance;
				nearestObject = obj;
			}
		}

		return nearestObject;
	}
}