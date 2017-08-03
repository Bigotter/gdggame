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

        private Texture2D _image;
        private Color _color;

        public Card(Texture2D image, Color color)
        {
            _image = image;
            _color = color;
        }
    }
}