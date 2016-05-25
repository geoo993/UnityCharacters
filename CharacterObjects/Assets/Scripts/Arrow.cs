﻿using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public GameObject[] arrowParts = null;

	[Range(10, 100)] public int range = 40;
	private GameObject movingObject = null;
	private GameObject targetObject = null;


	private Color col = Color.red;
	public Color closestToTarget = Color.green;
	public Color farFromTarget = Color.red;

	void Awake () {

		this.name = "pointer";
		movingObject = GameObject.Find ("MovingObject");
		targetObject = GameObject.Find ("Target");

		AddMaterial ();
	}

	void Update () {
		
		float dist = Vector3.Distance (movingObject.transform.position, targetObject.transform.position);

		col = Color.Lerp (closestToTarget, farFromTarget, dist / range);
		this.transform.LookAt (movingObject.transform);

		foreach (GameObject arrow in arrowParts) 
		{
			arrow.GetComponent<MeshRenderer> ().material.SetColor ("_Color", col);
		}

	}

	void AddMaterial(){

		foreach (GameObject arrow in arrowParts) 
		{
			Material mat = new Material (Shader.Find (".ShaderExample/Shield"));
			mat.SetColor ("_Color", col);
			arrow.GetComponent<MeshRenderer> ().material = mat;
		}
	}
}
