using UnityEngine;
using System.Collections;

public class FadeParticlesBasedOnDistanceToCollider : MonoBehaviour
{
	public float distanceFalloffMultiplier = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		ParticleSystem.Particle[] currentParticles = new ParticleSystem.Particle[particleSystem.particleCount];
		particleSystem.GetParticles(currentParticles);

		for(int i = 0; i < particleSystem.particleCount; i++)
		{
			//Vector3 colliderPoint = collider.ClosestPointOnBounds(currentParticles[i].position);

			//Vector3 particleVector = currentParticles[i].position - transform.position;

			//Vector3 modifiedVector = transform.rotation*particleVector;

			//modifiedVector.z = 0.0f;

			//Debug.Log((currentParticles[i].position - colliderPoint).magnitude);
			float alpha = Mathf.Lerp(1.0f,0.0f,(currentParticles[i].position - transform.position).sqrMagnitude*distanceFalloffMultiplier);

			Color newColor = currentParticles[i].color;
			newColor.a = alpha;
			currentParticles[i].color = newColor;
		}

		particleSystem.SetParticles(currentParticles, particleSystem.particleCount);
	}
}
