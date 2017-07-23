using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CalculateDeltaTouch : CalculateDelta
{

	void CalculateDelta.Update() 
	{
		
	}

	Vector3 CalculateDelta.CalculateDiffInPx ()
	{
		return Vector3.zero;	
	}

	Vector3 CalculateDelta.CalculateDiffInWorld ()
	{
		return Vector3.zero;
	}

	bool CalculateDelta.IsMoving ()
	{
		return false;
	}
}

