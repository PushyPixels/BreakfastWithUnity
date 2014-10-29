using UnityEngine;
using UnityEditor;

[CustomEditor( typeof (FreeLookCam) )]
public class FreeLookCamInspector : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		if ((target as FreeLookCam).warning != "")
		{
			EditorGUILayout.HelpBox((target as FreeLookCam).warning, MessageType.Warning);
		}
	}
}

