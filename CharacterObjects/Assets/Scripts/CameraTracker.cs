using UnityEngine;
using System.Collections;

public class CameraTracker : MonoBehaviour {

	public Transform target;
	[Range(-50.0f, 50.0f)]public float distanceUP, distanceBack, minimumHeight =  1.0f;

	private Vector3 positionVelocity;
	private Vector3 offset;
	private CharacterMovement craftMovement;

	bool getPos = true;



	void Awake ()
	{
		craftMovement = target.GetComponent<CharacterMovement> ();


	}

	private Vector3 gotoPos()
	{

		////calculate a new position to place the camera:
		Vector3 newPosition =  target.position + (target.forward * distanceBack);
		float newPosY = Mathf.Max (newPosition.y + distanceUP, minimumHeight);
		newPosition = new Vector3(newPosition.x, newPosY, newPosition.z);

		return newPosition;

	}

	void LateUpdate () {

		if (craftMovement.ballState == true) {
			
			FollowTargetWhenRolling ();

		} else {
			FollowTarget ();
		}

	}

	void FollowTargetWhenRolling ()
	{
//		float heightDamping = 4.0f;  //2.0f;
//		float rotationDamping = 3.0f;
//		float distance = 40.0f; //10.0f;
//		float height = 5.0f; //5.0f;
//		float wantedRotationAngle = target.eulerAngles.y;
//		float wantedHeight = target.position.y + height;
//
//		float currentRotationAngle = transform.eulerAngles.y;
//		float currentHeight = transform.position.y;
//
//		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
//		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
//
//		Quaternion currentRotation = Quaternion.Euler (0.0f, currentRotationAngle, 0.0f);
//
//		transform.position = target.transform.position;
//		transform.position -= currentRotation * Vector3.forward * distance;
//
//		transform.position = new Vector3(target.position.x, currentHeight, target.position.z - 20.0f);
//
//
//		Vector3 targetPoint = target.position;
//		Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
//		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f); 


		if (getPos) {
			transform.position = gotoPos();
			offset = transform.position - target.position ;
			getPos = false;
		}

		transform.position = target.position + offset ;
		//transform.LookAt (transform.position);

		float movement = Input.GetAxis ("Horizontal2") * 20f * Time.deltaTime;
		if(!Mathf.Approximately (movement, 0f)) {
			transform.RotateAround (target.position, Vector3.up, movement);
			offset = transform.position - target.position ;
		}


	}

	void FollowTarget (){

		getPos = true;

		////move to camera:
		//transform.position = newPosition;
		transform.position = Vector3.SmoothDamp(transform.position, gotoPos(), ref positionVelocity, 0.18f);


		////rotate the camera to look at where the target is pointing
		Vector3 lookAt = target.position + (target.forward * 5);
		transform.LookAt (lookAt);
	}



}
