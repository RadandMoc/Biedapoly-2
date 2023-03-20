using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Biedapoly
{
    [Serializable]
    public class Offer
    {
        PrePlayer thisPlayerMakeAnOffer; //Gracz oferujący
        PrePlayer thisPlayerGetAnOffer; //Gracz któremu przedstawiona jest oferta
        int howMuchMoneyOffer; //Ile pieniędzy proponuje gracz oferujący. kwoty mogą być ujemne, wtedy oferujący otrzymuje pieniądze.
        string escapeFromPrisonOfferted; //string z kartami wyjść z więzienia jakie oferuje (w takim samym formacie jak są zapisywane => więcej w klasie methods)
        string escapeFromPrisonWanted; //string z kartami wyjść z więzienia jakie chce otrzymać (w takim samym formacie jak są zapisywane => więcej w klasie methods)
        List<string> fieldOfferted; //lista kart oferowanych
        List<string> fieldWanted; //lista kart porządanych

        public PrePlayer ThisPlayerMakeAnOffer { get => thisPlayerMakeAnOffer; set => thisPlayerMakeAnOffer = value; }
        public PrePlayer ThisPlayerGetAnOffer { get => thisPlayerGetAnOffer; set => thisPlayerGetAnOffer = value; }
        public int HowMuchMoneyOffer { get => howMuchMoneyOffer; set => howMuchMoneyOffer = value; }
        public string EscapeFromPrisonOfferted { get => escapeFromPrisonOfferted; set => escapeFromPrisonOfferted = value; }
        public string EscapeFromPrisonWanted { get => escapeFromPrisonWanted; set => escapeFromPrisonWanted = value; }
        public List<string> FieldOfferted { get => fieldOfferted; set => fieldOfferted = value; }
        public List<string> FieldWanted { get => fieldWanted; set => fieldWanted = value; }

        
        public Offer()
        { }

        /// <summary>
        /// Tworzenie oferty dla innego gracza. Nie wszystkie pola trzeba uzupełniać, można zastosować jakiegoś nulla jeżeli coś nie jest oferowane/chciane
        /// </summary>
        /// <param name="thisPlayerMakeAnOffer">Gracz tworzący ofertę</param>
        /// <param name="thisPlayerGetAnOffer">gracz otrzymujący ofertę</param>
        /// <param name="howMuchMoneyOffer">ile oferujący oferuje pieniędzy (jeżeli poda ujemną kwotę, to znaczy że chce pieniędzy)</param>
        /// <param name="escapeFromPrisonOfferted">Jakie karty wyjścia z więzienia oferuje oferujący</param>
        /// <param name="escapeFromPrisonWanted">Jakie karty wyjścia z więzienia chce oferujący</param>
        /// <param name="fieldOfferted">Jakie pola są proponowane</param>
        /// <param name="fieldWanted">Jakie pola są chciane</param>
        public Offer(PrePlayer thisPlayerMakeAnOffer, PrePlayer thisPlayerGetAnOffer, int howMuchMoneyOffer, string escapeFromPrisonOfferted, string escapeFromPrisonWanted, List<string> fieldOfferted, List<string> fieldWanted) : this()
        {
            ThisPlayerMakeAnOffer = thisPlayerMakeAnOffer;
            ThisPlayerGetAnOffer = thisPlayerGetAnOffer;
            HowMuchMoneyOffer = howMuchMoneyOffer;
            EscapeFromPrisonOfferted = escapeFromPrisonOfferted;
            EscapeFromPrisonWanted = escapeFromPrisonWanted;
            FieldOfferted = fieldOfferted;
            FieldWanted = fieldWanted;
        }

        public static void SaveXML(string nazwa, Offer z)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Offer));
            TextWriter writer = new StreamWriter($"{nazwa}.xml");
            serializer.Serialize(writer, z);
            writer.Close();
        }
        public static Offer ReadXML(string nazwa)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Offer));
            FileStream fs = new FileStream($"{nazwa}.xml", FileMode.Open);
            return (Offer)serializer.Deserialize(fs);
        }

    }
}
