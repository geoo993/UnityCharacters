using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {


	public static int health = 100;
	public GameObject player = null;
	public Slider healthbar;

	void Start () {

		//InvokeRepeating ("ReduceHealth", 1, 1);

	}

	void Update () {
	
	}

	void ReduceHealth(){

		health -= 5;
		healthbar.value = health;
		if (health <= 0) {

			player.GetComponent<Player> ().speed = 0.0f;
			player.GetComponent<Player> ().rotateSpeed = 0.0f;
		}

	}
}
