using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuPause : MonoBehaviour {

	public GameObject _menuPause = null;
	public GameObject _yesOption = null;
	public GameObject _noOption = null;
	public GameObject _topBar = null;

	bool isPause = false;

	void Update () {
		if( Input.GetKeyDown(KeyCode.Escape))
		{
			SetPause (!isPause);
		}

		if (isPause) {
			checkButtons ();
		} else {
			if (_topBar != null) {
				RaycastHit2D hitInformation = getTouched ();

				if (hitInformation.collider != null) {
					GameObject touchedObject = hitInformation.transform.gameObject;
					Debug.Log ("hit " + touchedObject.name);
					if (touchedObject == _topBar) {
						SetPause (true);
					}
				}
			}
		}
	}

	void checkButtons ()
	{
		RaycastHit2D hitInformation = getTouched();

		if (hitInformation.collider != null) {
			GameObject touchedObject = hitInformation.transform.gameObject;
			Debug.Log ("hit "+touchedObject.name);
			if (touchedObject == _yesOption) {
				Application.Quit ();
			} else if( touchedObject == _noOption) {
				SetPause (false);
			} 
		}
	}	

	void SetPause (bool pause)
	{
		isPause = !pause;
		_menuPause.SetActive (pause);
	}

	RaycastHit2D getTouched() {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
			var touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
			return Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
		}

		if (Input.GetMouseButtonDown (0)) {
			var touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
			return Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward, 10.0f);
		}

		return new RaycastHit2D ();
	}
}
