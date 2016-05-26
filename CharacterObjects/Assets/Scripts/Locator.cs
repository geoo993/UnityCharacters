using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Locator : MonoBehaviour {

	public GameObject target = null;
	public GameObject player = null;

	private Color col = Color.red;
	public Color closestToTarget = Color.green;
	public Color farFromTarget = Color.red;

	private int range = 200;

	// Update is called once per frame
	void Update () {

		float dist = Vector3.Distance (player.transform.position, target.transform.position);
		col = Color.Lerp (closestToTarget, farFromTarget, dist / range);

		GetComponent<Image> ().color =  col;

		//Vector3 PlayerRot = target.transform.eulerAngles; 

		//GetComponent<RectTransform> ().Rotate (0.0f, 0.0f, 5.0f); 

		//Vector3 lookPos = target.transform.position - player.transform.position;
		//lookPos.x = 0.0f;
		//lookPos.y = 0.0f;

		//float angle = Mathf.Atan2 (lookPos.x, lookPos.z) * Mathf.Rad2Deg ;
		//print (angle);
		//GetComponent<RectTransform> ().eulerAngles = new Vector3(0, 0, angle);
		//GetComponent<RectTransform> ().rotation = Quaternion.AngleAxis(angle, Vector3.forward);


		//Quaternion rotation = Quaternion.LookRotation(lookPos);
		//GetComponent<RectTransform> ().eulerAngles = lookPos; //Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2.0f); 

//		Vector3 targetPoint = target.transform.position;
//		Quaternion targetRotation = Quaternion.LookRotation(targetPoint - player.transform.position, Vector3.up);
//		GetComponent<RectTransform> ().rotation = Quaternion.Slerp(GetComponent<RectTransform> ().rotation, targetRotation, Time.deltaTime * 2.0f); 
//
		//GetComponent<RectTransform> ().rotation = Quaternion.Slerp(GetComponent<RectTransform> ().rotation , Quaternion.Euler(0.0f , 0.0f, -angle ), Time.deltaTime * 2f);


//		float angle = Mathf.Atan2(lookPos.x, lookPos.z) * Mathf.Rad2Deg;
		//Quaternion q = Quaternion.AngleAxis(rotateToward (player, target) + player.transform.eulerAngles.y, Vector3.forward);
		//GetComponent<RectTransform> ().rotation  = Quaternion.AngleAxis(angle, Vector3.forward);
		//GetComponent<RectTransform> ().rotation = Quaternion.Slerp(GetComponent<RectTransform> ().rotation, q, Time.deltaTime * 1f);



	}



}
