using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {


	private void Awake () {
		//StartCoroutine(Explode());
	}

	void Start () {
		StartCoroutine(Explode());
	}

	private IEnumerator Explode () 
	{
		
		WaitForSeconds wait = new WaitForSeconds (4.0f);

		yield return wait;

		Destroy(gameObject);
	}

	


}
