using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIScoreText : MonoBehaviour
{
	Text uiText;

	void Start()
	{
		uiText = GetComponent<Text>();
		uiText.text = "0";

		ShipSoundController ship = GameObject.FindObjectOfType<ShipSoundController>();
		ship.OnCoinChange += OnCoinChange;
	}

	void OnDestroy()
	{
		ShipSoundController ship = GameObject.FindObjectOfType<ShipSoundController>();
		ship.OnCoinChange -=  OnCoinChange;

	}

	void OnCoinChange(int coin)
	{
		uiText.text = coin.ToString();
	}

}