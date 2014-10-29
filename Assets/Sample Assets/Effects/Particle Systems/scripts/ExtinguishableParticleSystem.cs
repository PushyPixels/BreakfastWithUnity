using UnityEngine;
using System.Collections;

public class ExtinguishableParticleSystem : MonoBehaviour {

	public float multiplier = 1;
	ParticleSystem[] systems;

	void Start()
	{
		systems = GetComponentsInChildren<ParticleSystem>();
	}

	public void Extinguish()
	{
		foreach (var system in systems) {
			system.enableEmission = false;
		}
	}
}
