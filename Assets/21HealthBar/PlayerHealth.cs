using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public Slider healthBar;
	public float healthAmount = 10.0f;
	private float currentHealth;

	public delegate void HealthEvent();
	public static event HealthEvent OnNoHealth; 

	// Use this for initialization
	void Start ()
	{
		currentHealth = healthAmount;
	}

	void OnEnable()
	{
		PlayerOxygen.OnNoOxygen += OnNoOxygen;
	}

	void OnDisable()
	{
		PlayerOxygen.OnNoOxygen -= OnNoOxygen;
	}

	void OnNoOxygen()
	{
		ReduceHealth(Time.deltaTime);
	}

	// Update is called once per frame
	void OnTriggerEnter()
	{
		ReduceHealth(1.0f);
	}

	void ReduceHealth(float amount)
	{
		currentHealth -= amount;
		healthBar.value = (float)currentHealth/(float)healthAmount;

		if(currentHealth <= 0)
		{
			currentHealth = 0;
			
			if(OnNoHealth != null)
			{
				OnNoHealth();
			}
		}
	}
}
