using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIHealthText : MonoBehaviour
{
	Slider uiSlider;

	void Start()
	{
		uiSlider = GetComponent<Slider>();
		uiSlider.value = 1.0f;

		GameScoreManager.instance.OnLifeUpdate += OnLifeUpdate;
	}

	void OnDestroy()
	{
		GameScoreManager.instance.OnLifeUpdate -= OnLifeUpdate;
	}

	void OnLifeUpdate(int life)
	{
		uiSlider.value = (float)life / GameScoreManager.instance.INIT_LIFE;
	}

}