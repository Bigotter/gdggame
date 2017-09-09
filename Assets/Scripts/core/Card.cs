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

		public string RightText { get; private set; }
		public int RightMoney { get; private set; }
		public int RightHappiness { get; private set; }
		public int RightTime { get; private set; }

		public string LeftText { get; private set; }
		public int LeftMoney { get; private set; }
		public int LeftHappiness { get; private set; }
		public int LeftTime { get; private set; }

        public string Description { get; private set; }

		public string Name { get; private set;}

        private Texture2D _image;
        private Color _color;
        

		public Card(Texture2D image, Color color, string description, 
			string rightText, int rightMoney, int rightHappiness, int rightTime,
			string leftText, int leftMoney, int leftHappiness, int leftTime, 
			string name)
        {
            _image = image;
            _color = color;
            Description = description;

			RightText = rightText;
			RightMoney = rightMoney;
			RightHappiness = rightHappiness;
			RightTime = rightTime;

			LeftText = leftText;
			LeftMoney = leftMoney;
			LeftHappiness = leftHappiness;
			LeftTime = leftTime;
			Name = name;
        }
    }
}