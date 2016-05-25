using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform player = null;
	public Transform target = null;

	[Range(-50.0f, 50.0f)]public float distanceUP, distanceBack, minimumHeight =  1.0f;
	public Vector3 speed =  new Vector3 (3.0f, 5.0f, 2.0f);


	private Vector3 cameraOriginalPosition = Vector3.zero;
	private Vector3 nextPosition = Vector3.zero;

	private float distance = 40.0f; //10.0f;
	private float height = 5.0f; //5.0f;
	
	private float heightDamping = 4.0f;  //2.0f;
	private float rotationDamping = 3.0f;

	private Vector3 offset;

	void Awake ()
	{
		cameraOriginalPosition = this.transform.position;

		transform.position = lookTargetFromBehind();
		offset = transform.position - target.position ;

	}

	void FixedUpdate ()
	{
		//FollowTargetWhenRolling ();

		FollowPlaneTarget();  
		LookAtPlane(); 

	}

	private Vector3 lookTargetFromBehind()
	{
		////calculate a new position to place the camera:
		Vector3 newPosition =  target.position + (target.forward * distanceBack);
		float newPosY = Mathf.Max (newPosition.y + distanceUP, minimumHeight);
		newPosition = new Vector3(newPosition.x, newPosY, newPosition.z);

		return newPosition;

	}

	void FollowTargetWhenRolling ()
	{

		transform.position = target.position + offset ;
		transform.LookAt (transform.position);

		float movement = Input.GetAxis ("Horizontal2") * 20f * Time.deltaTime;
		if(!Mathf.Approximately (movement, 0f)) {
			transform.RotateAround (target.position, Vector3.up, movement);
			offset = transform.position - target.position ;
		}



	}

	void FollowPlaneTarget ()
	{
		//plane
		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;
		
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;
		
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		Quaternion currentRotation = Quaternion.Euler (0.0f, currentRotationAngle, 0.0f);
		
		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;
		
		transform.position = new Vector3(target.position.x, currentHeight, target.position.z);


		//		nextPosition.x = Mathf.Lerp (transform.position.x, target.position.x, speed.x * Time.deltaTime);
		//		nextPosition.y = Mathf.Lerp (transform.position.y, target.position.y, speed.y * Time.deltaTime);
		//		nextPosition.z = Mathf.Lerp (transform.position.z, target.position.z, speed.z * Time.deltaTime);
//		
//		transform.position = nextPosition;
	}



	void LookAtPlane ()
	{
		this.transform.LookAt (player.position);

	}


	void CameraFirstPosition ()
	{
		this.transform.position = cameraOriginalPosition;
	}




}
