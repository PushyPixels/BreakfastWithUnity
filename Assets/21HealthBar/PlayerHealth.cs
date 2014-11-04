using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public Slider healthBar;
	public int healthAmount = 10;
	private int currentHealth;

	// Use this for initialization
	void Start ()
	{
		currentHealth = healthAmount;
	}


	
	// Update is called once per frame
	void OnTriggerEnter()
	{
		currentHealth--;
		healthBar.value = (float)currentHealth/(float)healthAmount;
	}
}
