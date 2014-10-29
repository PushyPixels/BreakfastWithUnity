using UnityEngine;

//[RequireComponent(typeof(GUITexture))]
public class TouchJoystick : MonoBehaviour
{
	public enum AxisOption {                                                    // Options for which axes to use                                                     
		Both,                                                                   // Use both
		OnlyHorizontal,                                                         // Only horizontal
		OnlyVertical                                                            // Only vertical
	}

	public enum ReturnStyleOption {                                             // Style for the joystick to return to center
		Linear,                                                                 // Linearly
		Curved                                                                  // Curved
	}

	public enum InputMode {
		Joystick,
		TouchPadPositional,
		TouchPadRelativePositional,
		TouchPadSwipe,
	}

	public enum SensitivityRelativeTo
	{
		ZoneSize,
		Resolution
	}

    public Vector2 deadZone = Vector2.zero;                                     // The dead zone where the joystick will not be regarded as having input
    public bool normalize;                                                      // Toggle for normalising the input from the joystick
	public Vector2 autoReturnSpeed = new Vector2(4,4);                          // The speed at which the joystick X and Y will return to center
	public string horizontalAxisName = "Horizontal";                            // The name given to the horizontal axis for the cross platform input
    public string verticalAxisName = "Vertical";                                // The name given to the vertical axis for the cross platform input 
    public AxisOption axesToUse = AxisOption.Both;                              // The options for the axes that the still will use
	public bool invertX = false;
	public bool invertY = false;
	public InputMode inputMode;													// the type of input mode. (joystick, positional touchpad or swipe touchpad
	public GUITexture touchZone;                                                // The area in which the joystick will accept touch input on the screen (useful for constraining a stick to inside an texture area)
	public float touchZonePadding = 0;											// An amount of padding around the inside of the touchzone which is not usable
	public ReturnStyleOption autoReturnStyle = ReturnStyleOption.Curved;        // The stored option for the return style of the joystick
	public float sensitivity = 1f;
	public float interpolateTime = 2f;
	public Vector2 startPosition = Vector2.zero;
	public bool relativeSensitivity;
	public SensitivityRelativeTo sensitivityRelativeTo;

    private static TouchJoystick[] joysticks;                                   // A static collection of all joysticks
    private static bool enumeratedJoysticks;                                    // A check so that we know we have an enumeration of all the joysticks
	private Rect touchZoneRect;                                                 // The area on the screen where the touch zone is
	private Vector2 position;                                                   // The position on screen of the joystick
    private int lastFingerId = -1;                                              // Finger last used for this joystick
    private GUITexture gui;                                                     // The texture used for the joystick
    private Rect defaultRect;                                                   // This stores the default rect so we can snap back to it
    private Boundary guiBoundary = new Boundary();                              // A boundary used for clamping the joystick movement
    private Vector2 guiTouchOffset;                                             // Offset to apply to touch input
    private Vector2 guiCenter;                                                  // center of joystick
	private bool moveStick;                                                     // whether the stick graphic should move (it shouldn't if it is being used as the touchpad zone)
	private bool touchPad;                                                       // Is this a touch pad
	private CrossPlatformInput.VirtualAxis horizontalVirtualAxis;               // Reference to the joystick in the cross platform input
    private CrossPlatformInput.VirtualAxis verticalVirtualAxis;                 // Reference to the joystick in the cross platform input
	private bool useX;                                                          // Toggle for using the x axis
	private bool useY;                                                          // Toggle for using the Y axis
	private bool getTouchZoneRect;
	private Vector2 lastTouchPos;
	private Vector2 touchDelta;
	private Vector2 touchStart;
	private float swipeScale;
	private float sensitivityRelativeX;
	private float sensitivityRelativeY;



    private void OnEnable()
    {
	
        // set axes to use
		useX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
		useY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

        // create new axes based on axes to use
		if (useX) {
			horizontalVirtualAxis = new CrossPlatformInput.VirtualAxis(horizontalAxisName);
		}
		if (useY) {
			verticalVirtualAxis = new CrossPlatformInput.VirtualAxis(verticalAxisName);
		} 

        // Cache this component at startup instead of looking up every frame	
        gui = GetComponent<GUITexture>();

		if (gui != null)
		{
	        // Store the default rect for the gui, so we can snap back to it
			defaultRect = gui.GetScreenRect();
		}

        transform.position = new Vector3(0.0f, 0.0f , transform.position.z);
		moveStick = true;
		if (inputMode == InputMode.TouchPadPositional || inputMode == InputMode.TouchPadRelativePositional || inputMode == InputMode.TouchPadSwipe) {
			touchPad = true;
			getTouchZoneRect = true;
            if (gui == null)
			{
				// no GUI on this object, so no stick to move
				moveStick = false;

			} else {
				if (touchZone == null) {
					// marked as a touchpad, but no touchzone gui assigned, so this object's
					// GUI is the touchzone, and no stick to move:
					touchZone = gui;
					moveStick = false;

				} else {
					// touchpad, plus we have GUI on this object and a separate touchzone,
					// so we do have a stick to move.
					moveStick = true;

				}
			} 

		} else {
			touchPad = false;

            // This is an offset for touch input to match with the top left
            // corner of the GUI
            guiTouchOffset.x = defaultRect.width*0.5f;
            guiTouchOffset.y = defaultRect.height*0.5f;

            // Cache the center of the GUI, since it doesn't change
            guiCenter.x = defaultRect.x + guiTouchOffset.x;
            guiCenter.y = defaultRect.y + guiTouchOffset.y;

            // Let's build the GUI boundary, so we can clamp joystick movement
            guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
            guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
            guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
            guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;

			moveStick = true;
        }

		if (gui != null)
		{
			gui.pixelInset = defaultRect;
			transform.localScale = Vector3.zero;
		}
    }


    private void OnDisable()
    {
        enumeratedJoysticks = false;

        // remove the joysticks from the cross platform input
		if (useX) {
		    horizontalVirtualAxis.Remove();
		}
		if (useY) {
		    verticalVirtualAxis.Remove();
		}
    }


    private void ResetJoystick()
    {
        // Release the finger control and set the joystick back to the default position
        lastFingerId = -1;
    }


    private void LatchedFinger(int fingerId)
    {
        // If another joystick has latched this finger, then we must release it
        if (lastFingerId == fingerId) {
            ResetJoystick();
		}
    }


    public void Update()
    {
		if (touchPad)
		{
			if (getTouchZoneRect) {
				getTouchZoneRect = false;
				touchZoneRect = touchZone.GetScreenRect();
				var center = touchZoneRect.center;
				touchZoneRect.width *= (1-touchZonePadding);
				touchZoneRect.height *= (1-touchZonePadding);
				touchZoneRect.center = center;
				position = startPosition;
				swipeScale = new Vector2(touchZoneRect.width,touchZoneRect.height).magnitude * .01f;
				switch (sensitivityRelativeTo)
				{
				case SensitivityRelativeTo.ZoneSize:
					// sensitivity relative to size of touch zone. Larger swipes required on larger screens.
					sensitivityRelativeX = touchZoneRect.width;
					sensitivityRelativeY = touchZoneRect.height;
					break;
				case SensitivityRelativeTo.Resolution:
					// arbitrary amount, so that sensitivity of 1 = 1 inch
					float dpi = (Screen.dpi > 0 ? Screen.dpi : 100 ); // use 100dpi if undiscoverable
					sensitivityRelativeX = dpi;
					sensitivityRelativeY = dpi;
					break;
				}
			}
		}

        // return the joystick/touchpad value to zero when not being used:
		if (lastFingerId == -1 || inputMode == InputMode.TouchPadSwipe) {
			if (touchPad) {
                // move the position based on the return style
				if (autoReturnStyle == ReturnStyleOption.Curved) {
					position.x = Mathf.Lerp(position.x, 0, Time.deltaTime*autoReturnSpeed.x);
					position.y = Mathf.Lerp(position.y, 0, Time.deltaTime*autoReturnSpeed.y);
				} else {
					position.x = Mathf.MoveTowards(position.x, 0, Time.deltaTime*autoReturnSpeed.x);
					position.y = Mathf.MoveTowards(position.y, 0, Time.deltaTime*autoReturnSpeed.y);
				}
			} else {

                // move the guitexture based on the return style
				Rect pRect = gui.pixelInset;
				if (autoReturnStyle == ReturnStyleOption.Curved) {
					pRect.x = Mathf.Lerp(pRect.x, defaultRect.x, Time.deltaTime*autoReturnSpeed.x*guiTouchOffset.x);
					pRect.y = Mathf.Lerp(pRect.y, defaultRect.y, Time.deltaTime*autoReturnSpeed.y*guiTouchOffset.y);
				} else {
					pRect.x = Mathf.MoveTowards(pRect.x, defaultRect.x, Time.deltaTime*autoReturnSpeed.x*guiTouchOffset.x);
					pRect.y = Mathf.MoveTowards(pRect.y, defaultRect.y, Time.deltaTime*autoReturnSpeed.y*guiTouchOffset.y);

				}
				gui.pixelInset = pRect;


			}
		}


        if (!enumeratedJoysticks)
        {
            // Collect all joysticks in the game, so we can relay finger latching messages
            joysticks = FindObjectsOfType<TouchJoystick>();
            enumeratedJoysticks = true;
        }

        var count = Input.touchCount;

        // if there are no touches reset the joystick
        if (count == 0) {
           ResetJoystick();
		}
        else
        {

            // loop through all the touched
            for (int i = 0; i < count; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 guiTouchPos = touch.position - guiTouchOffset;

                var shouldLatchFinger = false;

                // are we using a touch pad
                if (touchPad) {

                    // check that the touch is within the touch pad area
                    if (touchZoneRect.Contains(touch.position)) {
						shouldLatchFinger = true;
					}
                }
                else if (gui.HitTest(touch.position)) {
                    shouldLatchFinger = true;
                }

                // Latch the finger if this is a new touch
                if (shouldLatchFinger && (lastFingerId == -1 || lastFingerId != touch.fingerId)) {
                    if (touchPad) {
                        lastFingerId = touch.fingerId;
                    }
                    lastFingerId = touch.fingerId;

                    // Tell other joysticks we've latched this finger
                    for (int index = 0; index < joysticks.Length; index++) {
                        if (joysticks[index] != this) {
                            joysticks[index].LatchedFinger(touch.fingerId);
                        }
                    }
                }

                if (lastFingerId == touch.fingerId) {
                    if (touchPad) {
						switch(inputMode)
						{
							case (InputMode.TouchPadPositional):
							{
								// absolute position of touch relative to touchpad defines the input amount:
							Vector2 newAbsTouchPos = new Vector2( (touch.position.x - touchZoneRect.center.x) / sensitivityRelativeX, (touch.position.y - touchZoneRect.center.y) / sensitivityRelativeY) * 2;
						
								Vector2 newPosition = Vector2.Lerp (position, newAbsTouchPos * sensitivity, Time.deltaTime * interpolateTime );

		                        // scale & clamp the touch position inside the allowed touch zone, between -1 and 1
								if (useX) {
									position.x = Mathf.Clamp(newPosition.x, -1, 1);
								}
								if (useY) {
									position.y = Mathf.Clamp(newPosition.y, -1, 1);
								}

							} 
							break;

							case InputMode.TouchPadRelativePositional:
							{
								// position of touch relative to touch start position defines the input amount:
								if (touch.phase == TouchPhase.Began)
								{
									touchStart = touch.position;
								}
							Vector2 newRelativeTouchPos = new Vector2( (touch.position.x - touchStart.x)/sensitivityRelativeX, (touch.position.y - touchStart.y)/sensitivityRelativeY);
								
								Vector2 newPosition = Vector2.Lerp (position, newRelativeTouchPos * sensitivity * 2, Time.deltaTime * interpolateTime );
								
								// scale & clamp the touch position inside the allowed touch zone, between -1 and 1
								if (useX) {
									position.x = Mathf.Clamp(newPosition.x, -1, 1 );
								}
								if (useY) {
									position.y = Mathf.Clamp(newPosition.y, -1, 1 );
								}

							}
							break;

							case InputMode.TouchPadSwipe:
							{
								// swipe-based touchpad:
								// relative movement of touch within the touchpad defines the input amount.

								if (touch.phase == TouchPhase.Began)
								{
									lastTouchPos = touch.position;
									touchDelta = Vector2.zero;
								}
								touchDelta = Vector2.Lerp (touchDelta, (lastTouchPos-touch.position)/swipeScale, Time.deltaTime * interpolateTime);
								
								if (touch.deltaTime > 0)
								{
									if (useX) {
										float newx = touchDelta.x * sensitivity;
										position.x = newx;
									}
									if (useY) {
										float newy = touchDelta.y * sensitivity;
										position.y = newy;
									}
								}
								lastTouchPos = touch.position;
							}
							break;
						}
                    }
                    else
                    {

						// Change the location of the joystick graphic to match where the touch is
						gui.pixelInset = new Rect(
                            Mathf.Clamp(guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x),
                            Mathf.Clamp(guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y), 
                            gui.pixelInset.width, 
                            gui.pixelInset.height);
                    }

                    // if the touch is over then reset the joystick to its default position
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
	                	ResetJoystick();
					}
                }
            }
        }

        if (touchPad && moveStick) {
  
			// Change the location of the joystick graphic to match where the touch is
			gui.pixelInset = new Rect(
				Mathf.Lerp(touchZoneRect.x, touchZoneRect.x+touchZoneRect.width, position.x * 0.5f + 0.5f) - defaultRect.width * 0.5f,
				Mathf.Lerp(touchZoneRect.y, touchZoneRect.y+touchZoneRect.height, position.y * 0.5f + 0.5f) - defaultRect.height * 0.5f, 
				defaultRect.width, 
				defaultRect.height);
		}

		if (!touchPad) {
			// This is a regular touch-draggable joystick
            // Get a value between -1 and 1 based on the joystick graphic location
			if (useX) {
				position.x = (gui.pixelInset.x + guiTouchOffset.x - guiCenter.x)/guiTouchOffset.x;
			}
			if (useY) {
	            position.y = (gui.pixelInset.y + guiTouchOffset.y - guiCenter.y)/guiTouchOffset.y;
			}
        }

		// modifications before using as axis value:
		float modifiedX = position.x;
		float modifiedY = position.y;

        // Adjust for dead zone	
		var absoluteX = Mathf.Abs(modifiedX);
		var absoluteY = Mathf.Abs(modifiedY);

        if (absoluteX < deadZone.x) {
            // Report the joystick as being at the center if it is within the dead zone
			modifiedX = 0;
        } else if (normalize) {
            // Rescale the output after taking the dead zone into account
			modifiedX = Mathf.Sign(modifiedX)*(absoluteX - deadZone.x)/(1 - deadZone.x);
        }
        if (absoluteY < deadZone.y) {
            // Report the joystick as being at the center if it is within the dead zone
			modifiedY = 0;
        } else if (normalize) {
            // Rescale the output after taking the dead zone into account
			modifiedY = Mathf.Sign(modifiedY)*(absoluteY - deadZone.y)/(1 - deadZone.y);
        }

		// Adjust for inversions
		modifiedX *= invertX ? -1 : 1;
		modifiedY *= invertY ? -1 : 1;


        //update the relevant axes
		if (useX) {
			horizontalVirtualAxis.Update(modifiedX);
		}
		if (useY) {
			verticalVirtualAxis.Update(modifiedY);
		}

    }



	class Boundary 
	{
		public Vector2 min  = Vector2.zero;
		public Vector2 max  = Vector2.zero;
	}
}
