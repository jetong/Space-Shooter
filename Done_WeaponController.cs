using UnityEngine;
using System.Collections;

public class Done_WeaponController : MonoBehaviour
{
	public GameObject shot;
	public Transform shotSpawn1;
	public Transform shotSpawn2;
	public float fireRate;
	public float delay;

	public GameObject shotRef1;
	public GameObject shotRef2;
	Done_GameController gameController;

	void Start ()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Done_GameController> ();
		InvokeRepeating ("Fire", delay, fireRate);
	}

	void Update()
	{
		// if game is over, stop firing
		if (gameController.gameOver)
		{
			CancelInvoke("Fire");
		}
	}

	void Fire ()
	{
		shotRef1 = Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation) as GameObject;
		shotRef2 = Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation) as GameObject;
		audio.Play();
	}
}