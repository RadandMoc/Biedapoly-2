using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biedapoly
{
    [Serializable]
    public class Card
    {
        string cardName;
        Dictionary<string, string> listOfBonuses;
        string messageText;

        public string CardName { get => cardName; set => cardName = value; }
        public Dictionary<string, string> ListOfBonuses { get => listOfBonuses; set => listOfBonuses = value; }
        public string MessageText { get => messageText; set => messageText = value; }

        public Card(Dictionary<string, string> listOfBonuses, string messageText)
        {
            ListOfBonuses = listOfBonuses;
            MessageText = messageText;
        }
        public Card(string cardName, Dictionary<string, string> listOfBonuses, string messageText) : this(listOfBonuses,messageText)
        {
            CardName = cardName;
        }
    }
}
