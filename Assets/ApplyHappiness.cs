using System.Collections;
using System.Collections.Generic;
using core;
using UnityEngine;

public class ApplyHappiness : MonoBehaviour {

	void Start () {
		
	}
	
	void Update ()
	{
		var processCard = ProcessCard.Instance();
		var happiness = processCard.Happiness;
		
		var objectToCrop = transform.Find("mask");

		var totalWidht = objectToCrop.GetComponent<RectTransform>().sizeDelta.y;
		var size = totalWidht * happiness / ProcessCard.MaxHappiness;
		var moveCropper = totalWidht - size;
		objectToCrop.GetComponent<RectTransform>().localPosition = new Vector3(0f, -moveCropper,0f);
		
	}
}
