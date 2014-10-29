using UnityEngine;

public class TouchPadSwipe : TouchPad {

    protected override void ZeroWhenUnused()
    {
        // move the position based on the return style
        if (autoReturnStyle == ReturnStyleOption.Curved)
        {
            position.x = Mathf.Lerp(position.x, 0, Time.deltaTime * autoReturnSpeed.x);
            position.y = Mathf.Lerp(position.y, 0, Time.deltaTime * autoReturnSpeed.y);
        }
        else
        {
            position.x = Mathf.MoveTowards(position.x, 0, Time.deltaTime * autoReturnSpeed.x);
            position.y = Mathf.MoveTowards(position.y, 0, Time.deltaTime * autoReturnSpeed.y);
        }
    }


    protected override void ForEachTouch(Touch touch, Vector2 guiTouchPos)
    {
        base.ForEachTouch(touch, guiTouchPos);

        if (lastFingerId != touch.fingerId)
            return;
        // swipe-based touchpad:
        // relative movement of touch within the touchpad defines the input amount.

        if (touch.phase == TouchPhase.Began)
        {
            lastTouchPos = touch.position;
            touchDelta = Vector2.zero;
        }
        touchDelta = Vector2.Lerp(touchDelta, (lastTouchPos - touch.position) / swipeScale, Time.deltaTime * interpolateTime);

        if (touch.deltaTime > 0)
        {
            if (useX)
            {
                float newx = touchDelta.x * sensitivity;
                position.x = newx;
            }
            if (useY)
            {
                float newy = touchDelta.y * sensitivity;
                position.y = newy;
            }
        }
        lastTouchPos = touch.position;
        lastTouchPos = touch.position;

        // if the touch is over then reset the joystick to its default position
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            ResetJoystick();
        }
    }
}
