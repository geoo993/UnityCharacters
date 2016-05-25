using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour {

	[Range(0.0f, 20f)] public float speed = 2.0f;
	[Range(0.0f, 20f)] public float rotateSpeed = 2.0f;

	[Range(200.0f, 1000f)] public float ballSpeed = 50.0f;

	public Camera cam;
	private Rigidbody rigid;


	void Awake(){

		rigid = GetComponent<Rigidbody> ();


	}
	void Update () 
	{ 
		CubeMovement ();
	}  

	void CubeMovement(){

		float moveHorizontal = Input.GetAxis ("Horizontal"); 
		float moveVertical = Input.GetAxis ("Vertical");

		transform.Translate( moveVertical * speed, 0.0f, 0.0f);
		transform.Rotate(0.0f, moveHorizontal * rotateSpeed, 0.0f);



	}
	void BallMovement(){


		if (Mathf.Abs (Input.GetAxisRaw ("Vertical")) + Mathf.Abs (Input.GetAxisRaw ("Horizontal")) > 0.1f) 
		{
			
			Vector3 forward = Camera.main.transform.TransformDirection (Vector3.forward);
			forward.y = 0;
			forward = forward.normalized;
			Vector3 right = new Vector3 (forward.z, 0, -forward.x);
			float h = Input.GetAxis ("Horizontal");
			float v = Input.GetAxis ("Vertical");
			Vector3 moveDirection = (h * right + v * forward);
			moveDirection = Quaternion.Inverse (this.transform.rotation) * moveDirection;
			moveDirection = new Vector3 (moveDirection.x, 0, moveDirection.z); 
			moveDirection = this.transform.rotation * moveDirection;

			rigid.AddForce(moveDirection * ballSpeed * Time.deltaTime);
		}

	}



}

