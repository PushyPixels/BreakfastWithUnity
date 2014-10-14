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
						GameObject instance = Instantiate(shellGameObject,
						                                  transform.position-							//Our original position
						                                  transform.right*((numObjectsX*objectSpacing.x)/2.0f-2.5f)-		//Minus for center offset (x)
						                                  transform.up*((numObjectsY*objectSpacing.x)/2.0f-2.5f)-			//Minus for center offset (y)
						                                  transform.forward*((numObjectsZ*objectSpacing.x)/2.0f-2.5f)+		//Minus for center offset (z)
						                                  transform.right*x*objectSpacing.x+			//Plus current offset (x)
						                                  transform.up*y*objectSpacing.y+				//Plus current offset (y)
						                                  transform.forward*z*objectSpacing.z,			//Plus current offset (z)
						                                  Quaternion.identity) as GameObject;
						instance.transform.parent = transform;
					}
					else
					{
						GameObject instance = Instantiate(insideGameObject,
						                                  transform.position-							//Our original position
						                                  transform.right*((numObjectsX*objectSpacing.x)/2.0f-2.5f)-		//Minus for center offset (x)
						                                  transform.up*((numObjectsY*objectSpacing.x)/2.0f-2.5f)-			//Minus for center offset (y)
						                                  transform.forward*((numObjectsZ*objectSpacing.x)/2.0f-2.5f)+		//Minus for center offset (z)
						                                  transform.right*x*objectSpacing.x+			//Plus current offset (x)
						                                  transform.up*y*objectSpacing.y+				//Plus current offset (y)
						                                  transform.forward*z*objectSpacing.z,			//Plus current offset (z)
						                                  Quaternion.identity) as GameObject;
						instance.transform.parent = transform;
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
