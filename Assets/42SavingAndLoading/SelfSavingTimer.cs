using UnityEngine;
using System.Collections;

public class SelfSavingTimer : MonoBehaviour
{
	private float time = 0.0f;

	// Use this for initialization
	void Start ()
	{
		time = PlayerPrefs.GetFloat("Timer",0.0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		time += Time.deltaTime;
	}

	void OnDestroy()
	{
		PlayerPrefs.SetFloat("Timer", time);
		PlayerPrefs.Save();
	}

	void OnGUI()
	{
		GUILayout.Label (time.ToString());
	}
}
