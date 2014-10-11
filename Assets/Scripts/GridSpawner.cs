using UnityEngine;
using System.Collections;

public class GridSpawner : MonoBehaviour
{
	public GameObject insideGameObject;
	public GameObject shellGameObject;

	public int numObjectsX = 1;
	public int numObjectsY = 1;
	public int numObjectsZ = 1;

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
					if(x == 0 || x == numObjectsX - 1 ||
					   y == 0 || y == numObjectsY - 1 ||
					   z == 0 || z == numObjectsZ - 1)
					{
						Instantiate(shellGameObject,transform.position+transform.right*x*objectSpacing.x+transform.up*y*objectSpacing.y+transform.forward*z*objectSpacing.z,Quaternion.identity);
					}
					else
					{
						Instantiate(insideGameObject,transform.position+transform.right*x*objectSpacing.x+transform.up*y*objectSpacing.y+transform.forward*z*objectSpacing.z,Quaternion.identity);
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
