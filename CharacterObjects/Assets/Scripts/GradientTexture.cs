using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO; // Used for writing PNG textures to disk

public static class GradientToTexture{
	
	public static Texture2D ToTexture(this Gradient aGradient,int width)
	{
		if (width < 1)
			throw new System.ArgumentException("aWidth has to be 1 or greater");
		var colors = new Color[width];
		float denominator = Mathf.Max(1, width - 1);
		for(int i = 0; i < width; i++)
		{
			colors[i] = aGradient.Evaluate((float)i / denominator);
		}
		var tex = new Texture2D(width, 1);
		tex.SetPixels(colors);
		tex.Apply();
		return tex;
	}

}


public class GradientTexture : MonoBehaviour {


	[Range(2, 512)] public int resolution = 256;

	MeshRenderer renderer;
	private Texture2D texture;
	private Color color = Color.blue;

	public Gradient gradientColor = new Gradient();

	private Cubemap cubemap;

	//private void Awake () {
	private void OnEnable () {

		renderer = GetComponent<MeshRenderer> ();

	
			//FillTexture ();
		//CreateCubeMap();
	}


	private void Update () {

		if (transform.hasChanged) {
			transform.hasChanged = false;
			FillTexture();
		}


	}

	private void addGradient (Gradient g)
	{

		GradientColorKey blue = new GradientColorKey(Color.blue, 0.0f);
		GradientColorKey white = new GradientColorKey(Color.white, 0.3f);
		GradientColorKey black = new GradientColorKey(Color.black, 0.45f);
		GradientColorKey yellow = new GradientColorKey(Color.yellow, 0.6f);
		GradientColorKey red = new GradientColorKey(Color.red, 1f);

		GradientAlphaKey blueAlpha = new GradientAlphaKey(1,0);
		GradientAlphaKey yellowAlpha = new GradientAlphaKey(1,1);


		GradientColorKey[] colorKeys = new GradientColorKey[]{blue, white, black, yellow, red};
		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]{blueAlpha,yellowAlpha};
		g.SetKeys(colorKeys, alphaKeys);


	}


	public void FillTexture () {


		if (texture == null) {

			renderer.material.mainTexture = null;

			cubemap = new Cubemap(resolution, TextureFormat.ARGB32, false);

			texture = new Texture2D (cubemap.width, cubemap.height, TextureFormat.RGB24, false);
			//texture = new Texture2D (resolution, resolution, TextureFormat.RGB24, false);
			texture.name = "gradientTexture";

			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Trilinear;//FilterMode.Bilinear; //FilterMode.Point;
			texture.anisoLevel = 9;

			addGradient (gradientColor);
			texture = gradientColor.ToTexture (resolution);

			//renderer.material.mainTexture = texture;


			CubemapFace[] faces = new CubemapFace[] {
				CubemapFace.PositiveX, CubemapFace.NegativeX,
				CubemapFace.PositiveY, CubemapFace.NegativeY,
				CubemapFace.PositiveZ, CubemapFace.NegativeZ };

			foreach (CubemapFace face in faces) {
				texture.SetPixels(cubemap.GetPixels(face));        
				cubemap.Apply ();
				File.WriteAllBytes(Application.dataPath + "/" + cubemap.name + "_" + face.ToString() + ".png", texture.EncodeToPNG());
			}   


			//Material mat = new Material (Shader.Find ("Skybox/Cubemap"));
			Material mat = new Material (Shader.Find (".ShaderExample/ScreenScapeGradient"));
			//cubemap = Resources.Load ("ClearSkyRadiance") as Cubemap;
			mat.SetTexture("_MainTex",texture);

			renderer.material = mat;
			cubemap.Apply ();

		
		}





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

		face.name = "gradientTexture";
		// Use this to prevent white borders around edge of texture
		face.wrapMode = TextureWrapMode.Clamp;

		face.filterMode = FilterMode.Trilinear;//FilterMode.Bilinear; //FilterMode.Point;
		face.anisoLevel = 9;

		addGradient (gradientColor);
		face = gradientColor.ToTexture (resolution);

		//renderer.material.mainTexture = texture;


		//		// Loop through and create each face
		for(int i = 0; i < 6; i++) {
			
		
			face = Resize(face, resolution, resolution);
			// Flip the image across the x axis
			face = Flip(face);
			// Retrieve the pixelarray of colours for the current face
			Color[] faceColours = face.GetPixels();
			// Set the current cubemap face
			cm.SetPixels(faceColours, faces[i], 0);
			// Save the texture
			SaveTextureToFile(face, gameObject.name + "_" + faces[i].ToString() + ".png");
		}
		// Apply the SetPixel changes to the cubemap faces to make them take effect     
		cm.Apply();

		//		// Assign the cubemap to the _Cube texture of this object's material
			if(GetComponent<Renderer>().material.HasProperty("_Cube")) {
				GetComponent<Renderer>().material.SetTexture("_Cube", cm);
			}
	
			// Cleanup
			DestroyImmediate(face);
			//DestroyImmediate(cam);
	
			// Re-enable the renderer
			if(renderer) {
				GetComponent<Renderer>().enabled = true;
			}
}


	private Texture2D CaptureScreen() {
		Texture2D result;
		Rect captureZone = new Rect( 0f, 0f, Screen.width, Screen.height );
		result = new Texture2D( Mathf.RoundToInt(captureZone.width), Mathf.RoundToInt(captureZone.height), TextureFormat.RGB24, false);
		result.ReadPixels(captureZone, 0, 0, false);
		result.Apply();
		return result;
	}

	// Save a Texture2D as a PNG file
	// http://answers.unity3d.com/questions/245600/saving-a-png-image-to-hdd-in-standalone-build.html
	private void SaveTextureToFile(Texture2D texture, string fileName) {
		byte[] bytes = texture.EncodeToPNG();
		FileStream file = File.Open(Application.dataPath + "/" + fileName,FileMode.Create);
		BinaryWriter binary = new BinaryWriter(file);
		binary.Write(bytes);
		file.Close();
	}

	// Resize a Texture2D
	// http://docs-jp.unity3d.com/Documentation/ScriptReference/Texture2D.GetPixelBilinear.html
	Texture2D Resize(Texture2D sourceTex, int Width, int Height) {
		Texture2D destTex = new Texture2D(Width, Height, sourceTex.format, true);
		Color[] destPix = new Color[Width * Height];
		int y = 0;
		while (y < Height) {
			int x = 0;
			while (x < Width) {
				float xFrac = x * 1.0F / (Width );
				float yFrac = y * 1.0F / (Height);
				destPix[y * Width + x] = sourceTex.GetPixelBilinear(xFrac, yFrac);
				x++;
			}
			y++;
		}
		destTex.SetPixels(destPix);
		destTex.Apply();
		return destTex;
	}

	// Flip/Mirror the pixels in a Texture2D about the x axis
	Texture2D Flip(Texture2D sourceTex) {
		// Create a new Texture2D the same dimensions and format as the input
		Texture2D Output = new Texture2D(sourceTex.width, sourceTex.height, sourceTex.format, true);
		// Loop through pixels
		for (int y = 0; y < sourceTex.height; y++)
		{
			for (int x = 0; x < sourceTex.width; x++)
			{
				// Retrieve pixels in source from left-to-right, bottom-to-top
				Color pix = sourceTex.GetPixel(sourceTex.width + x, (sourceTex.height-1) - y);
				// Write to output from left-to-right, top-to-bottom
				Output.SetPixel(x, y, pix);
			}
		}
		return Output;
	}



}

