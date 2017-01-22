using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	public static bool isNowPlaying = false;

	void Awake() {
		PlayStartMusic();
	}

	void PlayStartMusic () {
		if(!MusicPlayer.isNowPlaying) {
			// GameObject.DontDestroyOnLoad(gameObject);
			MusicPlayer.isNowPlaying = true;
		} else { DestroyObject(gameObject); }
	}



}
