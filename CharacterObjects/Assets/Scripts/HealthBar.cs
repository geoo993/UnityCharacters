using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {


	public static int health = 100;
	public GameObject player = null;
	public Image sliderBar = null;
	public Image backgroundSliderBar = null;
	public Slider healthbar;

	private Color fullHealthColor = Color.green;
	private Color lowHealthColor = Color.red;


	void Start () {

		InvokeRepeating ("ReduceHealth", 1, 1);
		//backgroundSliderBar.color = Color.white;
	}

	void Update () {

		sliderBar.color = Color.Lerp (lowHealthColor, fullHealthColor, health / 100f);
	}

	void ReduceHealth(){

		health -= 5;
		healthbar.value = health;

		if (health <= 0) 
		{
			player.GetComponent<Player> ().speed = 0.0f;
			player.GetComponent<Player> ().rotateSpeed = 0.0f;
		}

	}
}
