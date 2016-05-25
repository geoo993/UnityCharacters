using UnityEngine;
using System.Collections;

public class Skybox : MonoBehaviour {

	private Camera camera;

	public Color topColor = Color.blue;
	public Color midColor = Color.blue;
	public Color bottomColor = Color.blue;

	public bool lerpTopColor = false;
	public bool lerpMidColor = false;
	public bool lerpBottomColor = false;

	[HideInInspector] public Color tc;
	[HideInInspector] public Color bc; 
	[HideInInspector] public Color mc;

	public Texture2D[] textures;

	private int resolution = 256;
	private float duration = 10.0f;
	private Color CurrentColor =  Color.red;
	private Color previousColor = Color.blue;
	private float t = 0;



	public static Material CreateSkyboxMaterial(SkyboxManifest manifest)
	{
		Material result = new Material(Shader.Find("RenderFX/Skybox"));
		result.SetTexture("_FrontTex", manifest.textures[0]);
		result.SetTexture("_BackTex", manifest.textures[1]);
		result.SetTexture("_LeftTex", manifest.textures[2]);
		result.SetTexture("_RightTex", manifest.textures[3]);
		result.SetTexture("_UpTex", manifest.textures[4]);
		result.SetTexture("_DownTex", manifest.textures[5]);
		return result;
	}
	public static Material CreateGradientMaterial(Color topColor, Color middleColor, Color bottomColor)
	{
		Material result = new Material (Shader.Find (".ShaderExample/GradientThreeColor"));

		result.SetFloat ("_Middle", 0.4f);
		result.SetColor("_ColorTop", topColor);
		result.SetColor("_ColorMid",middleColor);
		result.SetColor("_ColorBot", bottomColor);
		return result;
	}

	private Gradient addGradient ()
	{
		Gradient g = new Gradient ();

		GradientColorKey blue = new GradientColorKey(Color.blue, 0.0f);
		GradientColorKey white = new GradientColorKey(Color.white, 0.5f);
		GradientColorKey yellow = new GradientColorKey(Color.yellow, 1f);
		GradientAlphaKey blueAlpha = new GradientAlphaKey(1,0);
		GradientAlphaKey yellowAlpha = new GradientAlphaKey(1,1);


		GradientColorKey[] colorKeys = new GradientColorKey[]{blue, white, yellow};
		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]{blueAlpha,yellowAlpha};

		g.SetKeys(colorKeys, alphaKeys);

		return g;
	}

	private Texture2D CreateGradientTexture(){


		Texture2D texture = new Texture2D (resolution, resolution, TextureFormat.RGB24, false);
		texture.name = "gradientTexture";

		texture.wrapMode = TextureWrapMode.Clamp;
		Gradient gradientColor = addGradient ();

		texture = gradientColor.ToTexture (resolution);
		texture.Apply ();

		return texture;
	}


	//void OnEnable()
	//void OnEnableUpdate()
	void Update()
	{
		
		////type 1  set with external texture
//		SkyboxManifest manifest = new SkyboxManifest(textures[0], textures[1], textures[2], textures[3], textures[4], textures[5]);


		/////type 2  set with procedurally generated texture
//		Texture2D text = CreateGradientTexture();
//		SkyboxManifest manifest = new SkyboxManifest(text, text, text, text, text, text);
//		Material material = CreateSkyboxMaterial(manifest);
//


		////type 3 set with external material
		if (t < 1.0f) {
			t += Time.deltaTime * (1.0f / duration);
		} else {
			t = 0;
			duration = Random.Range (20f, 50f);

			CurrentColor = previousColor;
			previousColor = ExtensionMethods.RandomColor ();
		}
		Color lerp = Color.Lerp (CurrentColor,previousColor, t) / 2.0f;

		//print("time: "+t+" duration:  "+ duration);

		tc = lerpTopColor ? lerp : topColor;
		mc = lerpMidColor ? lerp : midColor;
		bc = lerpBottomColor ? lerp : bottomColor;

		Material material = CreateGradientMaterial(tc,mc,bc);
		SetSkybox(material);
		//enabled = false;

	}

	void SetSkybox(Material material)
	{
		GameObject camera = Camera.main.gameObject;
		Skybox skybox = camera.GetComponent<Skybox>();
		if (skybox == null)
			skybox = camera.AddComponent<Skybox>();
		RenderSettings.skybox = material;
	}
}

public struct SkyboxManifest
{
	public Texture2D[] textures;

	public SkyboxManifest(Texture2D front, Texture2D back, Texture2D left, Texture2D right, Texture2D up, Texture2D down)
	{
		textures = new Texture2D[6]
		{
			front,
			back,
			left,
			right,
			up,
			down
		};
	}
}