using UnityEngine;

public class TouchPadRelativePositional : TouchPad {

    protected override void ForEachTouch(Touch touch, Vector2 guiTouchPos)
    {
        base.ForEachTouch(touch, guiTouchPos);

        if (lastFingerId != touch.fingerId)
            return;
        // position of touch relative to touch start position defines the input amount:
        if (touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
        }
        Vector2 newRelativeTouchPos = new Vector2((touch.position.x - touchStart.x) / sensitivityRelativeX, (touch.position.y - touchStart.y) / sensitivityRelativeY);

        Vector2 newPosition = Vector2.Lerp(position, newRelativeTouchPos * sensitivity * 2, Time.deltaTime * interpolateTime);

        // scale & clamp the touch position inside the allowed touch zone, between -1 and 1
        if (useX)
        {
            position.x = Mathf.Clamp(newPosition.x, -1, 1);
        }
        if (useY)
        {
            position.y = Mathf.Clamp(newPosition.y, -1, 1);
        }

        // if the touch is over then reset the joystick to its default position
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            ResetJoystick();
        }
    }
}
