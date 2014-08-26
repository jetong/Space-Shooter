using UnityEngine;
using System.Collections;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float playerHealth;
	public float speed;
	public float tilt;
	public Done_Boundary boundary;

	public GameObject shot;
	public Transform shotSpawnPrimary;
	public Transform shotSpawnSecondary1;
	public Transform shotSpawnSecondary2;
	public float fireRate;
	public int powerup;

	private float nextFire;
	private Done_GameController gameController;


	void Start()
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
	
	void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;

			switch(powerup)
			{
				case 0: FirePrimary();
						break;
				case 1: FireSecondary ();
						break;
				default: FirePrimary();
						FireSecondary();
						break;
			}

			audio.Play ();
		}
	}

	void FirePrimary()
	{
		Instantiate(shot, shotSpawnPrimary.position, shotSpawnPrimary.rotation);
	}

	void FireSecondary()
	{
		Instantiate(shot, shotSpawnSecondary1.position, shotSpawnSecondary1.rotation);
		Instantiate(shot, shotSpawnSecondary2.position, shotSpawnSecondary2.rotation);
	}

	void FixedUpdate ()
	{
		PlayerPrefs.SetFloat ("savedHealth", playerHealth);
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed;
		
		rigidbody.position = new Vector3
		(
			Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
		);
		
		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
	}
}
