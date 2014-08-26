using UnityEngine;
using System.Collections;

public class Done_DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public GameObject boltExplosion;
	public int scoreValue;
	public float enemyHealth;

	private float dmg;
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

	void OnTriggerEnter (Collider other)
	{
		//================Destroy by Enemy Bolt===================
		//Hit Player
		if ((tag == "Enemy Bolt" || tag == "Enemy Missile") && other.tag == "Player")
		{
			if(tag == "Enemy Bolt")
			{
				GameObject bolt = GameObject.FindGameObjectWithTag ("Enemy Bolt");
				dmg = bolt.GetComponent <damage>().dmg;
				Instantiate (boltExplosion, transform.position, transform.rotation);
			}

			if(tag == "Enemy Missile")
			{
				GameObject missile = GameObject.FindGameObjectWithTag ("Enemy Missile");
				dmg = missile.GetComponent <damage>().dmg;
				Instantiate (explosion, transform.position, transform.rotation);
			}
			Destroy (gameObject);

			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			player.GetComponent <Done_PlayerController>().playerHealth -= dmg;
			gameController.UpdateHealth();

			if(player.GetComponent <Done_PlayerController>().playerHealth <= 0)
			{
				Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
				gameController.GameOver();
				Destroy(other.gameObject);
			}
		}

		//Hit Asteroid
		if((tag == "Enemy Bolt" || tag == "Enemy Missile") && other.tag == "Asteroid")
		{
			if(tag == "Enemy Bolt")
			{
				GameObject bolt = GameObject.FindGameObjectWithTag ("Enemy Bolt");
				dmg = bolt.GetComponent <damage>().dmg;
				Instantiate (boltExplosion, transform.position, transform.rotation);
			}
			
			if(tag == "Enemy Missile")
			{
				GameObject missile = GameObject.FindGameObjectWithTag ("Enemy Missile");
				dmg = missile.GetComponent <damage>().dmg;
				Instantiate (explosion, transform.position, transform.rotation);
			}
			Destroy (gameObject);

			GameObject asteroid = GameObject.FindGameObjectWithTag ("Asteroid");
			asteroid.GetComponent <Done_DestroyByContact>().enemyHealth -= dmg;

			if(asteroid.GetComponent <Done_DestroyByContact>().enemyHealth <= 0)
			{
				Instantiate(explosion, other.transform.position, other.transform.rotation);
				Destroy(other.gameObject);
			}
		}

		//Hit Enemy
		if ((tag == "Enemy Bolt" || tag == "Enemy Missile") && other.tag == "Enemy") 
		{
			GameObject bolt = this.gameObject;
			Done_WeaponController weaponController = other.GetComponent<Done_WeaponController>();
		
			if (bolt == weaponController.shotRef1 || bolt == weaponController.shotRef2)
			{
				return;
			}

			dmg = bolt.GetComponent <damage>().dmg;

			other.GetComponent <Done_DestroyByContact>().enemyHealth -= dmg/2;

			Instantiate (boltExplosion, transform.position, transform.rotation);
			Destroy (gameObject);
			if(other.GetComponent <Done_DestroyByContact>().enemyHealth <= 0)
			{
				Instantiate(explosion, other.transform.position, other.transform.rotation);
				Destroy(other.gameObject);
			}
		}

		//Hit Boss
		if ((tag == "Enemy Bolt" || tag == "Enemy Missile") && other.tag == "Boss") 
		{
//			GameObject bolt = GameObject.FindGameObjectWithTag ("Enemy Bolt");
// Use this.gameObject reference rather than find tag because we need to reference this specific object
// rather than any object with "Enemy Bolt" tag.  This way we can trace who shot the enemy bolts.
			GameObject bolt = this.gameObject;
			Done_WeaponController weaponController = other.GetComponent<Done_WeaponController>();
			
			if (bolt == weaponController.shotRef1 || bolt == weaponController.shotRef2)
			{
				return;
			}

			dmg = this.GetComponent <damage>().dmg;

			other.GetComponent <Done_DestroyByContact>().enemyHealth -= dmg/2;
			
			Instantiate (boltExplosion, transform.position, transform.rotation);
			Destroy (gameObject);
			if(other.GetComponent <Done_DestroyByContact>().enemyHealth <= 0)
			{
				Instantiate(explosion, other.transform.position, other.transform.rotation);
				Destroy(other.gameObject);
				gameController.levelComplete = true;
			}
		}


		//================Destroy by Player Bolt===================
		//Hit Asteroid
		if (tag == "Asteroid" && other.tag == "Bolt") 
		{
			Instantiate (boltExplosion, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
			GameObject bolt = GameObject.FindGameObjectWithTag ("Bolt");
			dmg = bolt.GetComponent <damage>().dmg;
			enemyHealth -= dmg;
			if(enemyHealth <= 0)
			{
				Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
				gameController.AddScore (scoreValue);
			}
		}

		//Hit Enemy
		if ((tag == "Enemy" || tag == "Boss") && other.tag == "Bolt")
		{
			Instantiate (boltExplosion, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
			GameObject bolt = GameObject.FindGameObjectWithTag ("Bolt");
			dmg = bolt.GetComponent <damage>().dmg;
			enemyHealth -= dmg;
			if (enemyHealth <= 0) 
			{
				Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
				gameController.AddScore (scoreValue);

				if(tag == "Boss")
					gameController.levelComplete = true;
			}
		}

		//===================== Collisions =======================
		//Player collides with Asteroid or Enemy
		if ((tag == "Enemy" || tag == "Asteroid") && other.tag == "Player")
		{
			Instantiate (boltExplosion, transform.position, transform.rotation);
			enemyHealth -= 1;
			if(enemyHealth <= 0)
			{
				Instantiate (explosion, transform.position, transform.rotation);
				gameController.AddScore (scoreValue);
				Destroy (gameObject);
			}
			
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			player.GetComponent <Done_PlayerController>().playerHealth--;
			gameController.UpdateHealth();

			if(player.GetComponent <Done_PlayerController>().playerHealth <= 0)
			{
				Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
				gameController.GameOver();
				Destroy(other.gameObject);
			}
		}

		//Player collides with Boss
		if (tag == "Boss" && other.tag == "Player")
		{
			Instantiate (explosion, transform.position, transform.rotation);
			enemyHealth -= 1;

			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			player.GetComponent <Done_PlayerController>().playerHealth--;
			gameController.UpdateHealth();

			if(player.GetComponent <Done_PlayerController>().playerHealth <= 0)
			{
				Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
				gameController.GameOver();
				Destroy(other.gameObject);
			}

			if(enemyHealth <= 0)
			{
				Instantiate (explosion, transform.position, transform.rotation);
				gameController.AddScore (scoreValue);
				Destroy (gameObject);
				gameController.levelComplete = true;
			}
		}

		if(tag == "Powerup" && other.tag == "Player")
		{
			Debug.Log ("got powerup");
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			player.GetComponent <Done_PlayerController>().playerHealth+=3;
			gameController.UpdateHealth();

			player.GetComponent<Done_PlayerController>().powerup++;

			Destroy (gameObject);
		}
	}
}