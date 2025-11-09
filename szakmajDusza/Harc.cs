using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace szakmajDusza
{
    public class Harc
    {
        //vizuáls gotta make it work
        public static async Task StartFight(List<Card> gyujt, Kazamata k, List<Card> pakli, WrapPanel player, WrapPanel kazamata, Label attack, Label defend, WrapPanel fightPlayer, WrapPanel fightKazamata)
        {
            List<Card> playerCopies = pakli.Select(c => c.GetCopy()).ToList();
            List<Card> kazamataCopies = k.Defenders.Select(c => c.GetCopy()).ToList();
            int index = 0;
            attack.Visibility = Visibility.Collapsed;
            defend.Visibility = Visibility.Collapsed;

            player.Children.Clear();
            kazamata.Children.Clear();

            foreach (var c in playerCopies)
                player.Children.Add(c.GetVisual());
            foreach (var c in kazamataCopies)
                kazamata.Children.Add(c.GetVisual());

            Card? kaz = null;
            Card? play = null;

            while ((kazamataCopies.Count != 0 || kaz != null) && (playerCopies.Count != 0 || play != null))
            {
                if (kaz == null && kazamataCopies.Count > 0)
                {
                    kaz = kazamataCopies[0];
                    kazamataCopies.RemoveAt(0);
                    kazamata.Children.Remove(kaz.GetVisual());
                    kaz.visualGroup.Width = 160;
                    kaz.visualGroup.Height = 200;
                    fightKazamata.Children.Add(kaz.GetVisual());
                }
                else if (kaz != null && play != null)
                {
                    play.HP -= (int)Math.Floor(kaz.Damage * Multiplier(kaz, play));
                    play.UpdateVisual();
                    defend.Visibility = Visibility.Visible;
                    attack.Visibility = Visibility.Collapsed;

                    if (play.HP <= 0)
                    {
                        //player.Children.Remove(play.GetVisual());
                        fightPlayer.Children.Remove(play.GetVisual());
                        play.visualGroup.Width = 140;
                        play.visualGroup.Height = 180;
                        //play.But.Background = Brushes.Gray;
                        //play.NameText.Foreground = Brushes.Gray;
                        player.Children.Add(play.GetVisual());
                        play = null;
                        
                        index++;
                    }
                }

                await Task.Delay(1500);

                if (play == null && playerCopies.Count > 0)
                {
                    play = playerCopies[0];
                    playerCopies.RemoveAt(0);
                    player.Children.Remove(play.GetVisual());
                    play.visualGroup.Width = 160;
                    play.visualGroup.Height = 200;
                    fightPlayer.Children.Add(play.GetVisual());
                }
                else if (play != null && kaz != null)
                {
                    kaz.HP -= (int)Math.Floor(play.Damage * Multiplier(play, kaz));
                    kaz.UpdateVisual();
                    defend.Visibility = Visibility.Collapsed;
                    attack.Visibility = Visibility.Visible;

                    if (kaz.HP <= 0)
                    {
                        //kazamata.Children.Remove(kaz.GetVisual());
                        fightKazamata.Children.Remove(kaz.GetVisual());
                        play.visualGroup.Width = 140;
                        play.visualGroup.Height = 180;
                        //kaz.But.Background = Brushes.Gray;
                        kaz.NameLabel.Foreground= Brushes.Gray;
                        kazamata.Children.Add(kaz.GetVisual());
                        kaz = null;
                    }
                }

                await Task.Delay(1500);
            }

            if (playerCopies.Count == 0 && play != null)
            {
                MessageBox.Show("Játékos veszített!");
                kazamata.Children.Clear();
                player.Children.Clear();
                fightPlayer.Children.Clear();
                fightKazamata.Children.Clear();
            }

            else
            {
                
                kazamata.Children.Clear();
                player.Children.Clear();
                fightPlayer.Children.Clear();
                fightKazamata.Children.Clear();

                switch (k.reward)
                {
                    case KazamataReward.eletero:
                        MessageBox.Show($"Játékos nyert! Nyereméy: +2 életerő {pakli[index].Name} kártyára!");
                        pakli[index].HP += 2;
                        pakli[index].UpdateVisual();
                        break;
                    case KazamataReward.sebzes:
                        MessageBox.Show($"Játékos nyert! Nyereméy: +1 sebzés {pakli[index].Name} kártyára!");
                        pakli[index].Damage += 1;
                        pakli[index].UpdateVisual();
                        break;
                    case KazamataReward.newcard:
                        foreach (var item in k.Defenders)
                        {
                            if (!gyujt.Contains(item))
                            {
                                gyujt.Add(item);
                                MessageBox.Show($"Játékos nyert! Új kártya: {item}!");
                                break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        
        //tesztes
        static public void StartFight(Kazamata k, List<Card> pakli, StreamWriter w)
        {
            Card? kaz = null;
            Card? play = null;
            bool kazWin = false;
            int kor = 1;
            w.WriteLine($"harc kezdodik;{k.Name}");
            while ((k.Defenders.Count != 0||kaz!=null) )
            {
                w.WriteLine();
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
            w.WriteLine();
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
