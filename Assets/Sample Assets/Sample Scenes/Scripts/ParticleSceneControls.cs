using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ParticleSceneControls : MonoBehaviour {
	
	public DemoParticleSystemList demoParticles;
	public float distFromSurface = 0.5f;
	
	public float multiply = 1;
	public bool clearOnChange = false;
	public GUIText titleGuiText;
	public Transform demoCam;
	public GUIText interactionGuiText;


	ParticleSystemMultiplier particleMultiplier;

	List<Transform> currentParticleList = new List<Transform>();

	Transform instance;
	static int selectedIndex = 0;
	Vector3 camOffsetVelocity = Vector3.zero;
	Vector3 lastPos;

	public enum Mode 
	{
		Activate,
		Instantiate,
		Trail
	}
	public enum AlignMode
	{
		Normal,
		Up
	}

	static DemoParticleSystem selected;
	
	void Awake()
	{
		Select(selectedIndex);
	}

	
	void Update()
	{
		demoCam.localPosition = Vector3.SmoothDamp (demoCam.localPosition, Vector3.forward * -selected.camOffset, ref camOffsetVelocity, 1);
	
		if (CrossPlatformInput.GetButtonDown("NextParticleSystem"))
		{
			selectedIndex++;
			if (selectedIndex == demoParticles.items.Length) selectedIndex = 0;
			Select (selectedIndex);
			return;
		}
		if (CrossPlatformInput.GetButtonDown("PreviousParticleSystem"))
		{
			selectedIndex--;
			if (selectedIndex == -1) selectedIndex = demoParticles.items.Length-1;
			Select (selectedIndex);
			return;
		}

		if (selected.mode == Mode.Activate)
		{
			// this is for a particle system that just needs activating, and needs no interaction (eg, duststorm)
			return;
		}


		bool oneShotClick = (Input.GetMouseButtonDown(0) && selected.mode == Mode.Instantiate);
		bool repeat = (Input.GetMouseButton(0) &&  selected.mode == Mode.Trail);
		
		if (oneShotClick || repeat)
		{
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			if (Physics.Raycast(ray,out hit))
			{
				var rot = Quaternion.LookRotation(hit.normal);

				if (selected.align == AlignMode.Up) rot = Quaternion.identity;

				var pos = hit.point + hit.normal * distFromSurface;

				if ((pos-lastPos).magnitude > selected.minDist)
				{

					if (selected.mode != Mode.Trail || instance == null)
					{

						instance = (Transform)Instantiate( selected.transform, pos, rot );
						
						if (particleMultiplier != null)
						{
							instance.GetComponent<ParticleSystemMultiplier>().multiplier = multiply;
						}

						currentParticleList.Add (instance);

						if (selected.maxCount > 0 && currentParticleList.Count > selected.maxCount)
						{
							if (currentParticleList[0] != null)
							{
								Destroy (currentParticleList[0].gameObject);
							}
							currentParticleList.RemoveAt(0);
							
						}

					} else {
						instance.position = pos;
						instance.rotation = rot;
					}

					if (selected.mode == Mode.Trail)
					{
						instance.transform.GetComponent<ParticleSystem>().enableEmission = false;
						instance.transform.GetComponent<ParticleSystem>().Emit(1);
					} 

					instance.parent = hit.transform;
					lastPos = pos;
				} 

			}
		}

	}



	void Select(int i)
	{
		selected = demoParticles.items[i];
		instance = null;
		foreach (var otherEffect in demoParticles.items)
		{
			if ((otherEffect != selected) && (otherEffect.mode == Mode.Activate))
			{
				if (otherEffect != null)
				{
					otherEffect.transform.gameObject.SetActive(false);
				}
			}
		}
		if (selected.mode == Mode.Activate)
		{
			selected.transform.gameObject.SetActive(true);
		}
		particleMultiplier = selected.transform.GetComponent<ParticleSystemMultiplier>();
		multiply = 1;
		if (clearOnChange)
		{
			while(currentParticleList.Count > 0)
			{
				Destroy (currentParticleList[0].gameObject);
				currentParticleList.RemoveAt(0);
			}
		}

		interactionGuiText.text = selected.instructionText;
		titleGuiText.text = selected.transform.name;
	}
	
	
	[System.Serializable]
	public class DemoParticleSystem
	{
		public Transform transform;
		public Mode mode;
		public AlignMode align;
		public int maxCount;
		public float minDist;
		public int camOffset = 15;
		public string instructionText;
	}
	
	[System.Serializable]
	public class DemoParticleSystemList
	{
		public DemoParticleSystem[] items;
	}
	

}

/*
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof(ParticleSceneControls.DemoParticleSystemList))]
public class DemoParticleSystemListDrawer : PropertyDrawer
{
	float lineHeight = 18;
	float spacing = 4;
	
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty (position, label, property);

		float x = position.x;
		float y = position.y;
		float inspectorWidth = position.width;
		
		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		
		var items = property.FindPropertyRelative ("items");
		float lineHeight = 18;
		bool changedLength = false;
		if (items.arraySize > 0)
		{
			
			for (int i=-1; i<items.arraySize; ++i) {
				
				var item = items.GetArrayElementAtIndex (i);

				string[] titles = new string[] { "Effect", "Mode", "Align To", "Max Count", "Min Dist", "Offset" };
				string[] props = new string[] { "transform", "mode", "align", "maxCount", "minDist", "camOffset"};
				float[] widths = new float[] { .35f, .2f, .15f, .1f, .1f, .1f };


				float rowX = x;
				for (int n=0; n<props.Length; ++n)
				{
					float w = widths[n] * inspectorWidth;
					
					// Calculate rects
					Rect rect = new Rect (rowX, y, w, lineHeight);
					rowX += w;
					
					if (i == -1)
					{
						// draw title labels
						EditorGUI.LabelField(rect, titles[n]);
					} else {
						if (props[n]=="-" || props[n]=="^" || props[n]=="v")
						{
							if (GUI.Button (rect, props[n]))
							{
								switch (props[n])
								{
								case "-":
									items.DeleteArrayElementAtIndex(i);
									items.DeleteArrayElementAtIndex(i);
									changedLength = true;
									break;
								case "v":
									if (i > 0) items.MoveArrayElement(i,i+1);
									break;
								case "^":
									if (i < items.arraySize-1) items.MoveArrayElement(i,i-1);
									break;
								}
								
							}
							
						} else {
							
							SerializedProperty prop = item.FindPropertyRelative(props[n]);	
							EditorGUI.PropertyField(rect, prop, GUIContent.none);
							
						}
					}
				}
				
				y += lineHeight + spacing;



				titles = new string[] { "Instructions Text", "", "", "" };
				props = new string[] { "instructionText", "^", "v", "-"  };
				widths = new float[] { .85f, .05f, .05f, .05f };
				
				
				rowX = x;
				for (int n=0; n<props.Length; ++n)
				{
					float w = widths[n] * inspectorWidth;
					
					// Calculate rects
					Rect rect = new Rect (rowX, y, w, lineHeight);
					rowX += w;
					
					if (i == -1)
					{
						// draw title labels
						EditorGUI.LabelField(rect, titles[n]);
					} else {
						if (props[n]=="-" || props[n]=="^" || props[n]=="v")
						{
							if (GUI.Button (rect, props[n]))
							{
								switch (props[n])
								{
								case "-":
									items.DeleteArrayElementAtIndex(i);
									items.DeleteArrayElementAtIndex(i);
									changedLength = true;
									break;
								case "v":
									if (i > 0) items.MoveArrayElement(i,i+1);
									break;
								case "^":
									if (i < items.arraySize-1) items.MoveArrayElement(i,i-1);
									break;
								}
								
							}
							
						} else {
							
							SerializedProperty prop = item.FindPropertyRelative(props[n]);	
							EditorGUI.PropertyField(rect, prop, GUIContent.none);
							
						}
					}
				}
				
				y += lineHeight + spacing + 5;


				if (changedLength)
				{
					break;
				}
			}
			
		}




		// add button
		var addButtonRect = new Rect ((x + position.width) - .05f*inspectorWidth, y, .05f*inspectorWidth, lineHeight);
		if (GUI.Button (addButtonRect, "+")) {
			items.InsertArrayElementAtIndex(items.arraySize);
		}
		
		y += lineHeight + spacing;
		
		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty ();
	}
	
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		SerializedProperty items = property.FindPropertyRelative ("items");
		float lineAndSpace = lineHeight + spacing;
		return 40 + (items.arraySize * ((lineAndSpace * 2) + 5)) + lineAndSpace + 5;
	}
	
}
#endif

*/


