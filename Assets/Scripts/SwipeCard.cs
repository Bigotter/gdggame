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
		MoveCard (buttonDown);

	}

	private void MoveCard(bool buttonDown) {
		if (buttonDown) {
			Vector3 currentPos = Input.mousePosition;
			Vector3 diff = currentPos - startMousePos;

			Vector3 currentPosInWorld = Camera.main.ScreenToWorldPoint(currentPos);
			Vector3 startMouseInWorld = Camera.main.ScreenToWorldPoint(startMousePos);
			Vector3 diffInWorld = currentPosInWorld - startMouseInWorld;

			Debug.Log ("down "+diff);


			Vector3 newPos = CalculatePosition (currentCard.transform.position, diffInWorld);


			Vector3 cardRotation = Vector3.zero;

			if (!IsCardXLimit (newPos)) 
			{
				cardRotation = calculateRotation (diff);
			}

			currentCard.transform.position = newPos;
			currentCard.transform.Rotate (cardRotation);

			startMousePos = currentPos;

		} else {
			startMousePos = Input.mousePosition;
		}
	}

	private bool IsCardXLimit(Vector3 newPos) 
	{
		return newPos.x == 3.5f || newPos.x == -3.5f;
	}

	private Vector3 CalculatePosition(Vector3 currentPosition, Vector3 diffInWorld)
	{
		Vector3 newPos = new Vector3 ();
		float newX = currentPosition.x + diffInWorld.x;

		if (newX > 3.5f) 
		{
			newX = 3.5f;
		}

		if (newX < -3.5f) 
		{
			newX = -3.5f;
		}

		newPos.x = newX;


		float newY = currentPosition.y + diffInWorld.y;
		if (newY > 1.0f) 
		{
			newY = 1.0f;
		}

		if (newY < -1.5f) 
		{
			newY = -1.5f;
		}

		newPos.y = newY;
		newPos.z = 0;

		return newPos;
	}

	private Vector3 calculateRotation(Vector3 diff) {
		float cardRotation = (diff.x * -90.0f) / Screen.width;
		Vector3 newRotation = new Vector3 ();
		newRotation.z = cardRotation;
		newRotation.x = 0;
		newRotation.y = 0;
		return newRotation;
	}
}
