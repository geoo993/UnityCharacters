using UnityEngine;
using System.Collections;

public class HoverController : MonoBehaviour {

	//values that controls the vehicle
	public float acceleration, rotationRate;

	//values for faking a nice turn display;
	public float turnRotationAngle, turnRotationSeekSpeed;

	//reference variable we don't directly use:
	private float rotationVelocity, groundAngleVelocity;

	private Rigidbody rigidB;

	public GameObject[] all = null;


	void Awake () 
	{
		rigidB = GetComponent<Rigidbody> ();
		rigidB.mass = 1000f;
		rigidB.drag = 1f;
		rigidB.angularDrag = 1f;

		for (int i = 0; i < 100; i++) {
			
			Vector3 pos = new Vector3(Random.Range (-200.0f, 200.0f), Random.Range (2.0f, 10.0f),Random.Range (-200.0f, 200.0f));
			GameObject a = (GameObject) Instantiate(all [Random.Range (0, all.Length - 1)], pos, Quaternion.identity);

			Vector3 scale = new Vector3 (Random.Range (1.0f, 10.0f), Random.Range (1.0f, 10.0f), Random.Range (1.0f, 10.0f));
			a.transform.localScale = scale;
		}
	}


	void FixedUpdate ()
	{


		//check if we are touching the ground:
		if (Physics.Raycast (transform.position, transform.up * -1, 3f)) {
			////we are on the ground; enable the accelartor and increase drag:
			rigidB.drag = 1;

			////calculate forward force:
			Vector3 forwardForce = transform.forward * acceleration * Input.GetAxis ("Vertical");

			////correct the force for deltatime and vehical mass:
			forwardForce = forwardForce * Time.deltaTime * rigidB.mass;

			rigidB.AddForce (forwardForce);

		} else {
			//we aren't on the ground and dont want to just halt in the mid-air; reduce drag:
			rigidB.drag = 0;
		}


		////you can turn in the air or on the ground:
		Vector3 turnTorque = Vector3.up * rotationRate * Input.GetAxis ("Horizontal");

		////correct the force for deltatime and vehicle mass:
		turnTorque =turnTorque * Time.deltaTime * rigidB.mass;
		rigidB.AddTorque (turnTorque);


		////"Fake" rotate the car when you are turning:
		Vector3 newRotation = transform.eulerAngles;
		float zRotation = Mathf.SmoothDampAngle (
			newRotation.z, Input.GetAxis ("Horizontal") * -turnRotationAngle,
			                  ref rotationVelocity, turnRotationSeekSpeed);
		newRotation = new Vector3 (newRotation.x, newRotation.y, zRotation);
		transform.eulerAngles = newRotation;







	}
}
