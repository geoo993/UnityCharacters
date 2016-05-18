////Unity 2.61
////~ M�rz 2010 - Moveairplane Build 49
//
////~ Flugscript
////~ Flying script
//
////~ Dieses Script ist ein kleiner Flugsimulator. Ihr k�nnt damit abheben, rumfliegen, und landen. H�ngt es an euer Flugzeug an.
////~ Das Flugverhalten ist nicht realistisch. Es ist Variablenbasiert. Vorsicht. Es kann dadurch zu Konflikten mit der Physik kommen. 
////~Herauszufinden wie sie zusammenh�ngen�berlasse ich euch. Ich �bernehme keinerlei Support f�r das Script. Viel Spass damit :)
//
////~ This script is a little flight simulator. You can take off, fly around, and land. Attach it to your airplane
////~ The flight behaviour is non realistic. It is variable based. Attention, this can lead to problems with the physics.
////~I keep it in your hands to find out how everyhing is connected. I don`t give any support for the script. Have fun :)
//
////~ Reiner "Tiles" Prokein
////http://.www.reinerstileset.de
//
//
////#################################################################################
////---------------------------------------------- Variablen || Variabales ---------------------------------------------------------------------------------------------------
////#################################################################################
//
////------------------------------------------------------------------- Blobshadow stuff --------------------------------------------------------------------------------
//static var airplaneangley: float=0.0;//Diese Variable geht zum Blob Shadow. || This goes to the blob shadow. 
////-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//
//static var gameover=0;//Flugzeugcode an und abschalten. Game Over || Turn on and off the airplane code. Game over
//var crashforce = 0; // Wenn GameOver brauchen wir eine Force um das Flugzeug crashen zu lassen||When gameover we need a force to let the airplane crash
//
////Rotation und Position unseres Flugzeugs. || Rotaton and position of our airplane
//static var rotationx=0;
//static var rotationy:float =0.0;
//static var rotationz:float =0.0;
//var positionx: float=0.0;
//static var positiony: float=0.0;
//var positionz: float=0.0;
//
//static var speed:float =0.0;// speed Variable gibt die Geschwindigkeit an || speed variable is the speed
//var uplift:float =0.0;// Auftrieb um abzuheben|| Uplift to take off
//var pseudogravitation:float=-0.3; //Abtrieb f�r Gel�ndefahrten|| downlift for driving through landscape
//
//var rightleftsoft:float=0.0; //Variable f�r sanften Kurvenflug || Variable for soft curveflight
//var rightleftsoftabs:float=0.0; // Positive rightleftsoft Variable 
//
//var divesalto:float =0.0; //Blockt den Vorw�rtssalto || blocks the forward salto
//var diveblocker:float=0.0; //Blockt seitlichenTaumelflug beim Sturzflug || blocks sideways stagger flight while dive
//
//
//function Update () {
//
////-------------------------------------------BlobShadow stuff------------------------------------------------------------------------------------------
//
//		//Schatten auf gleiche Drehung wie Flugzeug || shadow to the same angle than the airplane
//		airplaneangley= transform.eulerAngles.y; 
//		
////-------------------------------------------------------Game over ------------------------------------------------------------------------		
//
//	//Restart when gameover = 2
//	//Restart wenn gameover = 2
//	if ((gameover==2) && (Input.GetKey ("enter"))||(gameover==2) &&(Input.GetKey ("return")))	{
//	gameover=0;
//	GetComponent.<Rigidbody>().useGravity = false;
//	transform.position = Vector3(0, 1.67, 0);
//	transform.eulerAngles = Vector3(0,0,0);
//	}
//	
//	//Physik Zeug wenn gameover==1
//	// Physics stuff when gameover ==1
//	if (gameover==1)	{
//	GetComponent.<Rigidbody>().AddRelativeForce (0, 0, crashforce);
//	gameover=2;
//	}
//	
//		//Restart the level
//	if (Input.GetKey("f2")){
//		//We need to manually reset all important static values too. They are global, and keep their values across the levels
//		//Wir m�ssen alle wichtigen static Values zur�cksetzen. Sie sind global, und behalten ihre Werte �ber die Level bei
//		speed=0;
//		gameover=0;
//		GetComponent.<Rigidbody>().useGravity = false;
//		Application.LoadLevel(0);
//		} 
//	
////------------------------#####     Maincode fliegen || Maincode flying      #####--------------------------------------------------------
//	
//	// Mit gameover 0 ist der Code aktiv || Code is active when gameover = 0
//	if(gameover==0){
//
//		//Variablen auf Position und Rotation des Objekts einstellen || Turn variables to rotation and position of the object
//		rotationx=transform.eulerAngles.x; 
//		rotationy=transform.eulerAngles.y; 
//		rotationz=transform.eulerAngles.z; 
//		positionx=transform.position.x;
//		positiony=transform.position.y;
//		positionz=transform.position.z;
//
//	//------------------------- Drehungen des Flugzeugs / Rotations of the airplane -------------------------------------------------------------------------
//	
//	//Hoch Runter, limitiert auf eine Minimalgeschwindigkeit || Up Down, limited to a minimum speed
//	//Hoch Runter, limitiert auf eine Minimalgeschwindigkeit || Up Down, limited to a minimum speed
//		if ((Input.GetAxis("Vertical")<=0)&&((speed>595))) {
//			transform.Rotate((Input.GetAxis("Vertical")*Time.deltaTime*80),0,0); 
//		}
//		//Spezialfall sturzflug �ber 90 grad || Special case dive above 90 degrees
//		if ((Input.GetAxis("Vertical")>0)&&((speed>595))){
//			transform.Rotate((0.8-divesalto)*(Input.GetAxis("Vertical")*Time.deltaTime*80),0,0); 
//		}
//		
//	//Rechts Links am boden|| Left Right at the ground	
//		if (groundtrigger.triggered==1) transform.Rotate(0,Input.GetAxis("Horizontal")*Time.deltaTime*30,0,Space.World); 
//	//Rechts Links in der luft|| Left Right in the air
//		if (groundtrigger.triggered==0) transform.Rotate(0,Time.deltaTime*100*rightleftsoft,0,Space.World); 
//		
//	//Seitenneigung. Mal Minus 1 um in die richtige Richtung zu drehen || Tilt multiplied with minus 1 to go into the right direction	
//	//Seitenneigung nur in der Luft || Tilt just in the air
//		if ((groundtrigger.triggered==0)) transform.Rotate(0,0,Time.deltaTime*100*(1.0-rightleftsoftabs-diveblocker)*Input.GetAxis("Horizontal")*-1.0); 		
//
//	//------------------------------------------------ Neigungskalkulationen / Pitch and Tilt calculations ------------------------------------------
//		//variable rightleftsoft + rightleftsoftabs
//		
//		//Soft rotation calculation -----This prevents the airplaine to fly to the left while it is still tilted to the right
//		//Soft rotation Kalkulation  ----Dies verhindert dass das Flugzeug nach links fliegt w�hrend es noch nach Rechts geneigt ist
//		if ((Input.GetAxis ("Horizontal")<=0)&&(rotationz >0)&&(rotationz <90)) rightleftsoft=rotationz*2.2/100*-1;//linksrum || to the left
//		if ((Input.GetAxis ("Horizontal")>=0)&&(rotationz >270)) rightleftsoft=(7.92-rotationz*2.2/100);//rechtsrum ||to the right
//		
//		//rightleftsoft limitieren sodass der Switch nicht zu hart ist wenn man auf dem Kopf fliegt.
//		//Limit rightleftsoft so that the switch isn`t too hard when flying overhead
//		if (rightleftsoft>1) rightleftsoft =1;
//		if (rightleftsoft<-1) rightleftsoft =-1;
//		
//		//Pr�zisionsproblem rightleftsoft auf Null || Precisionproblem rightleftsoft to zero
//		if ((rightleftsoft>-0.01) && (rightleftsoft<0.01)) rightleftsoft=0;
//		
//		//Ergibt positive rightleftsoft Variable || Retreives positive rightleftsoft variable 
//		rightleftsoftabs=Mathf.Abs(rightleftsoft);
//		
//		// -------------------- Kalkulationen Salto vorw�rts abblocken / Calculations Block salto forward -----------------------------------------------------
//		
//		// Variable divesalto
//		//  sturzflug salto vorw�rts blocken || dive salto forward blocking
//		if (rotationx < 90) divesalto=rotationx/100.0;//Updown
//		if (rotationx > 90) divesalto=-0.2;//Updown
//		
//		//Variable diveblocker
//		// Blockt seitlichenTaumelflug beim Sturzflug || blocks sideways stagger flight while dive
//		if (rotationx <90) diveblocker=rotationx/200.0;
//		else diveblocker=0;
//
//		//----------------------------Alles zur�ckdrehen / everything rotate back ---------------------------------------------------------------------------------
//		
//		// Zur�ckrotieren wenn Key in die andere Richtung zeigt | rotateback when key wrong direction 
//		if ((rotationz <180)&&(Input.GetAxis ("Horizontal")>0)) transform.Rotate(0,0,rightleftsoft*Time.deltaTime*80);
//		if ((rotationz >180)&&(Input.GetAxis ("Horizontal")<0)) transform.Rotate(0,0,rightleftsoft*Time.deltaTime*80);
//
//		//Zur�ckdrehen Z Achse generell. Limitiert auf Horizontal Button ist nicht gedr�ckt
//		//Rotate back in z axis general, limited by no horizontal button pressed
//		if (!Input.GetButton ("Horizontal")){
//			if ((rotationz < 135)) transform.Rotate(0,0,rightleftsoftabs*Time.deltaTime*-100);
//			if ((rotationz > 225)) transform.Rotate(0,0,rightleftsoftabs*Time.deltaTime*100);
//			}
//			
//		//Zur�ckdrehen X Achse || Rotate back X axis
//		if ((!Input.GetButton ("Vertical"))&&(groundtrigger.triggered==0)){
//			if ((rotationx >0)&&(rotationx < 180)) transform.Rotate(Time.deltaTime*-50,0,0);
//			if ((rotationx >0)&&(rotationx > 180)) transform.Rotate(Time.deltaTime*50,0,0);
//			}
//			
//	//----------------------------Geschwindigkeit Fahren und Fliegen / Speed driving and flying ----------------------------------------------------------------
//		
//		// Geschwindigkeit || Speed
//		transform.Translate(0,0,speed/20*Time.deltaTime);
//		
//		//Wir brauchen ein minimales Geschwindigkeitslimit in der Luft. Wir limitieren wieder mit der groundtrigger.triggered Variable
//		//We need a minimum speed limit in the air. We limit again with the groundtrigger.triggered variable
//	
//		// Input Gas geben und abbremsen am Boden|| Input Accellerate and deccellerate at ground
//		if ((groundtrigger.triggered==1)&&(Input.GetButton("Fire1"))&&(speed<800)) speed+=Time.deltaTime*240;
//		if ((groundtrigger.triggered==1)&&(Input.GetButton("Fire2"))&&(speed>0)) speed-=Time.deltaTime*240;
//		
//				// Input Gas geben und abbremsen in der Luft|| Input Accellerate and deccellerate in the air
//		if ((groundtrigger.triggered==0)&&(Input.GetButton("Fire1"))&&(speed<800)) speed+=Time.deltaTime*240;
//		if ((groundtrigger.triggered==0)&&(Input.GetButton("Fire2"))&&(speed>600)) speed-=Time.deltaTime*240;
//		
//		if (speed<0) speed=0; //floatingpoint calculations makes a fix necessary so that speed cannot be below zero
//											//Floatingpoint Kalkulation macht ein Fix n�tig damit Speed nicht unter Null sein kann
//											
//		//Another speed floatingpoint fix:
//		if ((groundtrigger.triggered==0)&&(!Input.GetButton("Fire1"))&&(!Input.GetButton("Fire2"))&&(speed>695)&&(speed<705)) speed=700;
//		
//		//-----------------------------------------------------Auftrieb/Uplift  ----------------------------------------------------------------------
//		
//		//Wenn in der Luft weder Gasgeben noch Abbremsen gedr�ckt wird soll unser Flugzeug auf eine neutrale Geschwindigkeit gehen. 
//		//Mit dieser Geschwindigkeit soll es auch neutral in der H�he bleiben. Dr�ber soll es steigen, drunter soll es sinken. 
//		//Auf diesem Wege l�sst sich dann abheben und landen
//		//When we don`t accellerate or deccellerate we want to go to a neutral speed in the air. With this speed it has to stay at a neutral height. 
//		//Above this value the airplane has to climb, with a lower speed it has to  sink. That way we are able to takeoff and land then.
//		
//		//Neutrale Geschwindigkeit bei 700 || Neutral speed at 700
//		//Dieser Code stellt in der Luft die Geschwindigkeit auf 700 zur�ck wenn nicht gasgegeben oder abgebremst wird. Maximum 800, minimum 600
//		//This code resets the speed to 700 when there is no acceleration or deccelleration. Maximum 800, minimum 600
//		if((Input.GetButton("Fire1")==false)&&(Input.GetButton("Fire2")==false)&&(speed>595)&&(speed<700)) speed+=Time.deltaTime*240.0;
//		if((Input.GetButton("Fire1")==false)&&(Input.GetButton("Fire2")==false)&&(speed>595)&&(speed>700)) speed-=Time.deltaTime*240.0;
//		
//		//uplift - Auftrieb
//		transform.Translate(0,uplift*Time.deltaTime/10.0,0);
//				
//		//Uplift kalkulieren. Der Auftrieb || Calculate uplift
//		uplift = -700+speed;
//		
//	//Wir wollen am Boden keinen Abtrieb. Wenn die Uplift am Boden kleiner 0 ist, setzen wir sie auf 0. 
//	//We don`t want downlift. So when the uplift value lower zero we set it to 0
//		if ((groundtrigger.triggered==1)&&(uplift < 0)) uplift=0; 
//	
//	// ------------------------------- Rumfahren / driving around  ------------------------------------------------------------
//	
//	//Special case landschaft �berfahren. Wir ben�tigen sowas wie Pseudo Gravitation. 
//	//Und wir richten das Flugzeug am Untergrund aus
//	//Verwendet werden Sensorobjekte.
//	
//	//special case drive across landscape. We need something like pseudo gravitation. 
//	//And we align the airplane at the ground.
//	//We use sensorobjects for that
//	
//	//Fahren auf Grund geht bis Speed 600. F�nf Punkte Sicherheit || ground driving is up to Speed 600. Five points security
//	if (speed <595){
//	//Nase runter beim H�gelrunterfahren
//	if ((sensorfront.sensorfront ==0)&&(sensorrear.sensorrear ==1)) transform.Rotate(Time.deltaTime*20,0,0);
//	//Nase hoch beim H�gelrunterfahren
//	if ((sensorfront.sensorfront ==1)&&(sensorrear.sensorrear ==0)) transform.Rotate(Time.deltaTime*-20,0,0);
//	//Nase hoch beim H�gelanfahren
//	if (sensorfrontup.sensorfrontup ==1) transform.Rotate(Time.deltaTime*-20,0,0);
//	//Pseudoschwerkraft. K�nnte man na�rlich noch verfeinern indem man den Betrag erh�ht. 
//	if (groundtrigger.triggered==0) transform.Translate(0,pseudogravitation*Time.deltaTime/10.0,0);
//	}
//	
//	//--------------------------------------------- Debug ---------------------------------------------------------------------------------
//	//Mit Key 1 �ber den Buchstaben kann man sich in die H�he von 200 katapultieren. Mit Geschwindigkeit 700. Zum debuggen.
//	//Damit man nicht jedesmal starten muss ...
//	//With key 1 above the letters you can set the airplane to height 200. With speed 700. For debug reasons.
//	// so that you don`t have to takeoff all the time ...
//	
//	if (Input.GetKey ("1")) {
//	transform.position.y=200;
//	speed=700;
//		}
//		
//	}
//	//-------------------------------------------------- Limiting to playfield --------------------------------------------------------------------------
//	
//	//Here i wrap the airplane around the playfield so that you cannot fly under the landscape
//	//Hier wird das Flugzeug zur anderen Seite des Levels transportiert wenn es dem Rand zu nahe kommt. Damit ihr nicht unter die Landschaft fliegen k�nnt
//	if (transform.position.x >= 900.0) transform.position.x = 0;
//	else if (transform.position.x <= -900.0) transform.position.x = 900.0;
//	else if (transform.position.z >= 900.0) transform.position.z = 0;
//	else if (transform.position.z <= -900.0) transform.position.z = 900.0;
//	
//	//Hier wird die H�he limitiert || Here i limit the height
//	if (positiony > 1000) transform.position.y = 1000;
//
//}
//
//// ----------------------------------------------  Gameover activating ----------------------------------------------------------------
//
////Wenn unser Flugzeug in der Luft ist (groundtrigger.triggered=0), und mit was anderem als mit seinen R�dern (das groundtrigger Primitive) 
////den Boden ber�hrt wird das als Crash gewertet. 
////Wir m�ssen in diesem Fall die Geschwindigkeit zu einer Kraft machen damit wir das Flugzeug kollidieren lassen k�nnen
//
////When our airplane is in the air (groundtrigger.triggered=0), and touches the ground with something different than 
////the wheels (groundtrigger primitive) it will count as crash.
////We need to convert the speed into a force so that we can let our airplane collide
//
//function OnCollisionEnter(collision : Collision) {
//	if (groundtrigger.triggered==0) {
//	groundtrigger.triggered=1;
//	crashforce= speed*10000;
//	speed=0;
//	gameover=1;
//	GetComponent.<Rigidbody>().useGravity = true;
//	}
//}