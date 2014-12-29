using UnityEngine;
using System.Collections;

public class SetAnimationBoolOnTrigger : MonoBehaviour
{
	public string animationBoolName;
	public Animator target;

	void OnTriggerEnter()
	{
		target.SetBool(animationBoolName, true);
	}

	void OnTriggerExit()
	{
		target.SetBool(animationBoolName, false);
	}
}
