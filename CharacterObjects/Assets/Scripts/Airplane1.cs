using UnityEngine;
using System.Collections;

public class Airplane1 : MonoBehaviour 
{

    //Rotaton and position of our airplane
	static float rotationx = 0;
	static float rotationy = 0.0f;
	static float rotationz = 0.0f;

	static float speed = 0.0f;// speed variable is the speed
	//float uplift = 0.0f;// Uplift to take off
	float pseudogravitation = -0.3f; // downlift for driving through landscape

	float rightleftsoft = 0.0f; // Variable for soft curveflight
	float rightleftsoftabs = 0.0f; // Positive rightleftsoft Variable 

	float divesalto  = 0.0f; //blocks the forward salto
	float diveblocker = 0.0f; //blocks sideways stagger flight while dive

	private int groundtrigger = 0;
	private int sensorfront = 0;
	private int sensorfrontup = 0;
	private int sensorrear = 0;

	public GameObject groundSens = null;
	public GameObject frontSens = null;
	public GameObject frontUpSens = null;
	public GameObject rearSens = null; 

	private Rigidbody rigid; 

	private float rotateSpeed = 80.0f;
	private float playerSpeed = 50.0f;

	float power = 0.005f;
	float friction = 0.95f;
	bool right = false;
	bool left = false;
	private float fuel = 20;

	public bool carState = true;

	void Start() {

		rigid = GetComponent<Rigidbody> ();
		rigid.useGravity = false;
		rigid.isKinematic = true;

		groundtrigger = groundSens.GetComponent<SensorGround> ().triggered; 
		sensorfront = frontSens.GetComponent<SensorFront> ().sensorFront;
		sensorfrontup = frontUpSens.GetComponent<SensorFrontUp> ().sensorFrontUp;
		sensorrear = rearSens.GetComponent<SensorRear> ().sensorRear;

	}





	void CarMovement ()
	{
		rigid.useGravity = true;
		rigid.isKinematic = false;

		if(right){
			speed += power;
			fuel -= power;
		}
		if(left){
			speed -= power;
			fuel -= power;
		}


		if( Input.GetKeyDown (KeyCode.UpArrow)){
			right = true;
		}
		if( Input.GetKeyUp (KeyCode.UpArrow)){
			right = false;
		}
		if( Input.GetKeyDown (KeyCode.DownArrow) ){
			left = true;
		}
		if( Input.GetKeyUp (KeyCode.DownArrow)){
			left = false;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {

			if (fuel > 0) {
				this.transform.Rotate ( new Vector3( 0, rotateSpeed * Time.deltaTime, 0));
			}
		}
		else if (Input.GetKey (KeyCode.LeftArrow)) {

			if (fuel > 0) {
				this.transform.Rotate (new Vector3 (0, - rotateSpeed * Time.deltaTime, 0));
			}
		}

		if (fuel < 0)
		{
			speed = 0;
		} 

		speed *= friction;
		this.transform.Translate(Vector3.forward * speed);
	}

	void Update () 
	{


		if (carState) 
		{
			CarMovement ();
		}else{
			rigid.useGravity = false;

			// Turn variables to rotation and position of the object
			rotationx = (int)transform.eulerAngles.x; 
			rotationy = (int)transform.eulerAngles.y; 
			rotationz = (int)transform.eulerAngles.z; 

			//------------------------- Rotations of the airplane -------------------------------------------------------------------------
	
			//Up Down, limited to a minimum speed
			if ((Input.GetAxis ("Vertical") <= 0) && ((speed > 595))) {
				transform.Rotate ((Input.GetAxis ("Vertical") * Time.deltaTime * 80), 0, 0); 
			}
			//Special case dive above 90 degrees
			if ((Input.GetAxis ("Vertical") > 0) && ((speed > 595))) {
				transform.Rotate ((0.8f - divesalto) * (Input.GetAxis ("Vertical") * Time.deltaTime * 80), 0, 0); 
			}
		
			//Left Right at the ground	
			if (groundtrigger == 1)
				transform.Rotate (0, Input.GetAxis ("Horizontal") * Time.deltaTime * 30, 0, Space.World); 
			// Left Right in the air
			if (groundtrigger == 0)
				transform.Rotate (0, Time.deltaTime * 100 * rightleftsoft, 0, Space.World); 
		
			//Tilt multiplied with minus 1 to go into the right direction	
			if ((groundtrigger == 0))
				transform.Rotate (0, 0, Time.deltaTime * 100 * (1.0f - rightleftsoftabs - diveblocker) * Input.GetAxis ("Horizontal") * -1.0f); 		
			
			//------------------------------------------------ Pitch and Tilt calculations ------------------------------------------
			//variable rightleftsoft + rightleftsoftabs
		
//			//Soft rotation calculation -----This prevents the airplaine to fly to the left while it is still tilted to the right
			if ((Input.GetAxis ("Horizontal") <= 0) && (rotationz > 0) && (rotationz < 90))
				rightleftsoft = rotationz * 2.2f / 100 * -1; //to the left
			if ((Input.GetAxis ("Horizontal") >= 0) && (rotationz > 270))
				rightleftsoft = (7.92f - rotationz * 2.2f / 100);//to the right
		
			//Limit rightleftsoft so that the switch isn`t too hard when flying overhead
			if (rightleftsoft > 1)
				rightleftsoft = 1;
			if (rightleftsoft < -1)
				rightleftsoft = -1;
		
			//Precision problem rightleftsoft to zero
			if ((rightleftsoft > -0.01f) && (rightleftsoft < 0.01f))
				rightleftsoft = 0;
		
			//Retreives positive rightleftsoft variable 
			rightleftsoftabs = Mathf.Abs (rightleftsoft);
		
			// -------------------- Calculations Block salto forward -----------------------------------------------------
		
			// Variable divesalto
			//   dive salto forward blocking
			if (rotationx < 90)
				divesalto = rotationx / 100.0f;//Updown
			if (rotationx > 90)
				divesalto = -0.2f;//Updown
		
			//Variable diveblocker
			// blocks sideways stagger flight while dive
			if (rotationx < 90)
				diveblocker = rotationx / 200.0f;
			else
				diveblocker = 0;

			//---------------------------- everything rotate back ---------------------------------------------------------------------------------
		
			//  rotateback when key wrong direction 
			if ((rotationz < 180) && (Input.GetAxis ("Horizontal") > 0))
				transform.Rotate (0, 0, rightleftsoft * Time.deltaTime * 80);
			if ((rotationz > 180) && (Input.GetAxis ("Horizontal") < 0))
				transform.Rotate (0, 0, rightleftsoft * Time.deltaTime * 80);

			//Rotate back in z axis general, limited by no horizontal button pressed
			if (!Input.GetButton ("Horizontal")) 
			{
				if ((rotationz < 135))
					transform.Rotate (0, 0, rightleftsoftabs * Time.deltaTime * -100);
				if ((rotationz > 225))
					transform.Rotate (0, 0, rightleftsoftabs * Time.deltaTime * 100);
			}
			
			//Rotate back X axis
			if ((!Input.GetButton ("Vertical")) && (groundtrigger == 0)) {
				if ((rotationx > 0) && (rotationx < 180))
					transform.Rotate (Time.deltaTime * -50, 0, 0);
				if ((rotationx > 0) && (rotationx > 180))
					transform.Rotate (Time.deltaTime * 50, 0, 0);
			}
			
			//----------------------------Speed driving and flying ----------------------------------------------------------------
			// Speed
			transform.Translate (0, 0, speed / 20 * Time.deltaTime);
			//We need a minimum speed limit in the air. We limit again with the groundtrigger.triggered variable
	
			// Input Accellerate and deccellerate at ground
			if ((groundtrigger == 1) && (Input.GetKey ("z")) && (speed < 800))
				speed += Time.deltaTime * 240;
			if ((groundtrigger == 1) && (Input.GetKey ("x")) && (speed > 0))
				speed -= Time.deltaTime * 240;
		
			// Input Accellerate and deccellerate in the air
			if ((groundtrigger == 0) && (Input.GetKey ("z")) && (speed < 800))
				speed += Time.deltaTime * 240;
			if ((groundtrigger == 0) && (Input.GetKey ("x")) && (speed > 0))//(speed > 600))
				speed -= Time.deltaTime * 240;
		
			if (speed < 0)
				speed = 0; //floatingpoint calculations makes a fix necessary so that speed cannot be below zero
											
			//Another speed floatingpoint fix:
			if ((groundtrigger == 0) && (!Input.GetKey ("z")) && (!Input.GetKey ("x")) && (speed > 695) && (speed < 705))
				speed = 700;
		
			//----------------------------------------------------- Uplift  ----------------------------------------------------------------------
		
			//When we don`t accellerate or deccellerate we want to go to a neutral speed in the air. With this speed it has to stay at a neutral height. 
			//Above this value the airplane has to climb, with a lower speed it has to  sink. That way we are able to takeoff and land then.
		
			// Neutral speed at 700
			//This code resets the speed to 700 when there is no acceleration or deccelleration. Maximum 800, minimum 600

			if ((Input.GetKey ("z") == false) && (Input.GetKey ("x") == false) && (speed > 595) && (speed < 700))
				speed += Time.deltaTime * 240.0f;
			if ((Input.GetKey ("z") == false) && (Input.GetKey ("x") == false) && (speed > 595) && (speed > 700))
				speed -= Time.deltaTime * 240.0f;

//			// ------------------------------- driving around  ------------------------------------------------------------
		}
			


	}


	void OnGUI() {
		
		GUI.contentColor = Color.red; 
		GUI.Label(new Rect(2000, 10, 200, 20), ("Plane Speed: "+ speed).ToString());
		GUI.Label(new Rect(2000, 30, 200, 20), ("Plane Y: "+ this.transform.position.y).ToString());
		GUI.Label(new Rect(2000, 50, 200, 20), ("Plane Z: "+ this.transform.position.z).ToString());
		GUI.Label(new Rect(2000, 70, 200, 20), ("Plane X: "+ this.transform.rotation.x).ToString());
		GUI.Label(new Rect(2000, 90, 200, 20), ("Plane Rotation X: "+ this.transform.localRotation.x).ToString());
		GUI.Label(new Rect(2000, 110, 200, 20), ("Plane Rotation Y: "+ this.transform.localRotation.y).ToString());
		GUI.Label(new Rect(2000, 130, 200, 20), ("Plane Rotation Z: "+ this.transform.localRotation.z).ToString());
		GUI.Label(new Rect(2000, 150, 200, 20), ("Ground Trigger: "+ groundtrigger).ToString());
		GUI.Label(new Rect(2000, 170, 200, 20), ("Rear Trigger: "+ sensorrear).ToString());
		GUI.Label(new Rect(2000, 190, 200, 20), ("front sensor Trigger: "+ sensorfront).ToString());
		GUI.Label(new Rect(2000, 210, 200, 20), ("front up sensor: "+ sensorfrontup).ToString()); 
		GUI.Label(new Rect(2000, 230, 200, 20), ("rightleftsoftabs: "+ rightleftsoftabs).ToString());
		GUI.Label(new Rect(2000, 250, 200, 20), ("Pseudo gravitation: "+ pseudogravitation).ToString());
	}


}