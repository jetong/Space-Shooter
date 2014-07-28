using UnityEngine;
using System.Collections;

public class smartMissile : MonoBehaviour {
	private GameObject player;
	Vector3 targetPosition;
	public float speed;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void FixedUpdate()
	{
		targetPosition = player.transform.position;
		Quaternion targetRotation = Quaternion.LookRotation (-targetPosition, Vector3.up);
		rigidbody.MoveRotation (targetRotation);
		targetPosition = player.rigidbody.position - this.rigidbody.position;
		rigidbody.velocity = targetPosition * speed;
	}
}