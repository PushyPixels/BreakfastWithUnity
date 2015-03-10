using UnityEngine;
using System.Collections;

public class CubeSave : MonoBehaviour
{
	private int cubeID;
	private static int currentCubeID = 0;

	// Use this for initialization
	void Start()
	{
		cubeID = currentCubeID;
		currentCubeID++;

		if(PlayerPrefs.HasKey("CubePosition" + cubeID.ToString()))
		{
			transform.position = PlayerPrefsX.GetVector3("CubePosition" + cubeID.ToString());
			transform.rotation = PlayerPrefsX.GetQuaternion("CubeRotation" + cubeID.ToString());

			GetComponent<Rigidbody>().velocity = PlayerPrefsX.GetVector3("CubeRigidbodyVelocity" + cubeID.ToString(),GetComponent<Rigidbody>().velocity);
			GetComponent<Rigidbody>().angularVelocity = PlayerPrefsX.GetVector3("CubeRigidbodyAngularVelocity" + cubeID.ToString(),GetComponent<Rigidbody>().angularVelocity);
		}
	}
	
	void OnDestroy()
	{
		PlayerPrefsX.SetVector3("CubePosition" + cubeID.ToString(),transform.position);
		PlayerPrefsX.SetQuaternion("CubeRotation" + cubeID.ToString(),transform.rotation);

		PlayerPrefsX.SetVector3("CubeRigidbodyVelocity" + cubeID.ToString(),GetComponent<Rigidbody>().velocity);
		PlayerPrefsX.SetVector3("CubeRigidbodyAngularVelocity" + cubeID.ToString(),GetComponent<Rigidbody>().angularVelocity);
	}
}
