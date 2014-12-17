using UnityEngine;
using System.Collections;

public class AdvancedFootsteps : MonoBehaviour
{
	public GameObject leftFootprint;
	public GameObject rightFootprint;

	public Transform leftFootLocation;
	public Transform rightFootLocation;

	public float footprintOffset = 0.05f;

	public AudioSource leftFootAudioSource;
	public AudioSource rightFootAudioSource;

	void LeftFootstep()
	{
		//Raycast out and create footprint
		RaycastHit hit;

		if(Physics.Raycast(leftFootLocation.position,leftFootLocation.forward,out hit))
		{
			FootstepMaterialProperties footMat = hit.transform.GetComponent<FootstepMaterialProperties>();

			if(footMat != null)
			{
				if(footMat.showFootprints)
				{
					Instantiate(leftFootprint,hit.point+hit.normal*footprintOffset,Quaternion.LookRotation(hit.normal,leftFootLocation.up));
				}
				leftFootAudioSource.PlayOneShot(footMat.materialSound);
			}
			else
			{
				leftFootAudioSource.Play();
			}
		}
		else
		{
			leftFootAudioSource.Play();
		}
	}

	void RightFootstep()
	{
		//Raycast out and create footprint
		RaycastHit hit;
		
		if(Physics.Raycast(rightFootLocation.position,rightFootLocation.forward,out hit))
		{
			FootstepMaterialProperties footMat = hit.transform.GetComponent<FootstepMaterialProperties>();

			if(footMat != null)
			{
				if(footMat.showFootprints)
				{
					Instantiate(rightFootprint,hit.point+hit.normal*footprintOffset,Quaternion.LookRotation(hit.normal,rightFootLocation.up));
				}
				rightFootAudioSource.PlayOneShot(footMat.materialSound);
			}
			else
			{
				rightFootAudioSource.Play();
			}
		}
		else
		{
			rightFootAudioSource.Play();
		}
	}
}
