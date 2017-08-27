using UnityEngine;

namespace core
{
    public class ProcessCard
    {
        private static ProcessCard _instance;
        public const int MaxHappiness = 100;
        public const int MaxMoney = 100;
		public const int MaxTime = 28;
        public const int InitHappiness = MaxHappiness;
		public const int InitTime = MaxTime;
        public const int InitMoney = 100;
        

        public ProcessCard()
        {
        }

        public int Money { get; private set; }
        public int Happiness { get; private set; }
		public int Time { get; private set; }

        public static ProcessCard Instance()
        {
            return _instance ?? (_instance = new ProcessCard());
        }

        public void Reset()
        {
            Happiness = InitHappiness;
            Money = InitMoney;
        }

		public void ApplyCardEffect(Card currentCard, bool isLeft)
        {
			int money = 0;
			int happiness = 0;
			int time = 0;
			if (isLeft) {
				money = currentCard.LeftMoney;
				happiness = currentCard.LeftHappiness;
				time = currentCard.LeftTime;
			} else {
				money = currentCard.RightMoney;
				happiness = currentCard.RightHappiness;
				time = currentCard.RightTime;
			}
				
			ApplyHappiness(happiness);
			ApplyMoney(money);
			ApplyTime (time);
        }

        private void ApplyHappiness(int happinessMod)
        {
            Happiness += happinessMod;
			if (Happiness > MaxHappiness) {
				Happiness = MaxHappiness;
			}
			if (Happiness < 0) {
				Happiness = 0;
			}
        }

        private void ApplyMoney(int moneyModification)
        {
            Money += moneyModification;
			if (Money > MaxMoney) {
				Money = MaxMoney;
			}
			if (Money < 0) {
				Money = 0;
			}
        }

		private void ApplyTime(int timeModification)
		{
			Time += timeModification;
			if (Time > MaxMoney) {
				Money = MaxMoney;
			}
			if (Money < 0) {
				Money = 0;
			}
		}

    }
}