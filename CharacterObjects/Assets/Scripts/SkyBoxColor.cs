using UnityEngine;
using System.Collections;

public class SkyBoxColor : MonoBehaviour {
	
	public Color colorStart = Color.blue;
	public Color colorEnd = Color.green;
	public float duration = 1.0F;


	// Cubemap resolution - should be power of 2: 64, 128, 256 etc.
	public int resolution = 256;


	void Update() {
		float lerp = Mathf.PingPong(Time.time, duration) / duration;
		RenderSettings.skybox.SetColor("_Tint", Color.Lerp(colorStart, colorEnd, lerp));

//		if(Input.GetKeyDown(KeyCode.R)){
//			StartCoroutine(CreateCubeMap());
//		}
	}


	void Start () {

		// CreateCubeMap must be called in a co-routine, because we need it to wait for the end
		// of each frame render before grabbing the screen
		//StartCoroutine(CreateCubeMap());

		CreateCubeMap ();
	}

	// This is the coroutine that creates the cubemap images
	void CreateCubeMap()
	{
		// Initialise a new cubemap
		Cubemap cm = new Cubemap(resolution, TextureFormat.RGB24, true);

		// Disable any renderers attached to this object which may get in the way of our camera
		if(GetComponent<Renderer>()) {
			GetComponent<Renderer>().enabled = false;
		}

		// Face render order: Top, Right, Front, Bottom, Left, Back
		Quaternion[] rotations = { Quaternion.Euler(-90,0,0), Quaternion.Euler(0,90,0), Quaternion.Euler(0,0,0), Quaternion.Euler(90,0,0), Quaternion.Euler(0,-90,0), Quaternion.Euler(0,180,0)};
		CubemapFace[] faces = { CubemapFace.PositiveY, CubemapFace.PositiveX, CubemapFace.PositiveZ, CubemapFace.NegativeY, CubemapFace.NegativeX, CubemapFace.NegativeZ };

		// Create a single face matching the settings of the cubemap itself
		Texture2D face = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);

//		// Use this to prevent white borders around edge of texture
//		face.wrapMode = TextureWrapMode.Clamp;
//
//		// Create a camera that will be used to render the faces
//		//GameObject go = new GameObject("CubemapCamera", typeof(Camera));
//		Camera cam = new GameObject("CubemapCamera",typeof(Camera)).GetComponent<Camera>();
//
//		// Place the camera on this object
//		cam.transform.position = transform.position;
//
//		// Initialise the rotation - this will be changed for each texture grab
//		cam.transform.rotation = Quaternion.identity;
//
//		// We need each face of the cube to cover exactly 90 degree FoV
//		cam.fieldOfView = 90;
//
//		// Ensure this camera renders above all others
//		cam.depth = float.MaxValue;
//
//		// Loop through and create each face
//		for(int i = 0; i < 6; i++) {
//			// Set the camera direction
//			cam.transform.rotation = rotations[i];
//			// Important - wait the frame to be fully rendered before capturing
//			// See http://answers.unity3d.com/questions/326384/texture2dreadpixels-unknown-error-not-inside-drawi.html
//			yield return new WaitForEndOfFrame();
//			// Capture the pixels to the texture
//			face = CaptureScreen();
//			// Resize to the chosen resolution - cubemap faces must be square!
//			face = Resize(face, resolution, resolution);
//			// Flip the image across the x axis
//			face = Flip(face);
//			// Retrieve the pixelarray of colours for the current face
//			Color[] faceColours = face.GetPixels();
//			// Set the current cubemap face
//			cm.SetPixels(faceColours, faces[i], 0);
//			// Save the texture
//			SaveTextureToFile(face, gameObject.name + "_" + faces[i].ToString() + ".png");
//		}
//
//		// Apply the SetPixel changes to the cubemap faces to make them take effect     
//		cm.Apply();
//
//		// Assign the cubemap to the _Cube texture of this object's material
//		if(GetComponent<Renderer>().material.HasProperty("_Cube")) {
//			GetComponent<Renderer>().material.SetTexture("_Cube", cm);
//		}
//
//		// Cleanup
//		DestroyImmediate(face);
//		DestroyImmediate(cam);
//
//		// Re-enable the renderer
//		if(renderer) {
//			GetComponent<Renderer>().enabled = true;
//		}
	}




}