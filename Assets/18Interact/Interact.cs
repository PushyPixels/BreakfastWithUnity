using UnityEngine;
using System.Collections;

public class Interact : MonoBehaviour {
	public static Interact Instance;

	public float distance = 1.0f;
	public LayerMask layerMask = -1;
	public string buttonName = "Fire1";
	public Behaviour[] controlsToDisableDuringInteract;
	public float reinteractDelay = 0.1f;

	private bool displayInteract = false;
	private bool interactDisabled = false;

	// Use this for initialization
	void Start ()
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position,transform.forward,out hit,distance,layerMask))
		{
			if(!interactDisabled)
			{
				displayInteract = true;
				if(Input.GetButtonDown(buttonName))
				{
					hit.collider.SendMessageUpwards("InteractEvent");
				}
			}
			else
			{
				displayInteract = false;
			}
		}
		else
		{
			displayInteract = false;
		}
	}

	public static void DisableControl()
	{
		foreach(Behaviour bev in Instance.controlsToDisableDuringInteract)
		{
			bev.enabled = false;
		}
		Instance.interactDisabled = true;
	}

	public static void EnableControl()
	{
		foreach(Behaviour bev in Instance.controlsToDisableDuringInteract)
		{
			bev.enabled = true;
		}
		Instance.Invoke("InteractDisabledFalse",Instance.reinteractDelay);
	}

	void InteractDisabledFalse()
	{
		Instance.interactDisabled = false;
	}

	void OnGUI()
	{
		if(displayInteract == true)
		{
			GUILayout.Label("Press Fire1 to Interact");
		}
	}
}
