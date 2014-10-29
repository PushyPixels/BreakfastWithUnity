using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[InitializeOnLoad]
public class CrossPlatformInitialize {

	// Custom compiler defines:
	//
	// CROSS_PLATFORM_INPUT : denotes that cross platform input package exists, so that other packages can use their CrossPlatformInput functions.
	// EDITOR_MOBILE_INPUT : denotes that mobile input should be used in editor, if a mobile build target is selected. (i.e. using Unity Remote app).
	// MOBILE_INPUT : denotes that mobile input should be used right now!

	static CrossPlatformInitialize()
	{
		var defines = GetDefinesList(buildTargetGroups[0]);
		if (!defines.Contains("CROSS_PLATFORM_INPUT"))
		{
			SetEnabled("CROSS_PLATFORM_INPUT",true,false);
			SetEnabled("MOBILE_INPUT",true,true);
        }
    }
    
	[MenuItem("Mobile Input/Enable")]
	static void Enable()
	{

		SetEnabled("MOBILE_INPUT",true,true);
		switch (EditorUserBuildSettings.activeBuildTarget)
		{
			case BuildTarget.Android:
			case BuildTarget.iPhone:
			case BuildTarget.WP8Player:
			case BuildTarget.BB10:
			EditorUtility.DisplayDialog("Mobile Input","You have enabled Mobile Input. You'll need to use the Unity Remote app on a connected device to control your game in the Editor.","OK");
			break;

		default:
			EditorUtility.DisplayDialog("Mobile Input","You have enabled Mobile Input, but you have a non-mobile build target selected in your build settings. The mobile control rigs won't be active or visible on-screen until you switch the build target to a mobile platform.","OK");
			break; 
		}
	}

	[MenuItem("Mobile Input/Enable", true)]
	static bool EnableValidate()
	{
		var defines = GetDefinesList(mobileBuildTargetGroups[0]);
		return !defines.Contains("MOBILE_INPUT");
	}

	[MenuItem("Mobile Input/Disable")]
	static void Disable()
	{
		SetEnabled("MOBILE_INPUT",false,true);
		switch (EditorUserBuildSettings.activeBuildTarget)
		{
		case BuildTarget.Android:
		case BuildTarget.iPhone:
		case BuildTarget.WP8Player:
		case BuildTarget.BB10:
			EditorUtility.DisplayDialog("Mobile Input","You have disabled Mobile Input. Mobile control rigs won't be visible, and the Cross Platform Input functions will always return standalone controls.","OK");
			break;		
        }
        
        
	}
	[MenuItem("Mobile Input/Disable", true)]
	static bool DisableValidate()
	{
		var defines = GetDefinesList(mobileBuildTargetGroups[0]);
		return defines.Contains("MOBILE_INPUT");
	}

	static BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[]
	{
		BuildTargetGroup.Standalone,
		BuildTargetGroup.WebPlayer,
		BuildTargetGroup.Android,
		BuildTargetGroup.iPhone,
        BuildTargetGroup.WP8,
		BuildTargetGroup.BB10,
    };
    
	static BuildTargetGroup[] mobileBuildTargetGroups = new BuildTargetGroup[]
	{
		BuildTargetGroup.Android,
		BuildTargetGroup.iPhone,
        BuildTargetGroup.WP8,
		BuildTargetGroup.BB10,
    };
    
	static void SetEnabled(string defineName, bool enable, bool mobile)
	{
		//Debug.Log("setting "+defineName+" to "+enable);
		foreach (var group in mobile ? mobileBuildTargetGroups : buildTargetGroups)
		{
			var defines = GetDefinesList(group);
			if (enable)
			{
				if (!defines.Contains(defineName)) 
				{
					defines.Add(defineName);
				} else {
					return;
				}
			} else 	{
				if (defines.Contains(defineName))
				{
					while (defines.Contains(defineName)) defines.Remove(defineName);
				} else {
					return;
				}
			}
			string definesString = string.Join(";",defines.ToArray());
			PlayerSettings.SetScriptingDefineSymbolsForGroup( group, definesString );
		}
	}
	
	static List<string> GetDefinesList(BuildTargetGroup group)
	{
		return new List<string>( PlayerSettings.GetScriptingDefineSymbolsForGroup( group ).Split(';'));
	}

	
}
