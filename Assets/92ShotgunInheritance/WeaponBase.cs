using UnityEngine;
using System.Collections;

public abstract class WeaponBase : MonoBehaviour
{
	public float damage = 1.0f;
	public int maxAmmo = 0; //0 for infinite ammo
	public float fireDelay = 0.1f;
	public string primaryFire = "Fire1";
	public LayerMask layerMask = -1;
	public bool automaticFire = false;

	private bool readyToFire = true;
	private int currentAmmo;

	protected abstract void PrimaryFire();

	void Start()
	{
		currentAmmo = maxAmmo;
	}

	// Update is called once per frame
	void Update()
	{
		CheckInput();
	}

	protected virtual void CheckInput()
	{
		bool primaryFirePressed;
		
		if(automaticFire)
		{
			primaryFirePressed = Input.GetButton(primaryFire);
		}
		else
		{
			primaryFirePressed = Input.GetButtonDown(primaryFire);
		}
		
		if(primaryFirePressed) //GetButtonDown for semi-auto, GetButton for automatic fire
		{	
			if(readyToFire && (currentAmmo > 0 || maxAmmo == 0))
			{
				PrimaryFire();
				readyToFire = false;
				currentAmmo--;
				Invoke("SetReadyToFire", fireDelay);
			}
		}
	}

	void SetReadyToFire()
	{
		readyToFire = true;
	}
}
