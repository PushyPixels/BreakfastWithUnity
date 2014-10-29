using UnityEngine;

public class ButtonFactory
{
	public static AbstractButton GetPlatformSpecificButtonImplementation()
	{
		
		#if MOBILE_INPUT
		return new TouchButton();
		#else
		// click button always works as a tap button
		return new ClickButton();   
		#endif
	}
	
}

internal class TouchButton : ClickButton
{
	// touch button inherits from click button, and calls its base.Update
	// so that touchbuttons are always clickable in the editor with the mouse too!

	bool m_Pressed;			// whether the button is currently pressed
	
	public override void Update () {
		base.Update();

		for (int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch (i);
			
			// if the touch is within the button then change the button state to pressed
			// don't accept the touch if the touch has been dragged onto the button
			if (m_Rect.Contains (touch.position)) {
				
				if (m_Pressed)
					return;
				
				if (touch.phase != TouchPhase.Began) {
					continue;
				}
				
				m_Button.Pressed ();
				m_Pressed = true;
				
				// the button is pressed so exit
				return;
			}
		}
		
		if (m_Pressed) {
			m_Button.Released ();
			m_Pressed = false;
		}
	}
}

internal class ClickButton : AbstractButton {
	private bool pressed;
	
	public override void Update()
	{
		
		// if the mouse click is within the button then change the button state to pressed
		if (m_Rect.Contains (Input.mousePosition) && Input.GetMouseButtonDown (0)) 
		{
            if (!pressed) 
			{
				pressed = true;
				m_Button.Pressed ();
                return;
            }
		}
		if (Input.GetMouseButtonUp(0) && pressed) {
			pressed = false;
			m_Button.Released ();
		}
	}
}
