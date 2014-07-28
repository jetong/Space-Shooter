using UnityEngine;
using System.Collections;

public class BossPatrol : MonoBehaviour {
	public float currentSpeed;
	public float dodge;
	public float smoothing;
	public Done_Boundary boundary;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;
	
	private float targetManeuverX;
	private float targetManeuverZ;
	private Vector3 targetPosition;
	private Vector3 targetManeuver;

	void Start () {
//		rigidbody.velocity = transform.forward * currentSpeed;
		StartCoroutine (Patrol ());
	}

	IEnumerator Patrol ()
	{
		rigidbody.velocity = transform.forward * currentSpeed;
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while (true)
		{
			targetManeuver = new Vector3(Random.Range (boundary.xMin,boundary.xMax), 0.0f, Random.Range (boundary.zMin,boundary.zMax));
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetManeuver = new Vector3 (0,0,0);
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}
	}

	void FixedUpdate()
	{
		rigidbody.velocity = targetManeuver;
		
		rigidbody.position = new Vector3
		(
			Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
		);
	}


/*
	IEnumerator Patrol ()
	{
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while (true)
		{
			targetManeuverX = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x);
			targetManeuverZ = Random.Range (1, dodge) * -Mathf.Sign (transform.position.z);
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetManeuverX = 0;
			targetManeuverZ = 0;
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}
	}
	
	void FixedUpdate()
	{
		float newManeuverX = Mathf.MoveTowards (rigidbody.velocity.x, targetManeuverX, smoothing * Time.deltaTime);
		float newManeuverZ = Mathf.MoveTowards (rigidbody.velocity.x, targetManeuverZ, smoothing * Time.deltaTime);
		rigidbody.velocity = new Vector3(newManeuverX, 0.0f, newManeuverZ);

		rigidbody.position = new Vector3
			(
				Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
			);
	}
	*/
}
