using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetManager : MonoBehaviour
{
	public static TargetManager Instance;

	public float scaleMultiplier = 1.0f;
	public LayerMask targetMask = -1;
	public Animator targetingAnimator;
	public Text targetInfo;
	public Vector2 minSize = Vector2.one*16.0f;
	public float maxScreenPercentage = 1.0f;
	public string targetViewLayerName = "TargetView";
	public string disabledLayerName = "Disabled";

	public bool useScreenSpaceCamera = false;

	private RectTransform rectTransform;

	[HideInInspector]
	public Transform target = null;

	// Use this for initialization
	void Start ()
	{
		Instance = this;
		rectTransform = GetComponent<RectTransform>();
		transform.parent.position = Vector3.one*10000.0f;
	}

	void Update()
	{
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit,Mathf.Infinity,targetMask))
		{
			if(hit.transform != target)
			{
				targetingAnimator.SetTrigger("ActivateTargeting");

				if(target != null)
				{
					try
					{
						target.GetComponentInChildren<TargetView>().gameObject.layer = LayerMask.NameToLayer(disabledLayerName);
					}
					catch
					{
						MissingTargetError();
					}
				}
				target = hit.transform;

				try
				{
					target.GetComponentInChildren<TargetView>().gameObject.layer = LayerMask.NameToLayer(targetViewLayerName);
				}
				catch
				{
					MissingTargetError();
				}
			}
		}
	}

	void MissingTargetError()
	{
		Debug.LogError("Target " + target.name + " does not have a TargetView representation.  Please add one!", target);
	}
	
	// Update is called once per frame
	void LateUpdate()
	{
		if(target != null)
		{
			if(Vector3.Dot(target.position - Camera.main.transform.position, Camera.main.transform.forward) > 0)
			{
				if(!useScreenSpaceCamera)
				{
					//This only works for Screen Space - Overlay
					transform.parent.position = Camera.main.WorldToScreenPoint(target.position);
					Vector3 newPosition = transform.parent.localPosition;
					newPosition.z = 0.0f;
					transform.parent.localPosition = newPosition;
				}
				else
				{
					//This works for Screen Space - Overlay, and Screen Space - Camera
					//Neither is applicable for World Space
					Vector3 newPosition = Camera.main.WorldToScreenPoint(target.position);
					RectTransform parent = transform.parent.GetComponent<RectTransform>();
					parent.anchoredPosition = (Vector3)newPosition;

					newPosition = parent.localPosition;
					newPosition.z = 0.0f;
					parent.localPosition = newPosition;

					//Notes: anchoredPosition3D is broken, it does not set the y value properly
					//Setting Z on Screen Space Camera isn't possible through transform.position
					//You have to set the x and y using anchoredPosition (Set anchors to 0 in inspector), and then z via localPosition
					//Setting localPosition only could work as well, but requires half-screen offset when using Camera.main.WorldToScreenPoint(target.position);
				}
			}

			Rect worldBounds = GUIRectWithObject(target);
			
			rectTransform.sizeDelta = Vector2.Max(new Vector2(worldBounds.width,worldBounds.height)*scaleMultiplier,minSize);
			float smallestSide = Mathf.Min(Screen.width,Screen.height);
			rectTransform.sizeDelta = Vector2.Min(rectTransform.sizeDelta,Vector2.one*smallestSide*maxScreenPercentage);

			targetInfo.text = target.name + "\n" +
				"X:" + target.position.x.ToString("F1") + " Y:" + target.position.y.ToString("F1") + " Z:" + target.position.z.ToString("F1") + "\n" +
					"Distance: " + (target.position - Camera.main.transform.position).magnitude.ToString("F2") + "M";
		}
		else
		{
			transform.parent.position = Vector3.one*10000.0f;
		}
	}

	public static Rect GUIRectWithObject(Transform trans)
	{
		Vector3 cen = trans.renderer.bounds.center;
		Vector3 ext = trans.renderer.bounds.extents;
		Vector2[] extentPoints = new Vector2[8]
		{
			Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
			
			Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
			Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
		};
		
		Vector2 min = extentPoints[0];
		Vector2 max = extentPoints[0];
		
		foreach(Vector2 v in extentPoints)
		{
			min = Vector2.Min(min, v);
			max = Vector2.Max(max, v);
		}
		
		return new Rect(min.x, min.y, max.x-min.x, max.y-min.y);
	}
}
