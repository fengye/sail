using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameOverScreen : MonoBehaviour
{
	public Text scoreText;
	public Text coinText;
	public Text distanceText;

	void Start()
	{
		Refresh();
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			RestartGame();
		}
	}

	void RestartGame()
	{
		Debug.Log("RESTART");
		SceneManager.LoadScene("Tutorial");
	}

	public void Refresh()
	{
		if (scoreText == null || coinText == null || distanceText == null)
		{
			Debug.LogError("Missing text field!");
		}

		scoreText.text = string.Format("SCORE: {0}", GameScoreManager.instance.Score);
		coinText.text = string.Format("COIN: {0}", GameScoreManager.instance.Coin);
		distanceText.text = string.Format("SAILED: {0} KM", (int)GameScoreManager.instance.TravelDistance);
	}
}