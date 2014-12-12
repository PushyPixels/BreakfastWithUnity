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

			rigidbody.velocity = PlayerPrefsX.GetVector3("CubeRigidbodyVelocity" + cubeID.ToString(),rigidbody.velocity);
			rigidbody.angularVelocity = PlayerPrefsX.GetVector3("CubeRigidbodyAngularVelocity" + cubeID.ToString(),rigidbody.angularVelocity);
		}
	}
	
	void OnDestroy()
	{
		PlayerPrefsX.SetVector3("CubePosition" + cubeID.ToString(),transform.position);
		PlayerPrefsX.SetQuaternion("CubeRotation" + cubeID.ToString(),transform.rotation);

		PlayerPrefsX.SetVector3("CubeRigidbodyVelocity" + cubeID.ToString(),rigidbody.velocity);
		PlayerPrefsX.SetVector3("CubeRigidbodyAngularVelocity" + cubeID.ToString(),rigidbody.angularVelocity);
	}
}
