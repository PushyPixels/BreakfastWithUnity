using UnityEngine;


[RequireComponent(typeof(GUITexture))]
public class ForcedReset : MonoBehaviour {

    void Update () {
        
        // if we have forced a reset ...
        if (CrossPlatformInput.GetButtonDown ("ResetObject")) {
            
            //... reload the scene
            Application.LoadLevelAsync (Application.loadedLevelName);
        }
    }

}
