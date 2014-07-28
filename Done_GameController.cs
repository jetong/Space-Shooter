using UnityEngine;
using System.Collections;

public class Done_GameController : MonoBehaviour
{
	public GameObject boss;
	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public bool levelComplete;
	public float fadeSceneTime;
		
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText nextLevelText;

	private bool gameOver;
	private bool restart;
	private int score;
	private int level;
	private float timer;

	void Start ()
	{
		GetComponent<SceneFadeInOut> ().StartScene ();
		gameOver = false;
		restart = false;
		levelComplete = false;
		restartText.text = "";
		nextLevelText.text = "";
		gameOverText.text = "";

		if (Application.loadedLevelName == "Scene1") {
			level = 1;
			score = 0;
		} 
		else 
		{
			level = PlayerPrefs.GetInt ("savedLevel");
			score = PlayerPrefs.GetInt ("savedScore");

			Done_PlayerController playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<Done_PlayerController>();
			playerController.playerHealth = PlayerPrefs.GetInt ("savedHealth");
		}

		UpdateScore ();
		StartCoroutine (NextLevelDisplay ());
		StartCoroutine (SpawnWaves ());
	}
	
	void Update ()
	{
		if(levelComplete)
		{
			GetComponent<SceneFadeInOut>().EndScene ();

			if(guiTexture.color.a >= 0.95f)
			{
				level++;
				PlayerPrefs.SetInt ("savedLevel", level);
				Application.LoadLevel (level);
			}
		}
		
		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel ("Scene1");
			}
		}
	}
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < hazardCount; i++)
			{
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

			Instantiate (boss, new Vector3 (0.0f, spawnValues.y, spawnValues.z), Quaternion.identity);

			yield return new WaitForSeconds (waveWait);
			yield return new WaitForSeconds (waveWait);
			yield return new WaitForSeconds (waveWait);

			if (gameOver)
			{
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
		}
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