using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UITutorialScreen : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
		{
			SceneManager.LoadScene("Test_Ye");
		}
	}

}
