using System;
using UnityEngine;

public class MobileInput : VirtualInput {

    public override float GetAxis (string name, bool raw) {
        return virtualAxes.ContainsKey(name) ? virtualAxes[name].GetValue : 0;
    }

    public override bool GetButton (string name, CrossPlatformInput.ButtonAction action) {

        bool containsName = virtualButtons.ContainsKey (name);
        if(containsName)
		{
			switch (action)
			{
				// virtual buttons are activated by touch or mouse click
			    case CrossPlatformInput.ButtonAction.GetButton:
			        return  virtualButtons[name].GetButton;
			    case CrossPlatformInput.ButtonAction.GetButtonDown:
					return  virtualButtons[name].GetButtonDown;
			    case CrossPlatformInput.ButtonAction.GetButtonUp:
					return virtualButtons[name].GetButtonUp;
			}
		} else {
			// no virtual button with this name, check "real" (input manager) buttons:
			switch (action)
			{
			case CrossPlatformInput.ButtonAction.GetButton:
				return Input.GetButton(name);
			case CrossPlatformInput.ButtonAction.GetButtonDown:
				return  Input.GetButtonDown(name);
			case CrossPlatformInput.ButtonAction.GetButtonUp:
				return Input.GetButtonUp(name);
			}
		}
		return false;
    }

    public override Vector3 MousePosition () {
        return virtualMousePosition;
    }
}
