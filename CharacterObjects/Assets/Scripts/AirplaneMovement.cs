using UnityEngine;
using System.Collections;

public class AirplaneMovement : MonoBehaviour {



	// Rotaton and position of our airplane
	float rotationX = 0.0f;
	//float rotationY = 0.0f;
	float rotationZ = 0.0f;

	//float positionX = 0.0f;
	float positionY =  0.0f;
	//float positionZ = 0.0f;
	
	float speed = 0.0f;  // speed variable is the speed
	float uplift = 0.0f;   // Uplift to take off
	float pseudoGravitation = -0.3f; //downlift for driving through landscape
	
	float rightLeftSoft = 0.0f; // Variable for soft curveflight
	float rightLeftSoftAbs = 0.0f; // Positive rightleftsoft Variable 
	
	float diveSalto  = 0.0f; //blocks the forward salto
	float diveBlocker = 0.0f; // blocks sideways stagger flight while dive
	float gravityAcceleration = 0.0f;

	public GameObject groundSens = null;
	public GameObject frontSens = null;
	public GameObject frontUpSens = null;
	public GameObject rearSens = null; 
	
	private int groundTrig = 0;
	private int sFront = 0;
	private int sFrontUp = 0;
	private int sRear = 0;



	void Start ()
	{
		Color planeColor = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
		Color planeRearColor = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));


	}
	void Update () {


		//PlaneMove ();
	}


	public void PlaneMove () 
	{

		//OnGUI ();

		groundTrig = groundSens.GetComponent<SensorGround> ().triggered; 
//		sFront = frontSens.GetComponent<SensorFront> ().sensorFront;
//		sFrontUp = frontUpSens.GetComponent<SensorFrontUp> ().sensorFrontUp;
//		sRear = rearSens.GetComponent<SensorRear> ().sensorRear;


		// Turn variables to rotation and position of the object
		rotationX = transform.eulerAngles.x; 
		//rotationY = this.transform.eulerAngles.y; 
		rotationZ = transform.eulerAngles.z; 
		//positionX = this.transform.position.x;
		positionY = transform.position.y;
		//positionZ = this.transform.position.z;



			//Rotations of the airplane
			//down
			if ((Input.GetAxis ("Vertical") <= 0 ) && ((speed > 595))) {
				transform.Rotate ((Input.GetAxis ("Vertical") * Time.deltaTime * 80), 0.0f, 0.0f); 
			}
			//up
			if ((Input.GetAxis ("Vertical") > 0 ) && ((speed > 595)) || transform.position.y > 1) 
			{
				transform.Rotate ((0.8f - diveSalto) * (Input.GetAxis ("Vertical") * Time.deltaTime * 40), 0.0f, 0.0f); 
			} 


		if (transform.position.y > 5) 
		{
			if (groundTrig == 1) {
				transform.Rotate (0.0f, Input.GetAxis ("Horizontal") * Time.deltaTime * 30, 0.0f, Space.World); 
			}
	
			if (groundTrig == 0) {
				transform.Rotate (0.0f, Time.deltaTime * 100 * rightLeftSoft, 0, Space.World); 
			}

			if (groundTrig == 0) {
				transform.Rotate (0.0f, 0.0f, Time.deltaTime * 100 * (1.0f - rightLeftSoftAbs - diveBlocker) * Input.GetAxis ("Horizontal") * -1.0f); 
			}



			//Pitch and Tilt calculations
			if ((Input.GetAxis ("Horizontal") <= 0 ) && (rotationZ > 0) && (rotationZ < 90)) {
				rightLeftSoft = rotationZ * 2.2f / 100 * -1;
			}
			if ((Input.GetAxis ("Horizontal") >= 0 ) && (rotationZ > 270)) {
				rightLeftSoft = (7.92f - rotationZ * 2.2f / 100);
			}

			if (rightLeftSoft > 1) {
				rightLeftSoft = 1;
			}
			if (rightLeftSoft < - 1) {
				rightLeftSoft = -1;
			}

			if ((rightLeftSoft > -0.01) && (rightLeftSoft < 0.01)) { 
				rightLeftSoft = 0;
			}
			rightLeftSoftAbs = Mathf.Abs (rightLeftSoft);


			//Calculations Block salto forward
			if (rotationX < 90) 
			{
				diveSalto = rotationX / 100.0f;

				if (rotationX > 90) {
					diveSalto = -0.2f;
				}
			}

			if (rotationX < 90) 
			{
				diveBlocker = rotationX / 200.0f;
			} else {
				diveBlocker = 0;
			}

			//Everything rotate back
			//rotate back when key wrong direction 
			if ((rotationZ < 180) && (Input.GetAxis ("Horizontal") > 0 )) 
			{
				transform.Rotate (0.0f, 0.0f, rightLeftSoft * Time.deltaTime * 80);
				//transform.Rotate( 0.0f, 0.0f, rightLeftSoft * 80);
			}
			if ((rotationZ > 180) && (Input.GetAxis ("Horizontal") < 0 )) 
			{
				transform.Rotate (0.0f, 0.0f, rightLeftSoft * Time.deltaTime * 80);
				//transform.Rotate( 0.0f, 0.0f, rightLeftSoft * 80);
			}


			//Rotate back in z axis general, limited by no horizontal button pressed
			if (!Input.GetButton ("Horizontal") ) 
			{
				if ((rotationZ < 135)) {
					transform.Rotate (0.0f, 0.0f, rightLeftSoftAbs * Time.deltaTime * -100);
				}
				if ((rotationZ > 225)) {
					transform.Rotate (0.0f, 0.0f, rightLeftSoftAbs * Time.deltaTime * 100);
				}
			}

			//Rotate back X axis
//		if ( ( !Input.GetButton ("Vertical") || !Input.GetButton ( "PS4_RightAnalogVertical" ) ) && ( groundTrig == 0) )
//		{
//			if ( ( rotationX > 0 ) && ( rotationX < 180 ) )
//			{
//				transform.Rotate( Time.deltaTime * -50.0f, 0.0f, 0.0f );
//			}
//			if ( ( rotationX > 0 ) && ( rotationX > 180 ) ) 
//			{
//				transform.Rotate( Time.deltaTime * 50.0f, 0.0f, 0.0f );
//			}
//		}
		} else {
			//transform.Rotate ( 0.0f, Time.deltaTime * 400 * Input.GetAxis("PS4_RightAnalogHorizontal"), 0.0f );
			transform.Rotate ( 0.0f, Time.deltaTime * 100 * Input.GetAxis ("Horizontal"), 0.0f ); 
		}

		////if ((groundTrigger==1)&&(Input.GetButton("Fire1"))&&(speed<800)) speed+=Time.deltaTime*240;
		////if ((groundTrigger==1)&&(Input.GetButton("Fire2"))&&(speed>0)) speed-=Time.deltaTime*240;

		//on ground
		if ( ( groundTrig == 1 ) && ( Input.GetKey("z") || Input.GetKey (KeyCode.Joystick1Button7) ) && (speed < 800 ))
		{
			speed += Time.deltaTime * 240;
		}
		if ( ( groundTrig == 1) && ( Input.GetKey("x") || Input.GetKey (KeyCode.Joystick1Button6 )) && ( speed > 0 ) )
		{
			speed -= Time.deltaTime * 240;
		}

		//in air 
		if ( ( groundTrig == 0 ) && ( Input.GetKey("z") || Input.GetKey (KeyCode.Joystick1Button7 ) ) && ( speed < 1500 ) ) 
		{
			speed += Time.deltaTime * 240;  //accelerate
		}
		if ( ( groundTrig == 0 ) && ( Input.GetKey("x") || Input.GetKey (KeyCode.Joystick1Button6 ) ) && ( speed > 0 ) ) 
		{
			speed -= Time.deltaTime * 240;   //deacclerate
		}
		

		if ( speed < 0 )
		{ 
			speed = 0; 
		}								
		
		//Another speed floatingpoint fix:
		if ( ( groundTrig == 0 ) && ( !Input.GetKey("z") || !Input.GetKey (KeyCode.Joystick1Button7) ) && ( !Input.GetKey("x") || !Input.GetKey (KeyCode.Joystick1Button6) ) && ( speed > 695 ) && ( speed < 705 ) )
		{
		 	speed = 700;
		}
		//Debug.Log ("ground trigger:  " + groundTrig + ", speed: "+ speed+"   " +transform.position.y);



		////Uplift 

		//if((Input.GetButton("Fire1")==false)&&(Input.GetButton("Fire2")==false)&&(speed>595)&&(speed<700)) 
		if( ( Input.GetKey("z") == false || Input.GetKey ( KeyCode.Joystick1Button7) == false ) && ( Input.GetKey("x") == false || Input.GetKey (KeyCode.Joystick1Button6) == false ) && ( speed > 595 ) && ( speed < 700 ) ) 
		{
			//speed += Time.deltaTime * 240;
		}
//		//if((Input.GetButton("Fire1")==false)&&(Input.GetButton("Fire2")==false)&&(speed>595)&&(speed>700)) 
		if( ( Input.GetKey("z") == false || Input.GetKey (KeyCode.Joystick1Button7) == false ) && ( Input.GetKey("x") == false || Input.GetKey (KeyCode.Joystick1Button6) == false ) && ( speed > 595 ) && ( speed > 700 ) ) 
		{
			speed -= Time.deltaTime * 240;
		}
		
		//uplift
		//transform.Translate( 0.0f, uplift * Time.deltaTime / 10, 0.0f);
		
		// Calculate uplift
//		uplift = -700 + speed;
//		
//		//We don`t want downlift. So when the uplift value lower zero we set it to 0
//		if ( (groundTrig == 1) && (uplift < 0) ) 
//		{
//			uplift = 0; 
//		}

		// ground driving is up to Speed 600. Five points security
		if ( speed < 595 )
		{
			
//			if ( ( sFront == 0 ) && (sRear == 1) ) 
//			{
//				transform.Rotate( Time.deltaTime * 20 , 0.0f, 0.0f);
//			}
//			
//			if ( ( sFront == 1 ) && ( sRear == 0 ) ) 
//			{	
//				transform.Rotate( Time.deltaTime * -20, 0.0f, 0.0f);
//			}
//			
//			if ( sFrontUp == 1 ) 
//			{
//				transform.Rotate( Time.deltaTime * -20, 0.0f, 0.0f);
//			}

			if ( groundTrig == 0 ) 
			{
				transform.Translate( 0.0f, pseudoGravitation * Time.deltaTime / 10, 0.0f);
			}
		}



		//Limiting to playfield 
		if ( transform.position.x >= 900.0f ) 
		{
			transform.position = new Vector3 ( 0.0f, transform.position.y, transform.position.z );
		}
		else if ( transform.position.x <= -900.0f ) 
		{	
			transform.position = new Vector3 ( 900.0f, transform.position.y, transform.position.z );
		}
		else if ( transform.position.z >= 900.0f ) 
		{
			transform.position = new Vector3 ( transform.position.x, transform.position.y, 0.0f );
		}
		else if ( transform.position.z <= -900.0f ) 
		{
			transform.position = new Vector3 ( transform.position.x, transform.position.y, 900.0f );
		}

		if ( positionY > 1000.0f )
		{
			transform.position = new Vector3 ( transform.position.x, 1000.0f, transform.position.z );
		}

		//Speed driving and flying
		if (rotationX < 70 && rotationX > 20 && groundTrig == 0) {

			gravityAcceleration += Time.deltaTime * rotationX;  //accelerate

		} else {

			gravityAcceleration -= Time.deltaTime * speed;

			if (gravityAcceleration < 0)
			{
				gravityAcceleration = 0;
			}
		}

		if (speed < 500  && transform.position.y < 20 )
		{
			GetComponent<Rigidbody>().useGravity = true;

		}
		if (speed > 500 && transform.position.y < 20) {
			GetComponent<Rigidbody> ().useGravity = false;
		} 

		speed = speed + (gravityAcceleration/5);
		transform.Translate( 0.0f, 0.0f, speed / 20.0f * Time.deltaTime );

		//Debug.Log ("this rotation X: "+this.transform.rotation.x+" rotation X: "+rotationX);
	}


}
