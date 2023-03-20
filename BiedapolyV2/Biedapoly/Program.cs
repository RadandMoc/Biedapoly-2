using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biedapoly
{
    public class Program
    {
        static void Main(string[] args)
        {

            //Methods.PlayMusic("krok", true);
        
            //Test zapisu treści bonusów
            /*
            Methods cosiek = new Methods();
            Console.WriteLine(Methods.MakingValue(true, 20.2M, 9, false));
            Console.WriteLine(Methods.MakingValue(false, 4000M, 90, true));
            Console.WriteLine(Methods.MakingValue(true, 0.02M, 99, false));
            Console.WriteLine(Methods.MakingValue(true, 100.02M, 9, false));
            Console.WriteLine(Methods.MakingValue(true, 10.12M, 0, false));
            Console.WriteLine(Methods.MakingValue(true, 10.0M, 9, false));
            Console.WriteLine(Methods.MakingValue(false, 0M, 9, false));
            Console.WriteLine(Methods.MakingValue(false, 10M, 9, false));
            Console.WriteLine(Methods.MakingValue(true, 0.0M, 9, false));
            Console.WriteLine(Methods.MakingValue(false, 110M, 9, false));
            Console.WriteLine(Methods.OverridingValue(Methods.MakingValue(false, 110M, 9, false), true, 0.02M, 9, false));
            string zosiek = Methods.MakingValue(true, 1.1M, 1, false);
            Console.WriteLine();
            Console.WriteLine(zosiek);
            zosiek = Methods.OverridingValue(zosiek, false, 1501M, 0, true);
            zosiek = Methods.NextTurnForBonuses(zosiek);
            Console.WriteLine(zosiek);
            zosiek = Methods.NextTurnForBonuses(zosiek);
            Console.WriteLine(zosiek);
            Console.WriteLine(Methods.ChangeValue(10, zosiek));
            */
            //Stare losowanie kolejności graczy z tworzeniem pewnej mapki
            /*
            Heroes pionek = new Heroes("Pionek", 100, new Dictionary<string, string>());
            Player gracz1 = new Player(1000, pionek, Color.red, "Wulkanizator");
            Player gracz2 = new Player(1000, pionek, Color.red, "Eliminator");
            Player gracz3 = new Player(1000, pionek, Color.red, "Walaszek");
            Player gracz4 = new Player(1000, pionek, Color.red, "ToGrubas");
            Queue<Player> q = new Queue<Player>();
            q.Enqueue(gracz1);
            q.Enqueue(gracz2);
            q.Enqueue(gracz3);
            q.Enqueue(gracz4);
            Dictionary<int, int> CostHauses = new Dictionary<int, int>();
            CostHauses.Add(1, 100);
            CostHauses.Add(2, 100);
            CostHauses.Add(3, 100);
            CostHauses.Add(4, 100);
            CostHauses.Add(5, 100);
            List<Player> pl = new List<Player>();
            pl.Add(gracz1);
            pl.Add(gracz2);
            pl.Add(gracz3);
            pl.Add(gracz4);
            Dictionary<string,string>bonusy= new Dictionary<string,string>();
            Street ulPawia = new Street(100, 100, 500, 250, CostHauses, "101",pl,bonusy,gracz1,0);
            Street WZ = new Street(100, 100, 500, 250, CostHauses, "101", pl, bonusy, gracz1, 0);
            Dictionary<string,Territory>Places= new Dictionary<string,Territory>();
            Places.Add("Pawia",ulPawia);
            Places.Add("Wydzial Zarzadzania", WZ);
            Dictionary<int,int> defMoney= new Dictionary<int,int>();
            Dictionary<int, Card> Szansa = new Dictionary<int, Card>();
            Dictionary<string,string>alliance= new Dictionary<string,string>();
            Map sascosiek = new Map("Studentpoly",Places,defMoney,Szansa,Szansa,defMoney,alliance);
            DateTime dateTime = DateTime.Now;
            Game HungerGames = new Game(sascosiek, q, dateTime);
            
            HungerGames.PrintPlayers();
            HungerGames.RandomizeQueueOfPlayers();
            Console.WriteLine();
            HungerGames.PrintPlayers();
            Console.WriteLine();
            */
            //Sprawdzenie metody do odczytu kordów
            /*
            int int1, int2;
            (int1,int2) = Methods.ReturnCoordsFromString("321");
            Console.WriteLine(int1+ " " + int2);
            */
            //Sprawdzanie randomowania kolejności graczy
            /*
            Dictionary<string, string> testsortowania = new Dictionary<string, string>();
            Heroes hero1 = new Heroes("kot", 0, testsortowania);
            Heroes hero2 = new Heroes("pies",0,testsortowania);
            Heroes hero3 = new Heroes("słoń",0,testsortowania);
            Heroes hero4 = new Heroes("opos",0,testsortowania);
            Heroes hero5 = new Heroes("lemur", 0, testsortowania);
            List<Heroes> list = new List<Heroes>();

            list.Add(hero1);
            list.Add(hero2);
            list.Add(hero3);
            list.Add(hero4);
            list.Add(hero5);
            
            Queue<Heroes> queue = Methods.RandomizeListToQueue(list);
            Methods.PrintQueue(queue);
            */

            // Generowanie mapy Studentpoly


            //Console.WriteLine(Methods.ReturnRingFromString("000"));
            Dictionary<string,string>test = new Dictionary<string,string>();
            //test.Add("TST", "Z00100D01");
            Heroes counter = new Heroes("Counter", 15000, test);
            /*Player pl = new Player();
            pl.PlayerBonuses = test;
            pl.PlayerHero = counter;
            Console.WriteLine(pl.PlayerBonuses["TST"]);
            Methods.BonusesFromHeroToPlayer(pl);
            Console.WriteLine(pl.PlayerBonuses["TST"]);*/
            /*
            Map studentpoly = Methods.MakeStudentpolyMap(); //generowanie mapy studentpoly jest w metodzie w klasie Methods

            Game gameplay = Methods.SetGameWithConsole(studentpoly, counter); //generowanie gameplayu
                                                                              //gameplay.Players.Peek().Coordinates = "001";
                                                                              //Methods.Auction(gameplay, gameplay.Map.DictionaryOfTerritories["001"]);
            gameplay.ZapiszBin("test");
            Game cos2 = new Game();
            cos2 = (Game)cos2.OdczytajBin("test");
            Console.WriteLine(cos2.Players.Count);
            Console.WriteLine(cos2.Players.Peek().Name);
            Console.WriteLine(cos2.Map.MapName);
            List<string> ter = new List<string>();
            ter.Add("005");
            PrePlayer prep = new PrePlayer(gameplay.Players.Peek().Name);
            Offer off = new Offer(prep, prep, 500, "FFFFFFT99", "FFFFFFT99", ter,ter);
            Offer.SaveXML("ZapisXML", off);
            Offer off2 = Offer.ReadXML("ZapisXML");
            Console.WriteLine(off2.ThisPlayerGetAnOffer.Name);
            Console.WriteLine(off2.HowMuchMoneyOffer);
            Console.WriteLine(off2.EscapeFromPrisonOfferted);
            */
            /*
            
            
            if (gameplay.HowLongWeWantPlay == 31536000)
            {
                studentpoly.DictionaryOfTerritories["001"].Owner = gameplay.Players.Peek();
                studentpoly.DictionaryOfTerritories["003"].Owner = gameplay.Players.Peek();
                studentpoly.DictionaryOfTerritories["005"].Owner = gameplay.Players.Peek();
                studentpoly.DictionaryOfTerritories["006"].Owner = gameplay.Players.Peek();
                studentpoly.DictionaryOfTerritories["007"].Owner = gameplay.Players.Peek();
                studentpoly.DictionaryOfTerritories["008"].Owner = gameplay.Players.Peek();
                studentpoly.DictionaryOfTerritories["009"].Owner = gameplay.Players.Peek();

                while (Methods.HowMuchPlayerHaveMoney(gameplay) != 1 )
                {
                    Methods.PlayerRandomMove(gameplay);
                    Console.WriteLine($" gracz {gameplay.Players.Peek().Name} kasa {gameplay.Players.Peek().Money}");
                    gameplay.NextTurn();
                }
                string winner="";
                foreach (Player item in gameplay.Players)
                {
                    if (item.Money >= 0)
                        winner = item.Name;
                }
                Console.WriteLine($"Wygrał gracz: {winner}");
            }
            else
            {

            }
            */


            

            

            Console.ReadKey();
        }
    }
}
