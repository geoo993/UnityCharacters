using UnityEngine;
using System.Collections;

public class CameraStaticFollow : MonoBehaviour {


	public GameObject target = null; 
	Vector3 offset;

	void Start() 
	{
		offset =  target.transform.position - transform.position;
	}


	void LateUpdate() {

		FollowTarget ();
		LookAtTarget ();
	}
		
	void FollowTarget()
	{

		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = target.transform.eulerAngles.y;
		Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
		transform.position = target.transform.position - (rotation * offset);

	}

	void LookAtTarget(){

		transform.LookAt(target.transform);
	}



}
