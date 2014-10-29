using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PivotBasedCameraRig : AbstractTargetFollower
{
	// This script is designed to be placed on the root object of a camera rig,
	// comprising 3 gameobjects, each parented to the next:
	
	// 	Camera Rig
	// 		Pivot
	// 			Camera

	protected Transform cam;                              // the transform of the camera
	protected Transform pivot;                            // the point at which the camera pivots around
	protected Vector3 lastTargetPosition;

	[SerializeField] protected bool followTargetInEditMode = true;
	public string warning { get; private set; }

	protected virtual void Awake() {
		// find the camera in the object hierarchy
		cam = GetComponentInChildren<Camera>().transform;
		pivot = cam.parent;
	}

	protected override void Start ()
	{
		base.Start ();
	}
	
	virtual protected void Update()
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying && followTargetInEditMode)
		{
			if (target != null)
			{
				float delta = (target.position-transform.position).magnitude;
				if (delta > 0.1f && lastTargetPosition == target.position) {
					warning = "The Rig's position is automatically locked to the target's position. You can use the child objects (the Pivot and the Camera) to adjust the view.";
					transform.position = target.position;
				} else {
					warning = "";
				}
				FollowTarget(999);
				lastTargetPosition = target.position;
			}

			
			if (Mathf.Abs (cam.localPosition.x) > .5f || Mathf.Abs (cam.localPosition.y) > .5f)
			{
				EditorUtility.DisplayDialog("Camera Rig Warning", "You should only adjust this Camera's Z position. The X and Y values must remain zero. Instead, move the Camera's parent (the \"Pivot\") to adjust the camera view","OK");
				cam.localPosition = Vector3.Scale(cam.localPosition, Vector3.forward);
				EditorUtility.SetDirty(cam);
			}

			cam.localPosition = Vector3.Scale(cam.localPosition, Vector3.forward);

			return;
		} else {
			warning = "";
		}
		#endif
	}

	protected override void FollowTarget (float deltaTime)
	{
		// should be overridden
	}

	void OnDrawGizmos()
	{
		if (pivot != null && cam != null)
		{
			Gizmos.color = new Color(0,1,0,0.5f);
			Gizmos.DrawLine(transform.position, pivot.position);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(pivot.position, cam.transform.position);
		}
	}
	
}