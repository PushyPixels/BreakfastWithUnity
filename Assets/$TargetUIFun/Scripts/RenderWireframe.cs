using UnityEngine;
using System.Collections;

public class RenderWireframe : MonoBehaviour {

	void OnPreRender() {
		GL.wireframe = true;
	}
	void OnPostRender() {
		GL.wireframe = false;
	}
}
