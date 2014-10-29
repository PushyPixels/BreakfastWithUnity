using UnityEngine;

public abstract class JoystickAbstract : MonoBehaviour {

    public enum AxisOption
    {                                                    // Options for which axes to use                                                     
        Both,                                                                   // Use both
        OnlyHorizontal,                                                         // Only horizontal
        OnlyVertical                                                            // Only vertical
    }

    public enum ReturnStyleOption
    {                                             // Style for the joystick to return to center
        Linear,                                                                 // Linearly
        Curved                                                                  // Curved
    }

    public Vector2 deadZone = Vector2.zero;                                     // The dead zone where the joystick will not be regarded as having input
    public bool normalize;                                                      // Toggle for normalising the input from the joystick
    public Vector2 autoReturnSpeed = new Vector2(4, 4);                          // The speed at which the joystick X and Y will return to center
    public string horizontalAxisName = "Horizontal";                            // The name given to the horizontal axis for the cross platform input
    public string verticalAxisName = "Vertical";                                // The name given to the vertical axis for the cross platform input 
    public AxisOption axesToUse = AxisOption.Both;                              // The options for the axes that the still will use
    public bool invertX = false;
    public bool invertY = false;
    //public InputMode inputMode;													// the type of input mode. (joystick, positional touchpad or swipe touchpad
    public GUITexture touchZone;                                                // The area in which the joystick will accept touch input on the screen (useful for constraining a stick to inside an texture area)
    public float touchZonePadding = 0;											// An amount of padding around the inside of the touchzone which is not usable
    public ReturnStyleOption autoReturnStyle = ReturnStyleOption.Curved;        // The stored option for the return style of the joystick
    public float sensitivity = 1f;
    public float interpolateTime = 2f;
    public Vector2 startPosition = Vector2.zero;

    protected static JoystickAbstract[] joysticks;                                   // A static collection of all joysticks
    protected static bool enumeratedJoysticks;                                    // A check so that we know we have an enumeration of all the joysticks
    protected Rect touchZoneRect;                                                 // The area on the screen where the touch zone is
    protected Vector2 position;                                                   // The position on screen of the joystick
    protected int lastFingerId = -1;                                              // Finger last used for this joystick
    protected GUITexture gui;                                                     // The texture used for the joystick
    protected Rect defaultRect;                                                   // This stores the default rect so we can snap back to it
    protected Rect guiBoundary = new Rect();                                      // A boundary used for clamping the joystick movement
    protected Vector2 guiTouchOffset;                                             // Offset to apply to touch input
    protected Vector2 guiCenter;                                                  // center of joystick
    protected bool moveStick;                                                     // whether the stick graphic should move (it shouldn't if it is being used as the touchpad zone)
    protected bool touchPad;                                                      // Is this a touch pad
    protected CrossPlatformInput.VirtualAxis horizontalVirtualAxis;               // Reference to the joystick in the cross platform input
    protected CrossPlatformInput.VirtualAxis verticalVirtualAxis;                 // Reference to the joystick in the cross platform input
    protected bool useX;                                                          // Toggle for using the x axis
    protected bool useY;                                                          // Toggle for using the Y axis
    protected bool getTouchZoneRect;
    protected Vector2 lastTouchPos;
    protected Vector2 touchDelta;
    protected Vector2 touchStart;
    protected float swipeScale;


    protected virtual void TypeSpecificOnEnable ()
    {}


    protected void OnEnable()
    {
        CreateVirtualAxes();

        // Cache this component at startup instead of looking up every frame	
        gui = GetComponent<GUITexture>();

        if (gui != null)
        {
            // Store the default rect for the gui, so we can snap back to it
            defaultRect = gui.GetScreenRect();

            gui.pixelInset = defaultRect;
            transform.localScale = Vector3.zero;
        }

        transform.position = new Vector3(0.0f, 0.0f, transform.position.z);
        moveStick = true;
       
        TypeSpecificOnEnable();
       
        if (enumeratedJoysticks)
            return;
        // Collect all joysticks in the game, so we can relay finger latching messages
        joysticks = FindObjectsOfType<JoystickAbstract>();
        enumeratedJoysticks = true;
    }


    private void CreateVirtualAxes()
    {
        // set axes to use
        useX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
        useY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

        // create new axes based on axes to use
        if (useX)
            horizontalVirtualAxis = new CrossPlatformInput.VirtualAxis(horizontalAxisName);
        if (useY)
            verticalVirtualAxis = new CrossPlatformInput.VirtualAxis(verticalAxisName);
    }


    protected void OnDisable()
    {
        enumeratedJoysticks = false;

        // remove the joysticks from the cross platform input
        if (useX)
        {
            horizontalVirtualAxis.Remove();
        }
        if (useY)
        {
            verticalVirtualAxis.Remove();
        }
    }


    protected void ResetJoystick()
    {
        // Release the finger control and set the joystick back to the default position
        lastFingerId = -1;
    }


    protected internal virtual void LatchedFinger(int fingerId)
    {
        // If another joystick has latched this finger, then we must release it
        if (lastFingerId == fingerId)
        {
            ResetJoystick();
        }
    }


    protected virtual void TypeSpecificUpdate ()
    {}

    protected virtual void ZeroWhenUnused ()
    {}

    protected virtual void ForEachTouch (Touch touch, Vector2 guiTouchPos)
    {}

    protected virtual void MoveJoystickGraphic ()
    {}

    public void Update()
    {
        ZeroWhenUnused();
        
        var count = Input.touchCount;

        // if there are no touches reset the joystick
        if (count == 0)
        {
            ResetJoystick();
        }
        else
        {
            // loop through all the touched
            for (int i = 0; i < count; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 guiTouchPos = touch.position - guiTouchOffset;

                ForEachTouch(touch, guiTouchPos);
            }
        }

        MoveJoystickGraphic();

        // modifications before using as axis value:
        float modifiedX = position.x;
        float modifiedY = position.y;

        DeadZoneAndNormaliseAxes(ref modifiedX,ref modifiedY);

        AdjustAxesIfInverted(ref modifiedX, ref modifiedY);

        UpdateVirtualAxes(modifiedX, modifiedY);
    }


    private void DeadZoneAndNormaliseAxes(ref float modifiedX, ref float modifiedY)
    {
        // Adjust for dead zone	
        var absoluteX = Mathf.Abs(modifiedX);
        var absoluteY = Mathf.Abs(modifiedY);

        
        if (absoluteX < deadZone.x)
        {
            // Report the joystick as being at the center if it is within the dead zone
            modifiedX = 0;
        }
        else if (normalize)
        {
            // Rescale the output after taking the dead zone into account
            modifiedX = Mathf.Sign(modifiedX)*(absoluteX - deadZone.x)/(1 - deadZone.x);
        }
        if (absoluteY < deadZone.y)
        {
            // Report the joystick as being at the center if it is within the dead zone
            modifiedY = 0;
        }
        else if (normalize)
        {
            // Rescale the output after taking the dead zone into account
            modifiedY = Mathf.Sign(modifiedY)*(absoluteY - deadZone.y)/(1 - deadZone.y);
        }
    }


    private void AdjustAxesIfInverted(ref float modifiedX, ref float modifiedY)
    {
        // Adjust for inversions
        modifiedX *= invertX ? -1 : 1;
        modifiedY *= invertY ? -1 : 1;
    }


    private void UpdateVirtualAxes(float modifiedX, float modifiedY)
    {
        //update the relevant axes
        if (useX)
            horizontalVirtualAxis.Update(modifiedX);
        if (useY)
            verticalVirtualAxis.Update(modifiedY);
    }
}
