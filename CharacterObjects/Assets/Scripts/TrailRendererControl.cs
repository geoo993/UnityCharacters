using UnityEngine;
using System.Collections;

public class TrailRendererControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		Color[] trailColors = new Color[5] {
			Color.red,
			Color.cyan,
			Color.yellow,
			ExtensionMethods.RandomColor (),
			ExtensionMethods.RandomColor ()
		};

		TrailRenderer trail1 = gameObject.AddComponent (typeof(TrailRenderer)) as TrailRenderer;

		Material mat = new Material (Shader.Find ("Particles/Additive"));

		trail1.receiveShadows = true;

		trail1.motionVectors = true;

		trail1.time = 5.0f;

		trail1.startWidth = 0.4f;

		trail1.endWidth = 0.4f;
	

		trail1.material = mat;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
