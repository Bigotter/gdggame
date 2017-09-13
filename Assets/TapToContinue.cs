using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TapToContinue : MonoBehaviour {
	public GameObject text;
	private bool load = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (text.activeSelf && !load) {			
			if (Input.anyKey || Input.touchCount > 0) {
				SceneManager.LoadScene("coregame", LoadSceneMode.Single);
			}
		}
	}
}
