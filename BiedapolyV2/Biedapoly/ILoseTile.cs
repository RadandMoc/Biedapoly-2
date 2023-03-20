using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biedapoly
{
    public interface ILoseTile
    {
        decimal HowMuchIWillGetFromPawn();
        void PawnTile();
        int PropositionFromBank();
        void SellTileToBank(Map actualMap);
        void SoldTileToOtherPlayer(Player personBought, decimal price);
        void ChangeTileOwner(Player newOwner);
    }
}
