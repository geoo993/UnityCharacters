using UnityEngine;
using System.Collections;

public class CameraFollowSimple : MonoBehaviour {


	public GameObject target = null; 

	public float damping = 1f;
	private Vector3 offset;

	void Start() 
	{
		offset = transform.position - target.transform.position;
	}


	void LateUpdate() {

		FollowTarget ();
		LookAtTarget ();
	}

	void LookAtTarget(){

		transform.LookAt(target.transform.position);
	}

	void FollowTarget()
	{
		Vector3 desiredPosition = target.transform.position + offset;
		Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
		transform.position = position;

	}


}
