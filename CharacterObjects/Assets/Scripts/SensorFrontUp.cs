using UnityEngine;
using System.Collections;

public class SensorFrontUp : MonoBehaviour {

	public int sensorFrontUp = 0;
	

	void OnTriggerEnter  ( Collider other ) {

		sensorFrontUp = 1;
	}
	
	void OnTriggerExit  ( Collider other ) {

		sensorFrontUp = 0;
	}
}
