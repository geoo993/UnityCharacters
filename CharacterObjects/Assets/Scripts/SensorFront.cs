using UnityEngine;
using System.Collections;

public class SensorFront : MonoBehaviour {

	public int sensorFront = 0;
	
	void OnTriggerEnter  (Collider other ) {

		sensorFront = 1;
		//print ("front sensor entered");
	}
	
	void OnTriggerExit  ( Collider other ) {

		sensorFront = 0;
	}
}
