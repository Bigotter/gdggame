using System;
using UnityEngine;

namespace core
{
    public class Card
    {
        public Texture2D Image
        {
            get { return _image; }
        }

        public Color Color
        {
            get { return _color; }
        }

        public int Money { get; private set; }
        public int Happiness { get; private set; }
        public string Description { get; private set; }

        private Texture2D _image;
        private Color _color;
        

        public Card(Texture2D image, Color color, string description)
        {
            _image = image;
            _color = color;
            Money = 10;
            Happiness = -5;
            Description = description;
        }
    }
}