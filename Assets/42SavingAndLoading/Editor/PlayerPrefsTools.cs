using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayerPrefsTools : MonoBehaviour
{
	[MenuItem("Assets/ResetPlayerPrefs")]
	public static void ResetPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
}
