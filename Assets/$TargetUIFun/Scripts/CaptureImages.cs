using UnityEngine;
using System.Collections;

public class CaptureImages : MonoBehaviour {

	public int frameSkip = 3;


	int i = 0;
	int j = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(j % frameSkip == 0)
		{
			Application.CaptureScreenshot("Screenshot" + i.ToString("D3") + ".png");
			i++;
		}
		j++;
	}
}
