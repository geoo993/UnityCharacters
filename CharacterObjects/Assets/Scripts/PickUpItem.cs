using UnityEngine;
using System.Collections;

public class PickUpItem : MonoBehaviour {


	void OnTriggerEnter  ( Collider other ) {

		HealthBar.health += 20;
		Destroy (this.gameObject);
	}

	void OnTriggerExit  ( Collider other ) {

	}


}
