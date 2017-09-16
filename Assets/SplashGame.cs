using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashGame : MonoBehaviour {
	private static float TIME_SPLASH = 4.0f;
	private float _timeSplash;

	public string sceneName;

	void Start () {
		_timeSplash = 0; 
	}

	void Update () {
		_timeSplash += Time.deltaTime;

		if (_timeSplash > TIME_SPLASH) {
			SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		}
	}
}
