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

		GameScoreManager.instance.OnCoinUpdate += OnCoinChange;
	}

	void OnDestroy()
	{
		GameScoreManager.instance.OnCoinUpdate -= OnCoinChange;
	}

	void OnCoinChange(int coin)
	{
		uiText.text = coin.ToString();
	}

}