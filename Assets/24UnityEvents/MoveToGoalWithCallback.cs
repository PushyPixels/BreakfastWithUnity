using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class MoveToGoalWithCallback : MonoBehaviour
{
	public Transform target;
	public float moveTime = 1.0f;

	public UnityEvent atDestinationCallback;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(MoveToGoal());
	}

	public void MoveAgain()
	{
		StartCoroutine(MoveToGoal());
	}
	
	// Update is called once per frame
	IEnumerator MoveToGoal()
	{
		float t = 0.0f;

		Vector3 startPosition = transform.position;
		Vector3 goalPosition = target.position;

		while(t < moveTime)
		{
			t += Time.deltaTime;
			transform.position = Vector3.Lerp(startPosition, goalPosition, t/moveTime);
			yield return null;
		}

		transform.position = target.position;
		atDestinationCallback.Invoke();
	}
}
