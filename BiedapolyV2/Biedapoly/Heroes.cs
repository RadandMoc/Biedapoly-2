using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biedapoly
{
    [Serializable]
    public class Heroes
    {
        string heroname;
        Dictionary<string, string> heroesBonuses = new Dictionary<string, string>();
        int hero_pocket; 
        //proponuje, zeby to byla roznica poczatkowych pieniedzy jakie daje heros. np moze byc heros gorszy od normalnego ale z wieksza gotowka i na odwrot

        public string Heroname { get => heroname; set => heroname = value; }
        public int Hero_pocket { get => hero_pocket; set => hero_pocket = value; }
        public Dictionary<string, string> HeroesBonuses { get => heroesBonuses; set => heroesBonuses = value; }

        public Heroes() { }

        public Heroes(string name_of_hero, int hero_Pocket, Dictionary<string,string> BonusesOfHero)
        {
            Heroname= name_of_hero;
            Hero_pocket= hero_Pocket;
            HeroesBonuses= BonusesOfHero;
        }

        public override string ToString()
        {
            return $"{heroname}: (proposition: take {hero_pocket}% money like normal caounter)";
        }
    }
}
