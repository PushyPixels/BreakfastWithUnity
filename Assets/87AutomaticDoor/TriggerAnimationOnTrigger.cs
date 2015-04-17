using UnityEngine;
using System.Collections;

public class TriggerAnimationOnTrigger : MonoBehaviour
{
	public Animator animator;
	public string onTriggerEnterParameterName;
	public string onTriggerExitParameterName;

	void Start()
	{
		if(animator == null)
		{
			animator = GetComponent<Animator>();
			if(animator == null)
			{
				Debug.LogError("No animator component on this script!",gameObject);
			}
		}
	}

	void OnTriggerEnter()
	{
		if(onTriggerEnterParameterName != null)
		{
			animator.SetTrigger(onTriggerEnterParameterName);
		}
	}

	void OnTriggerExit()
	{
		if(onTriggerExitParameterName != null)
		{
			animator.SetTrigger(onTriggerExitParameterName);
		}
	}
}
