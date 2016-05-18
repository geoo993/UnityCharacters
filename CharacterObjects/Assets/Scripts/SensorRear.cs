using UnityEngine;
using System.Collections;

public class SensorRear : MonoBehaviour {

	public int sensorRear = 0;
	
	void OnTriggerEnter  ( Collider other ) {

		sensorRear = 1;
	}
	
	void OnTriggerExit  ( Collider other ) {

		sensorRear = 0 ;
	}
}
