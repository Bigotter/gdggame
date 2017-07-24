using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCard : MonoBehaviour {

	public GameObject currentCard;

	#if UNITY_ANDROID || UNITY_IOS
	private CalculateDelta CalculateDelta = new CalculateDeltaTouch();
	#else
	private CalculateDelta CalculateDelta = new CalculateDeltaMouse();
	#endif


	void Update () 
	{
		if (CalculateDelta.IsMoving ()) {
			MoveCard ();
		}
	}

	private void MoveCard() {
		if (CalculateDelta.IsMoving()) {

			CalculateDelta.Update ();

			Vector3 diff = CalculateDelta.CalculateDiffInPx (); 

			Vector3 diffInWorld = CalculateDelta.CalculateDiffInWorld();

			Debug.Log ("moving: " +diff.x + " " + diffInWorld.x );

			Vector3 newPos = CalculatePosition (currentCard.transform.position, diffInWorld);


			Vector3 cardRotation = Vector3.zero;

			if (!IsCardXLimit (newPos)) 
			{
				cardRotation = calculateRotation (diff);
			}

			currentCard.transform.position = newPos;
			currentCard.transform.Rotate (cardRotation);

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
