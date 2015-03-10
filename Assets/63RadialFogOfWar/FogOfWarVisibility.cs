using UnityEngine;
using System.Collections;

public class FogOfWarVisibility : MonoBehaviour
{
	private bool observed = false;

	// Use this for initialization
	void Start () {
	
	}

	void Update()
	{
		if(observed)
		{
			GetComponent<Renderer>().enabled = true;
		}
		else
		{
			GetComponent<Renderer>().enabled = false;
		}

		observed = false;
	}
	
	void Observed()
	{
		Debug.Log("Observed",gameObject);
		observed = true;
	}
}
