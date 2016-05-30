using UnityEngine;
using System.Collections;

public class CameraMouseAim : MonoBehaviour {


	public GameObject target = null; 
	public float rotateSpeed = 5;
	private Vector3 offset;

	void Start() 
	{
		offset = target.transform.position - transform.position;
	}

	void LateUpdate() {

		FollowTarget ();
		LookAtTarget ();
	}
		
	void FollowTarget()
	{
		float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
		target.transform.Rotate(0, horizontal, 0);

		float desiredAngle = target.transform.eulerAngles.y;
		Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
		transform.position = target.transform.position - (rotation * offset);


	}

	void LookAtTarget(){

		transform.LookAt(target.transform);
	}



}
