using UnityEngine;

public class TouchPad : JoystickAbstract {

    public enum SensitivityRelativeTo
    {
        ZoneSize,
        Resolution
    }

    public SensitivityRelativeTo sensitivityRelativeTo;
    protected float sensitivityRelativeX;
    protected float sensitivityRelativeY;

    protected override void TypeSpecificOnEnable()
    {
        if (gui == null)
        {
            // no GUI on this object, so no stick to move
            moveStick = false;
        }
        else
        {
            if (touchZone == null)
            {
                // marked as a touchpad, but no touchzone gui assigned, so this object's
                // GUI is the touchzone, and no stick to move:
                touchZone = gui;
                moveStick = false;
            }
            else
            {
                // touchpad, plus we have GUI on this object and a separate touchzone,
                // so we do have a stick to move.
                moveStick = true;
            }
        }

        //getTouchZoneRect = false;
        touchZoneRect = touchZone.GetScreenRect();
        var center = touchZoneRect.center;
        touchZoneRect.width *= (1 - touchZonePadding);
        touchZoneRect.height *= (1 - touchZonePadding);
        touchZoneRect.center = center;
        position = startPosition;
        swipeScale = new Vector2(touchZoneRect.width, touchZoneRect.height).magnitude * .01f;

        switch (sensitivityRelativeTo)
        {
            case SensitivityRelativeTo.ZoneSize:
                // sensitivity relative to size of touch zone. Larger swipes required on larger screens.
                sensitivityRelativeX = touchZoneRect.width;
                sensitivityRelativeY = touchZoneRect.height;
                break;
            case SensitivityRelativeTo.Resolution:
                // arbitrary amount, so that sensitivity of 1 = 1 inch
                float dpi = (Screen.dpi > 0 ? Screen.dpi : 100); // use 100dpi if undiscoverable
                sensitivityRelativeX = dpi;
                sensitivityRelativeY = dpi;
                break;
        }
    }


    protected override void ZeroWhenUnused()
    {
        if (lastFingerId != -1)
            return;
        if (autoReturnStyle == ReturnStyleOption.Curved)
        {
            position.x = Mathf.Lerp(position.x, 0, Time.deltaTime*autoReturnSpeed.x);
            position.y = Mathf.Lerp(position.y, 0, Time.deltaTime*autoReturnSpeed.y);
        }
        else
        {
            position.x = Mathf.MoveTowards(position.x, 0, Time.deltaTime*autoReturnSpeed.x);
            position.y = Mathf.MoveTowards(position.y, 0, Time.deltaTime*autoReturnSpeed.y);
        }
    }


    protected override void ForEachTouch(Touch touch, Vector2 guiTouchPos)
    {
       if (touchZoneRect.Contains(touch.position) && (lastFingerId == -1 || lastFingerId != touch.fingerId))
        {
            lastFingerId = touch.fingerId;

            // Tell other joysticks we've latched this finger
            for (int index = 0; index < joysticks.Length; index++)
            {
                if (joysticks[index] != this)
                {
                    joysticks[index].LatchedFinger(touch.fingerId);
                }
            }
        }
    }

    protected override void MoveJoystickGraphic()
    {
        if (moveStick)
        {
            // Change the location of the joystick graphic to match where the touch is
            gui.pixelInset = new Rect(
                Mathf.Lerp(touchZoneRect.x, touchZoneRect.x + touchZoneRect.width, position.x * 0.5f + 0.5f) - defaultRect.width * 0.5f,
                Mathf.Lerp(touchZoneRect.y, touchZoneRect.y + touchZoneRect.height, position.y * 0.5f + 0.5f) - defaultRect.height * 0.5f,
                defaultRect.width,
                defaultRect.height);
        }
    }
}
