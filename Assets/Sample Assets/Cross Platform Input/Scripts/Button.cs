using UnityEngine;

[RequireComponent(typeof(GUIElement))]
public class Button : MonoBehaviour
{

    public string buttonName = "Fire1";                     // The name used for the button
    public bool pairedWithInputManager;
    private AbstractButton m_Button;

    void OnEnable()
    {
        m_Button = ButtonFactory.GetPlatformSpecificButtonImplementation ();
        m_Button.Enable(buttonName, pairedWithInputManager, GetComponent<GUIElement>().GetScreenRect());
    }


    void OnDisable()
    {

        // remove the button from the cross platform input manager
        m_Button.Disable ();
    }

    void Update()
    {
        m_Button.Update ();
    }
}
