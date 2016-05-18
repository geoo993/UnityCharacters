using UnityEngine;
using System.Collections;

public class SensorGround : MonoBehaviour {

	public int triggered = 0;
	
	void OnTriggerEnter  (Collider other) {

		triggered = 1;

	}
	
	void OnTriggerExit  (Collider other) {

		triggered = 0;

	}

}
