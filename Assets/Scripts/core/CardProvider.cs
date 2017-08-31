using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;
using System.Linq;

namespace core
{
	public class CardProvider
	{
	    private static CardProvider _instance;

	    private readonly Color[] _colors =
	    {
	        new Color(10 / 255.0f, 162 / 255.0f, 90 / 255.0f),
	        new Color(249 / 255.0f, 199 / 255.0f, 64 / 255.0f),
	        new Color(211 / 255.0f, 61 / 255.0f, 51 / 255.0f),
	        new Color(68 / 255.0f, 132 / 255.0f, 242 / 255.0f)
	    };

	    private readonly String[] _ask =
	    {
	        "Let's build your GDG local community",
	        "I think this Rockstar speaker should be great for your event",
	        "This young promise might be good for this occassion",
	        "I would love to go to your event. I need to cover train tickets",
	        "Thanks for mentioning that! Your event sounds awesome",
	        "I will help with that",
	        "Thanks for giving me the opportunity to participate in your event",
	        "I need help with my train ticket",
	        "Remember to keep expenses organized"
	    };

		private readonly Dictionary<string,Texture2D> _faces = new Dictionary<string,Texture2D>();

		private Stack<CardDefinition> _preEventCards = new Stack<CardDefinition>();

		private Stack<CardDefinition> _eventCards = new Stack<CardDefinition>();

		private List<CardDefinition> _postEventCards = new List<CardDefinition>();

		int _currentLevel;

		List<CardDefinition> _allCards;

	    public CardProvider()
	    {
			var faces = new[] {
				"Abdon",
				"Alicia", 
				"Almo", 
				"anacidre",
				"Andreu2",
				"Bad_sponsor", 
				"eliezer",
				"Fran", 
				"gdgMalag",
				"gema_parreno",
				"jorge_barroso",
				"lauraMorillo",
				"MarioEzquerro",
				"nieves",
				"Paco",
				"paola",
				"pizza_guy",
				"place_organizer1",
				"Vanessa",
				"Vero"
			};
			var keys = new[] {
				"Abdon2.png", 
				"Alicia.png", 
				"Almo.png", 
				"Ana.png",
				"Andreu.png",
				"BadSponsor.png", 
				"Elizier.png",
				"Fran.png",
				"Ruben.png", 
				"Gema.png",
				"Rockstar.png",
				"Laura.png",
				"Mario Ezquerro.png",
				"nieves.png",
				"Paco.png",
				"Paola.png",
				"Pizzaguy.png",
				"FancyPlace.png",
				"Vanessa.png",
				"Vero.png"
			};
			int pos = 0;
	        foreach (var face in faces)
	        {
	            var name = "faces/" + face;
	            var textureToAdd = Resources.Load<Texture2D>(name) as Texture2D;
				_faces.Add(keys[pos],textureToAdd);
				pos++;
	        }
			LoadCards ();
			LoadLevel (0);
	    }

	    public Card CurrentCard { get; set; }
		public CardDefinition CurrentCardDefinition { get; set;}

	    public static CardProvider Instance()
	    {
	        return _instance ?? (_instance = new CardProvider());
	    }

	    public Card NextCard()
	    {
	        var color = RandomColor();

			var card = obtainNextCard ();
			var image = chooseFace(card.image);

			CurrentCardDefinition = card;
			CurrentCard = new Card(image: image, color: color, description: card.text, 
				rightText: card.right.text,rightMoney: card.right.money, rightHappiness:card.right.happiness, rightTime: card.right.time, 
				leftText: card.left.text, leftMoney: card.left.money, leftHappiness: card.left.happiness, leftTime: card.left.time);
			
	        return CurrentCard;
	    }

		void LoadCards ()
		{
			string dataAsJson = (Resources.Load("cards") as TextAsset).ToString ();
			var cards = JsonUtility.FromJson<AllCards>(dataAsJson);
			_allCards = cards.gameCards; 
		}

		void LoadLevel(int level) {
			_currentLevel = level;

			_preEventCards.Clear ();
			_postEventCards.Clear ();
			_eventCards.Clear ();

			foreach (var card in _allCards) {
				if (card.IsInitial()) {
					if (card.IsPreEvent ()) {
						_preEventCards.Push (card);
					} else if (card.IsEventCard ()) {
						_eventCards.Push (card);
					}
				}
			}

			_preEventCards = shuffle (_preEventCards);
			_eventCards = shuffle (_eventCards);

			Debug.Log ("num cards " + _preEventCards.Count);
		}

		private Stack<T> shuffle<T>(Stack<T> stack)
		{
			List<T> list = stack.ToList();
			stack.Clear ();
			while(list.Count > 0)
			{
				int randomIndex = Random.Range(0, list.Count);
				stack.Push(list[randomIndex]);
				list.RemoveAt(randomIndex);
			}

			return stack;
		}

	    private string RandomText()
	    {
	        var selected = Random.Range(0, _ask.Length);
	        return _ask[selected];
	    }

		private CardDefinition obtainNextCard ()
		{
			if (_preEventCards.Count > 0) {
				return _preEventCards.Pop ();
			} else {
				Debug.Log ("empty deck");
				LoadLevel (0);
				return obtainNextCard ();
			}
		}

		private Texture2D chooseFace (string image)
		{
			if (_faces.ContainsKey (image)) {
				return _faces [image];
			} else {
				Debug.Log ("not found: " + image);
				return _faces ["BadSponsor.png"];
			}
		}				

	    private Color RandomColor()
	    {
	        var selected = Random.Range(0, _colors.Length);
	        return _colors[selected];
	    }
	}
}