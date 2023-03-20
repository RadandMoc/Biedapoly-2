using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biedapoly
{
    [Serializable]
    public class Territory
    {
        string coordinates;
        List<Player> playersOnTile;
        Dictionary<string, string> bonusesOfTile;
        Player owner;
        int crosses; //Dla: 0-brak 1-tylko w środek, 2-tylko na zewnątrz, 3- we wszystkie strony
        string name;
        bool isPawned = false;

        public string Coordinates { get => coordinates; set => coordinates = value; }
        public List<Player> PlayersOnTile { get => playersOnTile; set => playersOnTile = value; }
        public Dictionary<string, string> BonusesOfTile { get => bonusesOfTile; set => bonusesOfTile = value; }
        public Player Owner { get => owner; set => owner = value; }
        public int Crosses { get => crosses; set => crosses = value; }
        public string Name { get => name; set => name = value; }
        public bool IsPawned { get => isPawned; set => isPawned = value; }

        public Territory() { }

        public Territory(string coordinates, Dictionary<string, string> bonusesOfTile, Player owner, int crosses, string name) 
        {
            Coordinates= coordinates;
            BonusesOfTile= bonusesOfTile;
            Owner = owner;
            Crosses = crosses;
            Name = name;
        }

        public int HowMuchPlayersAreAtTile() => PlayersOnTile.Count;
    }
}
