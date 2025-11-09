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
	public class Harc2
	{
		//vizuáls gotta make it work
		public static double playSpeedMultiplier = 1d;
		public static double basePlaySpeed = 750;//in miliseconds
		public static async Task StartFight(List<Card> gyujt, Kazamata k, List<Card> pakli, WrapPanel player, WrapPanel kazamata, Label attack, Label defend,Label attackDeploy,Label defendDeploy, WrapPanel fightPlayer, WrapPanel fightKazamata)
		{
			List<Card> playerCopies = pakli.Select(c => c.GetCopy()).ToList();
			List<Card> kazamataCopies = k.Defenders.Select(c => c.GetCopy()).ToList();
			int index = 0;
			attack.Visibility = Visibility.Collapsed;
			defend.Visibility = Visibility.Collapsed;
			attackDeploy.Visibility = Visibility.Collapsed;
			defendDeploy.Visibility = Visibility.Collapsed;
			player.Children.Clear();
			kazamata.Children.Clear();

			foreach (var c in playerCopies)
				player.Children.Add(c.GetVisual());
			foreach (var c in kazamataCopies)
				kazamata.Children.Add(c.GetVisual());

			Card? kaz = kazamataCopies[0];
			kazamataCopies.RemoveAt(0);
			kazamata.Children.Remove(kaz.GetVisual());
			kaz.visualGroup.Width = 160;
			kaz.visualGroup.Height = 200;
			fightKazamata.Children.Add(kaz.GetVisual());
			attack.Visibility = Visibility.Collapsed;
			defend.Visibility = Visibility.Collapsed;
			attackDeploy.Visibility = Visibility.Collapsed;
			defendDeploy.Visibility = Visibility.Visible;
			await Task.Delay((int)(basePlaySpeed / playSpeedMultiplier));
			Card? play = null;
			while ((kazamataCopies.Count!=0||kaz!=null)&&(playerCopies.Count!=0||play!=null))
			{
				//player action
				if (play==null)
				{
					if (playerCopies.Count != 0)
					{
						play = playerCopies[0];
						playerCopies.RemoveAt(0);
						player.Children.Remove(play.GetVisual());
						play.visualGroup.Width = 160;
						play.visualGroup.Height = 200;
						fightPlayer.Children.Add(play.GetVisual());
						attack.Visibility = Visibility.Collapsed;
						defend.Visibility = Visibility.Collapsed;
						attackDeploy.Visibility = Visibility.Visible;
						defendDeploy.Visibility = Visibility.Collapsed;
					}
					else
					{
						//player lost
						break;
					}
				}
				else if(kaz!=null)
				{
					kaz.HP -= (int)Math.Floor(play.Damage * Multiplier(play, kaz));
					kaz.UpdateVisual();
					attack.Visibility = Visibility.Visible;
					defend.Visibility = Visibility.Collapsed;
					attackDeploy.Visibility = Visibility.Collapsed;
					defendDeploy.Visibility = Visibility.Collapsed;

					if (kaz.HP <= 0)
					{
						kaz.HP = 0;
						kaz.UpdateVisual();
						//kazamata.Children.Remove(kaz.GetVisual());
						fightKazamata.Children.Remove(kaz.GetVisual());
						kaz.visualGroup.Width = 140;
						kaz.visualGroup.Height = 180;
						//kaz.But.Background = Brushes.Gray;
						kaz.NameLabel.Foreground = Brushes.Gray;
						kazamata.Children.Add(kaz.GetVisual());
						kaz = null;
					}
				}
				else
				{
					MessageBox.Show("KYS player action");
					//this shouldnt have happened xd
				}

				await Task.Delay((int)(basePlaySpeed / playSpeedMultiplier));

				//kazamata action
				if (kaz == null)
				{
					if (kazamataCopies.Count != 0)
					{
						kaz = kazamataCopies[0];
						kazamataCopies.RemoveAt(0);
						kazamata.Children.Remove(kaz.GetVisual());
						kaz.visualGroup.Width = 160;
						kaz.visualGroup.Height = 200;
						fightKazamata.Children.Add(kaz.GetVisual());
						attack.Visibility = Visibility.Collapsed;
						defend.Visibility = Visibility.Collapsed;
						attackDeploy.Visibility = Visibility.Collapsed;
						defendDeploy.Visibility = Visibility.Visible;
					}
					else
					{
						//kazamata lost
						break;
					}
				}
				else if (play != null)
				{
					play.HP -= (int)Math.Floor(kaz.Damage * Multiplier(kaz, play));
					play.UpdateVisual();
					attack.Visibility = Visibility.Collapsed;
					defend.Visibility = Visibility.Visible;
					attackDeploy.Visibility = Visibility.Collapsed;
					defendDeploy.Visibility = Visibility.Collapsed;

					if (play.HP <= 0)
					{
						play.HP = 0;
						play.UpdateVisual();
						//player.Children.Remove(play.GetVisual());
						fightPlayer.Children.Remove(play.GetVisual());
						play.visualGroup.Width = 140;
						play.visualGroup.Height = 180;
						//play.But.Background = Brushes.Gray;
						play.NameLabel.Foreground = Brushes.Gray;
						player.Children.Add(play.GetVisual());
						play = null;

						index++;
					}
				}
				else
				{
					MessageBox.Show("KYS kazamata action");
					//this shouldnt have happened xd
				}
				await Task.Delay((int)(basePlaySpeed / playSpeedMultiplier));
			}


			if (playerCopies.Count == 0 && play == null)
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
                        MessageBox.Show($"Játékos nyert! Nyeremény: +2 HP {pakli[index].Name}");
                        pakli[index].HP += 2;
						pakli[index].UpdateVisual();
						break;
					case KazamataReward.sebzes:
                        MessageBox.Show($"Játékos nyert! Nyeremény: +1 sebzés {pakli[index].Name}");
                        pakli[index].Damage += 1;
						pakli[index].UpdateVisual();
						break;
					case KazamataReward.newcard:
						foreach (var item in MainWindow.AllCards)
						{
							bool found=false;
							for (int i = 0; i < gyujt.Count; i++)
							{
								if (item.Name == gyujt[i].Name)
								{
									found=true; break;
								}
							}
							for (int i = 0; i < pakli.Count; i++)
							{
								if (item.Name == pakli[i].Name)
								{
									found = true; break;
								}
							}
							if (!found)
							{
								gyujt.Add(item.GetCopy());
								//MainWindow.Cards_Wrap.Children.Add(gyujt[gyujt.Count-1].GetVisual());
								//ui fix needed
								MessageBox.Show($"Játékos nyert! Nyeremény: {item.Name} kártya hozzáadva a gyűjteményhez!");
								return;
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
			while ((k.Defenders.Count != 0 || kaz != null))
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
					w.WriteLine($"{kor}.kor;kazamata;tamad;{kaz.Name};{Math.Floor(kaz.Damage * Multiplier(kaz, play))};{play.Name};{(play.HP > 0 ? play.HP : 0)}");
					if (play.HP <= 0)
					{
						play = null;
					}
				}

				if (play == null && pakli.Count != 0)
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
				if (k.Tipus == KazamataType.nagy)
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
					if (nemBirtok != null)
					{
						App.Jatekos.Add(nemBirtok);
						w.WriteLine($"jatekos nyert;{nemBirtok.Name}");
					}
				}
				else
				{
					if (k.reward == KazamataReward.eletero)
					{
						w.WriteLine($"jatekos nyert;eletero;{play.Name}");
						//increase the stats actually
						for (int i = 0; i < App.Pakli.Count; i++)
						{
							if (App.Pakli[i].Name == play.Name)
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
