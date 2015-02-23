using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CurvedText))] 
public class CurvedTextEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
	}
}
