using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biedapoly
{
    [Serializable]
    public class Station : Territory, ILoseTile
    {
        int costToBuy;
        int moneyFromPawn;
        Dictionary<int, int> costsForRent;

        public int CostToBuy { get => costToBuy; set => costToBuy = value; }
        public int MoneyFromPawn { get => moneyFromPawn; set => moneyFromPawn = value; }
        public Dictionary<int, int> CostsForRent { get => costsForRent; set => costsForRent = value; }

        public Station(int costToBuy, int moneyFromPawn, Dictionary<int, int> costsForRent, string coordinates, Dictionary<string, string> bonusesOfTile, Player owner, int crosses, string name) : base(coordinates, bonusesOfTile, owner, crosses, name)
        {
            CostToBuy = costToBuy;
            MoneyFromPawn = moneyFromPawn;
            CostsForRent = costsForRent;
            Name = name;
        }

        public decimal HowMuchIWillGetFromPawn()
        {
            decimal returner = MoneyFromPawn;
            string bonus;
            bool bol = Owner.PlayerBonuses.TryGetValue("GPM", out bonus);
            if (bol)
                returner = Methods.ChangeValue(returner, bonus);
            return returner;
        }

        public void PawnTile()
        {
            if (this.IsPawned == false)
            {
                string bonus;
                bool cosiek = this.Owner.PlayerBonuses.TryGetValue("GPM", out bonus);
                if (!cosiek)
                    this.Owner.Money += this.MoneyFromPawn;
                else
                    this.Owner.Money += (int)Methods.ChangeValue(this.MoneyFromPawn, bonus);
                this.IsPawned = true;
            }
        }

        public int PropositionFromBank()
        {
            string bonus;
            bool bol = this.Owner.PlayerBonuses.TryGetValue("STB", out bonus);
            if (!bol)
            {
                if (this.IsPawned)
                    return (int)((CostToBuy - MoneyFromPawn) / 2);
                else
                    return (int)((CostToBuy - MoneyFromPawn) / 2) + MoneyFromPawn;
            }
            else
            {
                if (this.IsPawned)
                    return (int)Methods.ChangeValue((decimal)((CostToBuy - MoneyFromPawn) / 2), bonus);
                else
                    return (int)Methods.ChangeValue((decimal)((CostToBuy - MoneyFromPawn) / 2) + MoneyFromPawn, bonus);
            }
        }

        public void SellTileToBank(Map actualMap)
        {
            this.Owner.Money += this.PropositionFromBank();
            this.Owner = actualMap.Bank;
            this.IsPawned = false;
        }

        public void SoldTileToOtherPlayer(Player personBought, decimal price)
        {
            this.Owner.Money += (int)price;
            this.Owner = personBought;
        }

        public void ChangeTileOwner(Player newOwner) => this.Owner = newOwner;

    }
}
