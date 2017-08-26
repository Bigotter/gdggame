using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;

namespace core
{
	[System.Serializable]
	class CardDefinition
	{
		public string id; 
		public string text;
		public string type;
		public string image;
		public string level;
		public string isInitial;
		public DecisionInfo left;
		public DecisionInfo right;
	}

}