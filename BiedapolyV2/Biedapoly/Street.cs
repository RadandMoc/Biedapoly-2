using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biedapoly
{
    [Serializable]
    public class Street : Territory, ILoseTile
    {
        int costToBuildHause;
        int costToBuildHotel;
        int costToBuy;
        int moneyFromPawn;
        int howMuchAreHauses = 0;
        Dictionary<int, int> costsForRent;

        public int CostToBuildHause { get => costToBuildHause; set => costToBuildHause = value; }
        public int CostToBuildHotel { get => costToBuildHotel; set => costToBuildHotel = value; }
        public int CostToBuy { get => costToBuy; set => costToBuy = value; }
        public int MoneyFromPawn { get => moneyFromPawn; set => moneyFromPawn = value; }
        public int HowMuchAreHauses { get => howMuchAreHauses; set => howMuchAreHauses = value; }
        public Dictionary<int, int> CostsForRent { get => costsForRent; set => costsForRent = value; }

        public Street() : base()
        { }
        public Street(int costToBuildHause, int costToBuildHotel, int costToBuy, int moneyFromPawn, Dictionary<int, int> costsForRent, string coordinates, Dictionary<string, string> bonusesOfTile, Player owner, int crosses, string name) : base(coordinates, bonusesOfTile, owner, crosses, name)
        {
            CostToBuildHause = costToBuildHause;
            CostToBuildHotel = costToBuildHotel;
            CostToBuy = costToBuy;
            MoneyFromPawn = moneyFromPawn;
            CostsForRent = costsForRent;
        }

        //PM buduje dom na ulicy
        /// <summary>
        /// Buduję dom na ulicy
        /// </summary>
        public void BuildHause()
        {
            string bonus = "";
            bool fake = Owner.PlayerBonuses.TryGetValue("BHA", out bonus);
            Owner.Money -= (int)Methods.ChangeValue(CostToBuildHause, bonus);
            HowMuchAreHauses++;
        }

        //PM Buduje hotel
        /// <summary>
        /// Buduję hotel na ulicy
        /// </summary>
        public void BuildHotel()
        {
            string bonus = "";
            bool fake = Owner.PlayerBonuses.TryGetValue("BHO", out bonus);
            Owner.Money -= (int)Methods.ChangeValue(CostToBuildHotel, bonus);
            HowMuchAreHauses++;
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
            if (this.HowMuchAreHauses == 0 && this.IsPawned == false)
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
