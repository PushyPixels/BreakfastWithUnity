using UnityEngine;

[RequireComponent (typeof(GUIElement))]
public class AxisTouchButton : MonoBehaviour {

	// designed to work in a pair with another axis touch button
	// (typically with one having -1 and one having 1 axisValues)
	public string axisName = "Horizontal";                  // The name of the axis
	public float axisValue = 1;                             // The axis that the value has
	public float responseSpeed = 3;                         // The speed at which the axis touch button responds
	public float returnToCentreSpeed = 3;                   // The speed at which the button will return to its centre

	private AxisTouchButton pairedWith;                      // Which button this one is paired with
	private Rect rect;                                      // The rect of the gui element on screen
    private CrossPlatformInput.VirtualAxis axis;            // A reference to the virtual axis as it is in the cross platform input
    private bool pressedThisFrame;                          // Was the button pressed this frame
    private float axisCentre;                               // The centre of the axis


	void OnEnable () {

        // if the axis doesnt exist create a new one in cross platform input
		axis = CrossPlatformInput.VirtualAxisReference(axisName) ?? new CrossPlatformInput.VirtualAxis(axisName);

        // get the screen rect of the gui element
	    rect = GetComponent<GUIElement>().GetScreenRect();

		FindPairedButton();

	}

	void FindPairedButton()
	{
		// find the other button witch which this button should be paired
		// (it should have the same axisName)
		var otherAxisButtons = FindObjectsOfType(typeof(AxisTouchButton)) as AxisTouchButton[];
		
		if (otherAxisButtons != null) {
			for (int i = 0; i < otherAxisButtons.Length; i++) {
				if (otherAxisButtons[i].axisName == axisName && otherAxisButtons[i] != this)
				{
					pairedWith = otherAxisButtons[i];
					
					// the axis centre may not be zero, so we calculate it as the average of the two paired button's axisValues
					axisCentre = (axisValue + otherAxisButtons[i].axisValue) / 2;
				}
			}
		}
	}

	void OnDisable() {

        // The object is disabled so remove it from the cross platform input system
		axis.Remove();
	}
	

	void Update()
	{
		if (pairedWith == null)
		{
			FindPairedButton();

		}
		pressedThisFrame = false;
        // check through all touches
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);

            // check that the touch is in the button position
			if (rect.Contains(touch.position))
			{
                // update the axis and record that the button has been pressed this frame
				axis.Update( Mathf.MoveTowards( axis.GetValue, axisValue, responseSpeed * Time.deltaTime ) );
				pressedThisFrame = true;
			} 
		}
	}


	void LateUpdate()
	{
		if (pairedWith != null && !pressedThisFrame && !pairedWith.pressedThisFrame)
		{
			// neither button is being pressed for this axis, so move towards the centre
			// (*0.5 because both buttons in pair will do this!)
			axis.Update( Mathf.MoveTowards( axis.GetValue, axisCentre, returnToCentreSpeed * Time.deltaTime * 0.5f ) );
		}
	}


}
