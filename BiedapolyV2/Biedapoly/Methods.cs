using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Runtime.CompilerServices;

namespace Biedapoly
{
    public class Methods
    {
        static private MediaPlayer myPlayer = new MediaPlayer(); // player do muzyki
        // ctrl + m + o    <=> bez tego się tu nie obędziesz
        // klucze bonusów w ok linijce nr 88

        public Methods() { }
        /* Słownik z właścicielami konkretnych pól:
         * Słownik powinien znajdować się w klasie Map / GameBoard
         * Kluczem są koordynacje konkretnego obiektu zapisane w 3 literowym stringu, przy czym są dwie zmienne, "pierscien" - która odpowiada za pas ruchu w grze oraz numer pola w danym pierścieniu
         * Wartością jest konkretny gracz.
         */

        // Zapis omawianego stringa w 14 linijce kodu w Methods
        /// <summary>
        /// Zapisuje koordynaty w string
        /// </summary>
        /// <param name="pierscien"></param>
        /// <param name="koordynacja"></param>
        /// <returns></returns>
        static public string SaveCoordsToString(int pierscien, int koordynacja)
        {
            string returner;
            if (koordynacja < 10)
                returner = $"0{koordynacja.ToString()}";
            else returner = koordynacja.ToString();
            return pierscien.ToString() + returner;
        }

        // Odczyt omawianego stringa w 14 linijce klasy Methods
        /// <summary>
        /// Odczytuję koordynaty gracza które są w stringu
        /// </summary>
        /// <param name="misteryCoordinates"></param>
        /// <returns></returns>
        static public (int pierscien, int koordynat) ReturnCoordsFromString(string misteryCoordinates)
        {
            int pierscien = Methods.CharToInt(misteryCoordinates[0]);
            int koordynat;
            if (Methods.CharToInt(misteryCoordinates[1]) != 0)
                koordynat = (Methods.CharToInt(misteryCoordinates[1]) * 10) + Methods.CharToInt(misteryCoordinates[2]);
            else
                koordynat = Methods.CharToInt(misteryCoordinates[2]);
            return (pierscien, koordynat);
        }

        //Odczyt numeru pierścienia
        /// <summary>
        /// Zwracam tylko numer pierścienia / rzędu.
        /// </summary>
        /// <param name="misteryCoordinates">zaszyfrowane w stringu koordynaty</param>
        /// <returns></returns>
        static public int ReturnRingFromString(string misteryCoordinates) => Methods.CharToInt(misteryCoordinates[0]);

        /* Słownik z bonusami
         * Klucz: 3 duże litery oznaczające skrót czynnika którego dotyczą.     Przykład: MFS - (MoneyForStart) pieniądze za przejście przez start
         * Wartość: Zakodowany string z treścią modyfikacji wartości której dotyczą. !!UWAGA: Może się zmieniąc dla poszczególnego klucza, ale zawsze musi mieć 9 znaków !!
         * Info: Może być więcej niż jedna zaszyfrowana wartość. nie ma maksymalnych ograniczeń. Jednak warto uważać, którą metodę się stosuje, czy tą która tworzy nową wartość czy tą, która ją dodaje.
         * 
         * Większe szczegóły:
         * // Dla modyfikatorów przepływów pieniężnych wartość zkłada się z:
         * // Jednego znaku P lub Z, gdzie P oznacza, że różnica zmiany jest iloczynowa, a Z oznacza, że zmiana wartości jest dodawana
         * // 5 znaków zmienianej wartości. w przypadku bonusu iloczynowego 3 pierwsze znaki są liczbami całkowitymi a 2 ostatnie po przecinku. W drugim przypadku (sum czyli znaku Z) jest to 5 cyfr całkowitych robiących zmianę
         * // Jeden znak definiujący czy podawana wartość jest ujemna (U) czy dodatnia (D).
         * // Dwóch cyfr definiujących czas trwania bonusu. przy czym znak 99 oznacza nieskończoność, a 00 oznacza jedną turę. analogicznie 01 oznacza 2 tury a 11 oznacza 12 tur
         * // 
         * // Przykłady deszyfracji klucza:
         * // Z00100D01 - Dana wartość jest powiększana o 100 jeszcze przez 2 tury włącznie z tą
         * // P00110D10 - Dana wartość jest powiększana o 10% jeszcze przez 11 tur włącznie z tą
         * // Z00001U99 - Dana wartość jest zmniejszana o 1 do końca gry, chyba że zostanie ten bonus zniesiony
         * // P00000D00 - Dana wartość jest zrównywana do 0. Ten bonus trwa tylko obecną turę
         * // P00200U05 - Dana wartość jest staje się wartością o przeciwnym znaku i jest dwukrotnością wcześniejszej wartości. Bonus trwa 6 tur włącznie z tą
         * // Z00100D01P00110D10 - Dana wartość jest powiększana o 100 jeszcze przez 2 tury włącznie z tą a następnie jest powiększana o 10%, co trwa jeszcze przez 11 tur włącznie z tą
         */
        //Chwilowo można edytować jest oznaczane na końcu poprzez *. Kiedy użyje ktoś tego oznaczenia proszę o usunięcie jej.
        /* Lista kluczy:
         * BHA - (build hause) stawianie domow 
         * BHO - (build hotel) budowanie hoteli
         * RHA - (remove hause) niszczenie domu każdego poziomu (poza hotelem)
         * RHO - (remove hotel) niszczenie hotelu do domu 4-ro poziomowego
         * GPM - (get pawn money) dostawanie pieniedzy z zastawu
         * PPM - (pay pawn money) Placenie za odzyskanie zastawu
         * MOV - zmiany w ruchu postaci (Skutkuje zmianą sumy rzutu kości)
         * PAP - (pay a payment) zmiany w platnosci pieniedzy dla innych graczy za wejscie na ich pole
         * GAP - (get a payment) zmiany w otrzymywaniu platnosci od innych graczy
         * ARE - czy jest w wiezieniu (zawiera ilosc tur kiblowania i kare do zaplacenia zeby wyjsc)
         * CIE - (can I escape?) Czy posiadam kartę wyjścia z więzienia. UWAGA: nie można zrobić ograniczonego czasu, a na końcu zawsze musi być 99; przykładowy klucz : FFFFFFT99 - można raz wyjść bezpłatnie (przez nieograniczony czas gry), TTTTTTT99FFFFFFT99 - mogę wykorzystać 8 kart wyjście z więzienia.
         * MFS - modyfikator zmiany pieniędzy za przejście przez start
         * HMD - (how much double) ile zostalo rzucone doubli. (do liczenia czy idzie do wiezienia bo za duzo doubli)
         * MON - (money) zmiana stanu gotówki (niezbędne dla kart np szansa)
         * RTL - (rent tile list) specjalny klucz, powodujący wymnożenie ilości wylosowanego ruchu przez gracza przez liczbę wartości w dictionary rent list
         * TCP - (take card prospect) weź kartę szansa
         * TCS - (take card social treasure) weź kartę kasa społeczna
         * PAR - (Parking) możesz budować wszędzie domki (o ile masz karty tego samego koloru)
         * STB - (selling tile to bank) zmiana otrzymywania pieniędzy od banku za sprzedaż pola (do banku) (*)
         * BTB - (buying tile from bank) zmiana kosztu zakupu pola od banku (*)
         */

        //Poniższa metoda tworzy zakodowany bonus. STOSOWAĆ TYLKO KIEDY MAMY PEWNOŚĆ ŻE DANY KLUCZ NIE MA ŻADNEGO BONUSU LUB CHCE USUNĄĆ WSZYSTKIE JUŻ ISTNIEJĄCE
        /// <summary>
        /// Zwracam string, który kondensuje wszystkie podane informacje. Stosuje się mnie do zapisu bonusów. Uwaga: wprowadzanie mnie jako klucza usunie stary klucz. (Ps. jak chcesz nadpisać bonusy to może sprawdź metodę OverridingValue)
        /// </summary>
        /// <param name="isProduct">Czy mam mnożyć o daną wartość (true jeśli tak)</param>
        /// <param name="val1">jaką wartością mam zmienić? (jak dodaje to zakres od 0 do 99999 a jak mnożę to przyjmuje liczby od 0 do 999,99)</param>
        /// <param name="turns">Ile turn mam trwać? (99 to nieskończoność, a 0 to tylko obecną)</param>
        /// <param name="isMinus">Czy wykonywane działanie ma być z minusem?</param>
        /// <returns></returns>
        static public string MakingValue(bool isProduct, decimal val1, int turns, bool isMinus)
        {
            string returner;
            if (isProduct)
            {
                returner = "P";
                if (val1 != 0)
                {
                    for (int i = 0; i < (3 - ((int)val1).ToString().Length); i++)
                    {
                        returner += "0";
                    }
                    if (val1 - (int)val1 > 0.0M)
                    {
                        int shortValue = (int)(val1 * 10);
                        if (((decimal)shortValue / 10) == val1)
                        {
                            returner += ((int)val1).ToString() + ((int)((val1 - (int)val1) * 10)).ToString() + "0";
                        }
                        else
                        {
                            returner += ((int)val1).ToString();
                            if ((int)((val1 - (int)val1) * 100) > 9)
                                returner += ((int)((val1 - (int)val1) * 100)).ToString();
                            else
                                returner += "0" + ((int)((val1 - (int)val1) * 100)).ToString();
                        }
                    }
                    else
                    {
                        returner += ((int)val1).ToString() + "00";
                    }
                }
                else
                    returner += "00000";
            }
            else
            {
                returner = "Z";
                for (int i = 0; i < (5 - val1.ToString().Length); i++)
                {
                    returner += "0";
                }
                returner += val1.ToString();
            }
            if (!isMinus)
                returner += "D";
            else
                returner += "U";
            if (turns < 10)
                returner += "0" + turns.ToString();
            else
                returner += turns.ToString();
            return returner;
        }

        //Poniższa metoda dodaje do istniejacego bonusu kolejny. Nowy bonus jest umieszczany na końcu
        /// <summary>
        /// Zwracam string, który kondensuje wszystkie podane informacje. Stosuje się mnie do zapisu bonusów. Uwaga: nowy klucz będzie ostatnim w kolejce. (Ps. jak chcesz zrobić nowe bonusy bez starych, to może sprawdź metodę MakingValue)
        /// </summary>
        /// <param name="overValue">Bonusy które są nadpisywane</param>
        /// <param name="isProduct">Czy mam mnożyć o daną wartość (true jeśli tak)</param>
        /// <param name="val1">jaką wartością mam zmienić? (jak dodaje to zakres od 0 do 99999 a jak mnożę to przyjmuje liczby od 0 do 999,99)</param>
        /// <param name="turns">Ile turn mam trwać? (99 to nieskończoność, a 0 to tylko obecną)</param>
        /// <param name="isMinus">Czy wykonywane działanie ma być z minusem?</param>
        /// <returns></returns>
        static public string OverridingValue(string overValue, bool isProduct, decimal val1, int turns, bool isMinus)
        {
            string returner = overValue;
            if (isProduct)
            {
                returner += "P";
                if (val1 != 0)
                {
                    for (int i = 0; i < (3 - ((int)val1).ToString().Length); i++)
                    {
                        returner += "0";
                    }
                    if (val1 - (int)val1 > 0.0M)
                    {
                        int shortValue = (int)(val1 * 10);
                        if (((decimal)shortValue / 10) == val1)
                        {
                            returner += ((int)val1).ToString() + ((int)((val1 - (int)val1) * 10)).ToString() + "0";
                        }
                        else
                        {
                            returner += ((int)val1).ToString();
                            if ((int)((val1 - (int)val1) * 100) > 9)
                                returner += ((int)((val1 - (int)val1) * 100)).ToString();
                            else
                                returner += "0" + ((int)((val1 - (int)val1) * 100)).ToString();
                        }
                    }
                    else
                    {
                        returner += ((int)val1).ToString() + "00";
                    }
                }
                else
                    returner += "00000";
            }
            else
            {
                returner += "Z";
                for (int i = 0; i < (5 - val1.ToString().Length); i++)
                {
                    returner += "0";
                }
                returner += val1.ToString();
            }
            if (!isMinus)
                returner += "D";
            else
                returner += "U";
            if (turns < 10)
                returner += "0" + turns.ToString();
            else
                returner += turns.ToString();
            return returner;
        }

        /// <summary>
        /// Zwracam string, który kondensuje wszystkie podane informacje. Stosuje się mnie do zapisu bonusów. Uwaga: nowy klucz będzie ostatnim w kolejce. (Ps. jak chcesz zrobić nowe bonusy bez starych, to może sprawdź metodę MakingValue)
        /// </summary>
        /// <param name="overValue"> nadpisywana wartość</param>
        /// <param name="addingValue"> dopisywana wartość</param>
        /// <returns></returns>
        static public string OverridingValue(string overValue, string addingValue)
        {
            for (int i = 0; i < addingValue.Count(); i++)
            {
                overValue += addingValue[i];
            }
            return overValue;
        }

        //Poniższa metoda sprawdza ile bonusow w jednym kluczu się znajduje
        /// <summary>
        /// Zwracam liczbę ile bonusów jest w danym kluczu
        /// </summary>
        /// <param name="val1">lista bonusów w danym kluczu</param>
        /// <returns></returns>
        static public int HowMuchIsValues(string val1) => val1.Length / 9;

        //Poniższa metoda modyfikuje jeden znak w stringu. Bierze zmieniany string, char na ktory ma byc zmieniony, oraz lokalizacje od poczatku wliczajac tez 0
        /// <summary>
        /// modyfikuję jeden znak w stringu
        /// </summary>
        /// <param name="val1">zmieniany string</param>
        /// <param name="val2">char na jaki mam zmienić</param>
        /// <param name="location">numer pozycji liczony od 0 (od lewej) w jakim miejscu mam zmienić znak</param>
        /// <returns></returns>
        static public string ChangeOneLetterInString(string val1, char val2, int location)
        {
            char[] array = val1.ToCharArray();
            array[location] = val2;
            return new string(array);
        }

        //chary same z siebie dodaja 48. poniższa metoda pozwala o tym nie pamietac
        /// <summary>
        /// Zwracam int z przyjmowanego chara
        /// </summary>
        /// <param name="letter">char</param>
        /// <returns></returns>
        static public int CharToInt(char letter) => (int)letter - 48;

        //Poniższa metoda obniża czas trwania wszystkich tur poza 99 o 1, a te bonusy które mają 00 usuwa. Powinna być wykonywana pod koniec każdej tury LUB po turze gracza. do uzgodnienia lub zdecydowania przez kodujacego. warto tu naspisac przy podejmowaniu decyzji.
        /// <summary>
        /// zmieniam bonusy o turę, przy czym 99 traktuję jako nieskończoność, a 00 usuwam
        /// </summary>
        /// <param name="val1">lista bonusów z danego klucza</param>
        /// <returns></returns>
        static public string NextTurnForBonuses(string val1)
        {
            int howMuch = Methods.HowMuchIsValues(val1);
            int turns;
            string assistance;
            string returner = val1;
            for (int i = 0; i < howMuch; i++)
            {
                turns = Methods.CharToInt(returner[7 + (i * 9)]) * 10;
                turns += Methods.CharToInt(returner[8 + (i * 9)]); //Troche smieszne, bo wczytywanie chara daje 48 dodatkowo. Rekompensuje to odejmowaniem. Na przyszlosc zrobilem metode ktora bedzie pozbywala sie tego problemu.
                if (turns != 0)
                {
                    if (turns != 99)
                        turns--;
                    if (turns < 10)
                        assistance = "0" + turns.ToString();
                    else
                        assistance = turns.ToString();
                    returner = Methods.ChangeOneLetterInString(returner, assistance[0], 7 + (i * 9));
                    returner = Methods.ChangeOneLetterInString(returner, assistance[1], 8 + (i * 9));
                }
                else
                {
                    i--;
                    howMuch--;
                    returner = returner.Remove(0, 9);
                }
            }
            return returner;
        }

        //PM zmienia dla każdego bonusu z każdego klucza bonusy o turę
        /// <summary>
        /// Zmieniam graczowi każdy bonus jako ominięcie tury. usuwam dany klucz, jeśli jego wartość przestanie istnieć
        /// </summary>
        /// <param name="player">Gracz, któremu omijamy turę</param>
        static public void NextTurnForEveryBonuses(Player player)
        {
            string key, val;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in player.PlayerBonuses)
            {
                key = item.Key;
                val = item.Value;
                val = Methods.NextTurnForBonuses(val);
                if (val != "")
                    dic[key] = val;
            }
            player.PlayerBonuses = dic;
        }
        /*Kod do przetestowania powyższej metody:
            Heroes counter = new Heroes("Counter", 15000, new Dictionary<string, string>());
            Dictionary<string,string> dic = new Dictionary<string,string>();
            dic.Add("MON", "Z00100D01");
            dic.Add("MOV", "Z00100D01P00110D10");
            dic.Add("CIE", "P00000D00");
            dic.Add("HIV", "P00000D00P00110D10");
            Player gamersik = new Player(100, counter, Color.red, "Kozak");
            gamersik.PlayerBonuses = dic;
            Methods.NextTurnForEveryBonuses(gamersik);
         */

        //Poniższa metoda modyfikuje orginalną wartość o wartości zawarte w bonusach. Przyjmuje cały string bonusów. Uzywajac nie potrzeba sprawdzac czy wartosc jest nullem. robi to automatycznie na poczatku i omija prawie wszystko jeśli jest nullem.
        /// <summary>
        /// przemielam wartości modyfikując zapisanymi bonusami. (zwracam decimal) Uwaga: sprawdź czy dany klucz jest modyfikatorem wartości jakiegoś obiektu, bo inaczej mogą być problemy. Uzywajac nie potrzeba sprawdzac czy wartosc jest pusta. robi to automatycznie na poczatku i omija prawie wszystko jeśli jest właśnie pusta
        /// </summary>
        /// <param name="val1">wartość zmieniana</param>
        /// <param name="bonuses">bonusy</param>
        /// <returns></returns>
        static public decimal ChangeValue(decimal val1, string bonuses)
        {
            decimal returner = val1;
            int howMuch = Methods.HowMuchIsValues(bonuses);
            char assistance;
            for (int i = 0; i < howMuch; i++)
            {
                assistance = bonuses[i * 9];
                if (assistance == 'P')
                    returner *= (decimal)Methods.CharToInt(bonuses[1 + (i * 9)]) * 100 + (decimal)Methods.CharToInt(bonuses[2 + (i * 9)]) * 10 + (decimal)Methods.CharToInt(bonuses[3 + (i * 9)]) + (decimal)Methods.CharToInt(bonuses[4 + (i * 9)]) / 10 + (decimal)Methods.CharToInt(bonuses[5 + (i * 9)]) / 100;
                else if (assistance == 'Z')
                {
                    if (bonuses[6 + i * 9] == 'D')
                    {
                        int helper = 1;
                        for (int j = 0; j < 5; j++)
                        {
                            returner += (decimal)(Methods.CharToInt(bonuses[5 - j + (i * 9)]) * helper);
                            helper *= 10;
                        }
                    }
                    else if (bonuses[6 + i * 9] == 'U')
                    {
                        int helper = 1;
                        for (int j = 0; j < 5; j++)
                        {
                            returner -= (decimal)(Methods.CharToInt(bonuses[5 - j + (i * 9)]) * helper);
                            helper *= 10;
                        }
                    }
                }
            }
            return returner;
        }

        //metoda wywoływana, kiedy gracz przejdzie przez start
        /// <summary>
        /// Dodaję pieniądze za przejście przez start. Zwracam uwagę na bonusy itp.
        /// </summary>
        /// <param name="game">tocząca się gra</param>
        static public void AddMoneyForStart(Game game)
        {
            if (!game.Players.Peek().WasChangeOfLane)
            {
                int ring = Methods.ReturnRingFromString(game.Players.Peek().Coordinates);
                int money;
                bool fake = game.Map.MoneyForStart.TryGetValue(ring, out money);
                if (fake)
                {
                    string bonusy;
                    bool fake2 = game.Players.Peek().PlayerBonuses.TryGetValue("MFS", out bonusy);
                    if (fake2)
                        money = (int)(Methods.ChangeValue((decimal)money, bonusy));
                }
                else
                    Console.WriteLine("Mapa nie zaimplementowała pieniędzy za przekroczenie tego pierścienia");
                game.Players.Peek().Money += money;
                Console.WriteLine($"Dodano pieniądze za przejście przez start w wysokości {money}");
            }
        }

        //Poniższa metoda symuluje rzut kostką. Zwraca int
        /// <summary>
        /// Symulacja rzutu kostką.
        /// </summary>
        /// <returns></returns>
        static public int ThrowDiceRoll()
        {
            Random random = new Random();
            return random.Next(1, 6);
        }

        // Symulacja pobierania karty Szasna
        /// <summary>
        /// Symulacja wyboru karty Szansa. Zwraca całą kartę.
        /// </summary>
        /// <param name="actualGame">aktualna rozgrywka</param>
        /// <returns></returns>
        static public Card Prospect(Game actualGame)
        {
            int val1, val2 = actualGame.Map.Prospect.Count();
            Random random = new Random();
            val1 = random.Next(0, val2 - 1);
            Card returner;
            actualGame.Map.Prospect.TryGetValue(val1, out returner);
            return returner;
        }

        /// <summary>
        /// Symulacja wyboru karty Kasa Społeczna. Zwraca całą kartę.
        /// </summary>
        /// <param name="actualGame">aktualna rozgrywka</param>
        /// <returns></returns>
        static public Card SocialTreasury(Game actualGame)
        {
            int val1, val2 = actualGame.Map.SocialTreasury.Count();
            Random random = new Random();
            val1 = random.Next(0, val2 - 1);
            Card returner;
            actualGame.Map.SocialTreasury.TryGetValue(val1, out returner);
            return returner;
        }

        //Poniższa metoda przepisuje listę bonusów z herosa na gracza.
        /// <summary>
        /// Przepisuje bonusy z herosa na gracza
        /// </summary>
        /// <param name="hero"></param>
        /// <param name="player"></param>
        static public void BonusesFromHeroToPlayer(Player player)
        {
            Dictionary<string, string> heroBonuses = new Dictionary<string, string>();
            foreach (var item in player.PlayerBonuses)
                heroBonuses.Add(item.Key, item.Value);
            string key, val, bin;
            foreach (var item in player.PlayerHero.HeroesBonuses)
            {
                key = item.Key;
                val = item.Value;
                if (!player.PlayerBonuses.TryGetValue(key, out bin))
                    heroBonuses.Add(key, val);
                else
                    heroBonuses[key] = Methods.OverridingValue(heroBonuses[key], val);
            }
            player.PlayerBonuses = heroBonuses;
        }
        /*Test
         Dictionary<string,string>test = new Dictionary<string,string>();
            test.Add("TST", "Z00100D01");
            Heroes counter = new Heroes("Counter", 15000, test);
            Player pl = new Player();
            pl.PlayerBonuses = test;
            pl.PlayerHero = counter;
            Console.WriteLine(pl.PlayerBonuses["TST"]);
            Methods.BonusesFromHeroToPlayer(pl);
            Console.WriteLine(pl.PlayerBonuses["TST"]);
        */

        //Poniższa metoda niszczy poziom budynku i daje połowę opłaty za budowę do portfelu właściciela
        /// <summary>
        /// Niszczę poziom budynku wzamian za pieniądze dla jego właściciela (dokładniej to połowę kwoty) Uwzględnia fakt, czy na polu stoi hotel czy domek
        /// </summary>
        /// <param name="TileStreet">edytowane pole</param>
        static public void RemoveBuilding(Street TileStreet)
        {
            if (TileStreet.HowMuchAreHauses < 5 && TileStreet.HowMuchAreHauses > 0)
            {
                TileStreet.HowMuchAreHauses--;
                string bonus;
                int getmoney;
                bool cosiek = TileStreet.Owner.PlayerBonuses.TryGetValue("RHA", out bonus);
                if (!cosiek)
                    getmoney = (int)(TileStreet.CostToBuildHause / 2);
                else
                    getmoney = (int)Methods.ChangeValue((TileStreet.CostToBuildHause / 2), bonus);
                TileStreet.Owner.Money += getmoney;
            }
            else if (TileStreet.HowMuchAreHauses == 5)
            {
                TileStreet.HowMuchAreHauses--;
                string bonus;
                int getmoney;
                bool cosiek = TileStreet.Owner.PlayerBonuses.TryGetValue("RHO", out bonus);
                if (!cosiek)
                    getmoney = (int)(TileStreet.CostToBuildHotel / 2);
                else
                    getmoney = (int)Methods.ChangeValue((TileStreet.CostToBuildHotel / 2), bonus);
                TileStreet.Owner.Money += getmoney;
            }
        }

        //Poniższa metoda zastawia pole. UWAGA: nie sprawdza czy użytkownik może zastawić, tylko jakie pole jest zastawiane. Ps. Sama sprawdza czy można zastawić.
        /// <summary>
        /// Zastawiam pole. Sprawdzam jakiego to rodzaju pole i zabezpieczam się sprawdzając warunki zastawienia konkrenego pola. Uwaga: to jest metoda tylko do zastawienia a nie do sprawdzenia czy ktoś coś może zastawić itp
        /// </summary>
        /// <param name="TileStreet">zastawiane pole</param>
        static public void PawnStreet(Territory TileStreet)
        {
            if (TileStreet.GetType() == typeof(Street))
            {
                Street st = (Street)TileStreet;
                if (st.HowMuchAreHauses == 0 && TileStreet.IsPawned == false)
                {
                    string bonus;
                    bool cosiek = st.Owner.PlayerBonuses.TryGetValue("GPM", out bonus);
                    if (!cosiek)
                        st.Owner.Money += st.MoneyFromPawn;
                    else
                        st.Owner.Money += (int)Methods.ChangeValue(st.MoneyFromPawn, bonus);
                    st.IsPawned = true;
                }
            }
            else if (TileStreet.GetType() == typeof(Station))
            {
                Station st = (Station)TileStreet;
                if (TileStreet.IsPawned == false)
                {
                    string bonus;
                    bool cosiek = st.Owner.PlayerBonuses.TryGetValue("GPM", out bonus);
                    if (!cosiek)
                        st.Owner.Money += st.MoneyFromPawn;
                    else
                        st.Owner.Money += (int)Methods.ChangeValue(st.MoneyFromPawn, bonus);
                    st.IsPawned = true;
                }
            }
        }

        /// <summary>
        /// Metoda do odstawienia pola 
        /// </summary>
        /// <param name="TileStreet"></param>
        static public void RePawnStreet(Territory TileStreet)
        {
            if (TileStreet.GetType() == typeof(Street))
            {
                Street st = (Street)TileStreet;
                if (st.HowMuchAreHauses == 0 && TileStreet.IsPawned == true)
                {
                    string bonus;
                    bool cosiek = st.Owner.PlayerBonuses.TryGetValue("PPM", out bonus);
                    if (!cosiek)
                        st.Owner.Money -= st.MoneyFromPawn;
                    else
                        st.Owner.Money -= (int)Methods.ChangeValue(st.MoneyFromPawn, bonus);
                    st.IsPawned = false;
                }
            }
            else if (TileStreet.GetType() == typeof(Station))
            {
                Station st = (Station)TileStreet;
                if (TileStreet.IsPawned == true)
                {
                    string bonus;
                    bool cosiek = st.Owner.PlayerBonuses.TryGetValue("PPM", out bonus);
                    if (!cosiek)
                        st.Owner.Money -= st.MoneyFromPawn;
                    else
                        st.Owner.Money -= (int)Methods.ChangeValue(st.MoneyFromPawn, bonus);
                    st.IsPawned = false;
                }
            }
        }

        //Poniższa metoda powoduje randomizacje herosów i zwraca queue
        /// <summary>
        /// Losuje kolejność herosów, przy czym przyjmuje listę, ale zwraca queue
        /// </summary>
        /// <param name="list"> lista herosów </param>
        /// <returns></returns>
        static public Queue<Heroes> RandomizeListToQueue(List<Heroes> list)
        {
            Queue<Heroes> queue = new Queue<Heroes>();
            while (list.Count != 0)
            {
                Random random = new Random();
                int liczba = random.Next(0, list.Count);
                Heroes myhero = list[liczba];
                queue.Enqueue(myhero);
                list.Remove(myhero);
                //Console.WriteLine(myhero.Heroname);
            }
            return queue;
        }

        //Poniższa metoda powoduje randomizacje graczy i zwraca queue
        /// <summary>
        /// Losuje kolejność graczy, przy czym przyjmuje listę, ale zwraca queue
        /// </summary>
        /// <param name="list"> Lista graczy </param>
        /// <returns></returns>
        static public Queue<Player> RandomizeListToQueue(List<Player> list)
        {
            Queue<Player> queue = new Queue<Player>();
            while (list.Count != 0)
            {
                Random random = new Random();
                int liczba = random.Next(0, list.Count);
                Player myhero = list[liczba];
                //myhero.Id = liczba;
                queue.Enqueue(myhero);
                list.Remove(myhero);
                //Console.WriteLine(myhero.Heroname);
            }
            return queue;
        }

        //PM powoduje zmiane na losowe koljnosci graczy biorac queue i je zwracajac
        /// <summary>
        /// Losuje kolejność graczy przyjmując queue i je zwracając
        /// </summary>
        /// <param name="list"> kolejka graczy </param>
        /// <returns></returns>
        static public Queue<Player> RandomizePlayers(Queue<Player> list)
        {
            List<Player> myList = new List<Player>();
            for (int i = (list.Count - 1); i >= 0; i--)
            {
                myList.Add(list.ElementAt(i));
            }
            return Methods.RandomizeListToQueue(myList);
        }

        //Poniższa metoda zmienia queue z herosami na liste
        /// <summary>
        /// Losuje kolejność herosów z queue i zwraca queue
        /// </summary>
        /// <param name="list"> kolejka herosów</param>
        /// <returns></returns>
        static public List<Heroes> QueueToList(Queue<Heroes> list)
        {
            List<Heroes> returner = new List<Heroes>();
            for (int i = 0; i < list.Count; i++)
                returner.Add(list.Dequeue());
            return returner;
        }

        //PM drukuje z queue herosów wszystkie elementy
        /// <summary>
        /// drukuje z queue herosów wszystkie elementy
        /// </summary>
        /// <param name="queue"></param>
        static public void PrintQueue(Queue<Heroes> queue)
        {
            int cos = queue.Count;
            Queue<Heroes> q1 = queue;
            for (int i = 0; i < cos; i++)
                Console.WriteLine(q1.Dequeue().Heroname);
        }

        //PM drukuje z queue graczy wszystkie elementy
        /// <summary>
        /// drukuje z queue graczy wszystkie elementy
        /// </summary>
        /// <param name="queue"></param>
        static public void PrintQueue(Queue<Player> queue)
        {
            int cos = queue.Count;
            //Queue<Player> q1 = queue;
            for (int i = 0; i < cos; i++)
                Console.WriteLine(queue.ElementAt(i).Name);
        }


        //Metoda odtwarza dzwięk z pliku .wav (folder bin/debug/sounds)
        /// <summary>
        /// Metoda odtwarza dzwięk z pliku .wav (folder bin/debug/sounds)
        /// </summary>
        /// <param name="file_name"></param>
        static public void PlaySound(string file_name)
        {


            string path = Environment.CurrentDirectory;
            string path_final = path + "\\sounds\\" + file_name + ".wav";
            Console.WriteLine(path_final);

            System.Media.SoundPlayer player = new System.Media.SoundPlayer(path_final);
            player.Play();
        }

        static public void PlayMusic(string file_name, bool loop)
        {


            string path = Environment.CurrentDirectory;
            string path_final = path + "\\sounds\\" + file_name + ".wav";
            Console.WriteLine(path_final);

            myPlayer.Open(new System.Uri(path_final));
            myPlayer.Play();

            if (loop)
                myPlayer.MediaEnded += new EventHandler(MediaEnded);



            //StringBuilder sb = new StringBuilder();
            //mciSendString("open \"" + path_final + "\" alias ", sb, 0, IntPtr.Zero);
        }

        static private void MediaEnded(object sender, EventArgs e)
        {
            myPlayer.Position = TimeSpan.Zero;
            myPlayer.Play();
        }

        //PM tworzy i zwraca mapę studentpoly
        /// <summary>
        /// Tworzy i zwraca mapę studentpoly
        /// </summary>
        /// <returns></returns>
        static public Map MakeStudentpolyMap()
        {
            Dictionary<string, Territory> dicOfTerritories = new Dictionary<string, Territory>();

            #region RentLists

            Dictionary<int, int> rentList1 = new Dictionary<int, int>();
            rentList1.Add(rentList1.Count, 20);
            rentList1.Add(rentList1.Count, 100);
            rentList1.Add(rentList1.Count, 300);
            rentList1.Add(rentList1.Count, 900);
            rentList1.Add(rentList1.Count, 1600);
            rentList1.Add(rentList1.Count, 2500);

            Dictionary<int, int> rentList2 = new Dictionary<int, int>();
            rentList2.Add(rentList2.Count, 40);
            rentList2.Add(rentList2.Count, 200);
            rentList2.Add(rentList2.Count, 600);
            rentList2.Add(rentList2.Count, 1800);
            rentList2.Add(rentList2.Count, 3200);
            rentList2.Add(rentList2.Count, 4500);

            Dictionary<int, int> rentList3 = new Dictionary<int, int>();
            rentList3.Add(rentList3.Count, 60);
            rentList3.Add(rentList3.Count, 300);
            rentList3.Add(rentList3.Count, 900);
            rentList3.Add(rentList3.Count, 2700);
            rentList3.Add(rentList3.Count, 4000);
            rentList3.Add(rentList3.Count, 5500);

            Dictionary<int, int> rentList4 = new Dictionary<int, int>();
            rentList4.Add(rentList4.Count, 80);
            rentList4.Add(rentList4.Count, 400);
            rentList4.Add(rentList4.Count, 1000);
            rentList4.Add(rentList4.Count, 3000);
            rentList4.Add(rentList4.Count, 4500);
            rentList4.Add(rentList4.Count, 6000);

            Dictionary<int, int> rentList5 = new Dictionary<int, int>();
            rentList5.Add(rentList5.Count, 100);
            rentList5.Add(rentList5.Count, 500);
            rentList5.Add(rentList5.Count, 1500);
            rentList5.Add(rentList5.Count, 4500);
            rentList5.Add(rentList5.Count, 6250);
            rentList5.Add(rentList5.Count, 7500);

            Dictionary<int, int> rentList6 = new Dictionary<int, int>();
            rentList6.Add(rentList6.Count, 120);
            rentList6.Add(rentList6.Count, 600);
            rentList6.Add(rentList6.Count, 1800);
            rentList6.Add(rentList6.Count, 5000);
            rentList6.Add(rentList6.Count, 7000);
            rentList6.Add(rentList6.Count, 9000);

            Dictionary<int, int> rentList7 = new Dictionary<int, int>();
            rentList7.Add(rentList7.Count, 140);
            rentList7.Add(rentList7.Count, 700);
            rentList7.Add(rentList7.Count, 2000);
            rentList7.Add(rentList7.Count, 5500);
            rentList7.Add(rentList7.Count, 7500);
            rentList7.Add(rentList7.Count, 9500);

            Dictionary<int, int> rentList8 = new Dictionary<int, int>();
            rentList8.Add(rentList8.Count, 160);
            rentList8.Add(rentList8.Count, 800);
            rentList8.Add(rentList8.Count, 2200);
            rentList8.Add(rentList8.Count, 6000);
            rentList8.Add(rentList8.Count, 8000);
            rentList8.Add(rentList8.Count, 10000);

            Dictionary<int, int> rentList9 = new Dictionary<int, int>();
            rentList9.Add(rentList9.Count, 180);
            rentList9.Add(rentList9.Count, 900);
            rentList9.Add(rentList9.Count, 2500);
            rentList9.Add(rentList9.Count, 7000);
            rentList9.Add(rentList9.Count, 8750);
            rentList9.Add(rentList9.Count, 10500);

            Dictionary<int, int> rentList10 = new Dictionary<int, int>();
            rentList10.Add(rentList10.Count, 200);
            rentList10.Add(rentList10.Count, 1000);
            rentList10.Add(rentList10.Count, 3000);
            rentList10.Add(rentList10.Count, 7500);
            rentList10.Add(rentList10.Count, 9250);
            rentList10.Add(rentList10.Count, 11000);

            Dictionary<int, int> rentList11 = new Dictionary<int, int>();
            rentList11.Add(rentList11.Count, 220);
            rentList11.Add(rentList11.Count, 1100);
            rentList11.Add(rentList11.Count, 3300);
            rentList11.Add(rentList11.Count, 8000);
            rentList11.Add(rentList11.Count, 9750);
            rentList11.Add(rentList11.Count, 11500);

            Dictionary<int, int> rentList12 = new Dictionary<int, int>();
            rentList12.Add(rentList12.Count, 240);
            rentList12.Add(rentList12.Count, 1200);
            rentList12.Add(rentList12.Count, 3600);
            rentList12.Add(rentList12.Count, 8500);
            rentList12.Add(rentList12.Count, 10250);
            rentList12.Add(rentList12.Count, 12000);

            Dictionary<int, int> rentList13 = new Dictionary<int, int>();
            rentList13.Add(rentList13.Count, 260);
            rentList13.Add(rentList13.Count, 1300);
            rentList13.Add(rentList13.Count, 3900);
            rentList13.Add(rentList13.Count, 9000);
            rentList13.Add(rentList13.Count, 11000);
            rentList13.Add(rentList13.Count, 12750);

            Dictionary<int, int> rentList14 = new Dictionary<int, int>();
            rentList14.Add(rentList14.Count, 280);
            rentList14.Add(rentList14.Count, 1500);
            rentList14.Add(rentList14.Count, 4500);
            rentList14.Add(rentList14.Count, 10000);
            rentList14.Add(rentList14.Count, 12000);
            rentList14.Add(rentList14.Count, 14000);

            Dictionary<int, int> rentList15 = new Dictionary<int, int>();
            rentList15.Add(rentList15.Count, 350);
            rentList15.Add(rentList15.Count, 1750);
            rentList15.Add(rentList15.Count, 5000);
            rentList15.Add(rentList15.Count, 11000);
            rentList15.Add(rentList15.Count, 13000);
            rentList15.Add(rentList15.Count, 15000);

            Dictionary<int, int> rentList16 = new Dictionary<int, int>();
            rentList16.Add(rentList16.Count, 500);
            rentList16.Add(rentList16.Count, 2000);
            rentList16.Add(rentList16.Count, 6000);
            rentList16.Add(rentList16.Count, 14000);
            rentList16.Add(rentList16.Count, 17000);
            rentList16.Add(rentList16.Count, 20000);

            Dictionary<int, int> rentList17 = new Dictionary<int, int>();
            rentList17.Add(rentList17.Count, 250);
            rentList17.Add(rentList17.Count, 500);
            rentList17.Add(rentList17.Count, 1000);
            rentList17.Add(rentList17.Count, 2000);

            Dictionary<int, int> rentList18 = new Dictionary<int, int>();
            rentList18.Add(rentList18.Count, 40);
            rentList18.Add(rentList18.Count, 100);

            #endregion RentLists

            #region Dictionaries

            Dictionary<string, string> bonusRentList18 = new Dictionary<string, string>();
            bonusRentList18.Add("RTL", "Z00000D99");
            Dictionary<string, string> emptyDic = new Dictionary<string, string>();
            Dictionary<string, string> prospectBonus = new Dictionary<string, string>();
            Dictionary<string, string> socialBonus = new Dictionary<string, string>();
            Dictionary<string, string> tax200 = new Dictionary<string, string>();
            Dictionary<string, string> tax100 = new Dictionary<string, string>();
            Dictionary<string, string> park = new Dictionary<string, string>();
            Dictionary<string, string> pris = new Dictionary<string, string>();
            Dictionary<string, string> strt = new Dictionary<string, string>();
            prospectBonus.Add("TCP", "Z00001D00");
            socialBonus.Add("TCS", "Z00001D00");
            tax200.Add("MON", "Z02000U00");
            tax100.Add("MON", "Z01000U00");
            park.Add("PAR", "Z00000U00");
            pris.Add("ARE", "Z00500U03");
            strt.Add("MFS", "Z02000U00");
            Heroes banker = new Heroes("Banker", 999999, emptyDic);
            Player bank = new Player(999999, banker, Color.white, "Banker", "000");
            Player bot = new Player(999999, banker, Color.white, "Bot", "000");

            #endregion Dictionaries

            #region TerritoriesMaking

            Territory Tile0 = new Territory("000", strt, bot, 0, "Start");
            Street Tile1 = new Street(500, 500, 600, 300, rentList1, "001", emptyDic, bank, 0, "IiE");
            Territory Tile2 = new Territory("002", socialBonus, bot, 0, "Kasa społeczna");
            Street Tile3 = new Street(500, 500, 600, 300, rentList2, "003", emptyDic, bank, 0, "Zarządzenie");
            Territory Tile4 = new Territory("004", tax200, bot, 0, "Podatek 2000");
            Station Tile5 = new Station(2000, 1000, rentList17, "005", emptyDic, bank, 0, "Dworzec Południowy");
            Street Tile6 = new Street(500, 500, 1000, 500, rentList3, "006", emptyDic, bank, 0, "cyberbezpieczeństwo");
            Territory Tile7 = new Territory("007", prospectBonus, bot, 0, "Szansa");
            Street Tile8 = new Street(500, 500, 1000, 500, rentList3, "008", emptyDic, bank, 0, "Teleinformatyka");
            Street Tile9 = new Street(500, 500, 1200, 600, rentList4, "009", emptyDic, bank, 0, "Elektronik");
            Territory Tile10 = new Territory("010", emptyDic, bot, 0, "Pomoc zkacowanym");
            Street Tile11 = new Street(1000, 1000, 1400, 700, rentList5, "011", emptyDic, bank, 0, "Inżynieria akustyczna");
            Station Tile12 = new Station(1500, 750, rentList18, "012", bonusRentList18, bank, 0, "Browar Górniczo-Hutniczy");
            Street Tile13 = new Street(1000, 1000, 1400, 700, rentList5, "013", emptyDic, bank, 0, "Automatyka przemyslowa i robotyka");
            Street Tile14 = new Street(1000, 1000, 1600, 800, rentList6, "014", emptyDic, bank, 0, "Mechanika i budowa maszyn");
            Station Tile15 = new Station(2000, 1000, rentList17, "015", emptyDic, bank, 0, "Dworzec Zachodni");
            Street Tile16 = new Street(1000, 1000, 1800, 900, rentList7, "016", emptyDic, bank, 0, "Fizyka medyczna");
            Territory Tile17 = new Territory("017", socialBonus, bot, 0, "Kasa społeczna");
            Street Tile18 = new Street(1000, 1000, 1800, 900, rentList7, "018", emptyDic, bank, 0, "Fizyka techniczna");
            Street Tile19 = new Street(1000, 1000, 2000, 1000, rentList8, "019", emptyDic, bank, 0, "Informatyka stosowana");
            Territory Tile20 = new Territory("020", park, bot, 0, "Parking");
            Street Tile21 = new Street(1500, 1500, 2200, 1100, rentList9, "021", emptyDic, bank, 0, "Geodezja i Kartografia");
            Territory Tile22 = new Territory("022", prospectBonus, bot, 0, "Szansa");
            Street Tile23 = new Street(1500, 1500, 2200, 1100, rentList9, "023", emptyDic, bank, 0, "Geoinformacja");
            Street Tile24 = new Street(1500, 1500, 2400, 1200, rentList10, "024", emptyDic, bank, 0, "Inżynieria i monitoring środowiska");
            Station Tile25 = new Station(2000, 1000, rentList17, "025", emptyDic, bank, 0, "Dworzec Północny");
            Street Tile26 = new Street(1500, 1500, 2600, 1300, rentList11, "026", emptyDic, bank, 0, "Edukacja techniczno-informatyczna");
            Street Tile27 = new Street(1500, 1500, 2600, 1300, rentList11, "027", emptyDic, bank, 0, "Inżynieria obliczeniowa");
            Station Tile28 = new Station(1500, 750, rentList18, "028", bonusRentList18, bank, 0, "Klub Studio");
            Street Tile29 = new Street(1500, 1500, 2800, 1400, rentList12, "029", emptyDic, bank, 0, "Inżynieria Ciepła");
            Territory Tile30 = new Territory("030", pris, bot, 0, "Idź na kaca");
            Street Tile31 = new Street(2000, 2000, 3000, 1500, rentList13, "031", emptyDic, bank, 0, "Informatyka społeczna");
            Street Tile32 = new Street(2000, 2000, 3000, 1500, rentList13, "032", emptyDic, bank, 0, "Kulturoznawstwo");
            Territory Tile33 = new Territory("033", socialBonus, bot, 0, "Kasa społeczna");
            Street Tile34 = new Street(2000, 2000, 3200, 1600, rentList14, "034", emptyDic, bank, 0, "Socjologia");
            Station Tile35 = new Station(2000, 1000, rentList17, "035", emptyDic, bank, 0, "Dworzec Wschodni");
            Territory Tile36 = new Territory("036", prospectBonus, bot, 0, "Szansa");
            Street Tile37 = new Street(2000, 2000, 3500, 1750, rentList15, "037", emptyDic, bank, 0, "Inżynieria Naftowa i klasowa");
            Territory Tile38 = new Territory("038", tax100, bot, 0, "Podatek 1000");
            Street Tile39 = new Street(2000, 2000, 4000, 2000, rentList16, "039", emptyDic, bank, 0, "Geoinżynieria i Górnictwo Otworowe");
            Territory Tile40 = new Territory("040", emptyDic, bot, 0, "Kac");

            dicOfTerritories.Add(Tile0.Coordinates, Tile0);
            dicOfTerritories.Add(Tile1.Coordinates, Tile1);
            dicOfTerritories.Add(Tile2.Coordinates, Tile2);
            dicOfTerritories.Add(Tile3.Coordinates, Tile3);
            dicOfTerritories.Add(Tile4.Coordinates, Tile4);
            dicOfTerritories.Add(Tile5.Coordinates, Tile5);
            dicOfTerritories.Add(Tile6.Coordinates, Tile6);
            dicOfTerritories.Add(Tile7.Coordinates, Tile7);
            dicOfTerritories.Add(Tile8.Coordinates, Tile8);
            dicOfTerritories.Add(Tile9.Coordinates, Tile9);
            dicOfTerritories.Add(Tile10.Coordinates, Tile10);
            dicOfTerritories.Add(Tile11.Coordinates, Tile11);
            dicOfTerritories.Add(Tile12.Coordinates, Tile12);
            dicOfTerritories.Add(Tile13.Coordinates, Tile13);
            dicOfTerritories.Add(Tile14.Coordinates, Tile14);
            dicOfTerritories.Add(Tile15.Coordinates, Tile15);
            dicOfTerritories.Add(Tile16.Coordinates, Tile16);
            dicOfTerritories.Add(Tile17.Coordinates, Tile17);
            dicOfTerritories.Add(Tile18.Coordinates, Tile18);
            dicOfTerritories.Add(Tile19.Coordinates, Tile19);
            dicOfTerritories.Add(Tile20.Coordinates, Tile20);
            dicOfTerritories.Add(Tile21.Coordinates, Tile21);
            dicOfTerritories.Add(Tile22.Coordinates, Tile22);
            dicOfTerritories.Add(Tile23.Coordinates, Tile23);
            dicOfTerritories.Add(Tile24.Coordinates, Tile24);
            dicOfTerritories.Add(Tile25.Coordinates, Tile25);
            dicOfTerritories.Add(Tile26.Coordinates, Tile26);
            dicOfTerritories.Add(Tile27.Coordinates, Tile27);
            dicOfTerritories.Add(Tile28.Coordinates, Tile28);
            dicOfTerritories.Add(Tile29.Coordinates, Tile29);
            dicOfTerritories.Add(Tile30.Coordinates, Tile30);
            dicOfTerritories.Add(Tile31.Coordinates, Tile31);
            dicOfTerritories.Add(Tile32.Coordinates, Tile32);
            dicOfTerritories.Add(Tile33.Coordinates, Tile33);
            dicOfTerritories.Add(Tile34.Coordinates, Tile34);
            dicOfTerritories.Add(Tile35.Coordinates, Tile35);
            dicOfTerritories.Add(Tile36.Coordinates, Tile36);
            dicOfTerritories.Add(Tile37.Coordinates, Tile37);
            dicOfTerritories.Add(Tile38.Coordinates, Tile38);
            dicOfTerritories.Add(Tile39.Coordinates, Tile39);
            dicOfTerritories.Add(Tile40.Coordinates, Tile40);

            #endregion TerritoriesMaking

            Dictionary<int, int> defaultMoney = new Dictionary<int, int>();
            defaultMoney.Add(0, 15000);
            defaultMoney.Add(1, 15000);
            defaultMoney.Add(2, 15000);
            defaultMoney.Add(3, 15000);
            defaultMoney.Add(4, 15000);
            defaultMoney.Add(5, 15000);

            #region CardsMaking

            Dictionary<string, string> dicchance0 = new Dictionary<string, string>();
            Dictionary<string, string> dicchance1 = new Dictionary<string, string>();
            Dictionary<string, string> dicchance2 = new Dictionary<string, string>();
            Dictionary<string, string> dicchance3 = new Dictionary<string, string>();
            dicchance0.Add("MON", "Z00100D00");
            dicchance1.Add("MON", "Z00150D00");
            dicchance2.Add("MON", "Z00050D00");
            dicchance3.Add("CIE", "FFFFFFT99");
            Card chance0 = new Card(dicchance0, "Pomogles koledze w nauce do kolokwium, donrze mu poszło wiec on ucieszony zapłacił Ci 100zl");
            Card chance1 = new Card(dicchance1, "Oddałeś do recepcji znalezione klucze, otrzymujesz znaleźne 150zl");
            Card chance2 = new Card(dicchance2, "Wracajac z imprezy znalazłeś na ziemi 50zl");
            Card chance3 = new Card(dicchance3, "Dostałeś od kolego super elektrolity dzięki którym możesz raz ominąć kaca");
            Dictionary<int, Card> prospect = new Dictionary<int, Card>();
            prospect.Add(0, chance0);
            prospect.Add(1, chance1);
            prospect.Add(2, chance2);
            prospect.Add(3, chance3);

            Dictionary<string, string> dicsocial0 = new Dictionary<string, string>();
            Dictionary<string, string> dicsocial1 = new Dictionary<string, string>();
            Dictionary<string, string> dicsocial2 = new Dictionary<string, string>();
            dicsocial0.Add("MON", "Z00300U00");
            dicsocial1.Add("MON", "Z00050U00");
            dicsocial2.Add("MON", "Z00100U00");
            Card social0 = new Card(dicsocial0, "Niestety potknęła Ci się noga na egzaminach i musisz zapłacić 300 zł za warunek ");
            Card social1 = new Card(dicsocial1, "Musisz postawić koledze pizzę i piwo za pomoc w projekcie, płacisz za to 50 zl");
            Card social2 = new Card(dicsocial2, "Skończyły Ci się wszytskie kosmetyki, musisz uzupełnić zapasy, idziesz do sklepu i wydajesz 100zl");
            Dictionary<int, Card> socialTreasury = new Dictionary<int, Card>();
            socialTreasury.Add(0, social0);
            socialTreasury.Add(1, social1);
            socialTreasury.Add(2, social2);

            Dictionary<int, int> moneyForPrison = new Dictionary<int, int>();
            moneyForPrison.Add(0, 500);

            #endregion CardsMaking

            #region AllyMaking

            Dictionary<Territory, int> alliances = new Dictionary<Territory, int>();
            alliances.Add(Tile0, 0);
            alliances.Add(Tile1, 3);
            alliances.Add(Tile2, 0);
            alliances.Add(Tile3, 3);
            alliances.Add(Tile4, 0);
            alliances.Add(Tile5, 1);
            alliances.Add(Tile6, 4);
            alliances.Add(Tile7, 0);
            alliances.Add(Tile8, 4);
            alliances.Add(Tile9, 4);
            alliances.Add(Tile10, 0);
            alliances.Add(Tile11, 5);
            alliances.Add(Tile12, 2);
            alliances.Add(Tile13, 5);
            alliances.Add(Tile14, 5);
            alliances.Add(Tile15, 1);
            alliances.Add(Tile16, 6);
            alliances.Add(Tile17, 0);
            alliances.Add(Tile18, 6);
            alliances.Add(Tile19, 6);
            alliances.Add(Tile20, 0);
            alliances.Add(Tile21, 7);
            alliances.Add(Tile22, 0);
            alliances.Add(Tile23, 7);
            alliances.Add(Tile24, 7);
            alliances.Add(Tile25, 1);
            alliances.Add(Tile26, 8);
            alliances.Add(Tile27, 8);
            alliances.Add(Tile28, 2);
            alliances.Add(Tile29, 8);
            alliances.Add(Tile30, 0);
            alliances.Add(Tile31, 9);
            alliances.Add(Tile32, 9);
            alliances.Add(Tile33, 0);
            alliances.Add(Tile34, 9);
            alliances.Add(Tile35, 1);
            alliances.Add(Tile36, 0);
            alliances.Add(Tile37, 10);
            alliances.Add(Tile38, 0);
            alliances.Add(Tile39, 10);
            alliances.Add(Tile40, 0);

            #endregion AllyMaking

            Dictionary<int, int> moneyForStart = new Dictionary<int, int>();
            moneyForStart.Add(0, 2000);
            moneyForStart.Add(1, 2000);
            moneyForStart.Add(2, 2000);

            Dictionary<int, int> rings = new Dictionary<int, int>();
            rings.Add(0, 39);

            Map studentpoly = new Map("Studentopoly", dicOfTerritories, defaultMoney, prospect, socialTreasury, moneyForPrison, alliances, moneyForStart, rings, "040", "010", bank, "000",6);
            return studentpoly;
        }

        //PM tworzy prePlayerów w takiej ilości ile chce użytkownik
        /// <summary>
        /// Tworzy prePlayerów w takiej ilości ile chce użytkownik
        /// </summary>
        /// <param name="i">nr prePlayera</param>
        /// <returns></returns>
        static public string AskForPrePlayers(int i)
        {
            Console.WriteLine($"Wprowadź nazwę gracza {i}: ");
            return (string)Console.ReadLine();
        }

        //PM tworzy rozgrywke przy pomocy konsoli.
        /// <summary>
        /// Tworzy rozgrywke przy pomocy konsoli.
        /// </summary>
        /// <param name="map">mapa na której chcesz zagrać</param>
        /// <param name="hero">heros jaki gracze mają (w przyszłości powinno być wprowadzane przez konsolę / gui)</param>
        /// <returns></returns>
        static public Game SetGameWithConsole(Map map, Heroes hero)
        {
            int num = 1;
            PrePlayer[] preGamer = new PrePlayer[1];
            for (int i = 0; i < num; i++)
            {
                preGamer[i] = new PrePlayer(Methods.AskForPrePlayers(num));
                Console.WriteLine("Chcesz dodać kolejnego gracza? (napisz y i zatwierdź enterem)");
                string varia = Console.ReadLine();
                if (varia == "y" | varia == "Y")
                {
                    num++;
                    PrePlayer[] preLamer = new PrePlayer[num];
                    for (int j = 0; j < num - 1; j++)
                    {
                        preLamer[j] = preGamer[j];
                    }
                    preGamer = new PrePlayer[num];
                    for (int j = 0; j < num - 1; j++)
                    {
                        preGamer[j] = preLamer[j];
                    }
                }
            }
            //Console.WriteLine(preGamer.Count());
            Player[] gamers = new Player[num];
            for (int i = 0; i < num; i++)
            {
                int varia;
                Console.WriteLine($"Ile pieniędzy ma mieć gracz {preGamer[i].Name}?");
                varia = Int32.Parse(Console.ReadLine());
                //Console.WriteLine(varia);
                gamers[i] = new Player(varia, hero, Color.red, preGamer[i].Name, map.StartCoordinates);
            }

            Console.WriteLine("Chcesz grać aż pozostali zbankrutują? (wpisz y jeśli tak)");
            string variab = Console.ReadLine();
            if (variab == "y" | variab == "Y")
            {
                int howLongWeWantPlay = 31536000;
                Queue<Player> playerQueue = new Queue<Player>();
                for (int i = 0; i < num; i++)
                {
                    playerQueue.Enqueue(gamers[i]);
                }
                Console.WriteLine("Czy odpowiada ci taka kolejność graczy? (y jeśli tak)");
                Methods.PrintQueue(playerQueue);
                variab = Console.ReadLine();
                while (variab != "y" & variab != "Y")
                {
                    playerQueue = Methods.RandomizePlayers(playerQueue);
                    Console.WriteLine("Czy odpowiada ci taka kolejność graczy? (y jeśli tak)");
                    Methods.PrintQueue(playerQueue);
                    variab = Console.ReadLine();
                }
                Game gra = new Game(map, playerQueue, howLongWeWantPlay);
                return gra;
            }
            else
            {
                int hours, minutes;
                Console.WriteLine($"Ile godzin chcecie grać?");
                hours = Int32.Parse(Console.ReadLine());
                Console.WriteLine($"Ile minut chcecie grać?");
                minutes = Int32.Parse(Console.ReadLine());
                int howLongWeWantPlay = ((hours * 60) + minutes) * 60;

                Queue<Player> playerQueue = new Queue<Player>();
                for (int i = 0; i < num; i++)
                {
                    playerQueue.Enqueue(gamers[i]);
                }
                Console.WriteLine("Czy odpowiada ci taka kolejność graczy? (y jeśli tak)");
                Methods.PrintQueue(playerQueue);
                variab = Console.ReadLine();
                while (variab != "y" & variab != "Y")
                {
                    playerQueue = Methods.RandomizePlayers(playerQueue);
                    Console.WriteLine("Czy odpowiada ci taka kolejność graczy? (y jeśli tak)");
                    Methods.PrintQueue(playerQueue);
                    variab = Console.ReadLine();
                }
                Game gra = new Game(map, playerQueue, howLongWeWantPlay);
                return gra;
            }
        }

        //PM sprawdza ile graczy nie zbankrutowało
        /// <summary>
        /// Ile graczy nie ma ujemnych pieniędzy? (Ujemne pieniądze oznaczają bankructwo gracza  co oznacza jego eliminację z dalszej gry)
        /// </summary>
        /// <param name="game">tocząca się rozgrywka</param>
        /// <returns></returns>
        static public int HowMuchPlayerHaveMoney(Game game)
        {
            int returner = 0;
            foreach (Player item in game.Players)
            {
                if (item.Money >= 0)
                    returner++;
            }
            return returner;
        }

        //PM dodaje pieniądze zapisane w bonusach które graczowi się po prostu należą.
        /// <summary>
        /// Metoda ta dodaje pieniądze zapisane w bonusach które graczowi się po prostu należą. (W przypadku bonusu iloczynowego wymnaża)
        /// </summary>
        /// <param name="player"></param>
        static public void AddMoneyFromMON(Player player)
        {
            string bonus = "";
            bool isTrue = player.PlayerBonuses.TryGetValue("MON", out bonus);
            if (isTrue)
                player.Money = (int)Methods.ChangeValue(player.Money, bonus);
        }

        //PM zwraca ilość tych samych liter we wprowadzanym bonusie
        /// <summary>
        /// Metoda zwraca ilość tych samych liter we wprowadzanym bonusie
        /// </summary>
        /// <param name="bonus"> bonus</param>
        /// <param name="letter"> znak</param>
        /// <returns></returns>
        static public int HowMuchLetterInBonusString(string bonus, char letter)
        {
            int returner = 0;
            int abc = Methods.HowMuchIsValues(bonus);
            for (int i = 0; i < abc; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (bonus[i * 9 + j] == letter)
                        returner++;
                }
            }
            return returner;
        }
        /*Test:
        Console.WriteLine(Methods.HowMuchLetterInBonusString("FFFFFFT99", 'T'));
        Console.WriteLine(Methods.HowMuchLetterInBonusString("FFFFFFT99", 'F'));
        Console.WriteLine(Methods.HowMuchLetterInBonusString("FFFFFFT99FFFFFFT99", 'F'));*/

        //PM dodaje poziomy wyjścia z więzienia o nieokreślonym czasie
        /// <summary>
        /// Dodaje wartości ile ma się kart bezpłatnego wyjścia z więzienia. Ge karty mają nieograniczony czas trwania
        /// </summary>
        /// <param name="player">gracz, którego bonusy zmieniamy</param>
        /// <param name="bonus">dodawany bonus</param>
        static public void AddPrisonEscape(Player player, string bonus)
        {
            string bon;
            bool forif = player.PlayerBonuses.TryGetValue("CIE", out bon);
            if (forif)
            {
                int assist = Methods.HowMuchLetterInBonusString(bon, 'T');
                int assist2 = Methods.HowMuchLetterInBonusString(bonus, 'T');
                int sum = assist + assist2;
                bon = "";
                for (int k = 0; k < (int)(sum / 7); k++)
                {
                    bon += "TTTTTTT99";
                }
                if (sum % 7 != 0)
                {
                    string bon2 = "FFFFFFF99";
                    for (int k = 0; k < sum % 7; k++)
                    {
                        bon2 = Methods.ChangeOneLetterInString(bon2, 'T', (6 - k));
                    }
                    bon += bon2;
                    player.PlayerBonuses["CIE"] = bon;
                }
                else
                    player.PlayerBonuses["CIE"] = bon;
            }
            else
                player.PlayerBonuses.Add("CIE", bonus);
        }
        /*  Test
            Player gracz = new Player(0, null, Color.red, "Gracz");
            Methods.AddPrisonEscape(gracz, "FFFFFFT99");
            Console.WriteLine(gracz.PlayerBonuses["CIE"]);
            Methods.AddPrisonEscape(gracz, "FFFTTTT99");
            Console.WriteLine(gracz.PlayerBonuses["CIE"]);
            Methods.AddPrisonEscape(gracz, "FFTTTTT99");
            Console.WriteLine(gracz.PlayerBonuses["CIE"]);
            Methods.AddPrisonEscape(gracz, "FTTTTTT99");
            Console.WriteLine(gracz.PlayerBonuses["CIE"]);*/

        //Usuwa jedną kartę wyjdź z więzienia
        /// <summary>
        /// Usuwa jedną kartę wyjdź z więzienia
        /// </summary>
        /// <param name="player">wybierz gracza</param>
        static public void DeletePrisonEscape(Player player)
        {
            string textval;
            bool wywalacz = player.PlayerBonuses.TryGetValue("CIE", out textval);
            if (wywalacz)
            {
                string returner = "";
                int val = Methods.HowMuchLetterInBonusString(textval, 'T');
                if (val == 1)
                {
                    player.PlayerBonuses.Remove("CIE");
                }
                else if (val % 7 > 1)
                {
                    returner = Methods.ChangeOneLetterInString(textval, 'F', ((int)(val / 7) * 9 + 9 - 2 - (val % 7)));
                    player.PlayerBonuses["CIE"] = returner;
                }
                else if (val % 7 == 0)
                {
                    returner = Methods.ChangeOneLetterInString(textval, 'F', ((int)(val / 7) * 9 - 2 - (val % 7)));
                    player.PlayerBonuses["CIE"] = returner;
                }
                else
                {
                    returner = textval.Remove(textval.Length - 9, 9);
                    player.PlayerBonuses["CIE"] = returner;
                }
            }
            else
                Console.WriteLine("Najprawdopodobniej użyto karty której gracz nie posiadał");
        }
        /*  Test
            Player gracz = new Player(0, null, Color.red, "Gracz");
            Methods.AddPrisonEscape(gracz, "FFFFFFT99");
            Methods.DeletePrisonEscape(gracz);
            //Console.WriteLine(gracz.PlayerBonuses["CIE"]);
            Methods.AddPrisonEscape(gracz, "FFFTTTT99");
            Methods.DeletePrisonEscape(gracz);
            Console.WriteLine(gracz.PlayerBonuses["CIE"]);
            Methods.AddPrisonEscape(gracz, "FFTTTTT99");
            Methods.DeletePrisonEscape(gracz);
            Console.WriteLine(gracz.PlayerBonuses["CIE"]);
            Methods.AddPrisonEscape(gracz, "FTTTTTT99");
            Methods.DeletePrisonEscape(gracz);
            Console.WriteLine(gracz.PlayerBonuses["CIE"]);
            Methods.AddPrisonEscape(gracz, "FFFFTTT99");
            Methods.DeletePrisonEscape(gracz);
            Console.WriteLine(gracz.PlayerBonuses["CIE"]); */

        //PM modyfikuje wszystko graczowi tak, żeby trafił do więzienia w tradycyjnych ustawieniach
        /// <summary>
        /// Ustawiam gracza do więzienia. Zmieniam bonusy żeby miał areszt o koszcie z dictionary mapy i pytam czy chce użyć karty wyjdź z więzienia, którą to czynność też implementuję
        /// </summary>
        /// <param name="game">tocząca się gra</param>
        /// <exception cref="WrongBonusException">Coś poszło nie tak przy zapisie bonusu. Możliwe, że kwota za wyjście z więzienia jest więcej jak pięciocyfrowa, czego ta metoda nie dopuszcza</exception>
        static public void GoToPrison(Game game)
        {
            int ring = Methods.ReturnRingFromString(game.Players.Peek().Coordinates);
            game.Players.Peek().Coordinates = game.Map.PrisonCoordinates;
            string bonuses;
            bool fake = game.Players.Peek().PlayerBonuses.TryGetValue("CIE", out bonuses);
            if (!fake)
            {
                string v2 = "Z";
                for (int i = 0; i < 5 - game.Map.MoneyForPrison[ring].ToString().Length; i++)
                {
                    v2 += "0";
                }
                v2 += game.Map.MoneyForPrison[ring].ToString() + "U03";
                if (v2.Length != 9)
                    throw new WrongBonusException(v2);
                game.Players.Peek().PlayerBonuses.Add("ARE", v2);
            }
            else
            {
                Console.WriteLine($"Chcesz użyć karty i wyjść z tego pola? (wpisz y jeśli tak). Masz obecnie {Methods.HowMuchLetterInBonusString(bonuses, 'T')} takich kart");
                string var1 = Console.ReadLine();
                if (var1 == "y" | var1 == "Y")
                {
                    Methods.DeletePrisonEscape(game.Players.Peek());
                    game.Players.Peek().Coordinates = game.Map.PrisonVisiterCoordinates;
                }
                else
                {
                    string v2 = "Z";
                    for (int i = 0; i < 5 - game.Map.MoneyForPrison[ring].ToString().Length; i++)
                    {
                        v2 += "0";
                    }
                    v2 += game.Map.MoneyForPrison[ring].ToString() + "U03";
                    if (v2.Length != 9)
                        throw new WrongBonusException(v2);
                    game.Players.Peek().PlayerBonuses.Add("ARE", v2);
                }
            }
        }
        //Test nie sprawdza kordów gracza. Trzeba to jeszcze sprawdzić
        /*Test
            Heroes counter = new Heroes("Counter", 15000, new Dictionary<string, string>());
            Map studentpoly = Methods.MakeStudentpolyMap(); //generowanie mapy studentpoly jest w metodzie w klasie Methods
            Game gameplay = Methods.SetGameWithConsole(studentpoly, counter); //generowanie gameplayu
            gameplay.Players.Peek().Coordinates = "025";
            Methods.GoToPrison(gameplay);
            Console.WriteLine(gameplay.Players.Peek().PlayerBonuses["ARE"]);*/

        //PM robi symulacje rzutu kośćmi i rusza gracza. A jak trzeba to wrzuca do więzienia jak przekroczy dopuszczalną ilość ruchu (za dużo będzie dubli). UWAGA: metoda ta musi być w jakimś do whileu dopóki IsDouble == true
        /// <summary>
        /// Metoda robi symulację rzutu kośćmi. Rusza gracza a następnie wywołuje następstwa po wejściu na każde pole. Metoda sama dodaje pieniądze graczom, którzy przekroczą lub wkroczą na start. (kwotę ustaloną w mapie) Dodatkowo wysyła gracza do więzienia, jeśli jest to potrzebne
        /// </summary>
        /// <param name="game">tocząca się gra</param>
        static public void PlayerRandomMove(Game game)
        {
            string bonus;
            bool fake = game.Players.Peek().PlayerBonuses.TryGetValue("ARE", out bonus);
            if (!fake)
            {
                game.ThrowingDices();
                int ring, coords, maxcoords;
                (ring, coords) = Methods.ReturnCoordsFromString(game.Players.Peek().Coordinates);
                maxcoords = game.Map.DictionaryOfRings[ring];
                fake = (((game.HowMuchMove + coords) % maxcoords) != (game.HowMuchMove + coords) & ((game.HowMuchMove + coords) % maxcoords) != 0);
                for (int i = 0; i < game.HowMuchMove; i++)
                {
                    coords = (coords + 1) % maxcoords;
                    game.Players.Peek().Coordinates = Methods.SaveCoordsToString(ring, coords);
                    //Main
                }
                Methods.StepInTile(game);
                if (fake)
                    Methods.AddMoneyForStart(game);
                fake = game.Players.Peek().PlayerBonuses.TryGetValue("HMD", out bonus);
                if (bonus == "Z00003U00")
                {
                    Methods.GoToPrison(game);
                    game.IsDouble = false;
                }
            }
            else
            {
                game.ThrowingDices();
                if (game.IsDouble)
                {
                    Methods.OutingFromPrison(game);
                    Methods.StepInTile(game);
                }
                else if (Methods.CharToInt(bonus[8]) != 0)
                {
                    string bonuses;
                    bool fake2 = game.Players.Peek().PlayerBonuses.TryGetValue("CIE", out bonuses);
                    if (!fake2)
                    {
                        int moneyForEscape = (int)Methods.ChangeValue(0M, bonus);
                        Console.WriteLine($"Chcesz zapłacić {moneyForEscape} i wyjść z tego pola? (wpisz y jeśli tak). Masz obecnie {game.Players.Peek().Money} pieniędzy");
                        string var1 = Console.ReadLine();
                        if (var1 == "y" | var1 == "Y")
                        {
                            if ((game.Players.Peek().Money + moneyForEscape) >= 0)
                            {
                                game.Players.Peek().Money = +moneyForEscape;
                                Methods.OutingFromPrison(game);
                                Methods.StepInTile(game);
                            }
                            else
                            {
                                Console.WriteLine("Ale cię nie stać");
                                Console.WriteLine($"Chcesz inaczej pozyskać {moneyForEscape} i wyjść z tego pola? (wpisz y jeśli tak). Masz obecnie {game.Players.Peek().Money} pieniędzy");
                                var1 = Console.ReadLine();
                                if (var1 == "y" | var1 == "Y")
                                {
                                    Methods.NotEnoughMoney(game, game.Players.Peek());
                                    if ((game.Players.Peek().Money + moneyForEscape) >= 0)
                                    {
                                        game.Players.Peek().Money = +moneyForEscape;
                                        Methods.OutingFromPrison(game);
                                        Methods.StepInTile(game);
                                    }
                                    else
                                        Console.WriteLine("No niestety nie możesz");
                                }
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine($"Chcesz użyć karty i wyjść z tego pola? (wpisz y jeśli tak). Masz obecnie {Methods.HowMuchLetterInBonusString(bonuses, 'T')} takich kart");
                        string var1 = Console.ReadLine();
                        if (var1 == "y" | var1 == "Y")
                        {
                            Methods.DeletePrisonEscape(game.Players.Peek());
                            Methods.OutingFromPrison(game);
                            Methods.StepInTile(game);
                        }
                    }
                }
                else
                {
                    string bonuses;
                    bool fake2 = game.Players.Peek().PlayerBonuses.TryGetValue("CIE", out bonuses);
                    if (!fake2)
                    {
                        int moneyForEscape = (int)Methods.ChangeValue(0M, bonus);
                        if ((game.Players.Peek().Money + moneyForEscape) >= 0)
                        {
                            game.Players.Peek().Money += moneyForEscape;
                            Methods.OutingFromPrison(game);
                            Methods.StepInTile(game);
                        }
                        else
                        {
                            Console.WriteLine("Nie masz wystarczająco pieniędzy");
                            Methods.NotEnoughMoney(game, game.Players.Peek());
                            if ((game.Players.Peek().Money + moneyForEscape) >= 0)
                            {
                                game.Players.Peek().Money += moneyForEscape;
                                Methods.OutingFromPrison(game);
                                Methods.StepInTile(game);
                            }
                            else
                            {
                                Methods.Bailiff(game, game.Players.Peek(), (moneyForEscape - game.Players.Peek().Money));
                                if ((game.Players.Peek().Money + moneyForEscape) >= 0)
                                {
                                    game.Players.Peek().Money += moneyForEscape;
                                    Methods.OutingFromPrison(game);
                                    Methods.StepInTile(game);
                                }
                                else
                                {
                                    game.Players.Peek().Money = -1;
                                    Console.WriteLine($"Gracz {game.Players.Peek().Name} zbankrutował");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Chcesz użyć karty i wyjść z tego pola? (wpisz y jeśli tak). Masz obecnie {Methods.HowMuchLetterInBonusString(bonuses, 'T')} takich kart. W przeciwnym wypadku pobierzemy od ciebie opłatę");
                        string var1 = Console.ReadLine();
                        if (var1 == "y" | var1 == "Y")
                        {
                            Methods.DeletePrisonEscape(game.Players.Peek());
                            Methods.OutingFromPrison(game);
                            Methods.StepInTile(game);
                        }
                        else
                        {
                            int moneyForEscape = (int)Methods.ChangeValue(0M, bonus);
                            if ((game.Players.Peek().Money + moneyForEscape) >= 0)
                            {
                                game.Players.Peek().Money = +moneyForEscape;
                                Methods.OutingFromPrison(game);
                                Methods.StepInTile(game);
                            }
                            else
                            {
                                Console.WriteLine("Ale cię nie stać. Z tego powodu wykorzystam twoją kartę");
                                Methods.DeletePrisonEscape(game.Players.Peek());
                                Methods.OutingFromPrison(game);
                                Methods.StepInTile(game);
                            }
                        }
                    }
                }
            }
        }

        //PM wychodzenie z wiezienia czysta metoda
        /// <summary>
        /// Metoda zmiany kordów gracza i symulacji jego ruchu. Uwaga: nie pobiera pieniędzy od gracza ani karty wyjścia z więzienia kiedy trzeba. Jest to sam ruch gracza po mapie w momencie kiedy był w więzieniu
        /// </summary>
        /// <param name="game">tocząca się gra</param>
        static public void OutingFromPrison(Game game)
        {
            game.Players.Peek().Coordinates = game.Map.PrisonVisiterCoordinates;
            int ring, coords, maxcoords;
            (ring, coords) = Methods.ReturnCoordsFromString(game.Players.Peek().Coordinates);
            maxcoords = game.Map.DictionaryOfRings[ring];
            for (int i = 0; i < game.HowMuchMove; i++)
            {
                coords = (coords + 1) % maxcoords;
                game.Players.Peek().Coordinates = Methods.SaveCoordsToString(ring, coords);
                //miejsce na implementację zmiany pozycji pionka w gui. Można dodać sleepa
            }
        }

        // PM wykonuje odpowiednie czynności po wejściu na pole. tzn płaci czynsz, idzie do więzienia, stawia domki itp.
        /// <summary>
        /// Metoda symuluje wszystkie czynności zachodzące podczas wejścia na pole. TZN. Jeżeli wejdziesz na pole typu ulica czy stacja to sprawdza czyje i daje możliwości takie jakie gracz ma na takim polu. Jak wejdziesz na szansę to bierzesz kartę szansa, jak parking to możesz się budować, jak start to daje ci pieniądze za wejście na start itd.
        /// </summary>
        /// <param name="game">Gra, ale uwaga. bierze gracza z góry kolejki (game.Players.Peek())</param>
        static public void StepInTile(Game game)
        {
            Territory stepIn = game.Map.DictionaryOfTerritories[game.Players.Peek().Coordinates];
            try //tu robimy działanie wejścia na ulicę
            {
                Street st = (Street)game.Map.DictionaryOfTerritories[game.Players.Peek().Coordinates];
                bool ownerIsRealPlayer = Methods.IsThisRealPlayer(st.Owner, game);
                if (ownerIsRealPlayer & st.Owner != game.Players.Peek())
                {
                    // tu stacja musi brać pod uwagę czy to jest lotnisko czy coś w stylu tvn. trzeba odpowiednio to rozegrać
                    string bonus;
                    bool ownerIsInArrest = st.Owner.PlayerBonuses.TryGetValue("ARE", out bonus);
                    if (!ownerIsInArrest)
                    {
                        if (!st.IsPawned)
                        {
                            bonus = "";
                            int price = st.CostsForRent[st.HowMuchAreHauses];
                            bool czasamiPiszacToNarawdePowaznieCierpie = st.Owner.PlayerBonuses.TryGetValue("GAP", out bonus);
                            if (czasamiPiszacToNarawdePowaznieCierpie)
                                price = (int)Methods.ChangeValue(price, bonus);
                            czasamiPiszacToNarawdePowaznieCierpie = game.Players.Peek().PlayerBonuses.TryGetValue("PAP", out bonus);
                            if (czasamiPiszacToNarawdePowaznieCierpie)
                                price = (int)Methods.ChangeValue(price, bonus);
                            if (st.HowMuchAreHauses == 0 & Methods.HowMuchTerritoriesIsInThisAlly(game.Map.Alliance[st], game.Map) == Methods.HowMuchTerritoriesIsNotPawned(game.Map.Alliance[st], game.Map, st.Owner))
                                price *= 2;
                            Console.WriteLine($"Gracz {game.Players.Peek().Name} musi zapłacić {price}");
                            if (game.Players.Peek().Money >= price) // gracz nie ma problemu z płatnością
                            {
                                game.Players.Peek().Money -= price;
                                st.Owner.Money += price;
                            }
                            else // gracz ma problemy z płatnością
                            {
                                Methods.NotEnoughMoney(game, game.Players.Peek()); // Próba naprawy budżetu
                                if (game.Players.Peek().Money >= price) //skuteczna
                                {
                                    game.Players.Peek().Money -= price;
                                    st.Owner.Money += price;
                                }
                                else // nieskuteczna, co powoduje wywołanie aukcji komorniczej
                                {
                                    Methods.Bailiff(game, game.Players.Peek(), price);
                                    if (game.Players.Peek().Money <= price) // jeżeli nie wystarczająca
                                    {
                                        Console.WriteLine($"Gracz {game.Players.Peek().Name} bankrutuje");
                                        st.Owner.Money += game.Players.Peek().Money;
                                        game.Players.Peek().Money = -1;
                                    }
                                    else // kiedy wystarczy
                                    {
                                        game.Players.Peek().Money -= price;
                                        st.Owner.Money += price;
                                    }
                                }
                            }
                        }
                        else
                            Console.WriteLine("Masz szczęście, gracz zastawił to pole");
                    }
                    else
                        Console.WriteLine("Masz szczęście. Właściciel jest w więzieniu więc nie pobiera opłaty");
                }
                else if (ownerIsRealPlayer)
                {
                    Console.WriteLine($"Gracz {game.Players.Peek().Name} wszedł na swoje pole");
                    for (int i = 0; i < 2; i++)
                    {
                        bool canIBuildSTH = Methods.CanIBuildSomething(game.Players.Peek(), game, st);
                        if (canIBuildSTH & st.HowMuchAreHauses < 4)
                        {
                            string bon = "";
                            bool isBonus = st.Owner.PlayerBonuses.TryGetValue("BHA", out bon);
                            int price = (int)Methods.ChangeValue(st.CostToBuildHause, bon);
                            Console.WriteLine($"Czy chcesz kupić domek na tym polu za około {price}? (Wpisz y jeśli tak)");
                            string answer = Console.ReadLine();
                            if (answer == "y" | answer == "Y")
                            {
                                if (game.Players.Peek().Money >= price)
                                    st.BuildHause();
                                else
                                {
                                    Console.WriteLine("Nie masz wystarczająco dużo pieniędzy. chcesz jakoś pozyskać pieniądze? (wpisz y żeby potwierdzić)");
                                    answer = Console.ReadLine();
                                    if (answer == "y" | answer == "Y")
                                    {
                                        Methods.NotEnoughMoney(game, game.Players.Peek());
                                        if (game.Players.Peek().Money >= price)
                                            st.BuildHause();
                                        else
                                            i++;
                                    }
                                    else
                                        i++;
                                }
                            }
                            else
                                i++;
                        }
                        else if (canIBuildSTH)
                        {
                            string bon = "";
                            bool isBonus = st.Owner.PlayerBonuses.TryGetValue("BHO", out bon);
                            int price = (int)Methods.ChangeValue(st.CostToBuildHotel, bon);
                            Console.WriteLine($"Czy chcesz kupić hotel na tym polu za około {price}? (Wpisz y jeśli tak)");
                            string answer = Console.ReadLine();
                            if (answer == "y" | answer == "Y")
                            {
                                if (game.Players.Peek().Money >= price)
                                    st.BuildHause();
                                else
                                {
                                    Console.WriteLine("Nie masz wystarczająco dużo pieniędzy. chcesz jakoś pozyskać pieniądze? (wpisz y żeby potwierdzić)");
                                    answer = Console.ReadLine();
                                    if (answer == "y" | answer == "Y")
                                    {
                                        Methods.NotEnoughMoney(game, game.Players.Peek());
                                        if (game.Players.Peek().Money >= price)
                                            st.BuildHause();
                                        else
                                            i++;
                                    }
                                    else
                                        i++;
                                }
                            }
                            else
                                i++;
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tu nic zbudować");
                            i++;
                        }
                    }
                }
                else if (st.Owner == game.Map.Bank)
                    Methods.BuingTerritoryFromBank(st, game);
                else
                    Console.WriteLine("Pole nie jest własnością ani gracza ani banku?");
            }
            catch (InvalidCastException)
            {
                try //tu robimy działanie wejścia gracza na stacje
                {
                    Station st = (Station)game.Map.DictionaryOfTerritories[game.Players.Peek().Coordinates];
                    bool ownerIsRealPlayer = Methods.IsThisRealPlayer(st.Owner, game);
                    if (ownerIsRealPlayer & st.Owner != game.Players.Peek())
                    {
                        if (!st.IsPawned)
                        {
                            string bonus;
                            bool ownerIsInArrest = st.Owner.PlayerBonuses.TryGetValue("ARE", out bonus);
                            if (!ownerIsInArrest)
                            {
                                bonus = "";
                                bool czasamiPiszacToNarawdePowaznieCierpie = st.BonusesOfTile.TryGetValue("RTL", out bonus);
                                if (!czasamiPiszacToNarawdePowaznieCierpie) //Dla stacji typu lotnisko
                                {
                                    int price = st.CostsForRent[Methods.HowMuchTerritoriesIsNotPawned(game.Map.Alliance[st], game.Map, st.Owner)];
                                    czasamiPiszacToNarawdePowaznieCierpie = st.Owner.PlayerBonuses.TryGetValue("GAP", out bonus);
                                    if (czasamiPiszacToNarawdePowaznieCierpie)
                                        price = (int)Methods.ChangeValue(price, bonus);
                                    czasamiPiszacToNarawdePowaznieCierpie = game.Players.Peek().PlayerBonuses.TryGetValue("PAP", out bonus);
                                    if (czasamiPiszacToNarawdePowaznieCierpie)
                                        price = (int)Methods.ChangeValue(price, bonus);
                                    Console.WriteLine($"Gracz {game.Players.Peek().Name} musi zapłacić {price}");
                                    if (game.Players.Peek().Money >= price) // gracz nie ma problemu z płatnością
                                    {
                                        game.Players.Peek().Money -= price;
                                        st.Owner.Money += price;
                                    }
                                    else // gracz ma problemy z płatnością
                                    {
                                        Methods.NotEnoughMoney(game, game.Players.Peek()); // Próba naprawy budżetu
                                        if (game.Players.Peek().Money >= price) //skuteczna
                                        {
                                            game.Players.Peek().Money -= price;
                                            st.Owner.Money += price;
                                        }
                                        else // nieskuteczna, co powoduje wywołanie aukcji komorniczej
                                        {
                                            Methods.Bailiff(game, game.Players.Peek(), price);
                                            if (game.Players.Peek().Money <= price) // jeżeli nie wystarczająca
                                            {
                                                Console.WriteLine($"Gracz {game.Players.Peek().Name} bankrutuje");
                                                st.Owner.Money += game.Players.Peek().Money;
                                                game.Players.Peek().Money = -1;
                                            }
                                            else // kiedy wystarczy
                                            {
                                                game.Players.Peek().Money -= price;
                                                st.Owner.Money += price;
                                            }
                                        }
                                    }
                                }
                                else //Dla stacji uzależnionych od ruchu gracza
                                {
                                    int price = st.CostsForRent[Methods.HowMuchTerritoriesIsNotPawned(game.Map.Alliance[st], game.Map, st.Owner)];
                                    price *= game.HowMuchMove;
                                    czasamiPiszacToNarawdePowaznieCierpie = st.Owner.PlayerBonuses.TryGetValue("GAP", out bonus);
                                    if (czasamiPiszacToNarawdePowaznieCierpie)
                                        price = (int)Methods.ChangeValue(price, bonus);
                                    czasamiPiszacToNarawdePowaznieCierpie = game.Players.Peek().PlayerBonuses.TryGetValue("PAP", out bonus);
                                    if (czasamiPiszacToNarawdePowaznieCierpie)
                                        price = (int)Methods.ChangeValue(price, bonus);
                                    Console.WriteLine($"Gracz {game.Players.Peek().Name} musi zapłacić {price}");
                                    if (game.Players.Peek().Money >= price) // gracz nie ma problemu z płatnością
                                    {
                                        game.Players.Peek().Money -= price;
                                        st.Owner.Money += price;
                                    }
                                    else // gracz ma problemy z płatnością
                                    {
                                        Methods.NotEnoughMoney(game, game.Players.Peek()); // Próba naprawy budżetu
                                        if (game.Players.Peek().Money >= price) //skuteczna
                                        {
                                            game.Players.Peek().Money -= price;
                                            st.Owner.Money += price;
                                        }
                                        else // nieskuteczna, co powoduje wywołanie aukcji komorniczej
                                        {
                                            Methods.Bailiff(game, game.Players.Peek(), price);
                                            if (game.Players.Peek().Money <= price) // jeżeli nie wystarczająca
                                            {
                                                Console.WriteLine($"Gracz {game.Players.Peek().Name} bankrutuje");
                                                st.Owner.Money += game.Players.Peek().Money;
                                                game.Players.Peek().Money = -1;
                                            }
                                            else // kiedy wystarczy
                                            {
                                                game.Players.Peek().Money -= price;
                                                st.Owner.Money += price;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                Console.WriteLine("Masz szczęście. Właściciel jest w więzieniu więc nie pobiera opłaty");
                        }
                        else
                            Console.WriteLine("Masz szczeście, właściciel zastawił to pole");
                    }
                    else if (ownerIsRealPlayer)
                        Console.WriteLine($"Gracz {game.Players.Peek().Name} wszedł na swoje pole");
                    else if (st.Owner == game.Map.Bank)
                        Methods.BuingTerritoryFromBank(st, game);
                    else
                        Console.WriteLine("Pole nie jest własnością ani gracza ani banku?");
                }
                catch (InvalidCastException) // tu na pola specjalne typu start, idź na więzienie itp
                {
                    string bonus;
                    bool variable = stepIn.BonusesOfTile.TryGetValue("TCP", out bonus);
                    if (variable)
                    {
                        Card card = Methods.Prospect(game);
                        Console.WriteLine(card.MessageText);
                        foreach (var item in card.ListOfBonuses)
                        {
                            if (item.Key == "MON")
                                game.Players.Peek().Money = (int)Methods.ChangeValue(game.Players.Peek().Money, item.Value);
                            else if (item.Key == "CIE")
                                Methods.AddPrisonEscape(game.Players.Peek(), item.Value);
                        }
                    }
                    else
                    {
                        variable = stepIn.BonusesOfTile.TryGetValue("TCS", out bonus);
                        if (variable)
                        {
                            Card card = Methods.SocialTreasury(game);
                            foreach (var item in card.ListOfBonuses)
                            {
                                if (item.Key == "MON")
                                    game.Players.Peek().Money = (int)Methods.ChangeValue(game.Players.Peek().Money, item.Value);
                                else if (item.Key == "CIE")
                                    Methods.AddPrisonEscape(game.Players.Peek(), item.Value);
                            }
                        }
                        else
                        {
                            variable = stepIn.BonusesOfTile.TryGetValue("MON", out bonus);
                            if (variable)
                                game.Players.Peek().Money = (int)Methods.ChangeValue(game.Players.Peek().Money, bonus);
                            else
                            { // Tu jest istotna rzecz. Gracz na wejściu na polu start dostaje pieniądze za start. Jest to spowodowane tym, że zamierzam, żeby gracz na mapie dostawał pieniądze za start jak (int)ilośćRuchu + pozycja/maksRuchyWPierścieniu > 0 & reszta > 0 
                                variable = stepIn.BonusesOfTile.TryGetValue("MFS", out bonus);
                                if (variable)
                                    Methods.AddMoneyForStart(game);
                                else
                                {
                                    variable = stepIn.BonusesOfTile.TryGetValue("ARE", out bonus);
                                    if (variable)
                                        Methods.GoToPrison(game);
                                    else
                                    {
                                        variable = stepIn.BonusesOfTile.TryGetValue("PAR", out bonus);
                                        if (variable)
                                        {
                                            string answer;
                                            bool layer = true;
                                            while (layer)
                                            {
                                                layer = false;
                                                Console.WriteLine("Czy chcesz coś wybudować? (y jeśli tak)");
                                                answer = Console.ReadLine();
                                                if (answer == "y" | answer == "Y")
                                                {
                                                    layer = false;
                                                    foreach (var item in game.Map.DictionaryOfTerritories)
                                                    {
                                                        try
                                                        {
                                                            Street st = (Street)item.Value;
                                                            if (Methods.CanIBuildSomething(game.Players.Peek(), game, st))
                                                            {
                                                                Console.WriteLine($"Możesz wybudować na polu {st.Name}");
                                                                layer = true;
                                                            }
                                                        }
                                                        catch (InvalidCastException)
                                                        {

                                                        }
                                                    }
                                                    if (!layer)
                                                        Console.WriteLine("Ale nie możesz");
                                                    else
                                                    {
                                                        Console.WriteLine("Więc wpisz na którym polu chcesz się pobudować");
                                                        answer = Console.ReadLine();
                                                        //sprawdzić czy na wpisanym polu można się budować, jeśli tak to zbuduj dom lub hotel w zależności od poziomu w przeciwnym wypadku napisz że coś poszło nie tak.
                                                        Street sm = new Street();
                                                        bool criticalError = false;
                                                        foreach (var item in game.Map.DictionaryOfTerritories)
                                                        {
                                                            if (item.Value.Name == answer)
                                                            {
                                                                try
                                                                {
                                                                    sm = (Street)item.Value;
                                                                    if (!Methods.CanIBuildSomething(game.Players.Peek(), game, sm))
                                                                        criticalError = true;
                                                                }
                                                                catch (InvalidCastException)
                                                                {
                                                                    criticalError = true;
                                                                }
                                                                break;
                                                            }
                                                        }
                                                        if (!criticalError)
                                                        {
                                                            if (sm.HowMuchAreHauses < 5)
                                                                sm.BuildHause();
                                                            else
                                                                sm.BuildHotel();
                                                        }
                                                        else
                                                            Console.WriteLine("Coś źle zostało wpisane");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            Console.WriteLine("Jesteś odwiedzającym");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //PM zwraca ilość niezastawionych terytoriów z danego sojuszu
        /// <summary>
        /// Zwraca ilość niezastawionych pól, których właścicielem jest podawany gracz.
        /// </summary>
        /// <param name="ally">numer sojuszu (wartość Dictionary Alliance w obiekcie Map)</param>
        /// <param name="map">mapa na której toczy się rozgrywka</param>
        /// <param name="player">właściciel pól których suma jest podawana</param>
        /// <returns></returns>
        static public int HowMuchTerritoriesIsNotPawned(int ally, Map map, Player player)
        {
            int returner = 0;
            foreach (var item in map.Alliance)
            {
                if (item.Value == ally & item.Key.Owner == player & !item.Key.IsPawned)
                    returner++;
            }
            return returner;
        }

        //PM jest wywoływana, kiedy graczowi brakuje potrzebnych pieniędzy. Jest dobrowolną deską ratunku przed bankructwem. tak naprawdę po prostu umożliwia handel pomiędzy graczami
        static public void NotEnoughMoney(Game game, Player player) // musi być w whilu z którego gracz decyduje czy chce wyjść.
        {
            Console.WriteLine("Not implemented yet");
            string answer = "N";
            bool exit = true;
            do
            {
                Console.WriteLine("Wybierz co chcesz zrobić: ");
                Console.WriteLine("Wpisz 1 i zatwierdź enterem jeśli chcesz zastawić jakieś terytorium");
                Console.WriteLine("Wpisz 2 i zatwierdź enterem jeśli chcesz sprzedać domki / hotele");
                Console.WriteLine("Wpisz 3 i zatwierdź enterem jeśli chcesz sprzedać coś innym graczom");
                Console.WriteLine("Wpisz 4 i zatwierdź enterem jeśli chcesz wystawić jakieś terytorium na aukcję");
                answer = Console.ReadLine();

                if (answer == "1")
                {
                    List<Territory> possibleAnswer = new List<Territory>();
                    foreach (var item in game.Map.DictionaryOfTerritories) // wypisywanie pól które można zastawić i twozenie listy z nimi by nie musieć znowu foreacha stosować
                    {
                        if (item.Value.Owner == player & !item.Value.IsPawned)
                        {
                            if (item.Value.GetType() == typeof(Street))
                            {
                                Street st = (Street)item.Value;
                                string bonus = "";
                                bool bin = player.PlayerBonuses.TryGetValue("GPM", out bonus);
                                Console.WriteLine($"Możesz zastawić {st.Name} za {(int)Methods.ChangeValue(st.MoneyFromPawn, bonus)}");
                                possibleAnswer.Add(st);
                            }
                            else if (item.Value.GetType() == typeof(Station))
                            {
                                Station st = (Station)item.Value;
                                string bonus = "";
                                bool bin = player.PlayerBonuses.TryGetValue("GPM", out bonus);
                                Console.WriteLine($"Możesz zastawić {st.Name} za {(int)Methods.ChangeValue(st.MoneyFromPawn, bonus)}");
                                possibleAnswer.Add(st);
                            }
                        }
                    }
                    Console.WriteLine("Wpisz pełną nazwę pola, które chcesz zastawić");
                    answer = Console.ReadLine(); // wybór pola do zasstawienia. powinno zapytać czy chce kolejne

                }
                else if (answer == "2")
                {

                }
                else if (answer == "3")
                {

                }
                else if (answer == "4")
                {

                }
                else
                {

                }

                Console.WriteLine("Czy chcesz zakończyć zdobywanie kapitału? (wpisz y, żeby potwierdzić)");
                answer = Console.ReadLine();
                if (answer == "y" | answer == "Y")
                    exit = false;
            } while (exit);



        }

        //PM jest metodą wywołania aukcji komorniczej. Przymusowo wyprzedaje się majątek gracza.
        static public void Bailiff(Game game, Player player, int howMuchMoneyNeed)  // Tomek
        {
            Console.WriteLine("Not implemented yet");
            //możesz mieć problem taki, że można zlicytować kartę wyjdź z więzienia. żeby ją usunąć (dokładnie jedną) użyj metody Methods.DeletePrisonEscape() (w nawiasie daj playera któremu usuwasz), a żaby dodać Methods.AddPrisonEscape(,"FFFFFFT99") , tylko przed przecinkiem daj osobę której chcesz dodać
            //Zrób cenę wywoławczą np w wysokości oryginalnego kosztu, a jak się nie uda to zmniejsz np o 50% a jeżeli znowu się nie uda to od 25% a jak się nie uda to od 1 a jak to się nie uda to uznaj że bank kupił za 1
        }

        // PM jest tradycyjną licytacją jakiegoś pola pomiędzy graczami jakiegoś pola
        static public void Auction(Game game, Territory place)  //Tomek zrobi
        {
            //Console.WriteLine("Not implemented yet");
            //zwykła licytacja terytorium.



            bool isEndOfAuction = false;
            Queue<Player> Auction = new Queue<Player>();
            Auction = game.Players;
            Auction.Dequeue(); //Usuwam gracza który ma kolejke
            int minumumPrice = 0;
            int priceInAuction = 0;
            if (place.GetType() == typeof(Street))
            {
                Street street = (Street)place;
                minumumPrice = street.CostToBuy;

            }

            else if (place.GetType() == typeof(Station))
            {
                Station station = (Station)place;
                minumumPrice = station.CostToBuy;

            }

            else
            {
                throw new WrongUsingOfFunctionExcepting();
            }



            while (!isEndOfAuction)
            {
                Console.WriteLine($"Licytacja o terytorium {place.Name} aktualna cena {minumumPrice}");
                char choice;
                Console.WriteLine($"Tura Gracza {Auction.Peek().Name}");
                Console.WriteLine("Napisz P jeżeli pasujesz jeżeli licytujesz wpisz cokolwiek ");
                choice = Console.ReadLine()[0];

                if (choice.ToString() != "P") //Wait
                {
                    bool isCorrectValue = false;
                    while (!isCorrectValue)
                    {
                        Console.WriteLine("Ile chcesz zapłacić");
                        Int32.TryParse(Console.ReadLine(), out priceInAuction);

                        if (priceInAuction > minumumPrice && priceInAuction <= game.Players.Peek().Money)
                        {
                            isCorrectValue = true;
                            minumumPrice = priceInAuction;
                        }




                    }
                    Auction.Enqueue(Auction.Dequeue());

                }

                else
                {
                    Auction.Dequeue();
                }

                if (Auction.Count == 1)
                {
                    isEndOfAuction = true;
                    place.Owner = Auction.Peek();
                    Auction.Peek().Money -= priceInAuction;
                }



            }

        }

        //PM jest wywoływana, kiedy kupuje się pole od banku
        static public void BuingTerritoryFromBank(Territory place, Game game)  // Tomek
        {
            Console.WriteLine("Not implemented yet");
            if (place.Owner == game.Map.Bank)
            {
                place.Owner = game.Players.Peek();
                if (place.GetType() == typeof(Street))
                {
                    Street street = (Street)place;
                    game.Players.Peek().Money -= street.CostToBuy;
                }
                else if (place.GetType() == typeof(Station))
                {
                    Station station = (Station)place;
                    game.Players.Peek().Money -= station.CostToBuy;
                }

            }

            //Jak się nie uda sprzedać to zrób licytację i cenę wywoławczą np w wysokości oryginalnego kosztu, a jak się nie uda to zmniejsz np o 50 % a jeżeli znowu się nie uda to od 25 % a jak się nie uda to od 1 a jak to się nie uda to uznaj że bank kupił za 1
        }

        //PM zwraca true jeśli podany Player jest na liście graczy aktywnie grających (jest rzeczywistym graczem)
        /// <summary>
        /// Zwraca wartość logiczną, true - kiedy wprowadzony gracz znajduje się w queue graczy którzy grają, a false jeżeli jest to gracz pomocniczy typu bank, bot itp
        /// </summary>
        /// <param name="player">sprawdzany gracz</param>
        /// <param name="game">tocząca się gra</param>
        /// <returns></returns>
        static public bool IsThisRealPlayer(Player player, Game game)
        {
            bool returner = false;
            foreach (Player item in game.Players)
            {
                if (player == item)
                    returner = true;
            }
            return returner;
        }

        //PM zlicza ile jest w sumie terytoriów dla tego sojuszu
        /// <summary>
        /// zwraca ilość terytoriów należących do tego sojuszu
        /// </summary>
        /// <param name="ally">sojusz, którego członkowie są zliczani</param>
        /// <param name="map">badana mapa</param>
        /// <returns></returns>
        static public int HowMuchTerritoriesIsInThisAlly(int ally, Map map)
        {
            int returner = 0;
            foreach (var item in map.Alliance)
            {
                if (item.Value == ally)
                    returner++;
            }
            return returner;
        }

        //PM zwraca true, jeżeli mogę zbudować budynek bo spełniam wszystkie warunki poza tym czy stoję w odpowiednim miejscu
        /// <summary>
        /// zwracam wartości logiczne, true - jeśli gracz spełnia warunki do postawienia budynku na sprawdzanej lokacji a false kiedy nie spełnia któregoś z warunków.
        /// </summary>
        /// <param name="player">gracz, dla którego sprawdzamy możliwość postawienia budynku</param>
        /// <param name="game">gra dla której przeprowadzamy test</param>
        /// <param name="locationToTest">terytorium dla którego sprawdzamy możliwość postawienia budynku</param>
        /// <returns></returns>
        static public bool CanIBuildSomething(Player player, Game game, Street locationToTest)
        {
            if (locationToTest.Owner == player) // sprawdzam czy podany gracz może budować na danym polu
            {
                if (locationToTest.HowMuchAreHauses == 0) //Sprawdzam czy gracz ma wszystkie pola sojuszu. jeśli tak to pozwalam na budowę
                {
                    int ally = game.Map.Alliance[locationToTest];
                    int ownerPlaces = Methods.HowMuchPlayerHaveThisAlly(player, game, ally);
                    int mayIsTru = 0;
                    foreach (var item in game.Map.Alliance)
                    {
                        if (item.Value == ally)
                        {
                            if (item.Key.IsPawned) //zwracam false kiedy wykryję że jakiekolwiek pole jest zastawione z konkretnego sojuszu
                                return false;
                            mayIsTru++;
                            if (ownerPlaces < mayIsTru)
                                return false;
                        }
                    }
                    return true;
                }
                else if (locationToTest.HowMuchAreHauses < 5) // sprawdzam czy na polu nie ma hotelu, bo jeśli tak, to w elsie zwracam false.
                { // sprawdzam tylko czy badane pole nie ma największej liczby budynków z sojuszu, bo jeśli tak, to nie pozwalam na budowę
                    int ally = game.Map.Alliance[locationToTest];
                    int maxLVLHause = 0;
                    foreach (var item in game.Map.Alliance)
                    {
                        if (item.Value == ally)
                        {
                            Street st = (Street)item.Key;
                            if (st.HowMuchAreHauses > maxLVLHause)
                                maxLVLHause = st.HowMuchAreHauses;
                        }
                    }
                    if (locationToTest.HowMuchAreHauses <= maxLVLHause)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        //PM zlicza ile gracz ma kart tego sojuszu
        /// <summary>
        /// Metoda zlicza ile gracz ma kart tego sojuszu
        /// </summary>
        /// <param name="player">sprawdzany gracz</param>
        /// <param name="game">gra</param>
        /// <param name="ally">sojusz o który chodzi</param>
        /// <returns></returns>
        static public int HowMuchPlayerHaveThisAlly(Player player, Game game, int ally)
        {
            int returner = 0;
            foreach (var item in game.Map.DictionaryOfTerritories)
            {
                if (game.Map.Alliance[item.Value] == ally & item.Value.Owner == player)
                    returner++;
            }
            return returner;
        }

        //PM zwraca prawdziwą cene domku
        /// <summary>
        /// Zwraca realną cenę postawienia kolejnego budynku na polu. W momencie kiedy pole posiada dziwną / nieprawidłową wartość zwraca 2 140 000 000
        /// </summary>
        /// <param name="place">Sprawdzane terytorium. (brane są bonusy od właściciela pola)</param>
        /// <returns></returns>
        static public int RealApartmentPrice(Street place)
        {
            string bonus;
            if (place.HowMuchAreHauses < 5 & place.HowMuchAreHauses >= 0)
            {
                if (place.Owner.PlayerBonuses.TryGetValue("BHA", out bonus))
                    return (int)Methods.ChangeValue(place.CostToBuildHause, bonus);
                else
                    return place.CostToBuildHause;
            }
            else if (place.HowMuchAreHauses == 5)
            {
                if (place.Owner.PlayerBonuses.TryGetValue("BHO", out bonus))
                    return (int)Methods.ChangeValue(place.CostToBuildHotel, bonus);
                else
                    return place.CostToBuildHotel;
            }
            else
                return 2140000000;
        }

        //PM zwraca int odpowaidającemu polu na które się wkroczyło.
        /*
         * 0- pola nic nie robiące (np odwiedzający)
         * 1- Ulica
         * 2- stacja niezależna od ruchu gracza
         * 3- Stacja zależna od ruchu gracza
         * 4- Szansa
         * 5- Kasa społeczna
         * 6- podatek
         * 7- start
         * 8- idź do więzienia
         * 9- parking
         */
        /// <summary>
        /// Zwracam numer funkcjonalności pola od 0 do 9 włącznie. Szczegóły jaki numer co znaczy w komentarzu nad metodą
        /// </summary>
        /// <param name="place">sprawdzane pole</param>
        /// <returns></returns>
        static public int WhichTileIsIt(Territory place)
        {
            string bin;
            if (place.GetType() == typeof(Street))
                return 1;
            else if (place.GetType() == typeof(Station))
            {
                if (!place.BonusesOfTile.TryGetValue("RTL", out bin))
                    return 2;
                else return 3;
            }
            else if (place.BonusesOfTile.TryGetValue("TCP", out bin))
                return 4;
            else if (place.BonusesOfTile.TryGetValue("TCS", out bin))
                return 5;
            else if (place.BonusesOfTile.TryGetValue("MON", out bin))
                return 6;
            else if (place.BonusesOfTile.TryGetValue("MFS", out bin))
                return 7;
            else if (place.BonusesOfTile.TryGetValue("ARE", out bin))
                return 8;
            else if (place.BonusesOfTile.TryGetValue("PAR", out bin))
                return 9;
            else return 0;
        }

        //PM jest odpowiednikiem metody StepInTile jednak zrobione pod GUI
        /// <summary>
        /// Wersja pod GUI modyfikacji graczy mapy itp. (Odpowiednik dla GUI wersji StepInTile)
        /// </summary>
        /// <param name="game">Tocząca się gra</param>
        static public void StepInTileGUI(Game game)
        {
            int assist = Methods.WhichTileIsIt(game.Map.DictionaryOfTerritories[game.Players.Peek().Coordinates]);
            if (assist < 5)
            {
                if (assist == 1)
                {
                    //tu robimy działanie wejścia na ulicę
                    Street st = (Street)game.Map.DictionaryOfTerritories[game.Players.Peek().Coordinates];
                    bool ownerIsRealPlayer = Methods.IsThisRealPlayer(st.Owner, game);
                    if (ownerIsRealPlayer & st.Owner != game.Players.Peek())
                    {
                        string bonus;
                        bool ownerIsInArrest = st.Owner.PlayerBonuses.TryGetValue("ARE", out bonus);
                        if (!ownerIsInArrest & !st.IsPawned)
                        {
                            bonus = "";
                            int price = st.CostsForRent[st.HowMuchAreHauses];
                            bool czasamiPiszacToNarawdePowaznieCierpie = st.Owner.PlayerBonuses.TryGetValue("GAP", out bonus);
                            if (czasamiPiszacToNarawdePowaznieCierpie)
                                price = (int)Methods.ChangeValue(price, bonus);
                            czasamiPiszacToNarawdePowaznieCierpie = game.Players.Peek().PlayerBonuses.TryGetValue("PAP", out bonus);
                            if (czasamiPiszacToNarawdePowaznieCierpie)
                                price = (int)Methods.ChangeValue(price, bonus);
                            if (st.HowMuchAreHauses == 0 & Methods.HowMuchTerritoriesIsInThisAlly(game.Map.Alliance[st], game.Map) == Methods.HowMuchTerritoriesIsNotPawned(game.Map.Alliance[st], game.Map, st.Owner))
                                price *= 2;
                            if (game.Players.Peek().Money >= price) // gracz nie ma problemu z płatnością
                            {
                                game.Players.Peek().Money -= price;
                                st.Owner.Money += price;
                            }
                            else // gracz ma problemy z płatnością
                                Methods.NotEnoughMoneyGUI(game, game.Players.Peek(), price);
                        }
                    }
                    else if (ownerIsRealPlayer)
                    {
                        if (Methods.CanIBuildSomething(st.Owner, game, st))
                        {
                            // Tu można postawić domek dla tego pola
                        }
                    }
                    else if (st.Owner == game.Map.Bank)
                    {
                        Methods.BuingTerritoryFromBankGUI(st, game);
                        if (st.Owner == game.Players.Peek())
                            if (Methods.CanIBuildSomething(st.Owner, game, st))
                            {
                                // Tu można postawić domek dla tego pola
                            }
                    }
                    //else
                    //Console.WriteLine("Pole nie jest własnością ani gracza ani banku?");

                }
                else if (assist == 2)
                {
                    Station st = (Station)game.Map.DictionaryOfTerritories[game.Players.Peek().Coordinates];
                    bool ownerIsRealPlayer = Methods.IsThisRealPlayer(st.Owner, game);
                    if (ownerIsRealPlayer & st.Owner != game.Players.Peek())
                    {
                        if (!st.IsPawned) // czy pole zastawione
                        {
                            string bonus;
                            bool ownerIsInArrest = st.Owner.PlayerBonuses.TryGetValue("ARE", out bonus); // czy właściciel jest w więzieniu?
                            if (!ownerIsInArrest) // jak nie
                            {
                                int price = st.CostsForRent[Methods.HowMuchTerritoriesIsNotPawned(game.Map.Alliance[st], game.Map, st.Owner)];
                                bool czasamiPiszacToNarawdePowaznieCierpie = st.Owner.PlayerBonuses.TryGetValue("GAP", out bonus);
                                if (czasamiPiszacToNarawdePowaznieCierpie)
                                    price = (int)Methods.ChangeValue(price, bonus);
                                czasamiPiszacToNarawdePowaznieCierpie = game.Players.Peek().PlayerBonuses.TryGetValue("PAP", out bonus);
                                if (czasamiPiszacToNarawdePowaznieCierpie)
                                    price = (int)Methods.ChangeValue(price, bonus);
                                if (game.Players.Peek().Money >= price) // gracz nie ma problemu z płatnością
                                {
                                    game.Players.Peek().Money -= price;
                                    st.Owner.Money += price;
                                }
                                else // gracz ma problemy z płatnością
                                {
                                    Methods.NotEnoughMoneyGUI(game, game.Players.Peek(), price); // Próba naprawy budżetu
                                }
                            }
                        }
                    }
                    else if (st.Owner == game.Map.Bank)
                        Methods.BuingTerritoryFromBankGUI(st, game);
                }
                else if (assist == 3)
                {
                    Station st = (Station)game.Map.DictionaryOfTerritories[game.Players.Peek().Coordinates];
                    bool ownerIsRealPlayer = Methods.IsThisRealPlayer(st.Owner, game);
                    if (ownerIsRealPlayer & st.Owner != game.Players.Peek())
                    {
                        if (!st.IsPawned) // czy pole zastawione
                        {
                            string bonus;
                            bool ownerIsInArrest = st.Owner.PlayerBonuses.TryGetValue("ARE", out bonus); // czy właściciel jest w więzieniu?
                            if (!ownerIsInArrest) // jak nie
                            {
                                bonus = st.BonusesOfTile["RTL"];
                                int price = st.CostsForRent[Methods.HowMuchTerritoriesIsNotPawned(game.Map.Alliance[st], game.Map, st.Owner)];
                                price *= game.HowMuchMove;
                                bool czasamiPiszacToNarawdePowaznieCierpie = st.Owner.PlayerBonuses.TryGetValue("GAP", out bonus);
                                if (czasamiPiszacToNarawdePowaznieCierpie)
                                    price = (int)Methods.ChangeValue(price, bonus);
                                czasamiPiszacToNarawdePowaznieCierpie = game.Players.Peek().PlayerBonuses.TryGetValue("PAP", out bonus);
                                if (czasamiPiszacToNarawdePowaznieCierpie)
                                    price = (int)Methods.ChangeValue(price, bonus);
                                if (game.Players.Peek().Money >= price) // gracz nie ma problemu z płatnością
                                {
                                    game.Players.Peek().Money -= price;
                                    st.Owner.Money += price;
                                }
                                else // gracz ma problemy z płatnością
                                {
                                    Methods.NotEnoughMoneyGUI(game, game.Players.Peek(), price); // Próba naprawy budżetu
                                }
                            }
                        }
                    }
                    else if (st.Owner == game.Map.Bank)
                        Methods.BuingTerritoryFromBankGUI(st, game);
                }
                else if (assist == 4)
                {
                    Territory st = (Territory)game.Map.DictionaryOfTerritories[game.Players.Peek().Coordinates];
                    string bonus = st.BonusesOfTile["TCP"];
                    Card card = Methods.Prospect(game);
                    //Console.WriteLine(card.MessageText);
                    //Tu trzeba wyświetlić info tekstu karty
                    foreach (var item in card.ListOfBonuses)
                    {
                        if (item.Key == "MON")
                            game.Players.Peek().Money = (int)Methods.ChangeValue(game.Players.Peek().Money, item.Value);
                        else if (item.Key == "CIE")
                            Methods.AddPrisonEscape(game.Players.Peek(), item.Value);
                    }
                    if (game.Players.Peek().Money < 0)
                        Methods.NotEnoughMoneyGUI(game, game.Players.Peek(), 0 - game.Players.Peek().Money);
                }
            }
            else if (assist < 10)
            {
                string bonus;
                if (assist == 5)
                {
                    Territory st = (Territory)game.Map.DictionaryOfTerritories[game.Players.Peek().Coordinates];
                    bonus = st.BonusesOfTile["TCS"];
                    Card card = Methods.SocialTreasury(game);
                    //Console.WriteLine(card.MessageText);
                    //Tu trzeba wyświetlić info tekstu karty
                    foreach (var item in card.ListOfBonuses)
                    {
                        if (item.Key == "MON")
                            game.Players.Peek().Money = (int)Methods.ChangeValue(game.Players.Peek().Money, item.Value);
                        else if (item.Key == "CIE")
                            Methods.AddPrisonEscape(game.Players.Peek(), item.Value);
                    }
                    if (game.Players.Peek().Money < 0)
                        Methods.NotEnoughMoneyGUI(game, game.Players.Peek(), 0 - game.Players.Peek().Money);
                }
                else if (assist == 6)
                {
                    Territory st = (Territory)game.Map.DictionaryOfTerritories[game.Players.Peek().Coordinates];
                    bonus = st.BonusesOfTile["MON"];
                    game.Players.Peek().Money = (int)Methods.ChangeValue(game.Players.Peek().Money, bonus);
                    if (game.Players.Peek().Money < 0)
                        Methods.NotEnoughMoneyGUI(game, game.Players.Peek(), 0 - game.Players.Peek().Money);
                }
                else if (assist == 7)
                    Methods.AddMoneyForStart(game);
                else if (assist == 8)
                    Methods.GoToPrison(game);
                else if (assist == 9)
                {
                    // Tutaj można budować na każdym polu, o ile pozwala na to Methods.CanIBuildSomething(game.Players.Peek(),game,)

                }
            }
        }

        static public void NotEnoughMoneyGUI(Game game, Player player, int HowMuch) { }

        static public void BuingTerritoryFromBankGUI(Territory place, Game game) { }

        static public bool CanIMove(Game game)
        {
            throw new Exception();
        }


        #region MethodsForGUI

        //Zwracam true jeśli gracz może cokolwiek wybudować. Sprawdzam także pozycje gracza.
        /// <summary>
        /// Zwracam true, jeśli gracz może cokolwiek wybudować. w przeciwnym wypadku zwracam false. UWAGA: patrzę na to, czy gracz jest w odpowiednim miejscu
        /// </summary>
        /// <param name="player">sprawdzany gracz</param>
        /// <param name="game">tocząca się rozgrywka</param>
        /// <returns></returns>
        static public bool CanIBuildSomethingGUI(Player player, Game game)
        {
            if (game.Map.DictionaryOfTerritories[player.Coordinates].BonusesOfTile.ContainsKey("PAR"))
            {
                foreach (var item in game.Map.DictionaryOfTerritories.Where(x => (x.Value.Owner == player & x.Value.GetType() == typeof(Street))))
                {
                    if (Methods.CanIBuildSomething(player, game, (Street)item.Value))
                        return true;
                }
            }
            else if (game.Map.DictionaryOfTerritories[player.Coordinates].Owner == player & game.Map.DictionaryOfTerritories[player.Coordinates].GetType() == typeof(Street))
                return Methods.CanIBuildSomething(player, game, (Street)game.Map.DictionaryOfTerritories[player.Coordinates]);
            return false;
        }

        // Zwracam listę graczy, którzy są na polu o podanych koordynatach
        /// <summary>
        /// Zwracam listę graczy, którzy są na polu o podanych koordynatach
        /// </summary>
        /// <param name="coordinates">sprawdzane koordynaty</param>
        /// <param name="game"></param>
        /// <returns></returns>
        static public List<Player> GetVisitors(string coordinates, Game game)
        {
            List<Player> list = new List<Player>();
            foreach (Player playeringame in game.Players)
            {
                if (playeringame.Coordinates == coordinates) { list.Add(playeringame); }
            }
            return list;
        }

        #endregion MethodsForGUI

    }

}
