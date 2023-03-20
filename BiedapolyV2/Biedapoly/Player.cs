using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biedapoly
{
    [Serializable]
    public enum Color { red, green, blue, white, grey }

    [Serializable]
    public class Player : PrePlayer
    {
        int money;
        int id = 0;
        Heroes playerHero;
        Dictionary<string, string> playerBonuses;
        Color playerColor;
        bool wasChangeOfLane = false;
        string coordinates;

        public int Money { get => money; set => money = value; }
        public int Id { get => id; set => id = value; }
        public Heroes PlayerHero { get => playerHero; set => playerHero = value; }
        public Dictionary<string, string> PlayerBonuses { get => playerBonuses; set => playerBonuses = value; }
        public Color PlayerColor { get => playerColor; set => playerColor = value; }
        public bool WasChangeOfLane { get => wasChangeOfLane; set => wasChangeOfLane = value; }
        public string Coordinates { get => coordinates; set => coordinates = value; }

        private Player(string Nick) : base(Nick)
        {
            Id++;
            PlayerBonuses = new Dictionary<string, string>();
        }

        public Player()
        { }

        public Player(int money, Color playerColor, string Name, string coordinates) : this(Name)
        {
            Money = money;
            PlayerColor = playerColor;
            Coordinates = coordinates;
        }

        public Player(int money, Heroes playerHero, Color playerColor, string Name, string coordinates) : this(Name)
        {
            Money = money;
            PlayerHero = playerHero;
            PlayerColor = playerColor;
            Coordinates= coordinates;
        }

        //Poniższą metoe wykorzystywać, kiedy gracz zmieni pas ruchu
        public void ChangingLane()
        {
            WasChangeOfLane = true;
        }

        public string MoneyToString() => $"Money: \n{this.Money}";

        public void CrossStart(Game game)
        {
            Methods.AddMoneyForStart(game);
        }

        /*public override string ToString()
        {
            return base.ToString() +" "+ Id.ToString();
        }*/
    }
}
