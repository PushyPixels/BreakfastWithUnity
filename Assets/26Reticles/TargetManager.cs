using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetManager : MonoBehaviour
{
	public float scaleMultiplier = 1.0f;
	public LayerMask targetMask = -1;
	public Text targetInfo;

	private RectTransform rectTransform;
	private CanvasRenderer canvasRenderer;
	private Transform target = null;

	// Use this for initialization
	void Start ()
	{
		rectTransform = GetComponent<RectTransform>();
		canvasRenderer = GetComponent<CanvasRenderer>();
		//canvasRenderer.renderer.enabled = false;
		transform.position = Vector3.one*10000.0f;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit,Mathf.Infinity,targetMask))
		{
			target = hit.transform;
		}

		if(target != null && Vector3.Dot(target.position - Camera.main.transform.position, Camera.main.transform.forward) > 0)
		{
			transform.position = Camera.main.WorldToScreenPoint(target.position);
			Rect worldBounds = GUIRectWithObject(target);

			rectTransform.sizeDelta = new Vector2(worldBounds.width,worldBounds.height)*scaleMultiplier;

			targetInfo.text = target.name + "\n" +
				"X:" + target.position.x.ToString("F1") + " Y:" + target.position.y.ToString("F1") + " Z:" + target.position.z.ToString("F1") + "\n" +
					"Distance: " + (Camera.main.transform.position - target.position).magnitude.ToString("F2");
		}
		else
		{
			transform.position = Vector3.one*10000.0f;
			//canvasRenderer.renderer.enabled = false;
		}
	}

	public static Rect GUIRectWithObject(Transform trans)
	{
		Vector3 cen = trans.GetComponent<Renderer>().bounds.center;
		Vector3 ext = trans.GetComponent<Renderer>().bounds.extents;
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
