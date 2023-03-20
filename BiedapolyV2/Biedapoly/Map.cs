using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biedapoly
{
    [Serializable]
    public class Map
    {
        string mapName;
        Dictionary<string, Territory> dictionaryOfTerritories;
        Dictionary<int, int> defaultMoney;
        Dictionary<int, Card> prospect;
        Dictionary<int, Card> socialTreasury;
        Dictionary<int, int> moneyForPrison;
        Dictionary<Territory, int> alliance;
        Dictionary<int, int> moneyForStart;
        string prisonCoordinates;
        string prisonVisiterCoordinates;
        Dictionary<int, int> dictionaryOfRings; //klucz to numer pierścienia, a value to numer ostatniego pola po którym gracz może się poruszać
        Player bank;
        string startCoordinates;
        int maxNumOfPlayers;

        public string MapName { get => mapName; set => mapName = value; }
        public Dictionary<string, Territory> DictionaryOfTerritories { get => dictionaryOfTerritories; set => dictionaryOfTerritories = value; }
        public Dictionary<int, int> DefaultMoney { get => defaultMoney; set => defaultMoney = value; }
        public Dictionary<int, Card> Prospect { get => prospect; set => prospect = value; }
        public Dictionary<int, Card> SocialTreasury { get => socialTreasury; set => socialTreasury = value; }
        public Dictionary<int, int> MoneyForPrison { get => moneyForPrison; set => moneyForPrison = value; }
        public Dictionary<Territory, int> Alliance { get => alliance; set => alliance = value; }
        public Dictionary<int, int> MoneyForStart { get => moneyForStart; set => moneyForStart = value; }
        public string PrisonCoordinates { get => prisonCoordinates; set => prisonCoordinates = value; }
        public Dictionary<int, int> DictionaryOfRings { get => dictionaryOfRings; set => dictionaryOfRings = value; }
        public string PrisonVisiterCoordinates { get => prisonVisiterCoordinates; set => prisonVisiterCoordinates = value; }
        public Player Bank { get => bank; set => bank = value; }
        public string StartCoordinates { get => startCoordinates; set => startCoordinates = value; }
        public int MaxNumOfPlayers { get => maxNumOfPlayers; set => maxNumOfPlayers = value; }

        public Map() { }

        public Map(string mapName, Dictionary<string, Territory> dictionaryOfTerritories, Dictionary<int, int> defaultMoney, Dictionary<int, Card> prospect, Dictionary<int, Card> socialTreasury, Dictionary<int, int> moneyForPrison, Dictionary<Territory, int> alliance, Dictionary<int,int> moneyForStart, Dictionary<int, int> dictionaryOfRings, string prisonCoordinates, string prisonVisiterCoordinates, Player bank, string startCoordinates, int maxNumOfPlayers)
        {
            MapName = mapName;
            DictionaryOfTerritories = dictionaryOfTerritories;
            DefaultMoney = defaultMoney;
            Prospect = prospect;
            SocialTreasury = socialTreasury;
            MoneyForPrison = moneyForPrison;
            Alliance = alliance;
            MoneyForStart = moneyForStart;
            DictionaryOfRings = dictionaryOfRings;
            PrisonCoordinates = prisonCoordinates;
            PrisonVisiterCoordinates = prisonVisiterCoordinates;
            Bank = bank;
            StartCoordinates = startCoordinates;
            MaxNumOfPlayers = maxNumOfPlayers;
        }

        public override string ToString()
        {
            return this.MapName;
        }
    }
}
