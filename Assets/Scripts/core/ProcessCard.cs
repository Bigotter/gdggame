using UnityEngine;

namespace core
{
    public class ProcessCard
    {
        private static ProcessCard _instance;
        public const int MaxHappiness = 100;
        public const int MaxMoney = 3000;
        public const int InitHappiness = MaxHappiness;
        public const int InitMoney = 0;
        
        private int _money = 0;

        public ProcessCard()
        {
        }

        public int Happiness { get; private set; }

        public static ProcessCard Instance()
        {
            return _instance ?? (_instance = new ProcessCard());
        }

        public void Reset()
        {
            Happiness = InitHappiness;
            _money = InitMoney;
        }

        public void ApplyCardEffect(Card currentCard)
        {
            ApplyHappiness(currentCard.Happiness);
            ApplyMoney(currentCard.Money);
        }

        private void ApplyHappiness(int happinessMod)
        {
            Happiness += happinessMod;
        }

        private void ApplyMoney(int moneyModification)
        {
            _money += moneyModification;
        }
    }
}