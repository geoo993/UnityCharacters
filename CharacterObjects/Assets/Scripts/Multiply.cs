using UnityEngine;
using System.Collections;


public class Multiply : MonoBehaviour
{
	public Camera source, destination;


	private RenderTexture renderTexture;
	private Texture2D sourceRender, destinationRender;


	void Start ()
	{
		renderTexture = new RenderTexture (Screen.width, Screen.height, 24);
		sourceRender = new Texture2D (Screen.width, Screen.height);
		destinationRender = new Texture2D (Screen.width, Screen.height);
	}


	void Update ()
	{
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;

		RenderTexture target = source.targetTexture;
		source.targetTexture = renderTexture;
		source.Render ();
		sourceRender.ReadPixels (new Rect (0.0f, 0.0f, renderTexture.width, renderTexture.height), 0, 0);
		source.targetTexture = target;

		Color background = destination.backgroundColor;
		destination.backgroundColor = Color.red;
		target = destination.targetTexture;
		destination.targetTexture = renderTexture;
		destination.Render ();
		destinationRender.ReadPixels (new Rect (0.0f, 0.0f, renderTexture.width, renderTexture.height), 0, 0);
		destination.targetTexture = target;
		destination.backgroundColor = background;

		RenderTexture.active = active;

		Color[] sourcePixels = sourceRender.GetPixels (), destinationPixels = destinationRender.GetPixels ();

		for (int i = 0; i < sourcePixels.Length; i++)
		{
			destinationPixels[i] = new Color (
				(sourcePixels[i].r * destinationPixels[i].r) / 1.0f,
				(sourcePixels[i].g * destinationPixels[i].g) / 1.0f,
				(sourcePixels[i].b * destinationPixels[i].b) / 1.0f,
				(sourcePixels[i].a * destinationPixels[i].a) / 1.0f
			);
		}

		destinationRender.SetPixels (destinationPixels);
		destinationRender.Apply ();
	}


	void OnGUI ()
	{
		GUI.DrawTexture (new Rect (0.0f, 0.0f, 300.0f, 300.0f), destinationRender);
	}
}