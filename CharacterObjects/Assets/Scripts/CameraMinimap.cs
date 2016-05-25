using UnityEngine;
using System.Collections;

public class CameraMinimap : MonoBehaviour {

	public GameObject target = null;
	
	// Update is called once per frame
	void Update () {

		this.transform.position = new Vector3 (target.transform.position.x, this.transform.position.y, target.transform.position.z);

	}
}
