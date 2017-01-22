using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuSceneManager : MonoBehaviour {

	private bool onStart;
	private Vector3 iconMoveDistance;

	public GameObject menuIcon;

	// Use this for initialization
	void Start () {
		onStart = true;
		iconMoveDistance = new Vector3(0f, 1f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		MoveMenuIcon();
		QuitOrStartGame(onStart);
	}

	void MoveMenuIcon() {
		if( onStart && Input.GetKeyDown("down") ) {
			onStart = false;
			menuIcon.transform.position -= iconMoveDistance;
		}
		if( !onStart && Input.GetKeyDown("up") ) {
			onStart = true;
			menuIcon.transform.position += iconMoveDistance;
		}
	}

	void QuitOrStartGame(bool onStart) {
		if(Input.GetKeyDown("return") || 
		Input.GetKeyDown("space")	) {
			if (onStart) {
				SceneManager.LoadScene("Test_Ye");
			} else {
				SceneManager.LoadScene("How_To_Play");
			}
		}
	}

	public void LoadStartMenu() {
		SceneManager.LoadScene("Menu");
	}

}
