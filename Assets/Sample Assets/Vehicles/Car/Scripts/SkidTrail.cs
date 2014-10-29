using UnityEngine;
using System.Collections;

public class SkidTrail : MonoBehaviour {

	// this script works in conjunction with a trail renderer to leave a skid trail behind wheels.
	// The Wheel script instantiates a new skid trail for each new skid, which begins parented to the wheel,
	// and is detached (by setting the transform.parent to null) when the skid stops.

	[SerializeField] float persistTime;     // the amount of time for the skid trail to persist
	[SerializeField] float fadeDuration;    // the amount of time before the skid trail will start to fade
	float startAlpha;

	// Use this for initialization
	IEnumerator Start () {

		while(true) {
			yield return new WaitForSeconds(1);

			// check whether this skid trail has finished
			// (the Wheel script sets the parent to null when the skid finishes)
			if (transform.parent == null) {

                // set the start colour
				Color startCol = renderer.material.color;

                // wait for the persist time
				yield return new WaitForSeconds(persistTime);

				float t = Time.time;

                // fade out the skid mark
				while (Time.time < t+fadeDuration) {
					float i = Mathf.InverseLerp(t,t+fadeDuration,Time.time);
					renderer.material.color = startCol * new Color(1,1,1,1-i);
					yield return null;
				}

                // the object has faded and is now done so destroy it
				Destroy(gameObject);
			}
		}
	
	}
}
