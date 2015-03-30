using System;
using System.Linq;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

internal class EndNameEdit : EndNameEditAction
{
	#region implemented abstract members of EndNameEditAction
	public override void Action (int instanceId, string pathName, string resourceFile)
	{
		AssetDatabase.CreateAsset(EditorUtility.InstanceIDToObject(instanceId), AssetDatabase.GenerateUniqueAssetPath(pathName));
	}

	#endregion
}

/// <summary>
/// Scriptable object window.
/// </summary>
public class ScriptableObjectWindow : EditorWindow
{
	private int selectedIndex;
	private string[] names;
	
	private Type[] types;
	
	public Type[] Types
	{ 
		get { return types; }
		set
		{
			types = value;
			names = types.Select(t => t.FullName).ToArray();
		}
	}
	
	public void OnGUI()
	{
		GUILayout.Label("ScriptableObject Class");
		selectedIndex = EditorGUILayout.Popup(selectedIndex, names);

		if (GUILayout.Button("Create"))
		{
			var asset = ScriptableObject.CreateInstance(types[selectedIndex]);
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
				asset.GetInstanceID(),
				ScriptableObject.CreateInstance<EndNameEdit>(),
				string.Format("{0}.asset", names[selectedIndex]),
				AssetPreview.GetMiniThumbnail(asset), 
				null);

			Close();
		}
	}
}