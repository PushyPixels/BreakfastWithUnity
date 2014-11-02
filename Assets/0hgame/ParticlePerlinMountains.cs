using UnityEngine;
using System.Collections;

public class ParticlePerlinMountains : MonoBehaviour {

	public int resolution = 100;
	private ParticleSystem.Particle[] points;
	private int currentResolution;

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		if (currentResolution != resolution || points == null) {
			CreatePoints();
		}
		for (int i = 0; i < points.Length; i++) {
			Vector3 p = points[i].position;
			points[i].position = p;
			Color c = points[i].color;
			c.g = p.y;
			points[i].color = c;
			points[i].lifetime = 10;
		}
		particleSystem.SetParticles(points, points.Length);
	}

	private void CreatePoints ()
	{
		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution * resolution];
		float increment = 1f / (resolution - 1);
		int i = 0;
		for (int x = 0; x < resolution; x++) {
			for (int z = 0; z < resolution; z++) {
				Vector3 p = new Vector3(x * increment, 0f, z * increment);
				points[i].position = p;
				points[i].color = new Color(p.x, 0f, p.z);
				points[i++].size = 0.1f;
			}
		}
	}
}
