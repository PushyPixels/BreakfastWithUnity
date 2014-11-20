using UnityEngine;
using System.Collections;

public class SpotShadow : MonoBehaviour
{
	public GameObject spotShadow;
	public float maxSize = 1.0f;
	public float maxDistance = 10.0f;
	public float offset = 0.05f;

	private GameObject spotShadowInstance;

	// Update is called once per frame
	void Update()
	{
		RaycastHit hit;

		if(spotShadowInstance == null)
		{
			spotShadowInstance = Instantiate(spotShadow) as GameObject;
		}

		if(Physics.Raycast(transform.position,Vector3.down,out hit,maxDistance))
		{
			spotShadowInstance.SetActive(true);

			spotShadowInstance.transform.position = hit.point + hit.normal*offset;
			spotShadowInstance.transform.rotation = Quaternion.LookRotation(-hit.normal);

			float currentSize = Mathf.Lerp(maxSize,0.0f,hit.distance/maxDistance);
			spotShadowInstance.transform.localScale = Vector3.one*currentSize;
		}
		else
		{
			spotShadowInstance.SetActive(false);
		}
	}
}
