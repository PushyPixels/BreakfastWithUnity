using UnityEngine;
using System.Collections;

public class BallisticTargeting : MonoBehaviour
{
	public Transform target;
	public Transform targetIndicator1;
	public Transform targetIndicator2;

	public GameObject targetHUD1;
	public GameObject targetHUD2;

	public float velocity = 10.0f; //Note: NOT LIVE TWEAKABLE because of caching

	public bool debugMode = false;

	private float v;
	private float vSquared;
	private float vHyperCubed;


	public class BallisticInfo
	{
		public float lowAngle;
		public float highAngle;
	}

	// Use this for initialization
	void Start ()
	{
		v = velocity;
		vSquared = v * v;
		vHyperCubed = vSquared * vSquared;
	}

	BallisticInfo CalculateTrajectoryAngles()
	{
		Vector3 targetVector = target.position - transform.position;
		float height = targetVector.y;
		targetVector.y = 0.0f;

		float x = targetVector.magnitude;
		float y = height;
		float g = Mathf.Abs(Physics.gravity.y);

		float underTheRoot = vHyperCubed - (g*(g*x*x + 2*y*vSquared));

		if(underTheRoot < 0.0f)
		{
			return null;
		}

		float root = Mathf.Sqrt(underTheRoot);

		float top1 = vSquared + root;
		float top2 = vSquared - root;
		float bottom = g*x;

		float angle1 = Mathf.Atan2(bottom,top1);
		float angle2 = Mathf.Atan2(bottom,top2);

		BallisticInfo returnValue = new BallisticInfo();
		returnValue.lowAngle = angle1*Mathf.Rad2Deg;
		returnValue.highAngle = angle2*Mathf.Rad2Deg;

		return returnValue;
	}

	// Update is called once per frame
	void Update ()
	{
		BallisticInfo info = CalculateTrajectoryAngles();

		if(info != null)
		{
			targetHUD1.SetActive(true);
			targetHUD2.SetActive(true);
			targetIndicator1.localEulerAngles = new Vector3(-info.lowAngle,0.0f,0.0f);
			targetIndicator2.localEulerAngles = new Vector3(-info.highAngle,0.0f,0.0f);
			if(debugMode)
			{
				transform.localRotation = Quaternion.LookRotation(target.position - transform.position);
				Camera.main.transform.localEulerAngles = new Vector3(-info.highAngle,0.0f,0.0f);
			}
		}
		else
		{
			targetHUD1.SetActive(false);
			targetHUD2.SetActive(false);
		}
	}
}
