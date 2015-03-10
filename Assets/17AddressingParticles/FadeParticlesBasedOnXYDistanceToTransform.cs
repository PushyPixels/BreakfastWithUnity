using UnityEngine;
using System.Collections;

public class FadeParticlesBasedOnXYDistanceToTransform : MonoBehaviour
{
	public float distanceFalloffMultiplier = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		ParticleSystem.Particle[] currentParticles = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
		GetComponent<ParticleSystem>().GetParticles(currentParticles);

		for(int i = 0; i < GetComponent<ParticleSystem>().particleCount; i++)
		{
			//Get vector from particle position to transform position
			Vector3 particleVector = currentParticles[i].position - transform.position;

			//Change vector into local space
			particleVector = Quaternion.Inverse(transform.rotation)*particleVector;

			//Remove z from local space
			//This is so falloff will only take X and Y distance into account
			particleVector.z = 0.0f;

			//Chanve vector back into world space
			particleVector = transform.rotation*particleVector;

			//Alpha falloff based on X/Y distance
			float alpha = Mathf.Lerp(1.0f,0.0f,particleVector.sqrMagnitude*distanceFalloffMultiplier);

			Color newColor = currentParticles[i].color;
			newColor.a = alpha;
			currentParticles[i].color = newColor;
		}

		GetComponent<ParticleSystem>().SetParticles(currentParticles, GetComponent<ParticleSystem>().particleCount);
	}
}
