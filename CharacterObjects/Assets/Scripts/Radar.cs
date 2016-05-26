using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadarObject {

	public Image icon { get; set; }
	public GameObject owner { get; set; }

}

public class Radar : MonoBehaviour {

	public Transform playerPos;
	public Transform arrow;
	private float mapScale = 2.0f;

	public static List<RadarObject> radarObjects = new List<RadarObject> ();

	public static void RegisterRadarObject(GameObject o, Image i)
	{

		Image image = Instantiate (i);
		radarObjects.Add ( new RadarObject(){owner = o, icon = image} );
	}

	public static void RemoveRadarObject(GameObject o)
	{
		List <RadarObject> newList = new List<RadarObject> ();

		for (int i = 0; i < radarObjects.Count; i++) {

			if (radarObjects [i].owner == o) {

				Destroy (radarObjects [i].icon);
				continue;
			} else {

				newList.Add (radarObjects [i]);
			}
				
		}

		radarObjects.RemoveRange (0, radarObjects.Count);
		radarObjects.AddRange (newList);


	}

	void DrawRadarDots(){

		foreach (RadarObject radObj in radarObjects) 
		{
			float angle = 0.0f; //270.0f
			Vector3 radarPos = (radObj.owner.transform.position - playerPos.position); 
			float distanceToObject = Vector3.Distance (playerPos.position, radObj.owner.transform.position) * mapScale;
			float deltaY = Mathf.Atan2 (radarPos.x, radarPos.z) * Mathf.Rad2Deg - angle - playerPos.eulerAngles.y;
			radarPos.x = distanceToObject * Mathf.Cos (deltaY * Mathf.Deg2Rad) * - 1f;
			radarPos.z = distanceToObject * Mathf.Sin (deltaY * Mathf.Deg2Rad);	

			radObj.icon.transform.SetParent(this.transform);
			radObj.icon.transform.position = new Vector3 (radarPos.x,radarPos.z, 0.0f) + this.transform.position;

		}


	}

	void Update(){

		DrawRadarDots ();


	}


}
