using UnityEngine;
using System.Collections;

public class CrossFadeToNewSceneOnButton : MonoBehaviour
{
	public string buttonName = "Fire1";
	public string sceneName;
	public float fadeTime = 2.0f;
	
	void Update ()
	{
		if(Input.GetButtonDown(buttonName))
		{
			StartCoroutine(ScreenWipe.use.CrossFadeToScene(sceneName, fadeTime));
		}
	}
}
