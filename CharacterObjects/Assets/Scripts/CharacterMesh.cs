using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]


public class CharacterMesh : MonoBehaviour {


	private GameObject topParent = null;
	private GameObject bottomParent = null;
	private GameObject rightParent = null;
	private GameObject leftParent = null;
	private GameObject frontParent = null;
	private GameObject backParent = null;
	private GameObject frontStrechParent = null;
	private GameObject backStrechParent = null;

	public GameObject sphere;
	private int currentSphereIndex = 0;
	private List <GameObject> newSpheres = new List<GameObject>();

	private int animateCount = 0;
	private string moveState = "idle";

	private int xSize = 10;
	private int ySize = 10;
	private int zSize = 10;
	private int roundness = 5;

	List <int> top = new List<int>(); 
	private float topChanger = 0;

	List <int> bottom = new List<int>(); 
	private float bottomChanger = 0;

	private bool nextAnim,prevAnim = false;
	private float ballId, carId, airplaneId, jetId, nasaId, rocketId = 0;
	 

	private int[] left, right, back, front, frontStretch, backStretch = new int[]{};

	private Rigidbody meshRigidBody; 
	//private MeshCollider meshCollider; 
	private SphereCollider sphereCollider; 
	private MeshFilter meshFilter ;
	private Renderer meshRenderer;
	private Mesh mesh;

	private Vector3[] vertices = new Vector3[]{};
	private Vector3[] normals = new Vector3[]{};
	private Vector2[] uv = new Vector2[]{};
	private int[] triangles = new int[]{}; 
	private static int
	SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {
		triangles[i] = v00;
		triangles[i + 1] = triangles[i + 4] = v01;
		triangles[i + 2] = triangles[i + 3] = v10;
		triangles[i + 5] = v11;
		return i + 6;
	}



	private GameObject createSphere(Vector3 pos , List <GameObject> Arr, int Id){

		GameObject a = (GameObject) Instantiate(sphere, pos, Quaternion.identity);
		a.name = "Cube" + Id;
		a.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
		a.GetComponent<Renderer> ().material.color = Color.red;
		a.transform.parent = this.transform;
		Arr.Add (a);
		return a;
	}
	GameObject createParent(Vector3 pos, string name)
	{
		GameObject a = new GameObject ();
		a.name = name;
		a.transform.parent = this.transform;
		a.transform.localPosition = pos;
		return a;
	}


	void Awake ()
	{
		this.name = "dynamic object";

		CreateVertices (); 
		CreateMesh ();
		CreateColorAndtexture ();
		CreateCollider();
		//CreateRigidBody ();


		AddSpheres ();
		GetSides ();
		SpheresID ();

	}

	void AddSpheres()
	{
		for (int i = 0; i < vertices.Length; i++) 
		{
			createSphere (this.transform.position + vertices[i], newSpheres, i);
		}
	}

	void GetSides()
	{
		//front
		front = new int[] {
			303, 304, 305, 306, 307,
			263, 264, 265, 266, 267,
			223, 224, 225, 226, 227,
			183, 184, 185, 186, 187,
			143, 144, 145, 146, 147
		};

		// back
		back = new int[] {
			283, 284, 285, 286, 287,
			243, 244, 245, 246, 247,
			203, 204, 205, 206, 207,
			163, 164, 165, 166, 167,
			123, 124, 125, 126, 127
		};


		//right
		right = new int[] {
			293, 294, 295, 296, 297,
			253, 254, 255, 256, 257,
			213, 214, 215, 216, 217,
			173, 174, 175, 176, 177,
			133, 134, 135, 136, 137
		};

		//left
		left = new int[] {
			313, 314, 315, 316, 317,
			273, 274, 275, 276, 277,
			233, 234, 235, 236, 237,
			193, 194, 195, 196, 197,
			153, 154, 155, 156, 157
		};


		///top
		for (int t = 360; t < 521; t++) {
			top.Add (t);
		}


		//bottom
		for (int b = 0; b < 80; b++) {
			bottom.Add (b);
		}
		for (int b2 = 521; b2 < 602; b2++) {
			bottom.Add (b2);
		}

		frontStretch = new int[] {
			96, 97, 98, 99, 100,
			101, 102, 103, 104, 105,
			106, 107, 108, 109, 110,
			111, 112, 113, 114, 115,

			138, 139, 140, 141, 142,
			148, 149, 150, 151, 152,

			178, 179, 180, 181, 182,
			188, 189, 190, 191, 192,

			218, 219, 220, 221, 222,
			228, 229, 230, 231, 232,

			258, 259, 260, 261, 262,
			268, 269, 270, 271, 272,

			298, 299, 300, 301, 302,
			308, 309, 310, 311, 312,

			336, 337, 338, 339, 340,
			341, 342, 343, 344, 345,
			346, 347, 348, 349, 350,
			351, 352, 353, 354, 355,

		};


		backStretch = new int[]{ 
		
			80,81,82,83,84,
			85,86,87,88,89,
			90,91,92,93,94,
			95,
			116,117,118,119,120,
			121,122,128,129,130,
			131,132,158,159,160,
			161,162,168,169,170,
			171,172,198,199,200,
			201,202,208,209,210,
			211,212,238,239,240,
			241,242,248,249,250,
			251,252,278,279,280,
			281,282,288,289,290,
			291,292,318,319,
			320,321,322,323,324,
			325,326,327,328,329,
			330,331,332,333,334,
			335,356,357,358,359
		
		};
	}


	void SpheresID ()
	{

		topParent = createParent(newSpheres [top [120]].transform.localPosition, "top");
		bottomParent = createParent(newSpheres [bottom [120]].transform.localPosition, "bottom");
		leftParent = createParent(newSpheres [left [12]].transform.localPosition, "left");
		rightParent = createParent(newSpheres [right [12]].transform.localPosition, "right");
		frontParent = createParent(newSpheres [front [12]].transform.localPosition, "front");
		backParent = createParent(newSpheres [back [12]].transform.localPosition, "back");
		frontStrechParent = createParent(newSpheres [front [12]].transform.localPosition, "front Strech");
		backStrechParent = createParent(newSpheres [back [12]].transform.localPosition, "back Strech");

		foreach (int fs in frontStretch) {
			newSpheres [fs].transform.parent = frontStrechParent.transform;
			newSpheres [fs].GetComponent<Renderer> ().material.color = Color.grey;
		}

		foreach (int bs in backStretch) {
			newSpheres [bs].transform.parent = backStrechParent.transform;
			newSpheres [bs].GetComponent<Renderer> ().material.color = Color.red;
		}
		foreach (int t in top) 
		{
			newSpheres [t].transform.parent = topParent.transform;
			newSpheres [t].GetComponent<Renderer> ().material.color = Color.cyan;
		}

		foreach (int b in bottom) 
		{
			newSpheres [b].transform.parent = bottomParent.transform;
			newSpheres [b].GetComponent<Renderer> ().material.color = Color.black;

		}

		for(int i = 0; i < 25; i++)
		{
			newSpheres [left[i]].transform.parent = leftParent.transform;
			newSpheres [right[i]].transform.parent = rightParent.transform;
			newSpheres [front[i]].transform.parent = frontParent.transform;
			newSpheres [back[i]].transform.parent = backParent.transform;

			newSpheres [left[i]].GetComponent<Renderer> ().material.color = Color.magenta;
			newSpheres [right[i]].GetComponent<Renderer> ().material.color = Color.green;
			newSpheres [front[i]].GetComponent<Renderer> ().material.color = Color.blue;
			newSpheres [back[i]].GetComponent<Renderer> ().material.color = Color.yellow;
		}
		newSpheres [left[12]].GetComponent<Renderer> ().material.color = Color.white;
		newSpheres [right[12]].GetComponent<Renderer> ().material.color = Color.white;
		newSpheres [front[12]].GetComponent<Renderer> ().material.color = Color.white;
		newSpheres [back[12]].GetComponent<Renderer> ().material.color = Color.white;
		newSpheres [top[120]].GetComponent<Renderer> ().material.color = Color.white;
		newSpheres [bottom[120]].GetComponent<Renderer> ().material.color = Color.white;

	}


	//// animate left and right
	void AnimateLR(Transform me, Vector3 target)
	{
		//me.Translate(((target.x - me.position.x) / 100.0f), ((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(((target.x - me.position.x) / 100.0f), -((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(((target.x - me.position.x) / 100.0f), 0.0f, 0.0f);
		//me.Translate(0.0f, ((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(0.0f, ((target.y - me.position.y) / 100.0f), -((target.z - me.position.z) / 100.0f));
	}
	void ReverseAnimateLR(Transform me, Vector3 target)
	{
				//me.Translate(- ((target.x - me.position.x) / 100.0f), - ((target.y - me.position.y) / 100.0f), 0.0f);
		//		me.Translate(-((target.x - me.position.x) / 100.0f), ((target.y - me.position.y) / 100.0f), 0.0f);
		//		me.Translate(-((target.x - me.position.x) / 100.0f), 0.0f, 0.0f);
		//		me.Translate(0.0f, -((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(0.0f, -((target.y - me.position.y) / 100.0f), ((target.z - me.position.z) / 100.0f));
	}


	//// animate front
	void AnimateFront(Transform me, Vector3 target)
	{
		
		//me.Translate(((target.x - me.position.x) / 100.0f), ((target.y - me.position.y) / 100.0f), 0.02f);
		//me.Translate(((target.x - me.position.x) / 100.0f), -((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(((target.x - me.position.x) / 100.0f), 0.0f, 0.0f);
		//me.Translate(0.0f, ((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(-((target.x - me.position.x) / 100.0f), ((target.y - me.position.y) / 100.0f), 0.0f);

	}
	//animate back
	void AnimateBack(Transform me, Vector3 target)
	{

		//me.Translate(((target.x - me.position.x) / 100.0f), ((target.y - me.position.y) / 100.0f), -0.02f);
		//me.Translate(((target.x - me.position.x) / 100.0f), -((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(((target.x - me.position.x) / 100.0f), 0.0f, 0.0f);
		//me.Translate(0.0f, ((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(-((target.x - me.position.x) / 100.0f), ((target.y - me.position.y) / 100.0f), 0.0f);

	}
	void ReverseAnimateFB(Transform me, Vector3 target)
	{
		//me.Translate(- ((target.x - me.position.x) / 100.0f), - ((target.y - me.position.y) / 100.0f), 0.0f);
//		me.Translate(-((target.x - me.position.x) / 100.0f), ((target.y - me.position.y) / 100.0f), 0.0f);
//		me.Translate(-((target.x - me.position.x) / 100.0f), 0.0f, 0.0f);
//		me.Translate(0.0f, -((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(((target.x - me.position.x) / 100.0f), -((target.y - me.position.y) / 100.0f), 0.0f);
	}

	float getDistance(Vector3 fr, Vector3 to)
	{
		float xDist = to.x - fr.x;
		float yDist = to.y - fr.y;
		//float zDist = to.z - fr.z;

		float distance = Mathf.Sqrt(xDist * xDist + yDist * yDist);
		return distance;

//		for(int b = 0; b < back.Length; b++)
//		{
//			float dist = Vector3.Distance (newSpheres [back [b]].transform.localPosition, newSpheres [back [12]].transform.localPosition);
//			float dist2 = getDistance (newSpheres [back [b]].transform.localPosition, newSpheres [back [12]].transform.localPosition);
//			print (b+"    dist:"+dist +"   dist2: "+dist2);
//		}
	}

	void BallAnimation()
	{
		if (moveState == "ball") {


			if (ballId < 1.0f) {

				ballId += Time.deltaTime * (1.0f / 10.0f);

			} else {

				if (nextAnim) 
				{
					print ("gone to 1");
					animateCount = 1;
					nextAnim = false;
				}
				if (prevAnim) 
				{
					print ("gone to 0");
					animateCount = 0;
					prevAnim = false;
				}
				moveState = "idle";

			} 
			//print (ballId);

			topChanger = Mathf.Lerp (topChanger, 0, ballId);
			bottomChanger = Mathf.Lerp (bottomChanger, 0, ballId);
		} else 
		{
			ballId = 0;
		}

	}
	void CarAnimation()
	{
		
		if (moveState == "car") {
			
			if (carId < 1.0f) {

				carId += Time.deltaTime * (1.0f / 10.0f);

			} else {
				
				if (nextAnim) 
				{
					print ("gone to 2");
					animateCount = 2;
					nextAnim = false;
				}
				if (prevAnim) 
				{
					print ("gone to 1");
					animateCount = 1;
					prevAnim = false;
				}
				moveState = "idle";

			} 


			if (nextAnim) {
				topChanger = Mathf.Lerp (topChanger, 0, carId);
				bottomChanger = Mathf.Lerp (bottomChanger, -2, carId);

				for (int i = 0; i < 25; i++) 
				{
					newSpheres [front [i]].transform.Translate (
						-((newSpheres [front [12]].transform.position.x - newSpheres [front [i]].transform.position.x) / 1000.0f), 
						-((newSpheres [front [12]].transform.position.y - newSpheres [front [i]].transform.position.y) / 1600.0f), 
						0.0f);

				}
				float frontZ = Mathf.Lerp (frontParent.transform.localPosition.z, 12.0f, carId);

				float backZ = Mathf.Lerp (backParent.transform.localPosition.z, -1.0f, carId);

				float rightX = Mathf.Lerp (rightParent.transform.localPosition.x, 9.5f, carId);
				float leftX = Mathf.Lerp (leftParent.transform.localPosition.x, 0.5f, carId);
				float frontStretchZ = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 11.0f, carId);
				float backStretchZ = Mathf.Lerp (backStrechParent.transform.localPosition.z, -0.60f, carId);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontParent.transform.localPosition.y, frontZ);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZ);
				leftParent.transform.localPosition = new Vector3 (leftX, leftParent.transform.localPosition.y, leftParent.transform.localPosition.z);
				rightParent.transform.localPosition = new Vector3 (rightX, rightParent.transform.localPosition.y, rightParent.transform.localPosition.z);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZ);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZ);

			}
			if (prevAnim) {
				topChanger = Mathf.Lerp (topChanger, 0, carId);
				bottomChanger = Mathf.Lerp (bottomChanger, 0, carId);

				for (int i = 0; i < 25; i++) {
					newSpheres [front [i]].transform.Translate (
						((newSpheres [front [12]].transform.position.x - newSpheres [front [i]].transform.position.x) / 1000.0f), 
						((newSpheres [front [12]].transform.position.y - newSpheres [front [i]].transform.position.y) / 1600.0f), 
						0.0f);

				}
				float frontZr = Mathf.Lerp (frontParent.transform.localPosition.z, 10.0f, carId);

				float backZr = Mathf.Lerp (backParent.transform.localPosition.z, 0.0f, carId);

				float rightXr = Mathf.Lerp (rightParent.transform.localPosition.x, 10f, carId);
				float leftXr = Mathf.Lerp (leftParent.transform.localPosition.x, 0f, carId);
				float frontStretchZr = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 10.0f, carId);
				float bachStretchZr = Mathf.Lerp (backStrechParent.transform.localPosition.z, 0f, carId);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontParent.transform.localPosition.y, frontZr);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZr);
				leftParent.transform.localPosition = new Vector3 (leftXr, leftParent.transform.localPosition.y, leftParent.transform.localPosition.z);
				rightParent.transform.localPosition = new Vector3 (rightXr, rightParent.transform.localPosition.y, rightParent.transform.localPosition.z);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZr);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, bachStretchZr);

			}


		} else {
			carId = 0;
		}


	}
	void PlaneAnimation()
	{
		if (moveState == "airplane") {

			if (airplaneId < 1.0f) {

				airplaneId += Time.deltaTime * (1.0f / 10.0f);
			} else {

				if (nextAnim) 
				{
					print ("gone to 3");
					animateCount = 3;
					nextAnim = false;
				}
				if (prevAnim) 
				{
					print ("gone to 2");
					animateCount = 2;
					prevAnim = false;
				}
				moveState = "idle";
			} 

			if (nextAnim) 
			{
				topChanger = Mathf.Lerp (topChanger, 0, airplaneId);
				bottomChanger = Mathf.Lerp (bottomChanger, -3, airplaneId);

				for (int i = 0; i < 25; i++) {
					newSpheres [front [i]].transform.Translate (
						((newSpheres [front [12]].transform.position.x - newSpheres [front [i]].transform.position.x) / 1000.0f), 
						((newSpheres [front [12]].transform.position.y - newSpheres [front [i]].transform.position.y) / 500.0f), 
						0.0f);

					newSpheres [back [i]].transform.Translate (
						((newSpheres [back [12]].transform.position.x - newSpheres [back [i]].transform.position.x) / 500.0f), 
						-((newSpheres [back [12]].transform.position.y - newSpheres [back [i]].transform.position.y) / 1000.0f), 
						0.0f);

				}


				float frontY = Mathf.Lerp (frontParent.transform.localPosition.y, 5.5f, airplaneId);

				float backZ = Mathf.Lerp (backParent.transform.localPosition.z, -10.0f, airplaneId);

				float backStretchZ = Mathf.Lerp (backStrechParent.transform.localPosition.z, -2.0f, airplaneId);


				float LeftRightZ = Mathf.Lerp (leftParent.transform.localPosition.z, 4.0f, airplaneId);
				float leftX = Mathf.Lerp (leftParent.transform.localPosition.x, -5.0f, airplaneId);
				float rightX = Mathf.Lerp (rightParent.transform.localPosition.x, 15.0f, airplaneId);
				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontY, frontParent.transform.localPosition.z);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZ);
				leftParent.transform.localPosition = new Vector3 (leftX, leftParent.transform.localPosition.y, LeftRightZ);
				rightParent.transform.localPosition = new Vector3 (rightX, rightParent.transform.localPosition.y, LeftRightZ);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZ);
			
			}
			if (prevAnim) {
				topChanger = Mathf.Lerp (topChanger, 0, airplaneId);
				bottomChanger = Mathf.Lerp (bottomChanger, -2, airplaneId);

				for (int i = 0; i < 25; i++) {
					newSpheres [front [i]].transform.Translate (
						-((newSpheres [front [12]].transform.position.x - newSpheres [front [i]].transform.position.x) / 1000.0f), 
						-((newSpheres [front [12]].transform.position.y - newSpheres [front [i]].transform.position.y) / 500.0f), 
						0.0f);

					newSpheres [back [i]].transform.Translate (
						-((newSpheres [back [12]].transform.position.x - newSpheres [back [i]].transform.position.x) / 500.0f), 
						((newSpheres [back [12]].transform.position.y - newSpheres [back [i]].transform.position.y) / 1000.0f), 
						0.0f);

				}

				float frontYr = Mathf.Lerp (frontParent.transform.localPosition.y, 5f, airplaneId);
				float backZr = Mathf.Lerp (backParent.transform.localPosition.z, -1.0f, airplaneId);
				float backStretchZr = Mathf.Lerp (backStrechParent.transform.localPosition.z, -0.60f, airplaneId);


				float LeftRightZr = Mathf.Lerp (leftParent.transform.localPosition.z, 5.0f, airplaneId);
				float leftXr = Mathf.Lerp (leftParent.transform.localPosition.x, 0.5f, airplaneId);
				float rightXr = Mathf.Lerp (rightParent.transform.localPosition.x, 9.5f, airplaneId);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontYr, frontParent.transform.localPosition.z);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZr);
				leftParent.transform.localPosition = new Vector3 (leftXr, leftParent.transform.localPosition.y, LeftRightZr);
				rightParent.transform.localPosition = new Vector3 (rightXr, rightParent.transform.localPosition.y, LeftRightZr);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZr);

			}
		} else {

			airplaneId = 0;
		}

	}

	void JetAnimation()
	{
		if (moveState == "jet") {

			if (jetId < 1.0f) {

				jetId += Time.deltaTime * (1.0f / 10.0f);
			} else {
				
				if (nextAnim) {
					print ("gone to 4");
					animateCount = 4;
					nextAnim = false;
				}
				if (prevAnim) {
					print ("gone to 3");
					animateCount = 3;
					prevAnim = false;
				}
				moveState = "idle";

			}


			if (nextAnim) {
				
				topChanger = Mathf.Lerp (topChanger, -1.5f, jetId);
				bottomChanger = Mathf.Lerp (bottomChanger, -3.2f, jetId);

				for (int i = 0; i < 25; i++) {
					newSpheres [left [i]].transform.Translate (
						((newSpheres [left [12]].transform.position.x - newSpheres [left [i]].transform.position.x) / 300.0f), 
						((newSpheres [left [12]].transform.position.y - newSpheres [left [i]].transform.position.y) / 300.0f), 
						0.00f);

					newSpheres [right [i]].transform.Translate (
						((newSpheres [right [12]].transform.position.x - newSpheres [right [i]].transform.position.x) / 300.0f), 
						((newSpheres [right [12]].transform.position.y - newSpheres [right [i]].transform.position.y) / 300.0f), 
						0.0f);

					newSpheres [front [i]].transform.Translate (
						((newSpheres [front [12]].transform.position.x - newSpheres [front [i]].transform.position.x) / 900.0f), 
						((newSpheres [front [12]].transform.position.y - newSpheres [front [i]].transform.position.y) / 400.0f), 
						0.0f);

					newSpheres [back [i]].transform.Translate (
						-((newSpheres [back [12]].transform.position.x - newSpheres [back [i]].transform.position.x) / 1000.0f), 
						((newSpheres [back [12]].transform.position.y - newSpheres [back [i]].transform.position.y) / 300.0f), 
						0.0f);
				}

				//stretch y
				foreach (int bs in backStretch) {
					newSpheres [bs].transform.Translate (
						0.0f, 
						((newSpheres [left [12]].transform.position.y - newSpheres [bs].transform.position.y) / 500.0f), 
						0.0f);
				}
				foreach (int fs in frontStretch) {
					newSpheres [fs].transform.Translate (
						0.0f, 
						((newSpheres [left [12]].transform.position.y - newSpheres [fs].transform.position.y) / 500.0f), 
						0.0f);
				}



				float frontY = Mathf.Lerp (frontParent.transform.localPosition.y, 5.0f, jetId);
				float LeftRightZ = Mathf.Lerp (leftParent.transform.localPosition.z, 1.0f, jetId);
				float leftX = Mathf.Lerp (leftParent.transform.localPosition.x, -2.0f, jetId);
				float rightX = Mathf.Lerp (rightParent.transform.localPosition.x, 12.0f, jetId);
				float backZ = Mathf.Lerp (backParent.transform.localPosition.z, -5.0f, jetId);
				float backStretchZ = Mathf.Lerp (backStrechParent.transform.localPosition.z, -1.0f, jetId);
				float frontStretchZ = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 12.0f, jetId);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontY, frontParent.transform.localPosition.z);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZ);
				leftParent.transform.localPosition = new Vector3 (leftX, leftParent.transform.localPosition.y, LeftRightZ);
				rightParent.transform.localPosition = new Vector3 (rightX, rightParent.transform.localPosition.y, LeftRightZ);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZ);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZ);
			}
			if (prevAnim) {

				topChanger = Mathf.Lerp (topChanger, 0, jetId);
				bottomChanger = Mathf.Lerp (bottomChanger, -3, jetId);


				for (int i = 0; i < 25; i++) {
					newSpheres [left [i]].transform.Translate (
						-((newSpheres [left [12]].transform.position.x - newSpheres [left [i]].transform.position.x) / 300.0f), 
						-((newSpheres [left [12]].transform.position.y - newSpheres [left [i]].transform.position.y) / 300.0f), 
						0.00f);

					newSpheres [right [i]].transform.Translate (
						-((newSpheres [right [12]].transform.position.x - newSpheres [right [i]].transform.position.x) / 300.0f), 
						-((newSpheres [right [12]].transform.position.y - newSpheres [right [i]].transform.position.y) / 300.0f), 
						0.0f);

					newSpheres [front [i]].transform.Translate (
						-((newSpheres [front [12]].transform.position.x - newSpheres [front [i]].transform.position.x) / 900.0f), 
						-((newSpheres [front [12]].transform.position.y - newSpheres [front [i]].transform.position.y) / 400.0f), 
						0.0f);

					newSpheres [back [i]].transform.Translate (
						((newSpheres [back [12]].transform.position.x - newSpheres [back [i]].transform.position.x) / 1000.0f), 
						-((newSpheres [back [12]].transform.position.y - newSpheres [back [i]].transform.position.y) / 300.0f), 
						0.0f);
				}

				//stretch y
				foreach (int bs in backStretch) {
					newSpheres [bs].transform.Translate (
						0.0f, 
						-((newSpheres [left [12]].transform.position.y - newSpheres [bs].transform.position.y) / 500.0f), 
						0.0f);
				}
				foreach (int fs in frontStretch) {
					newSpheres [fs].transform.Translate (
						0.0f, 
						-((newSpheres [left [12]].transform.position.y - newSpheres [fs].transform.position.y) / 500.0f), 
						0.0f);
				}

				float frontYr = Mathf.Lerp (frontParent.transform.localPosition.y, 5.5f, jetId);
				float LeftRightZr = Mathf.Lerp (leftParent.transform.localPosition.z, 4.0f, jetId);
				float leftXr = Mathf.Lerp (leftParent.transform.localPosition.x, -5.0f, jetId);
				float rightXr = Mathf.Lerp (rightParent.transform.localPosition.x, 15.0f, jetId);
				float backZr = Mathf.Lerp (backParent.transform.localPosition.z, -10.0f, jetId);
				float backStretchZr = Mathf.Lerp (backStrechParent.transform.localPosition.z, -2.0f, jetId);
				float frontStretchZr = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 11.0f, jetId);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontYr, frontParent.transform.localPosition.z);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZr);
				leftParent.transform.localPosition = new Vector3 (leftXr, leftParent.transform.localPosition.y, LeftRightZr);
				rightParent.transform.localPosition = new Vector3 (rightXr, rightParent.transform.localPosition.y, LeftRightZr);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZr);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZr);
			}



		} else 
		{

			jetId = 0;
		}
	}

	void NasaPlaneAnimation()
	{


		if (moveState == "nasa") {

			if (nasaId < 1.0f) {

				nasaId += Time.deltaTime * (1.0f / 10.0f);
			} else {

				if (nextAnim) {
					print ("gone to 5");
					animateCount = 5;
					nextAnim = false;
				}
				if (prevAnim) {
					print ("gone to 4");
					animateCount = 4;
					prevAnim = false;
				}
				moveState = "idle";
			} 

			if (nextAnim) {
				
				topChanger = Mathf.Lerp (topChanger, -2.5f, nasaId);
				bottomChanger = Mathf.Lerp (bottomChanger, -3.2f, nasaId);

				for (int i = 0; i < 25; i++) {
					newSpheres [front [i]].transform.Translate (
						((newSpheres [front [12]].transform.position.x - newSpheres [front [i]].transform.position.x) / 500.0f), 
						((newSpheres [front [12]].transform.position.y - newSpheres [front [i]].transform.position.y) / 400.0f), 
						((newSpheres [front [12]].transform.position.z - newSpheres [front [i]].transform.position.z) / 200.0f));
				
					newSpheres [left [i]].transform.Translate (
						((newSpheres [left [12]].transform.position.x - newSpheres [left [i]].transform.position.x) / 400.0f), 
						((newSpheres [left [12]].transform.position.y - newSpheres [left [i]].transform.position.y) / 400.0f), 
						0.00f);

					newSpheres [right [i]].transform.Translate (
						((newSpheres [right [12]].transform.position.x - newSpheres [right [i]].transform.position.x) / 400.0f), 
						((newSpheres [right [12]].transform.position.y - newSpheres [right [i]].transform.position.y) / 400.0f), 
						0.0f);
				}

				//stretch y
				foreach (int bs in backStretch) {
					newSpheres [bs].transform.Translate (
						((newSpheres [top [120]].transform.position.x - newSpheres [bs].transform.position.x) / 1000.0f), 
						((newSpheres [left [12]].transform.position.y - newSpheres [bs].transform.position.y) / 1000.0f), 
						0.0f);
				}
				foreach (int fs in frontStretch) {
					newSpheres [fs].transform.Translate (
						((newSpheres [top [120]].transform.position.x - newSpheres [fs].transform.position.x) / 1000.0f), 
						((newSpheres [left [12]].transform.position.y - newSpheres [fs].transform.position.y) / 1000.0f), 
						0.0f);
				}

				float frontZ = Mathf.Lerp (frontParent.transform.localPosition.z, 11.0f, nasaId);
				float LeftRightZ = Mathf.Lerp (leftParent.transform.localPosition.z, 3.0f, nasaId);
				float leftX = Mathf.Lerp (leftParent.transform.localPosition.x, -5.0f, nasaId);
				float rightX = Mathf.Lerp (rightParent.transform.localPosition.x, 15.0f, nasaId);
				float backZ = Mathf.Lerp (backParent.transform.localPosition.z, 0.0f, nasaId);
				float backStretchZ = Mathf.Lerp (backStrechParent.transform.localPosition.z, 0.0f, nasaId);
				float frontStretchZ = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 11.0f, nasaId);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontParent.transform.localPosition.y, frontZ);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZ);
				leftParent.transform.localPosition = new Vector3 (leftX, leftParent.transform.localPosition.y, LeftRightZ);
				rightParent.transform.localPosition = new Vector3 (rightX, rightParent.transform.localPosition.y, LeftRightZ);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZ);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZ);

			}

			if (prevAnim) {

				topChanger = Mathf.Lerp (topChanger, -1.5f, nasaId);
				bottomChanger = Mathf.Lerp (bottomChanger, -3.2f, nasaId);

				for (int i = 0; i < 25; i++) {
					newSpheres [front [i]].transform.Translate (
						-((newSpheres [front [12]].transform.position.x - newSpheres [front [i]].transform.position.x) / 500.0f), 
						-((newSpheres [front [12]].transform.position.y - newSpheres [front [i]].transform.position.y) / 400.0f), 
						-((newSpheres [front [12]].transform.position.z - newSpheres [front [i]].transform.position.z) / 200.0f));

					newSpheres [left [i]].transform.Translate (
						-((newSpheres [left [12]].transform.position.x - newSpheres [left [i]].transform.position.x) / 400.0f), 
						-((newSpheres [left [12]].transform.position.y - newSpheres [left [i]].transform.position.y) / 400.0f), 
						0.00f);

					newSpheres [right [i]].transform.Translate (
						-((newSpheres [right [12]].transform.position.x - newSpheres [right [i]].transform.position.x) / 400.0f), 
						-((newSpheres [right [12]].transform.position.y - newSpheres [right [i]].transform.position.y) / 400.0f), 
						0.0f);
				}

				//stretch y
				foreach (int bs in backStretch) {
					newSpheres [bs].transform.Translate (
						-((newSpheres [top [120]].transform.position.x - newSpheres [bs].transform.position.x) / 1000.0f), 
						-((newSpheres [left [12]].transform.position.y - newSpheres [bs].transform.position.y) / 1000.0f), 
						0.0f);
				}
				foreach (int fs in frontStretch) {
					newSpheres [fs].transform.Translate (
						-((newSpheres [top [120]].transform.position.x - newSpheres [fs].transform.position.x) / 1000.0f), 
						-((newSpheres [left [12]].transform.position.y - newSpheres [fs].transform.position.y) / 1000.0f), 
						0.0f);
				}


				float frontZr = Mathf.Lerp (frontParent.transform.localPosition.z, 12.0f, nasaId);

				float LeftRightZr = Mathf.Lerp (leftParent.transform.localPosition.z, 1.0f, nasaId);
				float leftXr = Mathf.Lerp (leftParent.transform.localPosition.x, -2.0f, nasaId);
				float rightXr = Mathf.Lerp (rightParent.transform.localPosition.x, 12.0f, nasaId);
				float backZr = Mathf.Lerp (backParent.transform.localPosition.z, -5.0f, nasaId);
				float backStretchZr = Mathf.Lerp (backStrechParent.transform.localPosition.z, -1.0f, nasaId);
				float frontStretchZr = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 12.0f, nasaId);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontParent.transform.localPosition.y, frontZr);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZr);
				leftParent.transform.localPosition = new Vector3 (leftXr, leftParent.transform.localPosition.y, LeftRightZr);
				rightParent.transform.localPosition = new Vector3 (rightXr, rightParent.transform.localPosition.y, LeftRightZr);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZr);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZr);



			}
		} else {
			nasaId = 0;
		}


	}


	void RocketAnimation()
	{
		if (moveState == "rocket") {

			if (rocketId < 1.0f) {

				rocketId += Time.deltaTime * (1.0f / 10.0f);
			} else {

				if (nextAnim) {
					print ("gone to 6 ");
					animateCount = 6;
					nextAnim = false;
				}
				if (prevAnim) {
					print ("gone to 5");
					animateCount = 5;
					prevAnim = false;
				}

				moveState = "idle";
			} 

			if (nextAnim) {

			
				topChanger = Mathf.Lerp (topChanger, -3.8f, rocketId);
				bottomChanger = Mathf.Lerp (bottomChanger, -4.0f, rocketId);

				for (int i = 0; i < 25; i++) {
					newSpheres [back [i]].transform.Translate (
						((newSpheres [back [12]].transform.position.x - newSpheres [back [i]].transform.position.x) / 800.0f), 
						((newSpheres [back [12]].transform.position.y - newSpheres [back [i]].transform.position.y) / 400.0f), 
						0.0f);
				}

				foreach (int bs in backStretch) {
					newSpheres [bs].transform.Translate (
						((newSpheres [top [120]].transform.position.x - newSpheres [bs].transform.position.x) / 1000.0f), 
						((newSpheres [front [12]].transform.position.y - newSpheres [bs].transform.position.y) / 1000.0f), 
						0.0f);
				}
				foreach (int fs in frontStretch) {
					newSpheres [fs].transform.Translate (
						((newSpheres [top [120]].transform.position.x - newSpheres [fs].transform.position.x) / 1000.0f), 
						((newSpheres [front [12]].transform.position.y - newSpheres [fs].transform.position.y) / 1000.0f), 
						0.0f);
				}

				float frontZ = Mathf.Lerp (frontParent.transform.localPosition.z, 10f, rocketId);
				float LeftRightY = Mathf.Lerp (leftParent.transform.localPosition.y, 4.5f, rocketId);
				float leftX = Mathf.Lerp (leftParent.transform.localPosition.x, 1.5f, rocketId);
				float rightX = Mathf.Lerp (rightParent.transform.localPosition.x, 8.5f, rocketId);
				float backZ = Mathf.Lerp (backParent.transform.localPosition.z, 1.0f, rocketId);
				float frontStretchZ = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 10f, rocketId);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontParent.transform.localPosition.y, frontZ);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZ);
				leftParent.transform.localPosition = new Vector3 (leftX, LeftRightY, leftParent.transform.localPosition.z);
				rightParent.transform.localPosition = new Vector3 (rightX, LeftRightY, rightParent.transform.localPosition.z);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZ);

			}
			if (prevAnim) {
				topChanger = Mathf.Lerp (topChanger, -2.5f, rocketId);
				bottomChanger = Mathf.Lerp (bottomChanger, -3.2f, rocketId);


				for (int i = 0; i < 25; i++) {
					newSpheres [back [i]].transform.Translate (
						-((newSpheres [back [12]].transform.position.x - newSpheres [back [i]].transform.position.x) / 800.0f), 
						-((newSpheres [back [12]].transform.position.y - newSpheres [back [i]].transform.position.y) / 400.0f), 
						0.0f);
				}

				foreach (int bs in backStretch) {
					newSpheres [bs].transform.Translate (
						-((newSpheres [top [120]].transform.position.x - newSpheres [bs].transform.position.x) / 1000.0f), 
						-((newSpheres [front [12]].transform.position.y - newSpheres [bs].transform.position.y) / 1000.0f), 
						0.0f);
				}
				foreach (int fs in frontStretch) {
					newSpheres [fs].transform.Translate (
						-((newSpheres [top [120]].transform.position.x - newSpheres [fs].transform.position.x) / 1000.0f), 
						-((newSpheres [front [12]].transform.position.y - newSpheres [fs].transform.position.y) / 1000.0f), 
						0.0f);
				}


				float frontZr = Mathf.Lerp (frontParent.transform.localPosition.z, 11f, rocketId);
				float LeftRightYr = Mathf.Lerp (leftParent.transform.localPosition.y, 5.0f, rocketId);
				float leftXr = Mathf.Lerp (leftParent.transform.localPosition.x, -5.0f, rocketId);
				float rightXr = Mathf.Lerp (rightParent.transform.localPosition.x, 15.0f, rocketId);
				float backZr = Mathf.Lerp (backParent.transform.localPosition.z, 0.0f, rocketId);
				float frontStretchZr = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 11f, rocketId);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontParent.transform.localPosition.y, frontZr);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZr);
				leftParent.transform.localPosition = new Vector3 (leftXr, LeftRightYr, leftParent.transform.localPosition.z);
				rightParent.transform.localPosition = new Vector3 (rightXr, LeftRightYr, rightParent.transform.localPosition.z);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZr);


			}

		} else {
			rocketId = 0;
		}


	}

	void Update()
	{


		UpdateMesh ();


		BallAnimation ();
		CarAnimation ();
		PlaneAnimation ();
		JetAnimation ();
		NasaPlaneAnimation ();
		RocketAnimation ();

		if (Input.GetKeyDown ("d")) 
		{
			prevAnim = false;
			if (animateCount == 0 && moveState == "idle" && nextAnim == false) {
				moveState = "ball"; 
				nextAnim = true;
			}
			if (animateCount == 1 && moveState == "idle" && nextAnim == false) {
				moveState = "car"; 
				nextAnim = true;
			}
			if (animateCount == 2 && moveState == "idle" && nextAnim == false) {
				moveState = "airplane"; 
				nextAnim = true;
			}
			if (animateCount == 3 && moveState == "idle" && nextAnim == false) {
				moveState = "jet"; 
				nextAnim = true;
			}
			if (animateCount == 4 && moveState == "idle" && nextAnim == false) {
				moveState = "nasa"; 
				nextAnim = true;
			}
			if (animateCount == 5 && moveState == "idle" && nextAnim == false) {
				moveState = "rocket"; 
				nextAnim = true;
			}
			print("moveState:  "+ moveState+"   count: "+animateCount +" prev: "+prevAnim+"  next: "+nextAnim);
		}

		if (Input.GetKeyDown ("a")) 
		{
			nextAnim = false;
			if (animateCount == 6 && moveState == "idle"  && prevAnim == false) {
				moveState = "rocket";
				prevAnim = true;
			}
			if (animateCount == 5 && moveState == "idle" && prevAnim == false) {
				moveState = "nasa"; 
				prevAnim = true;
			}
			if (animateCount == 4 && moveState == "idle" && prevAnim == false) {
				moveState = "jet"; 
				prevAnim = true;
			}
			if (animateCount == 3 && moveState == "idle" && prevAnim == false) {
				moveState = "airplane"; 
				prevAnim = true;
			}
			if (animateCount == 2 && moveState == "idle" && prevAnim == false) {
				moveState = "car"; 
				prevAnim = true;
			}
			if (animateCount == 1 && moveState == "idle" && prevAnim == false) {
				moveState = "ball"; 
				prevAnim = true;
			}
				
			print("moveState:  "+ moveState+"   count: "+animateCount +" prev: "+prevAnim+"  next: "+nextAnim);
		}


	}

	void ClearAll(){
		
		meshFilter.sharedMesh = null;
		//meshCollider.sharedMesh = null;
		sphereCollider =  null;
		meshRigidBody = null;
		vertices = null;
		triangles = null; 
		normals = null;
		uv = null;

	}


	void UpdateVertices()
	{
		//		for(int i = 0; i < vertices.Length; i++)
		//		{
		//			vertices [i] = newSpheres [i].transform.localPosition ;
		//		}

		for (int i = 0; i < 25; i++) 
		{
			vertices [left [i]] = newSpheres [left [i]].transform.localPosition +  leftParent.transform.localPosition;
			vertices [right [i]] = newSpheres [right [i]].transform.localPosition +  rightParent.transform.localPosition;
			vertices [front [i]] = newSpheres [front [i]].transform.localPosition +  frontParent.transform.localPosition;
			vertices [back [i]] = newSpheres [back [i]].transform.localPosition +  backParent.transform.localPosition;
		}


//		for (int j = 0; j < 161; j++) 
//		{
			topParent.transform.localPosition = vertices [top [120]] ;
			bottomParent.transform.localPosition = vertices [bottom [120]] ;

//			////vertices [top [j]] = newSpheres [top [j]].transform.localPosition +  topParent.transform.localPosition;
//			////vertices [bottom [j]] = newSpheres [bottom [j]].transform.localPosition +  bottomParent.transform.localPosition;
//		}

		foreach (int fs in frontStretch) {
			vertices [fs] = newSpheres [fs].transform.localPosition  +  frontStrechParent.transform.localPosition;
		}
		foreach (int bs in backStretch) {
			vertices [bs] = newSpheres [bs].transform.localPosition  +  backStrechParent.transform.localPosition;
		}

	}

	private void UpdateMesh()
	{

		meshFilter = GetComponent<MeshFilter>();
		if (meshFilter == null){
			Debug.LogError("MeshFilter not found!");
			return;
		}

		mesh = meshFilter.sharedMesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}
		mesh.name = "dynamic mesh";
		mesh.Clear();



		CreateVertices ();
		UpdateVertices ();
		CreateTriangles();

		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.normals = normals;
		mesh.uv = uv;

		mesh.RecalculateNormals();
		CalculateTangent.TangentSolver (mesh);
		mesh.RecalculateBounds();
		mesh.Optimize();

		//CreateCollider();
		//CreateColorAndtexture ();

	}

	private void CreateMesh()
	{

		meshFilter = GetComponent<MeshFilter>();
		if (meshFilter == null){
			Debug.LogError("MeshFilter not found!");
			return;
		}

		mesh = meshFilter.sharedMesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}
		mesh.name = "dynamic mesh";
		mesh.Clear();

		//CreateVertices();
		CreateTriangles();
	
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.normals = normals;
		mesh.uv = uv;

		mesh.RecalculateNormals();
		CalculateTangent.TangentSolver (mesh);
		mesh.RecalculateBounds();
		mesh.Optimize();

	}


	private void CreateVertices() {


		int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize - 3) * 4;

		int faceVertices = (
			(xSize - 1) * (ySize - 1) +
			(xSize - 1) * (zSize - 1) +
			(ySize - 1) * (zSize - 1)) * 2;
		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
		normals = new Vector3[vertices.Length];
		uv = new Vector2[vertices.Length];


		int v = 0;
		// sides
		for (int y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++) {
				SetVertex(v++, x, y, 0);
			}
			for (int z = 1; z <= zSize; z++) {
				SetVertex(v++, xSize, y, z);
			}
			for (int x = xSize - 1; x >= 0; x--) {
				SetVertex(v++, x, y, zSize);
			}
				
			for (int z = zSize - 1; z > 0; z--) {
				SetVertex(v++, 0, y, z);
			}
		}



		// top 
		for (int z = 1; z < zSize; z++) {
			for (int x = 1; x < xSize; x++) {
				SetVertex(v++, x, ySize, z);
			}
		}
		//bottom
		for (int z = 1; z < zSize; z++) {
			for (int x = 1; x < xSize; x++) {
				SetVertex(v++, x, 0, z);
			}
		}


	}
	private void SetVertex (int i, int x, int y, int z) {
		Vector3 inner = vertices[i] = new Vector3(x, y, z);


		////sides
		if (x < roundness) {
			inner.x = roundness;
		}
		else if (x > xSize - roundness) {
			inner.x = xSize - roundness;
		}

		////top and bottom
		if (y < roundness) {

			// bottom rounder
			inner.y = roundness - bottomChanger;
		}
		else if (y > ySize - roundness) 
		{
			// top rounder
			inner.y = ySize - (roundness - topChanger);
		}

		////front and back
		if (z < roundness) {

			//front rounder
			inner.z = roundness;
		}
		else if (z > zSize - roundness) {
			//back rounder
			inner.z = zSize - roundness;
		}

		normals[i] = (vertices[i] - inner).normalized;
		vertices[i] = inner + normals[i] * roundness;
		uv[i] = new Vector2((float)x / (xSize * 4), (float)y / (ySize * 4));
	}


	private void CreateTriangles () {
		
		int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
		triangles = new int[quads * 6];
		int ring = (xSize + zSize) * 2;
		int t = 0, v = 0;

		for (int y = 0; y < ySize; y++, v++) {
			for (int q = 0; q < ring - 1; q++, v++) {
				t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
			}
			t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
		}

		t = CreateTopFace(triangles, t, ring);
		t = CreateBottomFace(triangles, t, ring);

	
	}

	private int CreateTopFace (int[] triangles, int t, int ring) {
		int v = ring * ySize;
		for (int x = 0; x < xSize - 1; x++, v++) {
			t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
		}
		t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

		int vMin = ring * (ySize + 1) - 1;
		int vMid = vMin + 1;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
			}
			t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
		}

		int vTop = vMin - 2;
		t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
		for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
		}
		t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

		return t;
	}

	private int CreateBottomFace (int[] triangles, int t, int ring) {
		int v = 1;
		int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
		t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
		for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
		}
		t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

		int vMin = ring - 2;
		vMid -= xSize - 2;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
			}
			t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
		}

		int vTop = vMin - 1;
		t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
		for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
		}
		t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

		return t;
	}
	private void CreateCollider(){

		sphereCollider = gameObject.AddComponent (typeof(SphereCollider)) as SphereCollider;
//		meshCollider = GetComponent(typeof(MeshCollider)) as MeshCollider;
//		meshCollider.sharedMesh = mesh; // Give it your mesh here.

	}

	private void CreateRigidBody(){
		meshRigidBody = GetComponent (typeof(Rigidbody)) as Rigidbody;
		//meshRigidBody.isKinematic = true;
		//meshRigidBody.useGravity = true;
	}

	private void CreateColorAndtexture() {

		meshRenderer = GetComponent<MeshRenderer>();

		//Material material = new Material (Shader.Find ("Standard"));
		//material.color = Color.red;


		Texture[] textures = new Texture[] {

			Resources.Load ("Burnout") as Texture,
			Resources.Load ("Magma") as Texture,
			Resources.Load ("Noise") as Texture,
			Resources.Load ("Oilrush") as Texture,
			Resources.Load ("Turbulance") as Texture,
			Resources.Load ("Water") as Texture
		};


		Material material = new Material (Shader.Find (".ShaderExample/DissolveToColor"));
		//material.SetTexture ("_MainTex", textures [Random.Range (0, textures.Length - 1)]);
		material.SetTexture ("_DissolveMap", textures [Random.Range (0, textures.Length - 1)]);
		material.SetFloat ("_DissolveVal", 1.2f);
		material.SetFloat ("_LineWidth", 0.2f);
		material.SetColor ("_LineColor", ExtensionMethods.RandomColor());
		material.SetColor ("_DissolveColor", ExtensionMethods.RandomColor());


		meshRenderer.material = material;


	}



}
