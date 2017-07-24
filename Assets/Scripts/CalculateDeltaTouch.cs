using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CalculateDeltaTouch : CalculateDelta
{
	private bool isMoving;
	private Vector2 lastPosition;

	private Vector2 diffInPix;
	Vector3 diffInWorld;

	Vector3 GetDeltaInWorld (Vector2 lastPosition, Vector2 currentPosition)
	{
		Vector3 lastPositionInWorld = Camera.main.ScreenToWorldPoint (lastPosition);
		Vector3 currentPositionInWorld = Camera.main.ScreenToWorldPoint (currentPosition);

		return currentPositionInWorld - lastPositionInWorld;
	}

	void CalculateDelta.Update() 
	{
		if (Input.touchCount > 0) {

			Vector2 currentPosition = Input.GetTouch (0).position;

			diffInPix = currentPosition - lastPosition;

			diffInWorld = GetDeltaInWorld (lastPosition, currentPosition);

			lastPosition = currentPosition;
		}
	}

	Vector3 CalculateDelta.CalculateDiffInPx ()
	{
		return diffInPix;
	}

	Vector3 CalculateDelta.CalculateDiffInWorld ()
	{
		return diffInWorld;
	}

	bool CalculateDelta.IsMoving ()
	{
		if (Input.touchCount > 0) {
			if (!isMoving && Input.GetTouch (0).phase == TouchPhase.Moved) {
				lastPosition = Input.GetTouch (0).position;
				isMoving = true;
			} else if (isMoving && Input.GetTouch (0).phase != TouchPhase.Moved){
				isMoving = false;
			}
		}

		return isMoving;
	}
}

