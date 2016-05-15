using UnityEngine;
using System.Collections;
//using System.IO; // Used for writing PNG textures to disk

public class CubemapProbe : MonoBehaviour {
	
//	// Cubemap resolution - should be power of 2: 64, 128, 256 etc.
//	public int resolution = 256;
//	
//	#region Cubemap functions
//	
//	// Return screencap as a Texture2D
//	// http://wiki.unity3d.com/index.php?title=RenderTexture_Free
//	private Texture2D CaptureScreen() {
//		Texture2D result;
//		Rect captureZone = new Rect( 0f, 0f, Screen.width, Screen.height );
//		result = new Texture2D( Mathf.RoundToInt(captureZone.width), Mathf.RoundToInt(captureZone.height), TextureFormat.RGB24, false);
//		result.ReadPixels(captureZone, 0, 0, false);
//		result.Apply();
//		return result;
//	}
//	
//	// Save a Texture2D as a PNG file
//	// http://answers.unity3d.com/questions/245600/saving-a-png-image-to-hdd-in-standalone-build.html
//	private void SaveTextureToFile(Texture2D texture, string fileName) {
//		byte[] bytes = texture.EncodeToPNG();
//		FileStream file = File.Open(Application.dataPath + "/" + fileName,FileMode.Create);
//		BinaryWriter binary = new BinaryWriter(file);
//		binary.Write(bytes);
//		file.Close();
//	}
//	
//	// Resize a Texture2D
//	// http://docs-jp.unity3d.com/Documentation/ScriptReference/Texture2D.GetPixelBilinear.html
//	Texture2D Resize(Texture2D sourceTex, int Width, int Height) {
//		Texture2D destTex = new Texture2D(Width, Height, sourceTex.format, true);
//		Color[] destPix = new Color[Width * Height];
//		int y = 0;
//		while (y < Height) {
//			int x = 0;
//			while (x < Width) {
//				float xFrac = x * 1.0F / (Width );
//				float yFrac = y * 1.0F / (Height);
//				destPix[y * Width + x] = sourceTex.GetPixelBilinear(xFrac, yFrac);
//				x++;
//			}
//			y++;
//		}
//		destTex.SetPixels(destPix);
//		destTex.Apply();
//		return destTex;
//	}
//	
//	// Flip/Mirror the pixels in a Texture2D about the x axis
//	Texture2D Flip(Texture2D sourceTex) {
//		// Create a new Texture2D the same dimensions and format as the input
//		Texture2D Output = new Texture2D(sourceTex.width, sourceTex.height, sourceTex.format, true);
//		// Loop through pixels
//		for (int y = 0; y < sourceTex.height; y++)
//		{
//			for (int x = 0; x < sourceTex.width; x++)
//			{
//				// Retrieve pixels in source from left-to-right, bottom-to-top
//				Color pix = sourceTex.GetPixel(sourceTex.width + x, (sourceTex.height-1) - y);
//				// Write to output from left-to-right, top-to-bottom
//				Output.SetPixel(x, y, pix);
//			}
//		}
//		return Output;
//	}
//	#endregion
//	
//	// Use this for initialization
//	void Start () {
//		
//		// CreateCubeMap must be called in a co-routine, because we need it to wait for the end
//		// of each frame render before grabbing the screen
//		StartCoroutine(CreateCubeMap());
//	}
//	
//	// This is the coroutine that creates the cubemap images
//	IEnumerator CreateCubeMap()
//	{
//		// Initialise a new cubemap
//		Cubemap cm = new Cubemap(resolution, TextureFormat.RGB24, true);
//		
//		// Disable any renderers attached to this object which may get in the way of our camera
//		if(GetComponent<Renderer>()) {
//			GetComponent<Renderer>().enabled = false;
//		}
//		
//		// Face render order: Top, Right, Front, Bottom, Left, Back
//		Quaternion[] rotations = { Quaternion.Euler(-90,0,0), Quaternion.Euler(0,90,0), Quaternion.Euler(0,0,0), Quaternion.Euler(90,0,0), Quaternion.Euler(0,-90,0), Quaternion.Euler(0,180,0)};
//		CubemapFace[] faces = { CubemapFace.PositiveY, CubemapFace.PositiveX, CubemapFace.PositiveZ, CubemapFace.NegativeY, CubemapFace.NegativeX, CubemapFace.NegativeZ };
//		
//		// Create a single face matching the settings of the cubemap itself
//		Texture2D face = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
//		
//		// Use this to prevent white borders around edge of texture
//		face.wrapMode = TextureWrapMode.Clamp;
//		
//		// Create a camera that will be used to render the faces
//		GameObject go = new GameObject("CubemapCamera", typeof(Camera));
//		
//		// Place the camera on this object
//		go.transform.position = transform.position;
//		
//		// Initialise the rotation - this will be changed for each texture grab
//		go.transform.rotation = Quaternion.identity;
//		
//		// We need each face of the cube to cover exactly 90 degree FoV
//		go.GetComponent<Camera>().fieldOfView = 90;
//		
//		// Ensure this camera renders above all others
//		go.GetComponent<Camera>().depth = float.MaxValue;
//		
//		// Loop through and create each face
//		for(int i = 0; i < 6; i++) {
//			// Set the camera direction
//			go.transform.rotation = rotations[i];
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
//		DestroyImmediate(go);
//		
//		// Re-enable the renderer
//		if(GetComponent<Renderer>()) {
//			GetComponent<Renderer>().enabled = true;
//		}
//	}
//	
//	
//	// Update is called once per frame
//	void Update () {
//		
//		// Recalculate the cubemap if "R" is pressed)
//		if(Input.GetKeyDown(KeyCode.R)){
//			StartCoroutine(CreateCubeMap());
//		}
//	}  
}