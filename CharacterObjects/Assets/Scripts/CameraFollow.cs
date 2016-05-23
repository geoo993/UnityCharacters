using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform plane = null;
	public Transform planeTarget = null;


	public Vector3 speed =  new Vector3 (3.0f, 5.0f, 2.0f);

	private Vector3 cameraOriginalPosition = Vector3.zero;
	private Vector3 nextPosition = Vector3.zero;

	private float distance = 40.0f; //10.0f;
	private float height = 5.0f; //5.0f;
	
	private float heightDamping = 4.0f;  //2.0f;
	private float rotationDamping = 3.0f;

	void Awake ()
	{
		cameraOriginalPosition = this.transform.position;

	}

	void Update ()
	{
		

	}
	void LateUpdate () {

		FollowPlaneTarget();  
		LookAtPlane(); 


	}
	

	void FollowPlaneTarget ()
	{
		//plane
		float wantedRotationAngle = planeTarget.eulerAngles.y;
		float wantedHeight = planeTarget.position.y + height;
		
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;
		
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		Quaternion currentRotation = Quaternion.Euler (0.0f, currentRotationAngle, 0.0f);
		
		transform.position = planeTarget.position;
		transform.position -= currentRotation * Vector3.forward * distance;
		
		transform.position = new Vector3(planeTarget.position.x, currentHeight, planeTarget.position.z);


//		nextPosition.x = Mathf.Lerp (transform.position.x, planeTarget.position.x, speed.x * Time.deltaTime);
//		nextPosition.y = Mathf.Lerp (transform.position.y, planeTarget.position.y, speed.y * Time.deltaTime);
//		nextPosition.z = Mathf.Lerp (transform.position.z, planeTarget.position.z, speed.z * Time.deltaTime);
//		
//		transform.position = nextPosition;
	}



	void LookAtPlane ()
	{
		this.transform.LookAt (plane.position);

	}


	void CameraFirstPosition ()
	{
		this.transform.position = cameraOriginalPosition;
	}




}
