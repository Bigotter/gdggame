using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using core;

public class ApplyTime : MonoBehaviour {

	private GameObject[] _crosses = new GameObject[28];

	void Start()
	{
		for (int i = 0; i < 28; i++) {
			_crosses[i] = transform.Find("cross_"+(i+1)).gameObject;
		}
	}

	void reset()
	{
		foreach (var cross in _crosses) {
			cross.SetActive (false);
		}
	}

	void Update()
	{
		var processCard = ProcessCard.Instance();
		int pos = 0;
		foreach (var cross in _crosses) {
			if (pos < processCard.Time) {
				cross.SetActive (true);
			} else {
				cross.SetActive (false);
			}
			pos++;
		}
	}
}
