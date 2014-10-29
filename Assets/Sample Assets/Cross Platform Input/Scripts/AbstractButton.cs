using UnityEngine;

public abstract class AbstractButton {

    protected CrossPlatformInput.VirtualButton m_Button;        // A reference to the button in the cross platform input system
    protected Rect m_Rect;                                      // The zone on screen where the button exists


    public void Enable (string name,bool pairwithinputmanager, Rect rect) {

        m_Button = new CrossPlatformInput.VirtualButton(name, pairwithinputmanager);
        m_Rect = rect;   
    }

    public void Disable () {

        m_Button.Remove();
    }

    // override this to implement the logic for new button types i.e. touch for mobile and click for windows/osx/linux
    public abstract void Update();

}
