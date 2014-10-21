using UnityEngine;
using System.Collections;

[AddComponentMenu("Spawn/Grid Spawner Shell")]
public class GridSpawnerShell : MonoBehaviour
{
	[Header("Required GameObject variables")]
	[Tooltip("Object to spawn outside of grid")]
	public GameObject shellObject;
	[Tooltip("Object to spawn inside of grid")]
	public GameObject interiorObject;

	[Header("Tweak values")]
	[Tooltip("Number of objects in the X direction")]
	public int numObjectsX = 1;
	[Tooltip("Number of objects in the Y direction")]
	public int numObjectsY = 1;
	[Tooltip("Number of objects in the Z direction")]
	public int numObjectsZ = 1;

	[Space(10.0f)]
	public Vector3 objectSpacing = Vector3.one;

	// Use this for initialization
	void Start ()
	{
		for(int x = 0; x < numObjectsX; x++)
		{
			for(int y = 0; y < numObjectsY; y++)
			{
				for(int z = 0; z < numObjectsZ; z++)
				{
					if(x == 0 || y == 0 || z == 0 || x == numObjectsX-1 || y == numObjectsY-1 || z == numObjectsZ-1)
					{
						Instantiate(shellObject,transform.position+transform.right*x*objectSpacing.x+transform.up*y*objectSpacing.y+transform.forward*z*objectSpacing.z,Quaternion.identity);
					}
					else
					{
						Instantiate(interiorObject,transform.position+transform.right*x*objectSpacing.x+transform.up*y*objectSpacing.y+transform.forward*z*objectSpacing.z,Quaternion.identity);
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
