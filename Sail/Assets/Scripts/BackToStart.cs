﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BackToStart : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("return") ||
			Input.GetKeyDown("space")) { SceneManager.LoadScene("StartMenu"); }
	}
}
