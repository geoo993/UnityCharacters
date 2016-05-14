using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class DynamicCubeMesh : MonoBehaviour {

//	public Camera mainCamera;
//	private Ray ray;
//	private RaycastHit hit;
//	private GameObject hitObject = null;
//
	public GameObject sphere;
	private int currentSphereIndex = 0;
	private List <GameObject> newSpheres = new List<GameObject>();


	[Range(2,30)] public int xSize = 9;
	[Range(10,40)]public int ySize = 4;
	[Range(2,30)]public int zSize = 2;
	[Range(0,10)]public int roundness = 0;


	private MeshCollider meshCollider; 
	private MeshFilter meshFilter;
	private Renderer meshRenderer;
	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles; 

	private Vector3[] normals;
	private Vector2[] uv;
	private static int
	SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {
		triangles[i] = v00;
		triangles[i + 1] = triangles[i + 4] = v01;
		triangles[i + 2] = triangles[i + 3] = v10;
		triangles[i + 5] = v11;
		return i + 6;
	}


	private GameObject createSphere(Vector3 pos , List <GameObject> objectArr){

		GameObject a = (GameObject) Instantiate(sphere, pos, Quaternion.identity);
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

		for (int i = 0; i < vertices.Length; i++) {

			createSphere (vertices[i], newSpheres);

		}

	}

	void Update()
	{


		for(int i = 0; i < vertices.Length; i++)
		{
			vertices [i] = newSpheres [i].transform.localPosition;
		}


		CreateMesh ();

		///		if (Input.GetMouseButtonDown (0)) {
		//
		//			ray = mainCamera.ScreenPointToRay (Input.mousePosition);
		//
		//			if (Physics.Raycast (ray, out hit)) {
		//
		//
		//				for (int i = 0; i < newSpheres.Count; i++) {
		//
		//					if (hit.collider.gameObject == newSpheres [i]) {
		//						hitObject = hit.collider.gameObject;
		//
		//						//hitObject.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
		//
		//						//currentSphereIndex = mySpheres.IndexOf(hit.collider.gameObject);
		//						currentSphereIndex = i;
		//
		//
		//						Debug.Log ("index: "+currentSphereIndex+"   world pos: "+ newSpheres [i].transform.position +"   local pos: "+newSpheres [i].transform.localPosition+"  current vertexpoint: "+vertices[i]);
		//
		//
		//
		//					}
		//				}
		//
		//
		//			}
		//		}



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
		mesh.RecalculateBounds();
		mesh.Optimize();

		CreateCollider();
		CreateColorAndtexture ();




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
		
		meshCollider = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
		meshCollider.sharedMesh = mesh; // Give it your mesh here.
	}
	private void CreateColorAndtexture() {

		meshRenderer = GetComponent<MeshRenderer>();
		Material material = new Material (Shader.Find ("Standard"));
		material.color = Color.red;  //Color.Lerp(Color.white, ExtensionMethods.RandomColor(), Random.Range(0.0f, 1.0f));

		meshRenderer.material = material;


	}



}
