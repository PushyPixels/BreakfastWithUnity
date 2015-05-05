using UnityEngine;
using System.Collections;

public class SpeedTest : MonoBehaviour
{
	public GameObject interfaceSpeedTest;
	public GameObject sendMessageSpeedTest;
	public GameObject getComponentSpeedTest;

	public int iterations = 1000000;

	private float initialTime;

	// Use this for initialization
	void Start ()
	{
		
		initialTime = Time.realtimeSinceStartup;
		
		for(int i = 0; i<iterations; i++)
		{
			InterfaceSpeedTestInterface interfaceTest = interfaceSpeedTest.GetComponent(typeof(InterfaceSpeedTestInterface)) as InterfaceSpeedTestInterface;
			interfaceTest.DoStuff();
			//interfaceSpeedTest.GetInterface<InterfaceSpeedTestInterface>().DoStuff(); //This one is REAL slow
		}
		
		Debug.Log ("Interface time: " + (Time.realtimeSinceStartup - initialTime).ToString());

		initialTime = Time.realtimeSinceStartup;

		for(int i = 0; i<iterations; i++)
		{
			sendMessageSpeedTest.SendMessage("DoStuff");
		}

		Debug.Log ("SendMessage time: " + (Time.realtimeSinceStartup - initialTime).ToString());

		initialTime = Time.realtimeSinceStartup;
		
		for(int i = 0; i<iterations; i++)
		{
			getComponentSpeedTest.GetComponent<SendMessageSpeedTest>().DoStuff();
		}
		
		Debug.Log ("GetComponent time: " + (Time.realtimeSinceStartup - initialTime).ToString());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
