using UnityEngine;
using System.Collections;

public class DrawPositionVectorOnDrawGizmos : MonoBehaviour
{
	public Color color = Color.green;
	public float arrowHeadLength = 0.25f;
	public bool showLocalPosition = true;

	void OnDrawGizmos()
	{
		Gizmos.color = color;

		if(showLocalPosition && transform.parent != null)
		{
			Gizmos.DrawLine(transform.parent.position,transform.position);

			if(Camera.current != null)
			{
				Vector3 positionVector = transform.localPosition.normalized;
				
				Vector3 arrowhead1 = Vector3.Cross(positionVector,Camera.current.transform.forward).normalized*arrowHeadLength - positionVector*arrowHeadLength;
				Gizmos.DrawLine(transform.position,transform.position+arrowhead1);
				
				Vector3 arrowhead2 = Vector3.Cross(Camera.current.transform.forward,positionVector).normalized*arrowHeadLength - positionVector*arrowHeadLength;
				Gizmos.DrawLine(transform.position,transform.position+arrowhead2);
			}
		}
		else
		{
			Gizmos.DrawLine(Vector3.zero,transform.position);
			if(Camera.current != null)
			{
				Vector3 positionVector = transform.position.normalized;

				Vector3 arrowhead1 = Vector3.Cross(positionVector,Camera.current.transform.forward).normalized*arrowHeadLength - positionVector*arrowHeadLength;
				Gizmos.DrawLine(transform.position,transform.position+arrowhead1);

				Vector3 arrowhead2 = Vector3.Cross(Camera.current.transform.forward,positionVector).normalized*arrowHeadLength - positionVector*arrowHeadLength;
				Gizmos.DrawLine(transform.position,transform.position+arrowhead2);
			}
		}
	}
}
