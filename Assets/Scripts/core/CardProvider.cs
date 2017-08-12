using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private readonly List<Texture2D> _faces = new List<Texture2D>();

        public CardProvider()
        {
            var faces = new[] {"Abdon", "Alicia", "Almo", "Bad_sponsor", "Fran", "gdgMalag", "lauraMorillo", "Vero"};
            foreach (var face in faces)
            {
                var name = "faces/" + face;
                var textureToAdd = Resources.Load<Texture2D>(name) as Texture2D;
                _faces.Add(textureToAdd);
            }
        }

        public Card CurrentCard { get; set; }

        public static CardProvider Instance()
        {
            return _instance ?? (_instance = new CardProvider());
        }

        public Card NextCard()
        {
            var color = RandomColor();
            var image = RandomFace();
            CurrentCard = new Card(image: image, color: color, description: RandomText());
            return CurrentCard;
        }

        private string RandomText()
        {
            var selected = Random.Range(0, _ask.Length);
            return _ask[selected];
        }

        private Texture2D RandomFace()
        {
            var selected = Random.Range(0, _faces.Count);
            return _faces[selected];
        }

        private Color RandomColor()
        {
            var selected = Random.Range(0, _colors.Length);
            return _colors[selected];
        }
    }
}