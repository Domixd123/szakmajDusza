using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szakmajDusza
{
    public class Harc
    {
        static public void StartFight(Kazamata k, List<Card> pakli, bool test)
        {
            Card? kaz = null;
            Card? play = null;
            while (k.Defenders.Count != 0 && pakli.Count != 0)
            {
                if (kaz == null)
                {
                    kaz = k.Defenders[0];
                    k.Defenders.RemoveAt(0);
                }
                else
                {
                    play.HP -= (int)Math.Floor(kaz.Damage * Multiplier(kaz, play));
                    if (play.HP <= 0)
                    {
                        play = null;
                    }
                }

                if (play == null)
                {
                    play = pakli[0];
                    pakli.RemoveAt(0);
                }
                else
                {
                    kaz.HP -= (int)Math.Floor(play.Damage * Multiplier(play, kaz));
                    if (kaz.HP <= 0)
                    {
                        kaz = null;
                    }
                }
            }
            if (pakli.Count == 0)
            {
                //jatekos veszít
            }
            else
            {
                //jatekos nyer
            }
        }
        static public float Multiplier(Card attack, Card def)
        {
            if (attack.Tipus == def.Tipus)
            {
                return 1;
            }
            else if ((attack.Tipus == KartyaTipus.tuz || attack.Tipus == KartyaTipus.levego) && (def.Tipus == KartyaTipus.fold || def.Tipus == KartyaTipus.viz))
            {
                return 2;
            }
            else if ((attack.Tipus == KartyaTipus.fold || attack.Tipus == KartyaTipus.viz) && (def.Tipus == KartyaTipus.tuz || def.Tipus == KartyaTipus.levego))
            {
                return 2;
            }
            return 0.5f;
        }
    }
}
