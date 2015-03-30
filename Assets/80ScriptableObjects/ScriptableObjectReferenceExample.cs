using UnityEngine;
using System.Collections;

public class ScriptableObjectReferenceExample : MonoBehaviour
{
	public TestScriptableObject scriptableObjectReference;

	// Use this for initialization
	void Start ()
	{
		Debug.Log(scriptableObjectReference.exampleFloat);
		Instantiate(scriptableObjectReference.gameObjectReference,scriptableObjectReference.exampleVector,Quaternion.identity);
	}
}
