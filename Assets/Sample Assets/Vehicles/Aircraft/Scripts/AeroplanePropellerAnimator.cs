using UnityEngine;

public class AeroplanePropellerAnimator : MonoBehaviour
{
    [SerializeField] private Transform propellorModel;                // The model of the the aeroplane's propellor.
    [SerializeField] private Transform propellorBlur;                 // The plane used for the blurred propellor textures.
    [SerializeField] private Texture2D[] propellorBlurTextures;       // An array of increasingly blurred propellor textures.
	
	[SerializeField] [Range(0f,1f)] private float throttleBlurStart = 0.25f;     // The point at which the blurred textures start.
    [SerializeField] [Range(0f, 1f)] private float throttleBlurEnd = 0.5f;       // The point at which the blurred textures stop changing.
    [SerializeField] private float maxRpm = 2000;                                // The maximum speed the propellor can turn at.

	private AeroplaneController plane;              // Reference to the aeroplane controller.
	private int propellorBlurState = -1;            // To store the state of the blurred textures.
    private const float RpmToDps = 60f;             // For converting from revs per minute to degrees per second.
	

	void Awake ()
    {
        // Set up the reference to the aeroplane controller.
		plane = GetComponent<AeroplaneController>();

        // Set the propellor blur gameobject's parent to be the propellor.
		propellorBlur.parent = propellorModel;
	}
	
	
	void Update ()
    {
        // Rotate the propellor model at a rate proportional to the throttle.
		propellorModel.Rotate(0, maxRpm * plane.Throttle * Time.deltaTime * RpmToDps, 0 );
		
        // Create an integer for the new state of the blur textures.
		var newBlurState = 0;

        // choose between the blurred textures, if the throttle is high enough
        if (plane.Throttle > throttleBlurStart)
        {
            var throttleBlurProportion = Mathf.InverseLerp(throttleBlurStart, throttleBlurEnd, plane.Throttle);
            newBlurState = Mathf.FloorToInt(throttleBlurProportion * (propellorBlurTextures.Length - 1));
        }

        // If the blur state has changed
	    if (newBlurState != propellorBlurState)
		{
		    propellorBlurState = newBlurState;
				
	        if (propellorBlurState == 0)
		    {
				// switch to using the 'real' propellor model
				propellorModel.renderer.enabled = true;
		        propellorBlur.renderer.enabled = false;
		    }
		    else
		    {
	            // Otherwise turn off the propellor model and turn on the blur.
		        propellorModel.renderer.enabled = false;
		        propellorBlur.renderer.enabled = true;

	            // set the appropriate texture from the blur array
		        propellorBlur.renderer.material.mainTexture = propellorBlurTextures[propellorBlurState];
		    }
		}
    }
}
