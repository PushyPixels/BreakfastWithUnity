using UnityEngine;
using System.Collections;

public class ToggleGameObjectsOnInteract : MonoBehaviour
{
	public GameObject[] gameObjectsToToggle;

	private bool activeNow = false;

	// Use this for initialization
	void Start () {
	
	}

	void InteractEvent()
	{
		activeNow = !activeNow;

		foreach(GameObject obj in gameObjectsToToggle)
		{
			obj.SetActive(activeNow);
		}
	}
}
