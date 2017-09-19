using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;

namespace core
{
	[System.Serializable]
	public class CardDefinition
	{
		public const string TYPE_MASTER = "Master";
		public const string TYPE_SPEAKER = "Speaker";
		public const string TYPE_PLACE = "Place";
		public const string TYPE_MEDIA = "Media";
		public const string TYPE_EVENT_START = "Event_Start";
		public const string TYPE_EVENT_START_ANIM= "Event_Start_Anim";
		public const string TYPE_EVENT_END_ANIM= "Event_End_Anim";
		public const string TYPE_EVENT_PLACE= "Event_Place";
		public const string TYPE_EVENT_SPEAKER = "Event_Speaker";
		public const string TYPE_EVENT_ATTENDANTS = "Event_Attendants";
		public const string TYPE_EVENT_MEDIA = "Event_Media ";
		public const string TYPE_EVENT_END = "Event_End";
		public const string TYPE_END_MASTER = "End_Master";
		public const string TYPE_LEVEL = "Level";
		public const string TYPE_EVENT_LEVEL_ANIM = "Level_Anim";


		public string id; 
		public string text;
		public string type;
		public string image;
		public int level;
		public string isInitial;
		public DecisionInfo left;
		public DecisionInfo right;

		public bool IsPreEvent ()
		{
			return type.Equals (TYPE_MEDIA)
			|| type.Equals (TYPE_PLACE)
			|| type.Equals (TYPE_SPEAKER)
			|| type.Equals (TYPE_MASTER);
		}

		public bool IsEventCard ()
		{
			return type.Equals (TYPE_EVENT_PLACE)
			|| type.Equals (TYPE_EVENT_SPEAKER)
			|| type.Equals (TYPE_EVENT_ATTENDANTS)
			|| type.Equals (TYPE_EVENT_MEDIA);
		}

		public bool IsPostEventCard ()
		{
			return type.Equals (TYPE_END_MASTER);
		}

		public bool IsStartEvent ()
		{
			return type.Equals(TYPE_EVENT_START);
		}

		public bool IsInitial() {
			return isInitial.Trim().Equals ("TRUE");
		}
	}

}