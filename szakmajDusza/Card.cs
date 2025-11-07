using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szakmajDusza
{
    public class Card
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int HP { get; set; }
        public KartyaTipus Tipus { get; set; }
        public bool Vezer { get; set; }

        public Card(string n, int d, int h, string tipus, bool vezer) 
        {
            Name = n;
            Damage = d;
            HP = h;
            switch (tipus)
            {
                case "tuz":
                    Tipus = KartyaTipus.tuz;
                    break;
                case "fold":
                    Tipus = KartyaTipus.fold;
                    break;
                case "viz":
                    Tipus = KartyaTipus.viz;
                    break;
                case "levego":
                    Tipus = KartyaTipus.levego;
                    break;
                default:
                    break;
            }
            Vezer = vezer;
        }
    }

    public enum KartyaTipus : byte
    {
        fold,
        levego,
        tuz,
        viz
    }
}
