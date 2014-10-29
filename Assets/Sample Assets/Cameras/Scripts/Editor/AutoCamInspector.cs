using UnityEngine;
using UnityEditor;

[CustomEditor( typeof (AutoCam) )]
public class AutoCamInspector : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		if ((target as AutoCam).warning != "")
		{
			EditorGUILayout.HelpBox((target as AutoCam).warning, MessageType.Warning);
		}
	}
}

