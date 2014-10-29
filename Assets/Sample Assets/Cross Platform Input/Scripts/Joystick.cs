using UnityEngine;

public class Joystick : JoystickAbstract {

    protected override void TypeSpecificOnEnable()
    {
        // This is an offset for touch input to match with the top left
        // corner of the GUI
        guiTouchOffset.x = defaultRect.width * 0.5f;
        guiTouchOffset.y = defaultRect.height * 0.5f;

        // Cache the center of the GUI, since it doesn't change
        guiCenter.x = defaultRect.x + guiTouchOffset.x;
        guiCenter.y = defaultRect.y + guiTouchOffset.y;

        // Let's build the GUI boundary, so we can clamp joystick movement
        guiBoundary.xMin = defaultRect.x - guiTouchOffset.x;
        guiBoundary.xMax = defaultRect.x + guiTouchOffset.x;
        guiBoundary.yMin = defaultRect.y - guiTouchOffset.y;
        guiBoundary.yMax = defaultRect.y + guiTouchOffset.y;

        moveStick = true;
    }


    protected override void ZeroWhenUnused()
    {
        if (lastFingerId == -1)
        {
            // move the guitexture based on the return style
            Rect pRect = gui.pixelInset;
            if (autoReturnStyle == ReturnStyleOption.Curved)
            {
                pRect.x = Mathf.Lerp(pRect.x, defaultRect.x, Time.deltaTime*autoReturnSpeed.x*guiTouchOffset.x);
                pRect.y = Mathf.Lerp(pRect.y, defaultRect.y, Time.deltaTime*autoReturnSpeed.y*guiTouchOffset.y);
            }
            else
            {
                pRect.x = Mathf.MoveTowards(pRect.x, defaultRect.x, Time.deltaTime*autoReturnSpeed.x*guiTouchOffset.x);
                pRect.y = Mathf.MoveTowards(pRect.y, defaultRect.y, Time.deltaTime*autoReturnSpeed.y*guiTouchOffset.y);
            }
            gui.pixelInset = pRect;
        }
    }


    protected override void ForEachTouch(Touch touch, Vector2 guiTouchPos)
    {

        bool shouldLatchFinger = gui.HitTest(touch.position);

        if (shouldLatchFinger && (lastFingerId == -1 || lastFingerId != touch.fingerId)) {
            lastFingerId = touch.fingerId;

            // Tell other joysticks we've latched this finger
            for (int index = 0; index < joysticks.Length; index++) {
                if (joysticks[index] != this) {
                    joysticks[index].LatchedFinger (touch.fingerId);
                }
            }
        }
        if (lastFingerId == touch.fingerId) {
                // Change the location of the joystick graphic to match where the touch is
                gui.pixelInset = new Rect (
                    Mathf.Clamp (guiTouchPos.x, guiBoundary.xMin, guiBoundary.xMax),
                    Mathf.Clamp (guiTouchPos.y, guiBoundary.yMin, guiBoundary.yMax),
                    gui.pixelInset.width,
                    gui.pixelInset.height);

                // if the touch is over then reset the joystick to its default position
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                    ResetJoystick ();
                }
            }
    }

    protected override void MoveJoystickGraphic()
    {
        // Get a value between -1 and 1 based on the joystick graphic location
        if (useX)
        {
            position.x = (gui.pixelInset.x + guiTouchOffset.x - guiCenter.x) / guiTouchOffset.x;
        }
        if (useY)
        {
            position.y = (gui.pixelInset.y + guiTouchOffset.y - guiCenter.y) / guiTouchOffset.y;
        }
    }
}
