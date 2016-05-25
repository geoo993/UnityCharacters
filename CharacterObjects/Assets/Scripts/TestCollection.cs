using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestCollection : MonoBehaviour {


	public List<GameObject> collectorsList = new List<GameObject> ();

	int v = 0; 

	// Use this for initialization
	void Start () {
	

		for (int e = 0; e < collectorsList.Count; e++) {

			collectorsList [e].SetActive(false);
		}

		collectorsList [0].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown ("space")) 
		{


			removeFirst ();
			showNext ();


			print ("ob Count: "+ collectorsList.Count);
		}
	}


	void removeFirst()
	{
		
		//collectorsList.RemoveAt(1);
		DestroyImmediate (collectorsList [v]);
		v++;

	}

	void showNext()
	{
		
		if (v < collectorsList.Count ) {
			collectorsList [v].SetActive (true);

		}
	}



}
