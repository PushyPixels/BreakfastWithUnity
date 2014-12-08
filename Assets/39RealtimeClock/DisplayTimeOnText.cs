using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayTimeOnText : MonoBehaviour
{
	private Text clockText;

	void Start()
	{
		clockText = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update ()
	{
		System.DateTime time = System.DateTime.Now;
	
		clockText.text = time.ToString("hh:mm");
	}
}
