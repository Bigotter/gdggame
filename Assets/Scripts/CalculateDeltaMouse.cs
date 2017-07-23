using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CalculateDeltaMouse : CalculateDelta
{
	private Vector3 startMousePos;
	private Vector3 diffInPix;
	private Vector3 difInWorld;
	private bool buttonDown;

	private Vector3 getInputPosition ()
	{
		return Input.mousePosition;
	}
		
	void CalculateDelta.Update() {
		Vector3 currentPos = getInputPosition();
		diffInPix = currentPos - startMousePos;

		Vector3 currentPosInWorld = Camera.main.ScreenToWorldPoint(currentPos);
		Vector3 startMouseInWorld = Camera.main.ScreenToWorldPoint(startMousePos);
		difInWorld = currentPosInWorld - startMouseInWorld;

		startMousePos = currentPos;
	}

	Vector3 CalculateDelta.CalculateDiffInPx ()
	{
		return diffInPix;
	}

	Vector3 CalculateDelta.CalculateDiffInWorld ()
	{
		return difInWorld;
	}

	bool CalculateDelta.IsMoving ()
	{
		if (!buttonDown && Input.GetMouseButtonDown (0)) {
			Debug.Log ("press");
			startMousePos = getInputPosition();
			buttonDown = true;
		}
		else if (buttonDown && Input.GetMouseButtonUp(0))
		{
			buttonDown = false;
		}

		return buttonDown;
	}
}

