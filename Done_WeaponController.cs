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

	void Start ()
	{
		InvokeRepeating ("Fire", delay, fireRate);
	}

	void Fire ()
	{
		shotRef1 = Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation) as GameObject;
		shotRef2 = Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation) as GameObject;
		audio.Play();
	}
}