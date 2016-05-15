using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GradientTexture : MonoBehaviour {


	[Range(2, 512)] public int resolution = 256;

	MeshRenderer renderer;
	private Texture2D texture;
	private Color color = Color.blue;

	[Range(2, 36)] public float frequency = 16f;
	[Range(1, 8)] public int octaves = 1;
	[Range(1f, 4f)] public float lacunarity = 2f;
	[Range(0f, 1f)] public float persistence = 0.5f;
	[Range(1, 3)] public int dimensions = 3;
	public Gradient gradientColor = new Gradient();

	List<Vector2[]> pixelsPoints = new List<Vector2[]>();
	List <Color> interpolateColorsA = new List<Color>();
	List <Color> interpolateColorsB = new List<Color>();
	List <Color> interpolateComplete = new List<Color>();

	Color[] colors = new Color[]
	{
		Color.red, 
		Color.yellow, 
		Color.green, 
		//Color.blue,
		Color.cyan,
		//Color.gray,
		//Color.magenta,
		//Color.black,
		Color.white
	};
	//private void Awake () {
	private void OnEnable () {

		renderer = GetComponent<MeshRenderer> ();

		if (texture == null) {


			// new Texture2D (width , heigth, TextureFormat: format, bool : mipmap)
			texture = new Texture2D (resolution, resolution, TextureFormat.RGB24, false);
			//texture = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);
			texture.name = "Procedural Texture";

			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Trilinear;//FilterMode.Bilinear; //FilterMode.Point;
			texture.anisoLevel = 9;

			renderer.material.mainTexture = texture;
		}
	
			//FillTexture ();
		
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


		if (texture.width != resolution) {
			texture.Resize(resolution, resolution);
		}

			renderer.material.mainTexture = null;

			texture = new Texture2D (resolution, resolution, TextureFormat.RGB24, false);
			texture.name = "gradientTexture";

			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Trilinear;//FilterMode.Bilinear; //FilterMode.Point;
			texture.anisoLevel = 9;

		
			//the local coordinates of a quad with center (0,0)
			Vector3 point00 = transform.TransformPoint (new Vector3 (-0.5f, -0.5f, 0f));
			Vector3 point10 = transform.TransformPoint (new Vector3 (0.5f, -0.5f, 0f));
			Vector3 point01 = transform.TransformPoint (new Vector3 (-0.5f, 0.5f, 0f));
			Vector3 point11 = transform.TransformPoint (new Vector3 (0.5f, 0.5f, 0f));

			float stepSize = 1f / resolution;
			//Random.seed = 42;

			for (int y = 0; y < resolution; y++) {

				////interpolate between the bottom left and top left corner based on y
				Vector3 point0 = Vector3.Lerp (point00, point01, (y + 0.5f) * stepSize);

				////interpolate between the bottom right and top right corner based on x
				Vector3 point1 = Vector3.Lerp (point10, point11, (y + 0.5f) * stepSize);	


				for (int x = 0; x < resolution; x++) {

					////use bilinear interpolation to find the final point, which we directly convert into a color.
					Vector3 point = Vector3.Lerp (point0, point1, (x + 0.5f) * stepSize);

					//color = ((x & y) != 0 ? Color.white : Color.gray);  //// pattern 0
					//color = new Color(x * stepSize, y * stepSize, 0f);   //// pattern 1
					//color = new Color((x + 0.5f) * stepSize, (y + 0.5f) * stepSize, 0f);   //// pattern 2
					//color = new Color((x + 0.5f) * stepSize % 0.1f, (y + 0.5f) * stepSize % 0.1f, 0f) * 10f;  //// pattern 3 //// Repeating the pattern.
					Vector3 pattern2 = new Vector3 ((x + 0.5f) * stepSize, (y + 0.5f) * stepSize, 0f);

					//color = new Color(point.x, point.y, point.z);

					color = Color.white * Random.value; ////  noise
					//color = Color.white * method(point,frequency,true); //// pseudorandom noise  and Perlin Noise


					//color = addGradient (gradientColor);


					////each pixel psotion and color given
					texture.SetPixel (x, y, color);


				}
			}
			

			//// Apply all SetPixel calls
			texture.Apply();

			renderer.material.mainTexture = texture;
		

	}
		
}

