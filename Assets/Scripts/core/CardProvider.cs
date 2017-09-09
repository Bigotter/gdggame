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

		private static int COLOR_GREEN = 0;
		private static int COLOR_YELLOW = 1;
		private static int COLOR_RED = 2;
		private static int COLOR_BLUE = 3;
		private static int COLOR_WHITE = 4;

	    private readonly Color[] _colors =
	    {
	        new Color(10 / 255.0f, 162 / 255.0f, 90 / 255.0f),
	        new Color(249 / 255.0f, 199 / 255.0f, 64 / 255.0f),
	        new Color(211 / 255.0f, 61 / 255.0f, 51 / 255.0f),
	        new Color(68 / 255.0f, 132 / 255.0f, 242 / 255.0f),
			new Color(1.0f,1.0f,1.0f)
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

		private Stack<CardDefinition> _postEventCards = new Stack<CardDefinition>();

		private int _currentLevel = 0; 
		private EVENT_STATE _currentEventState;


		Dictionary<String,CardDefinition> _allCards = new Dictionary<String, CardDefinition>();
		Dictionary<String,CardDefinition> _levelCards = new Dictionary<String, CardDefinition>();

		private CardDefinition _startLevelCard;

		private Dictionary<string,string> _names = new Dictionary<string,string>();

	    public CardProvider()
	    {
			string[] fileNames = new[] {
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
				"Vero",
				"Asistant",
				"Place2"
			};

			string[] names = new[] {
				"Abdon",
				"Alicia", 
				"Almo", 
				"Ana",
				"Andreu",
				"Sponsor", 
				"Eliezer",
				"Fran", 
				"Rubén",
				"Gem",
				"Flipper",
				"Laura",
				"Mario",
				"Nieves",
				"Paco",
				"Paola",
				"Pizza Guy",
				"Fancy Sponsor",
				"Vanessa",
				"Vero",
				"Fer",
				"Awesome Sponsor"
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
				"Vero.png",
				"Asistant.png",
				"Place2.png"
			};
			int pos = 0;
	        foreach (var face in fileNames)
	        {
				
	            var name = "faces/" + face;
	            var textureToAdd = Resources.Load<Texture2D>(name) as Texture2D;
				_faces.Add(keys[pos],textureToAdd);

				_names.Add (keys[pos], names [pos]);

				pos++;
	        }
			LoadCards ();
			LoadLevel ();
	    }

	    public Card CurrentCard { get; set; }
		public CardDefinition CurrentCardDefinition { get; set;}

	    public static CardProvider Instance()
	    {
	        return _instance ?? (_instance = new CardProvider());
	    }

	    public Card NextCard()
	    {
			Debug.Log ("num cards " + _preEventCards.Count);
			var card = obtainNextCard ();
			Debug.Log ("next card " + card.id);
			var color = RandomColor(card.type);
			var image = chooseFace(card.image);
			var name = chooseName (card.image);

			CurrentCardDefinition = card;
			CurrentCard = new Card(image: image, color: color, description: card.text, 
				rightText: card.right.text,rightMoney: card.right.money, rightHappiness:card.right.happiness, rightTime: card.right.time, 
				leftText: card.left.text, leftMoney: card.left.money, leftHappiness: card.left.happiness, 
				leftTime: card.left.time,name: name, type: card.type);
			
	        return CurrentCard;
	    }

		public void AddCards (bool isLeft)
		{
			if (CurrentCardDefinition == null) {
				return;		
			}

			var decision = (isLeft) ? CurrentCardDefinition.left : CurrentCardDefinition.right;
			if (decision.cardsToAdd.Count > 0) {
				decision.cardsToAdd.ForEach (id => {
					AddCard(id);
				});
				shuffle (_preEventCards);
				shuffle (_eventCards);
				shuffle (_postEventCards);
			}

			if (!string.IsNullOrEmpty (decision.nextCard)) {
				AddCard (decision.nextCard, true);
			}
		}

		void AddCard(String id) {
			AddCard(id, false);
		}

		void AddCard (string id, bool force)
		{
			Debug.Log ("add card " + id + " "+force);
			var card = getCard (id, force);
			if (card != null) {
				if (card.IsPreEvent ()) {
					_preEventCards.Push (card);
				} else if (card.IsEventCard ()) {
					_eventCards.Push (card);
				} else if (card.IsPostEventCard ()) {
					_postEventCards.Push (card);
				}
			}
		}

		void LoadCards ()
		{
			string dataAsJson = (Resources.Load("cards") as TextAsset).ToString ();
			var cards = JsonUtility.FromJson<AllCards>(dataAsJson);
			_allCards.Clear ();
			cards.gameCards.ForEach (card => _allCards.Add(card.id, card));

		}

		void LoadLevel() {
			_currentLevel ++;
			if (_currentLevel > 2) {
				_currentLevel = 1;
			}
			_currentEventState = EVENT_STATE.STATE_PRE_EVENT;

			_preEventCards.Clear ();
			_postEventCards.Clear ();
			_eventCards.Clear ();
			_levelCards.Clear ();

			foreach (var card in _allCards.Values) {
				if (card.level == _currentLevel) {
					if (card.IsInitial ()) {
						if (card.IsPreEvent ()) {
							_preEventCards.Push (card);
						} else if (card.IsEventCard ()) {
							_eventCards.Push (card);
						} else if (card.IsPostEventCard()) {
							_postEventCards.Push(card);
						}
					} else {
						_levelCards.Add (card.id, card);
					}

				  	if (card.IsStartEvent ()) {
						_startLevelCard = card;
					}
				}
			}
				
			_preEventCards = shuffle (_preEventCards);
			_eventCards = shuffle (_eventCards);
		}

		CardDefinition getCard (string id, bool force)
		{
			try {
				var cardToReturn = (force)? _allCards[id] : _levelCards [id];
				_levelCards.Remove(id);
				return cardToReturn;
			
			} catch (Exception exception) {
				Debug.Log ("key not found: " + id);
				return null;
			}

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
			if (_currentEventState == EVENT_STATE.STATE_PRE_EVENT) {
				if (_preEventCards.Count > 0 && ProcessCard.Instance ().Time < 28) {
					return _preEventCards.Pop ();
				} else {
					return StartEvent ();
				}
			} else if (_currentEventState == EVENT_STATE.STATE_EVENT) {
				if (_eventCards.Count > 0) {
					return _eventCards.Pop ();
				} else {
					return StartPostEvent ();
				}
			} else if (_currentEventState == EVENT_STATE.STATE_POST_EVENT) {
				if (_postEventCards.Count > 0) {
					return _postEventCards.Pop ();
				} else {
					ProcessCard.Instance ().Reset ();
					LoadLevel ();
					return obtainNextCard ();
				}
			} else {
				ProcessCard.Instance ().Reset ();
				LoadLevel ();
				return obtainNextCard ();
			}

		}

		CardDefinition StartEvent ()
		{
			_eventCards.Push (_startLevelCard);
			_currentEventState = EVENT_STATE.STATE_EVENT;

			CardDefinition startLevelCard = new CardDefinition();
			startLevelCard.type = CardDefinition.TYPE_EVENT_START_ANIM;
			startLevelCard.isInitial = "FALSE";
			startLevelCard.id = "-1";
			startLevelCard.image = "Almo";
			startLevelCard.text = "Start the Event. Are you ready?";
			var leftDecision = new DecisionInfo ();
			leftDecision.text = "Come On!";
			leftDecision.nextCard = "";
			leftDecision.cardsToAdd = new List<string>();
			startLevelCard.left = leftDecision; 
			var rightDecision = new DecisionInfo ();
			rightDecision.text = "Yeah!!";
			rightDecision.nextCard = "";
			rightDecision.cardsToAdd = new List<string>();
			startLevelCard.right = rightDecision; 

			return startLevelCard;

		}

		void AddPostEvents ()
		{
			foreach (var cardTuple in _levelCards) {
				var card = cardTuple.Value;
				if (card.IsPostEventCard()) {
					var currentHappiness = ProcessCard.Instance ().Happiness;
					var currentMoney = ProcessCard.Instance ().Money;

					if (card.right.happiness > 0 && currentHappiness <= card.right.happiness) {
						_postEventCards.Push (card);
					} else if (card.left.happiness > 0 && currentHappiness >= card.left.happiness) {
						_postEventCards.Push (card);
					} else if (card.right.money > 0 && currentMoney <= card.right.money) {
						_postEventCards.Push (card);
					} else if (card.left.money > 0 && currentMoney >= card.left.money) {
						_postEventCards.Push (card);
					}
				}
			}

			shuffle (_postEventCards);
		}

		CardDefinition StartPostEvent ()
		{
			AddPostEvents ();
			_currentEventState = EVENT_STATE.STATE_POST_EVENT;

			CardDefinition endLevelCard = new CardDefinition();
			endLevelCard.type = CardDefinition.TYPE_EVENT_END_ANIM;
			endLevelCard.isInitial = "FALSE";
			endLevelCard.id = "-1";
			endLevelCard.image = "Almo";
			endLevelCard.text = "The event ended. Yay!";
			var leftDecision = new DecisionInfo ();
			leftDecision.text = "Yeah!";
			leftDecision.nextCard = "";
			leftDecision.cardsToAdd = new List<string>();
			endLevelCard.left = leftDecision; 
			var rightDecision = new DecisionInfo ();
			rightDecision.text = "Uff!";
			rightDecision.nextCard = "";
			rightDecision.cardsToAdd = new List<string>();
			endLevelCard.right = rightDecision; 

			return endLevelCard;
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

		private string chooseName (string image)
		{
			if (_names.ContainsKey (image)) {
				return _names [image];
			} else {
				
				return "Someone";
			}
		}

			

		private Color RandomColor(string cardType)
	    {
			if (cardType == CardDefinition.TYPE_EVENT_START) {
				return _colors [COLOR_WHITE];
			}
				
	        var selected = Random.Range(0, _colors.Length);
	        return _colors[selected];
	    }
	}
}