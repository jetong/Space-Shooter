using UnityEngine;
using System.Collections;

public class Done_DestroyByBoundary : MonoBehaviour
{
	private Done_GameController gameController;
	
	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if (other.tag == "Boss" && !gameController.gameOver)
		{
			Destroy (other.gameObject);
			gameController.levelComplete = true;
		}

		Destroy (other.gameObject);
	}
}