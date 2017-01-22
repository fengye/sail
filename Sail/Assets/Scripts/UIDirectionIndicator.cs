using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDirectionIndicator : MonoBehaviour
{
	public ShipSoundController shipController;
	void Start()
	{
		if (shipController == null)
		{
			Debug.LogError("No ship controller!");
		}
	}

	void Update()
	{
		Quaternion rotation = shipController.CachedTargetRotation;
		this.transform.rotation = rotation;
	}



}