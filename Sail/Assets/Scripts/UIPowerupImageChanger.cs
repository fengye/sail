using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIPowerupImageChanger : MonoBehaviour
{
	Image uiImage;

	public Sprite spriteBoost;
	public Sprite spriteSlow;
	public Sprite spriteFever;
	public Sprite spriteCannon;

	void Start()
	{
		GameScoreManager.instance.OnPowerupUpdate += OnPowerupChange;

		uiImage = GetComponent<Image>();
		uiImage.enabled = false;
	}

	void OnDestroy()
	{
		GameScoreManager.instance.OnPowerupUpdate -= OnPowerupChange;
	}

	void OnPowerupChange(PowerupMode powerup)
	{
		if (powerup == PowerupMode.NONE)
		{
			uiImage.enabled = false;
		}
		else
		{
			uiImage.enabled = true;

			if (powerup == PowerupMode.BOOST)
			{
				uiImage.sprite = spriteBoost;
			}

			if (powerup == PowerupMode.SLOW)
			{
				uiImage.sprite = spriteSlow;
			}

			if (powerup == PowerupMode.CANNON)
			{
				uiImage.sprite = spriteCannon;
			}

			if (powerup == PowerupMode.FEVER)
			{
				uiImage.sprite = spriteFever;
			}

		}
		// uiImage.
	}
}
