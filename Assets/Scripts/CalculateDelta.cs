using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface CalculateDelta
{
	Vector3 CalculateDiffInPx();
	Vector3 CalculateDiffInWorld();
	bool IsMoving();
	void Update();
}

