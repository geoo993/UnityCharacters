using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
//[RequireComponent(typeof(Rigidbody))]


public class CharacterMesh : MonoBehaviour {

//	public Camera mainCamera;
//	private Ray ray;
//	private RaycastHit hit;
//	private GameObject hitObject = null;


	public GameObject sphere;
	private int currentSphereIndex = 0;
	private List <GameObject> newSpheres = new List<GameObject>();


	private float rightLeftPos = 0.0f; 



	private float spreadCount = 0;
	private bool spreadState =false;
	private bool moveState = false;
	private bool reverseState = false;

	private int xSize = 10;
	private int ySize = 10;
	private int zSize = 10;
	[Range(0,6)]public int roundness = 5;

	List <int> top = new List<int>(); 
	List <int> bottom = new List<int>(); 
	int[] left = new int[]{};
	int[] right = new int[]{};
	int[] front = new int[]{};
	int[] back = new int[]{};

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


	private GameObject createSphere(Vector3 pos , List <GameObject> objectArr, int Id){

		GameObject a = (GameObject) Instantiate(sphere, pos, Quaternion.identity);
		a.name = "Cube" + Id;
		a.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
		a.GetComponent<Renderer> ().material.color = Color.red;
		a.transform.parent = this.transform;
		objectArr.Add (a);

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

		moveLeftRightWings ();
		moveFrontRear ();
		moveTopBottom ();


	}

	void AddSpheres()
	{
		for (int i = 0; i < vertices.Length; i++) {

			createSphere (vertices[i], newSpheres, i);
		}
	}

	void GetSides()
	{
		front = new int[]{
			303,304,305,306,307,
			263,264,265,266,267,
			223,224,225,226,227,
			183,184,185,186,187,
			143,144,145,146,147

		};

		back = new int[]{
			283,284,285,286,287,
			243,244,245,246,247,
			203,204,205,206,207,
			163,164,165,166,167,
			123,124,125,126,127
		};

		right = new int[]{
			293,294,295,296,297,
			253,254,255,256,257,
			213,214,215,216,217,
			173,174,175,176,177,
			133,134,135,136,137

		};

		left = new int[]{
			313,314,315,316,317,
			273,274,275,276,277,
			233,234,235,236,237,
			193,194,195,196,197,
			153,154,155,156,157

		};

		for (int t = 360; t < 521; t++) {
			top.Add(t);

		}
		for (int b = 0; b < 80; b++) {
			bottom.Add(b);
		}
		for (int b2 = 521; b2 < 602; b2++) {
			bottom.Add(b2);
		}


	}

	void moveTopBottom ()
	{
		foreach (int t in top) {

			newSpheres [t].GetComponent<Renderer> ().material.color = Color.cyan;
		}

		foreach (int b in bottom) {
			newSpheres [b].GetComponent<Renderer> ().material.color = Color.black;
		}
	}
	void moveLeftRightWings ()
	{
		for(int l = 0; l < left.Length; l++)
		{
			newSpheres [left[l]].transform.localPosition = new Vector3( 
				newSpheres [left[l]].transform.localPosition.x - 5.0f,
				newSpheres [left[l]].transform.localPosition.y,
				newSpheres [left[l]].transform.localPosition.z - rightLeftPos);

			newSpheres [left[l]].GetComponent<Renderer> ().material.color = Color.magenta;
		}
		newSpheres [left[12]].GetComponent<Renderer> ().material.color = Color.white;



		for(int r = 0; r < right.Length; r++)
		{
			newSpheres [right[r]].transform.localPosition = new Vector3( 
				newSpheres [right[r]].transform.localPosition.x + 5.0f,
				newSpheres [right[r]].transform.localPosition.y,
				newSpheres [right[r]].transform.localPosition.z - rightLeftPos);

			newSpheres [right[r]].GetComponent<Renderer> ().material.color = Color.green;
		}
		newSpheres [right[12]].GetComponent<Renderer> ().material.color = Color.white;


	}



	void moveFrontRear()
	{

		for(int f = 0; f < front.Length; f++)
		{
			newSpheres [front[f]].transform.localPosition = new Vector3( 
				newSpheres [front[f]].transform.localPosition.x,
				newSpheres [front[f]].transform.localPosition.y,
				newSpheres [front[f]].transform.localPosition.z + 3.0f);

			newSpheres [front[f]].GetComponent<Renderer> ().material.color = Color.blue;
		}
		newSpheres [front[12]].GetComponent<Renderer> ().material.color = Color.white;



		for(int b = 0; b < back.Length; b++)
		{
			newSpheres [back[b]].transform.localPosition = new Vector3( 
				newSpheres [back[b]].transform.localPosition.x,
				newSpheres [back[b]].transform.localPosition.y,
				newSpheres [back[b]].transform.localPosition.z - 10.0f);

			newSpheres [back[b]].GetComponent<Renderer> ().material.color = Color.yellow;
		}

		newSpheres [back[12]].GetComponent<Renderer> ().material.color = Color.white;
	}



	////left and right
	void lrmove(Transform me, Vector3 target)
	{
		me.Translate(((target.x - me.position.x) / 100.0f), ((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(((target.x - me.position.x) / 100.0f), -((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(((target.x - me.position.x) / 100.0f), 0.0f, 0.0f);
		//me.Translate(0.0f, ((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(0.0f, ((target.y - me.position.y) / 100.0f), -((target.z - me.position.z) / 100.0f));
	}
	void lrmoveReverse(Transform me, Vector3 target)
	{
				me.Translate(- ((target.x - me.position.x) / 100.0f), - ((target.y - me.position.y) / 100.0f), 0.0f);
		//		me.Translate(-((target.x - me.position.x) / 100.0f), ((target.y - me.position.y) / 100.0f), 0.0f);
		//		me.Translate(-((target.x - me.position.x) / 100.0f), 0.0f, 0.0f);
		//		me.Translate(0.0f, -((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(0.0f, -((target.y - me.position.y) / 100.0f), ((target.z - me.position.z) / 100.0f));
	}



	////front and back
	void fbmove(Transform me, Vector3 target)
	{
		//me.Translate(((target.x - me.position.x) / 100.0f), ((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(((target.x - me.position.x) / 100.0f), -((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(((target.x - me.position.x) / 100.0f), 0.0f, 0.0f);
		//me.Translate(0.0f, ((target.y - me.position.y) / 100.0f), 0.0f);
		//me.Translate(-((target.x - me.position.x) / 100.0f), ((target.y - me.position.y) / 100.0f), 0.0f);

	}
	void fbmoveReverse(Transform me, Vector3 target)
	{
//		me.Translate(- ((target.x - me.position.x) / 100.0f), - ((target.y - me.position.y) / 100.0f), 0.0f);
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


	void Update()
	{


		UpdateMesh ();

		//ClearAll();
		//CreateMesh ();


		if (moveState) 
		{
			for (int i = 0; i < 25; i++) 
			{
				//fbmove (newSpheres [back [i]].transform, newSpheres [back [12]].transform.localPosition);
				//fbmove (newSpheres [front [i]].transform, newSpheres [front [12]].transform.localPosition);

				//lrmove (newSpheres [left [i]].transform, newSpheres [left [12]].transform.localPosition);
				//lrmove (newSpheres [right [i]].transform, newSpheres [right [12]].transform.localPosition);

				//newSpheres [left [i]].transform.Translate(0.01f, 0.0f, 0.0f);
				//newSpheres [right [i]].transform.Translate(-0.01f, 0.0f, 0.0f);
			}
		}
		if (reverseState) 
		{
			for (int i = 0; i < 25; i++) 
			{
				//fbmoveReverse (newSpheres [back [i]].transform, newSpheres [back [12]].transform.localPosition);
				//fbmoveReverse (newSpheres [front [i]].transform, newSpheres [front [12]].transform.localPosition);
				//lrmoveReverse (newSpheres [left [i]].transform, newSpheres [left [12]].transform.localPosition);
				//lrmoveReverse (newSpheres [right [i]].transform, newSpheres [right [12]].transform.localPosition);

				//newSpheres [left [i]].transform.Translate(-0.01f, 0.0f, 0.0f);
				//newSpheres [right [i]].transform.Translate(0.01f, 0.0f, 0.0f);

			}
		}


		if (spreadState) 
		{
			spreadCount += 1.0f * Time.deltaTime;
			if (spreadCount > 4.0f) {

				spreadCount = 0;

				moveState = false;
				reverseState = false;
				spreadState = false;
			}

		}

		if (Input.GetKeyDown ("space")) {

			spreadState = true;
			moveState = !moveState;

		}
		if (Input.GetKeyDown ("r")) {

			spreadState = true;
			reverseState = !reverseState;

		}


		if (Input.GetKeyDown (KeyCode.RightArrow)) {

			rightLeftPos += 1.0f;
			//updateLeftRightWings ();
			for(int i = 0; i < left.Length; i++)
			{
				newSpheres [left[i]].transform.localPosition = new Vector3( 
					newSpheres [left[i]].transform.localPosition.x,
					newSpheres [left[i]].transform.localPosition.y,
					newSpheres [left[i]].transform.localPosition.z + 1.0f);

				newSpheres [right[i]].transform.localPosition = new Vector3( 
					newSpheres [right[i]].transform.localPosition.x,
					newSpheres [right[i]].transform.localPosition.y,
					newSpheres [right[i]].transform.localPosition.z + 1.0f);
			}

		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			rightLeftPos -= 1.0f;

			for(int i = 0; i < left.Length; i++)
			{
				newSpheres [left[i]].transform.localPosition = new Vector3( 
					newSpheres [left[i]].transform.localPosition.x,
					newSpheres [left[i]].transform.localPosition.y,
					newSpheres [left[i]].transform.localPosition.z - 1.0f);

				newSpheres [right[i]].transform.localPosition = new Vector3( 
					newSpheres [right[i]].transform.localPosition.x,
					newSpheres [right[i]].transform.localPosition.y,
					newSpheres [right[i]].transform.localPosition.z - 1.0f);
			}

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


		for(int i = 0; i < vertices.Length; i++)
		{
			vertices [i] = newSpheres [i].transform.localPosition;
		}
	

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

		//CreateCollider();
		//CreateColorAndtexture ();

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

			//
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

		if (x < roundness) {
			inner.x = roundness;
		}
		else if (x > xSize - roundness) {
			inner.x = xSize - roundness;
		}
		if (y < roundness) {
			inner.y = roundness;
		}
		else if (y > ySize - roundness) {
			inner.y = ySize - roundness;
		}
		if (z < roundness) {
			inner.z = roundness;
		}
		else if (z > zSize - roundness) {
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
		meshRigidBody.isKinematic = true;
		meshRigidBody.useGravity = true;
	}
	private void CreateColorAndtexture() {

		meshRenderer = GetComponent<MeshRenderer>();

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
