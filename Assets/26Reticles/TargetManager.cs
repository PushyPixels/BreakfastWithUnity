using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetManager : MonoBehaviour
{
	public float scaleMultiplier = 1.0f;
	public LayerMask targetMask = -1;
	public Animator targetingAnimator;
	public Text targetInfo;

	private RectTransform rectTransform;
	private Transform target = null;

	// Use this for initialization
	void Start ()
	{
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
			}
			target = hit.transform;
		}
	}
	
	// Update is called once per frame
	void LateUpdate()
	{
		if(target != null)
		{
			transform.parent.position = Camera.main.WorldToScreenPoint(target.position);
			Rect worldBounds = GUIRectWithObject(target);

			rectTransform.sizeDelta = new Vector2(worldBounds.width,worldBounds.height)*scaleMultiplier;
		}
		else
		{
			transform.parent.position = Vector3.one*10000.0f;
		}

		targetInfo.text = target.name + "\n" +
			"X:" + target.position.x.ToString("F1") + " Y:" + target.position.y.ToString("F1") + " Z:" + target.position.z.ToString("F1") + "\n" +
				"Distance: " + (target.position - Camera.main.transform.position).magnitude.ToString("F1") + "M";
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
