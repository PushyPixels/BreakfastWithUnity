using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerOxygen : MonoBehaviour
{
	public Slider oxygenBar;
	public float oxygenAmount = 10.0f;
	private float currentOxygen;
	private bool isUnderwater = false;

	// Use this for initialization
	void Start ()
	{
		currentOxygen = oxygenAmount;
	}

	// Update is called once per frame
	void OnTriggerStay()
	{
		isUnderwater = false;
		currentOxygen -= Time.deltaTime;
		oxygenBar.value = currentOxygen/oxygenAmount;
	}

	void Update()
	{
		if(!isUnderwater)
		{
			Debug.Log ("Not in water!");
			currentOxygen += Time.deltaTime;
			if(currentOxygen >= oxygenAmount)
			{
				currentOxygen = oxygenAmount;
			}
			oxygenBar.value = currentOxygen/oxygenAmount;
		}
		isUnderwater = false;
	}
}
