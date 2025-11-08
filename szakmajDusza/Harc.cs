using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szakmajDusza
{
    public class Harc
    {
        //vizuáls gotta make it work
        static public void StartFight(Kazamata k, List<Card> pakli)
        {
            Card? kaz = null;
            Card? play = null;
            while (k.Defenders.Count != 0 && pakli.Count != 0)
            {
                if (kaz == null)
                {
                    kaz = k.Defenders[0].GetCopy();
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
                    play = pakli[0].GetCopy();
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
        //tesztes
        static public void StartFight(Kazamata k, List<Card> pakli, StreamWriter w)
        {
            Card? kaz = null;
            Card? play = null;
            bool kazWin = false;
            int kor = 1;
            while (k.Defenders.Count != 0 && pakli.Count != 0)
            {
                if (kaz == null)
                {
                    kaz = k.Defenders[0].GetCopy();
                    w.WriteLine($"{kor}.kor;kazamata;kijatszik;{kaz.Name};{kaz.Damage};{kaz.HP};{kaz.Tipus.ToString()}");
                    k.Defenders.RemoveAt(0);
                }
                else
                {
                    play.HP -= (int)Math.Floor(kaz.Damage * Multiplier(kaz, play));
                    w.WriteLine($"{kor}.kor;kazamata;tamad;{kaz.Name};{Math.Floor(kaz.Damage*Multiplier(kaz, play))};{play.Name};{(play.HP>0?play.HP : 0)}");
                    if (play.HP <= 0)
                    {
                        play = null;
                    }
                }

                if (play == null&&pakli.Count!=0)
                {
                    play = pakli[0].GetCopy();
                    w.WriteLine($"{kor}.kor;jatekos;kijatszik;{play.Name};{play.Damage};{play.HP};{play.Tipus.ToString()}");

                    pakli.RemoveAt(0);
                }
                else if (play == null && pakli.Count == 0)
                {
                    kazWin = true;
                    break;
                }
                else
                {
                    kaz.HP -= (int)Math.Floor(play.Damage * Multiplier(play, kaz));
                    w.WriteLine($"{kor}.kor;jatekos;tamad;{play.Name};{Math.Floor(play.Damage * Multiplier(play, kaz))};{kaz.Name};{(kaz.HP > 0 ? kaz.HP : 0)}");

                    if (kaz.HP <= 0)
                    {
                        kaz = null;
                    }
                }
                kor++;
            }
            if (kazWin)
            {
                w.WriteLine("jatekos vesztett");
            }
            else
            {
                if (k.Tipus==KazamataType.nagy)
                {
                    Card? nemBirtok = null;
                    for (int i = 0; i < App.Cards.Count; i++)
                    {
                        if (!App.Jatekos.Contains(App.Cards[i]))
                        {
                            nemBirtok = App.Cards[i];
                            break;
                        }
                    }
                    if (nemBirtok!=null)
                    {
                        App.Jatekos.Add(nemBirtok);
                        w.WriteLine($"jatekos nyert;{nemBirtok.Name}");
                    }
                }
                else
                {
                    if (k.reward==KazamataReward.eletero)
                    {
                        w.WriteLine($"jatekos nyert;eletero;{play.Name}");
                        //increase the stats actually
                        for (int i = 0; i < App.Pakli.Count; i++)
                        {
                            if (App.Pakli[i].Name==play.Name)
                            {
                                App.Pakli[i].HP += 2;
                            }
                        }
                        for (int i = 0; i < App.Jatekos.Count; i++)
                        {
                            if (App.Jatekos[i].Name == play.Name)
                            {
                                App.Jatekos[i].HP += 2;
                            }
                        }
                    }
                    else
                    {
                        w.WriteLine($"jatekos nyert;sebzes;{play.Name}");
                        //increase the stats actually
                        for (int i = 0; i < App.Pakli.Count; i++)
                        {
                            if (App.Pakli[i].Name == play.Name)
                            {
                                App.Pakli[i].Damage += 1;
                            }
                        }
                        for (int i = 0; i < App.Jatekos.Count; i++)
                        {
                            if (App.Jatekos[i].Name == play.Name)
                            {
                                App.Jatekos[i].HP += 1;
                            }
                        }
                    }
                }
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
