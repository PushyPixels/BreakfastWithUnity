using UnityEngine;

public class TouchPadPositional : TouchPad {

    protected override void ForEachTouch(Touch touch, Vector2 guiTouchPos)
    {
        base.ForEachTouch(touch, guiTouchPos);

        if (lastFingerId == touch.fingerId)
        {
            // absolute position of touch relative to touchpad defines the input amount:
            Vector2 newAbsTouchPos = new Vector2((touch.position.x - touchZoneRect.center.x) / sensitivityRelativeX, (touch.position.y - touchZoneRect.center.y) / sensitivityRelativeY) * 2;

            Vector2 newPosition = Vector2.Lerp(position, newAbsTouchPos * sensitivity, Time.deltaTime * interpolateTime);

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
}
