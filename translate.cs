using UnityEngine;
using System.Collections;

public class translate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (transform.forward * Time.deltaTime);
	}
}
