using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDistanceText : MonoBehaviour
{
	Text uiText;

	void Start()
	{
		uiText = GetComponent<Text>();
		uiText.text = "0";

		GameScoreManager.instance.OnTravelDistanceUpdate += OnTravelDistanceChange;
	}

	void OnDestroy()
	{
		GameScoreManager.instance.OnTravelDistanceUpdate -= OnTravelDistanceChange;
	}

	void OnTravelDistanceChange(float distance)
	{
		uiText.text = ((int)distance).ToString();
	}

}