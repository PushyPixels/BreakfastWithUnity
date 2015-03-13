using UnityEngine;
using System.Collections;

public class Reload : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		Invoke ("ReloadNow", 5.0f);
	}
	
	// Update is called once per frame
	void ReloadNow () {
		Application.LoadLevel(Application.loadedLevel);
	}
}
