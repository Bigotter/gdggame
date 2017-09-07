using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;

namespace core
{
	[System.Serializable]
	public class DecisionInfo {
		public string text;
		public int money;
		public int happiness;
		public int time;
		public string nextCard;
		public  List<String> cardsToAdd;
	}
}