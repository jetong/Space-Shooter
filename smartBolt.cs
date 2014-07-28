using UnityEngine;
using System.Collections;

public class smartBolt : MonoBehaviour
{
	private GameObject player;
	Vector3 targetPosition;
	public float speed;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		targetPosition = player.rigidbody.position - rigidbody.position;
		Quaternion targetRotation = Quaternion.LookRotation (targetPosition, Vector3.up);
		rigidbody.MoveRotation (targetRotation);
	}

	void FixedUpdate()
	{
		rigidbody.velocity = targetPosition * speed;
	}
}