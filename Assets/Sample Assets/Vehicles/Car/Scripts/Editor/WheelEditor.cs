using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Wheel))]
public class WheelEditor : Editor
{
	public override void OnInspectorGUI()
	{
		// display the "original" inspector stuff
		base.OnInspectorGUI();


		// and add a button underneath
		if (GUILayout.Button("Align to assigned wheel model"))
		{
			foreach(var target in targets)
			{
				Wheel wheel = (Wheel) target;
				wheel.transform.position = wheel.wheelModel.transform.position;
				var bounds = wheel.wheelModel.renderer.bounds;
				wheel.GetComponent<WheelCollider>().radius = bounds.extents.y;
			}
		}
	}
}