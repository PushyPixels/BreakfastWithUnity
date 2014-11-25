using UnityEngine;
using System.Collections;

public class RandomlyPlaceObjectsOnSurface : MonoBehaviour
{
	public GameObject[] objectsToSpawn;
	public float spawnRadius = 10.0f;
	public int numberOfObjects = 10;
	public bool randomOrientation = false;
	public bool orientToSurface = false;

	// Use this for initialization
	void Start ()
	{
		for(int i = 0; i < numberOfObjects; i++)
		{
			//What we will spawn
			GameObject objectToSpawn = objectsToSpawn[Random.Range(0,objectsToSpawn.Length)];

			Vector2 spawnPositionV2 = Random.insideUnitCircle*spawnRadius;

			Vector3 spawnPosition = new Vector3(spawnPositionV2.x,0.0f,spawnPositionV2.y);

			Vector3 transformOffsetSpawnPosition = transform.position+spawnPosition;

			RaycastHit hit;
			if(Physics.Raycast(transformOffsetSpawnPosition,Vector3.down,out hit))
			{
				Vector3 finalSpawnPosition = hit.point;

				Quaternion orientation;
				
				if(randomOrientation)
				{
					orientation = Random.rotation;
				}
				else if(orientToSurface)
				{
					orientation = Quaternion.LookRotation(hit.normal);
				}
				else
				{
					orientation = objectToSpawn.transform.rotation;
				}

				Instantiate(objectToSpawn,finalSpawnPosition,orientation);
			}
		}
	}
}
