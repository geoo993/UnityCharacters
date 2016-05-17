using UnityEngine;
using System.Collections;

public class MeshDeformationInput : MonoBehaviour {

	public float force = 10f;
	[Range(0.0f,2.0f)]public float forceOffset = 0.1f;

	void Update () {
		if (Input.GetMouseButton(0)) {
			HandleInput();
		}
	}

	void HandleInput () 
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(inputRay, out hit)) {
			MeshDeformation deformer = hit.collider.GetComponent<MeshDeformation>();

			//Debug.Log("object: "+ hit.collider.gameObject);

			if (deformer) 
			{
				Vector3 point = hit.point;
				point += hit.normal * forceOffset;
				deformer.AddDeformingForce(point, force);
			}

		}




	}




}
