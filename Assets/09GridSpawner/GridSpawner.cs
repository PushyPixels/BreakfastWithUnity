using UnityEngine;
using System.Collections;

[AddComponentMenu("Spawn/Grid Spawner")]
public class GridSpawner : MonoBehaviour
{
	public GameObject objectToSpawn;

	public int numObjectsX = 1;
	public int numObjectsY = 1;
	public int numObjectsZ = 1;

	public Vector3 objectSpacing = Vector3.one;

	// Use this for initialization
	[ContextMenu("SpawnCubesNow")]
	void Start ()
	{
		for(int x = 0; x < numObjectsX; x++)
		{
			for(int y = 0; y < numObjectsY; y++)
			{
				for(int z = 0; z < numObjectsZ; z++)
				{
					Instantiate(objectToSpawn,transform.position+transform.right*x*objectSpacing.x+transform.up*y*objectSpacing.y+transform.forward*z*objectSpacing.z,Quaternion.identity);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
