using UnityEngine;
using System.Collections;

public class CameraTracker : MonoBehaviour {

	public Transform target;
	[Range(-30.0f, 30.0f)]public float distanceUP, distanceBack, minimumHeight =  1.0f;

	private Vector3 positionVelocity;

	void FixedUpdate () {
	
		//FollowCarTarget ();
		//LookAtCar(); 
	}

	void LateUpdate () {

		FollowCarTarget ();
		LookAtCar(); 

	}

	void FollowCarTarget (){
		////calculate a new position to place the camera:
		Vector3 newPosition =  target.position + (target.forward * distanceBack);
		float newPosY = Mathf.Max (newPosition.y + distanceUP, minimumHeight);
		newPosition = new Vector3(newPosition.x, newPosY, newPosition.z);


		////move to camera:
		//transform.position = newPosition;
		transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref positionVelocity, 0.18f);
	}

	void LookAtCar(){

		////rotate the camera to l	ook at where the target is pointing
		Vector3 lookAt = target.position + (target.forward * 5);
		transform.LookAt (lookAt);
	}


}
