using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Biedapoly
{
    [Serializable]
    public class Game
    {
        Map map;
        Queue<Player> players;
        int WhichTurnIs = 0;
        int howLongWeWantPlay; //int oznaczający ilość sekund do końca gry. 31 536 000 oznacza że gra bedzie trwala w nieskonczonosc
        DateTime howLongWePlay = new DateTime(1, 1, 1, 0, 0, 0);
        int howMuchMove;
        bool isDouble;
        int firstThrow, secondThrow;
        // HowLongWePlay jest datą startu tury. Realny czas jak długo gramy musi być wynikową różnicy czasu obecnego od czasu startu tury

        public Map Map { get => map; set => map = value; }
        public Queue<Player> Players { get => players; set => players = value; }
        public int HowLongWeWantPlay { get => howLongWeWantPlay; set => howLongWeWantPlay = value; }
        public DateTime HowLongWePlay { get => howLongWePlay; set => howLongWePlay = value; }
        public int HowMuchMove { get => howMuchMove; set => howMuchMove = value; }
        public bool IsDouble { get => isDouble; set => isDouble = value; }
        public int FirstThrow { get => firstThrow; set => firstThrow = value; }
        public int SecondThrow { get => secondThrow; set => secondThrow = value; }

        public Game() { }

        // Jeżeli gra ma toczyć się do końca, to HowLongWeWantPlay będzie ustawiony na DateTime(99, 1, 1, 20, 0, 0);
        public Game(Map map, Queue<Player> players, int howLongWeWantPlay)
        {
            Map = map;
            Players = players;
            HowLongWeWantPlay = howLongWeWantPlay;
        }


        //Poniższa metoda losuje kolejność graczy która będzie toczyła się w grze
        public void RandomizeQueueOfPlayers()//Jako taki
        {
            int sum = Players.Count;
            if (sum > 2)
            {
                int random;
                for (int i = 0; i < (sum * (sum - 1)); i++)
                {
                    Random random1 = new Random();
                    random = random1.Next(1, 12);
                    switch (i)
                    {
                        case 0:
                            {
                                random *= 2;
                                break;
                            }
                        case 1:
                            {
                                random *= (int)DateTime.Now.Day;
                                random = random % 13;
                                break;
                            }
                        case 2:
                            {
                                random += (int)DateTime.Now.Month;
                                random = random % 13;
                                break;
                            }
                        case 3:
                            {
                                random += (int)DateTime.Now.Year;
                                random = random % 13;
                                break;
                            }
                        case 4:
                            {
                                random += ((int)DateTime.Now.Day - (int)DateTime.Now.Month);
                                if (random < 0)
                                    random *= -1;
                                random = random % 13;
                                break;
                            }
                        case 5 | 12:
                            {
                                random += (int)DateTime.Now.Millisecond;
                                random = random % 13;
                                break;
                            }
                        case 6:
                            {
                                random += (int)DateTime.Now.Minute;
                                random = random % 13;
                                break;
                            }
                        case 7 | 13:
                            {
                                random += (int)((int)DateTime.Now.Year / (int)DateTime.Now.Second);
                                random = random % 13;
                                break;
                            }
                        case 8:
                            {
                                random *= (int)(((int)DateTime.Now.Year - (int)DateTime.Now.Second) / (int)DateTime.Now.Millisecond);
                                random = random % 13;
                                break;
                            }
                        case 9:
                            {
                                random += (int)DateTime.Now.Month * (random - 1);
                                random = random % 13;
                                break;
                            }
                        case 10 | 14:
                            {
                                random += (int)DateTime.Now.Millisecond;
                                random = random % 13;
                                break;
                            }
                        case 11:
                            {
                                random += (int)((int)DateTime.Now.Millisecond - 2) / 3;
                                if (random < 0)
                                    random *= -1;
                                random = random % 13;
                                break;
                            }
                        default:
                            break;
                    }
                    if (random < 6)
                    {
                        Player val1 = Players.Dequeue();
                        Player val2 = Players.Dequeue();
                        Player val3 = Players.Dequeue();
                        Players.Enqueue(val2);
                        Players.Enqueue(val3);
                        Players.Enqueue(val1);
                    }
                    else if (random < 9)
                    {
                        Player val1 = Players.Dequeue();
                        Player val2 = Players.Dequeue();
                        Player val3 = Players.Dequeue();
                        Players.Enqueue(val3);
                        Players.Enqueue(val1);
                        Players.Enqueue(val2);
                    }
                    else if (random < 12)
                    {
                        Player val1 = Players.Dequeue();
                        Player val2 = Players.Dequeue();
                        Player val3 = Players.Dequeue();
                        Players.Enqueue(val2);
                        Players.Enqueue(val1);
                        Players.Enqueue(val3);
                    }
                }
            }
            else if (sum == 2)
            {
                Random random = new Random();
                if (random.Next(1, 2) == 1)
                {
                    Player val1 = Players.Dequeue();
                    Players.Enqueue(val1);
                }
            }
            else
                throw new NotEnoughPlayersException();
        }

        public int Random()
        {
            Random random = new Random();
            int fistthrow = random.Next(1, 6);
            int secondtrow = random.Next(1, 6);
            int Sum = fistthrow + secondtrow;
            return Sum;
        }

        //Poniższa metoda zwraca informację która tura czeczywiście jest (ta ujawniana graczom)
        public int WhichRealTurnIs() => (int)(WhichTurnIs / Players.Count) + 1;

        //Poniższa metoda zmienia turę kiedy gracz zakończy
        public void NextTurn()
        {
            WhichTurnIs++;
            Players.Enqueue(Players.Dequeue());
        }

        //Poniższa metoda służy do sprawdzania graczy w kolejce. wypisuje ich kolejność w konsoli
        public void PrintPlayers()
        {
            foreach (Player item in Players)
            {
                Console.Write($"{item.Name} ");
            }
        }

        //PM jest pełną symulacją rzutu kości. Zmienia wartość HowMuchMove i IsDouble
        public void ThrowingDices()
        {
            FirstThrow = Methods.ThrowDiceRoll();
            SecondThrow = Methods.ThrowDiceRoll();
            bool lying;
            string bonuses;
            if (firstThrow == secondThrow)
            {
                IsDouble = true;
                lying = Players.Peek().PlayerBonuses.TryGetValue("HMD", out bonuses);
                if (!lying)
                    Players.Peek().PlayerBonuses.Add("HMD", Methods.MakingValue(false, 1, 0, true));
                else
                {
                    if (bonuses == "Z00001U00")
                    {
                        Players.Peek().PlayerBonuses.Remove("HMD");
                        Players.Peek().PlayerBonuses.Add("HMD", Methods.MakingValue(false, 2, 0, true));
                    }
                    else if (bonuses == "Z00002U00")
                    {
                        Players.Peek().PlayerBonuses.Remove("HMD");
                        Players.Peek().PlayerBonuses.Add("HMD", Methods.MakingValue(false, 3, 0, true));
                    }
                    else if (bonuses == "Z00003U00")
                    {
                        Players.Peek().PlayerBonuses.Remove("HMD");
                        Players.Peek().PlayerBonuses.Add("HMD", Methods.MakingValue(false, 4, 0, true));
                    }
                    else if (bonuses == "Z00004U00")
                    {
                        Players.Peek().PlayerBonuses.Remove("HMD");
                        Players.Peek().PlayerBonuses.Add("HMD", Methods.MakingValue(false, 5, 0, true));
                    }
                }
            }
            else
                IsDouble = false;
            firstThrow += secondThrow;
            lying = Players.Peek().PlayerBonuses.TryGetValue("MOV", out bonuses);
            if (lying)
                HowMuchMove = (int)Methods.ChangeValue(firstThrow, bonuses);
            else
                HowMuchMove = firstThrow;
        }

        public void SaveBinary(string nazwa)
        {
            FileStream fs = new FileStream($"{nazwa}.bin", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, this);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
        static public object ReadBinary(string nazwa)
        {
            Game zespol;
            FileStream fs = new FileStream($"{nazwa}.bin", FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                zespol = (Game)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return zespol;
        }

    }

}
