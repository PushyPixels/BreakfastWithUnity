using UnityEngine;

[RequireComponent(typeof(GUIText))]
public class AeroplaneGUI : MonoBehaviour
{

    public AeroplaneController plane;           // Reference to the aeroplane controller.
    private const float MpsToKph = 3.6f;        // Constant for converting metres per second to kilometres per hour.
   
	// template for string output
	private string displayText = "Throttle: {0:0%}\nSpeed: {1:0000}KM/H\nAltitude: {2:0000}M";


    private void Update () {

        // setup the args for the string formatting
        object[] args = new object[] { plane.Throttle, plane.ForwardSpeed * MpsToKph, plane.Altitude };

        // display the aeroplane gui information
        guiText.text = string.Format(displayText, args);
    }

}
