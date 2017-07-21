using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCard : MonoBehaviour {

	public GameObject currentCard;

	private bool buttonDown = false;
	private Vector3 startMousePos;
	private Vector3 startPos;

	private bool IsButtonDown() 
	{
		if (!buttonDown && Input.GetMouseButtonDown (0)) {
			return true;
		}
		else if (buttonDown && Input.GetMouseButtonUp(0))
		{
			return false;
		}

		return buttonDown;
	}


	void Update () 
	{
		buttonDown = IsButtonDown();
		if (buttonDown) {
			Vector3 currentPos = Input.mousePosition;
			Vector3 diff = currentPos - startMousePos;

			Vector3 currentPosInWorld = Camera.main.ScreenToWorldPoint(currentPos);
			Vector3 startMouseInWorld = Camera.main.ScreenToWorldPoint(startMousePos);
			Vector3 diffInWorld = currentPosInWorld - startMouseInWorld;

			Debug.Log ("down "+diff);


			Vector3 newPos = new Vector3 ();
			newPos.x = currentCard.transform.position.x + diffInWorld.x;
			newPos.y = currentCard.transform.position.y + diffInWorld.y;
			newPos.z = 0;

			float cardRotation = calculateRotation(diff);
			Vector3 newRotation = new Vector3 ();
			newRotation.z = cardRotation;
			newRotation.x = 0;
			newRotation.y = 0;


			currentCard.transform.position = newPos;
			currentCard.transform.Rotate (newRotation);

			startMousePos = currentPos;

		} else {
			startMousePos = Input.mousePosition;
		}
	}

	private float calculateRotation(Vector3 diff) {
		return (diff.x * -90.0f) / Screen.width;
	}
}
