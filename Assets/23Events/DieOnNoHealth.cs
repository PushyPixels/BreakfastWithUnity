using UnityEngine;
using System.Collections;

public class DieOnNoHealth : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable()
	{
		PlayerHealth.OnNoHealth += OnNoHealth;
	}

	void OnDisable()
	{
		PlayerHealth.OnNoHealth -= OnNoHealth;
	}

	void OnNoHealth()
	{
		Destroy(gameObject);
	}
}
