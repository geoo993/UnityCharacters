using UnityEngine;
using System.Collections;

public class Airplane : MonoBehaviour {

	//~ Flying script

	//~ This script is a little flight simulator. You can take off, fly around, and land. Attach it to your airplane
	//~ The flight behaviour is non realistic. It is variable based. Attention, this can lead to problems with the physics.
	//~I keep it in your hands to find out how everyhing is connected. I don`t give any support for the script. Have fun :)

	//~ Reiner "Tiles" Prokein
	//http://.www.reinerstileset.de


	//#################################################################################
	//---------------------------------------------- Variablen || Variabales ---------------------------------------------------------------------------------------------------
	//#################################################################################

	//-------


	static int gameover = 0;  //Turn on and off the airplane code. Game over
	int crashforce = 0; //When gameover we need a force to let the airplane crash

//Rotation und Position unseres Flugzeugs. || Rotaton and position of our airplane
	static int rotationx = 0;
	static float rotationy = 0.0f;
	static float rotationz = 0.0f;
	float positionx = 0.0f;
	static float positiony = 0.0f;
	float positionz = 0.0f;

	static float speed = 0.0f;// speed variable is the speed
	float uplift = 0.0f;// Uplift to take off
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



	void Start() {

		GetComponent<Rigidbody> ().useGravity = false;
		//transform.position = new Vector3 (0, 1.67f, 0);
		transform.eulerAngles = new Vector3 (0, 0, 0);

//
//		groundtrigger = groundSens.GetComponent<SensorGround> ().triggered; 
//		sensorfront = frontSens.GetComponent<SensorFront> ().sensorFront;
//		sensorfrontup = frontUpSens.GetComponent<SensorFrontUp> ().sensorFrontUp;
//		sensorrear = rearSens.GetComponent<SensorRear> ().sensorRear;

	}


void Update () {


//------------------------#####     Maincode fliegen || Maincode flying      #####--------------------------------------------------------
	
		//Code is active when gameover = 0
		//if (gameover == 0) {

			// Turn variables to rotation and position of the object
			rotationx = (int)transform.eulerAngles.x; 
			rotationy = (int)transform.eulerAngles.y; 
			rotationz = (int)transform.eulerAngles.z; 
//			positionx = (int)transform.position.x;
//			positiony = (int)transform.position.y;
//			positionz = (int)transform.position.z;

			//------------------------- Drehungen des Flugzeugs / Rotations of the airplane -------------------------------------------------------------------------
	
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
			//Rechts Links in der luft|| Left Right in the air
			if (groundtrigger == 0)
				transform.Rotate (0, Time.deltaTime * 100 * rightleftsoft, 0, Space.World); 
		
			//Tilt multiplied with minus 1 to go into the right direction	
			if ((groundtrigger == 0))
				transform.Rotate (0, 0, Time.deltaTime * 100 * (1.0f - rightleftsoftabs - diveblocker) * Input.GetAxis ("Horizontal") * -1.0f); 		

			//------------------------------------------------ Neigungskalkulationen / Pitch and Tilt calculations ------------------------------------------
			//variable rightleftsoft + rightleftsoftabs
		
			//Soft rotation calculation -----This prevents the airplaine to fly to the left while it is still tilted to the right
			if ((Input.GetAxis ("Horizontal") <= 0) && (rotationz > 0) && (rotationz < 90))
				rightleftsoft = rotationz * 2.2f / 100 * -1; //to the left
			if ((Input.GetAxis ("Horizontal") >= 0) && (rotationz > 270))
				rightleftsoft = (7.92f - rotationz * 2.2f / 100);//to the right
		
			//rightleftsoft limitieren sodass der Switch nicht zu hart ist wenn man auf dem Kopf fliegt.
			//Limit rightleftsoft so that the switch isn`t too hard when flying overhead
			if (rightleftsoft > 1)
				rightleftsoft = 1;
			if (rightleftsoft < -1)
				rightleftsoft = -1;
		
			//Precisionproblem rightleftsoft to zero
			if ((rightleftsoft > -0.01f) && (rightleftsoft < 0.01f))
				rightleftsoft = 0;
		
			//Retreives positive rightleftsoft variable 
			rightleftsoftabs = Mathf.Abs (rightleftsoft);
		
			// -------------------- Kalkulationen Salto vorw�rts abblocken / Calculations Block salto forward -----------------------------------------------------
		
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

			//----------------------------Alles zur�ckdrehen / everything rotate back ---------------------------------------------------------------------------------
		
			//  rotateback when key wrong direction 
			if ((rotationz < 180) && (Input.GetAxis ("Horizontal") > 0))
				transform.Rotate (0, 0, rightleftsoft * Time.deltaTime * 80);
			if ((rotationz > 180) && (Input.GetAxis ("Horizontal") < 0))
				transform.Rotate (0, 0, rightleftsoft * Time.deltaTime * 80);

			//Rotate back in z axis general, limited by no horizontal button pressed
			if (!Input.GetButton ("Horizontal")) {
				if ((rotationz < 135))
					transform.Rotate (0, 0, rightleftsoftabs * Time.deltaTime * -100);
				if ((rotationz > 225))
					transform.Rotate (0, 0, rightleftsoftabs * Time.deltaTime * 100);
			}
			
			//Zur�ckdrehen X Achse || Rotate back X axis
			if ((!Input.GetButton ("Vertical")) && (groundtrigger == 0)) {
				if ((rotationx > 0) && (rotationx < 180))
					transform.Rotate (Time.deltaTime * -50, 0, 0);
				if ((rotationx > 0) && (rotationx > 180))
					transform.Rotate (Time.deltaTime * 50, 0, 0);
			}
			
			//----------------------------Geschwindigkeit Fahren und Fliegen / Speed driving and flying ----------------------------------------------------------------
		
			// Speed
			transform.Translate (0, 0, speed / 20 * Time.deltaTime);
		
			//Wir brauchen ein minimales Geschwindigkeitslimit in der Luft. Wir limitieren wieder mit der groundtrigger.triggered Variable
			//We need a minimum speed limit in the air. We limit again with the groundtrigger.triggered variable
	
			// Input Accellerate and deccellerate at ground
			if ((groundtrigger == 1) && (Input.GetKey ("z")) && (speed < 800))
				speed += Time.deltaTime * 240;
			if ((groundtrigger == 1) && (Input.GetKey ("x")) && (speed > 0))
				speed -= Time.deltaTime * 240;
		
			// Input Accellerate and deccellerate in the air
			if ((groundtrigger == 0) && (Input.GetKey ("z")) && (speed < 800))
				speed += Time.deltaTime * 240;
			if ((groundtrigger == 0) && (Input.GetKey ("x")) && (speed > 600))
				speed -= Time.deltaTime * 240;
		
			if (speed < 0)
				speed = 0; //floatingpoint calculations makes a fix necessary so that speed cannot be below zero
											
			//Another speed floatingpoint fix:
			if ((groundtrigger == 0) && (!Input.GetKey ("z")) && (!Input.GetKey ("x")) && (speed > 695) && (speed < 705))
				speed = 700;
		
			//-----------------------------------------------------Auftrieb/Uplift  ----------------------------------------------------------------------
		
			//Wenn in der Luft weder Gasgeben noch Abbremsen gedr�ckt wird soll unser Flugzeug auf eine neutrale Geschwindigkeit gehen. 
			//Mit dieser Geschwindigkeit soll es auch neutral in der H�he bleiben. Dr�ber soll es steigen, drunter soll es sinken. 
			//Auf diesem Wege l�sst sich dann abheben und landen
			//When we don`t accellerate or deccellerate we want to go to a neutral speed in the air. With this speed it has to stay at a neutral height. 
			//Above this value the airplane has to climb, with a lower speed it has to  sink. That way we are able to takeoff and land then.
		
			// Neutral speed at 700
			//This code resets the speed to 700 when there is no acceleration or deccelleration. Maximum 800, minimum 600
			//Fire1  = z ,  Fire2 = x
			if ((Input.GetKey ("z") == false) && (Input.GetKey ("x") == false) && (speed > 595) && (speed < 700))
				speed += Time.deltaTime * 240.0f;
			if ((Input.GetKey ("z") == false) && (Input.GetKey ("x") == false) && (speed > 595) && (speed > 700))
				speed -= Time.deltaTime * 240.0f;
		
			//uplift 
			transform.Translate (0, uplift * Time.deltaTime / 10.0f, 0);
				
			//Calculate uplift
			uplift = -700 + speed;
		
			//We don`t want downlift. So when the uplift value lower zero we set it to 0
			if ((groundtrigger == 1) && (uplift < 0))
				uplift = 0; 
	
			// ------------------------------- Rumfahren / driving around  ------------------------------------------------------------
	
			//Special case landschaft �berfahren. Wir ben�tigen sowas wie Pseudo Gravitation. 
			//Und wir richten das Flugzeug am Untergrund aus
			//Verwendet werden Sensorobjekte.
	
			//special case drive across landscape. We need something like pseudo gravitation. 
			//And we align the airplane at the ground.
			//We use sensorobjects for that
	
			//ground driving is up to Speed 600. Five points security
			if (speed < 595) {
				//Nase runter beim H�gelrunterfahren
				if ((sensorfront == 0) && (sensorrear == 1))
					transform.Rotate (Time.deltaTime * 20, 0, 0);
				//Nase hoch beim H�gelrunterfahren
				if ((sensorfront == 1) && (sensorrear == 0))
					transform.Rotate (Time.deltaTime * -20, 0, 0);
				//Nase hoch beim H�gelanfahren
				if (sensorfrontup == 1)
					transform.Rotate (Time.deltaTime * -20, 0, 0);
				//Pseudoschwerkraft. K�nnte man na�rlich noch verfeinern indem man den Betrag erh�ht. 
				if (groundtrigger == 0)
					transform.Translate (0, pseudogravitation * Time.deltaTime / 10.0f, 0);
			}
		
			//--------------------------------------------- Debug ---------------------------------------------------------------------------------
			
			//With key 1 above the letters you can set the airplane to height 200. With speed 700. For debug reasons.
			// so that you don`t have to takeoff all the time ...
		
			if (Input.GetKey ("1")) {
				transform.position = new Vector3 (transform.position.x, transform.position.y + 200.0f, transform.position.z);
				//transform.position.y = 200.0f;
				speed = 700;
			}
		
		//}
		//-------------------------------------------------- Limiting to playfield --------------------------------------------------------------------------
	
		//Here i wrap the airplane around the playfield so that you cannot fly under the landscape
		//Hier wird das Flugzeug zur anderen Seite des Levels transportiert wenn es dem Rand zu nahe kommt. Damit ihr nicht unter die Landschaft fliegen k�nnt


		//Limiting to playfield 
		if (transform.position.x >= 900.0f) {
			transform.position = new Vector3 (0.0f, transform.position.y, transform.position.z);
		} else if (transform.position.x <= -900.0f) {	
			transform.position = new Vector3 (900.0f, transform.position.y, transform.position.z);
		} else if (transform.position.z >= 900.0f) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0.0f);
		} else if (transform.position.z <= -900.0f) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 900.0f);
		}

		if (positiony > 1000.0f) {
			transform.position = new Vector3 (transform.position.x, 1000.0f, transform.position.z);
		}


	}

// ----------------------------------------------  Gameover activating ----------------------------------------------------------------


	//When our airplane is in the air (groundtrigger.triggered=0), and touches the ground with something different than 
	//the wheels (groundtrigger primitive) it will count as crash.
	//We need to convert the speed into a force so that we can let our airplane collide

	void OnCollisionEnter( Collision collision ) 
	{
		if (groundtrigger == 0) {
			groundtrigger = 1;
			crashforce = (int)speed * 10000;
			speed = 0;
			gameover = 1;
			GetComponent<Rigidbody>().useGravity = true;

			print (" gameover");
		}
	}
	

}