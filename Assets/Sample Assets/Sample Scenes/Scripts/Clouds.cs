using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Clouds : MonoBehaviour {

	[SerializeField] [Range(0,1)] float density;
	[SerializeField] float noiseScale = 0.0003f;
	[SerializeField] float minSize = 2000;
	[SerializeField] float maxSize = 4000;
	[SerializeField] float stepSize = 500;

	Bounds area;

	void Start () {
		area = (collider as BoxCollider).bounds;
		StartCoroutine(GenerateClouds());
	}
	
	public IEnumerator GenerateClouds()
	{
		ParticleSystem system = GetComponent<ParticleSystem>();
	
		system.Clear();
		
		Random.seed = 0;

		for (float x=area.min.x; x<area.max.x; x += stepSize)
		{
			for (float z=area.min.z; z<area.max.z; z += stepSize)
			{
				float p = Mathf.PerlinNoise(x*noiseScale + area.min.x,z*noiseScale+area.min.z);

				if (p < density)
				{
					float size = Mathf.Lerp (minSize,maxSize, Mathf.InverseLerp(density,0,p));

					float y = area.center.y + area.size.y * (Random.value-0.5f) * (Random.value-0.5f);
					Vector3 pos = new Vector3(x,y,z);
					pos += new Vector3( Random.value * stepSize, 0, Random.value * stepSize);
					system.Emit(pos, Vector3.zero, size, 99999, Color.white);

				}
			}
			//yield return null;
		}
		yield return null;
	}

}
