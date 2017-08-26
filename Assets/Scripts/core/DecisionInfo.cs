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
		public string money;
		public string happiness;
		public string time;
		public string nextCard;
		public string cardsToAdd;
	}
}