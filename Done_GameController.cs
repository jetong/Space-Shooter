using UnityEngine;
using System.Collections;

public class Done_GameController : MonoBehaviour
{
	public GameObject boss;
	public GameObject[] hazards;
	public GameObject powerup;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public bool levelComplete;
	public float fadeSceneTime;
	public bool gameOver;

	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText nextLevelText;
	public GUIText healthText;
	
	private int score;
	private int level;
	private float timer;
	private int power;
	private Done_PlayerController playerController;

	void Start ()
	{
		GetComponent<SceneFadeInOut> ().StartScene ();
		gameOver = false;
		levelComplete = false;
		restartText.text = "";
		nextLevelText.text = "";
		gameOverText.text = "";
		healthText.text = "";
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<Done_PlayerController>();

		if (Application.loadedLevelName == "Scene1") {
			level = 1;
			score = 0;
		} 
		else 
		{
			level = PlayerPrefs.GetInt ("savedLevel");
			score = PlayerPrefs.GetInt ("savedScore");
			playerController.powerup = PlayerPrefs.GetInt("savedPowerup");
			playerController.playerHealth = PlayerPrefs.GetFloat ("savedHealth");
		}

		UpdateScore ();
		UpdateHealth ();
		StartCoroutine (NextLevelDisplay ());
		StartCoroutine (SpawnWaves ());
	}
	
	void Update ()
	{
		if(levelComplete && !gameOver) // don't advance level when player dies just as boss dies
		{
			GetComponent<SceneFadeInOut>().EndScene ();

			if(guiTexture.color.a >= 0.95f)
			{
				level++;
				PlayerPrefs.SetInt ("savedLevel", level);
				PlayerPrefs.SetFloat ("savedHealth", playerController.playerHealth);
				PlayerPrefs.SetInt("savedPowerup", playerController.powerup);
				Application.LoadLevel (level-1);
			}
		}

		if (gameOver)
		{
			restartText.text = "Press 'R' for Restart";
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel ("Scene1");
			}
		}
	}
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);

		for (int i = 0; i < hazardCount; i++)
		{
			GameObject hazard = hazards [Random.Range (0, hazards.Length)];
			Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (hazard, spawnPosition, spawnRotation);

			float rand = Random.value;
			if (rand > .85)
			{
				Vector3 spawnPowerupPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Instantiate (powerup, spawnPowerupPosition, Quaternion.identity);
			}

			yield return new WaitForSeconds (spawnWait);
		}


		yield return new WaitForSeconds (waveWait);

		Instantiate (boss, new Vector3 (0.0f, spawnValues.y, spawnValues.z), Quaternion.identity);
	}

	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		PlayerPrefs.SetInt ("savedScore", score);
		UpdateScore ();
	}
	
	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}

	public void UpdateHealth()
	{
		healthText.text = "Health: " + playerController.playerHealth;
	}

	IEnumerator NextLevelDisplay ()
	{
		nextLevelText.text = "Level: " + level;
		yield return new WaitForSeconds (3);
		nextLevelText.text = "";
	}

	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}