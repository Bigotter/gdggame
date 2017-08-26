using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;

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

		private List<CardDefinition> _loadedCards = null;

	    public CardProvider()
	    {
			var faces = new[] {"Abdon", "Alicia", "Almo", "Bad_sponsor", "Fran", "gdgMalag", "lauraMorillo", "Vero"};
			var keys = new[] {"Abdon2.png", "Alicia.png", "Almo.png", "BadSponsor.png", "Fran.png", "Ruben.png", "Laura.png", "Vero.png"};
			int pos = 0;
	        foreach (var face in faces)
	        {
	            var name = "faces/" + face;
	            var textureToAdd = Resources.Load<Texture2D>(name) as Texture2D;
				_faces.Add(keys[pos],textureToAdd);
				pos++;
	        }
			LoadCards ();
	    }

	    public Card CurrentCard { get; set; }

	    public static CardProvider Instance()
	    {
	        return _instance ?? (_instance = new CardProvider());
	    }

	    public Card NextCard()
	    {
	        var color = RandomColor();

			var card = RandomCard ();
			var image = chooseFace(card.image);
			CurrentCard = new Card(image: image, color: color, description: card.text, rightText: card.right.text, leftText: card.left.text);
	        return CurrentCard;
	    }

		void LoadCards ()
		{
			// Path.Combine combines strings into a file path
			// Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
			string filePath = Path.Combine(Application.streamingAssetsPath, "cards.json");

			if(File.Exists(filePath))
			{
				// Read the json from the file into a string
				string dataAsJson = File.ReadAllText(filePath); 
				// Pass the json to JsonUtility, and tell it to create a GameData object from it
				var allCards = JsonUtility.FromJson<AllCards>(dataAsJson);
				_loadedCards = allCards.gameCards; 
			}
			else
			{
				Debug.LogError("Cannot load game data!");
			}

		}

	    private string RandomText()
	    {
	        var selected = Random.Range(0, _ask.Length);
	        return _ask[selected];
	    }

		private CardDefinition RandomCard ()
		{
			var selected = Random.Range(0, _loadedCards.Count);
			return _loadedCards[selected];
		}

		private Texture2D chooseFace (string image)
		{
			if (_faces.ContainsKey (image)) {
				return _faces [image];
			} else {
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