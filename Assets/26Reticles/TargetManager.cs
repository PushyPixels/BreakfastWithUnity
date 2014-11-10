using UnityEngine;
using System.Collections;

public class TargetManager : MonoBehaviour
{
	public Transform target;
	public float scaleMultiplier = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		//transform.position = Camera.main.WorldToScreenPoint(target.position);
		transform.localScale = target.renderer.bounds.size*scaleMultiplier;
	}
}
