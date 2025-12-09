using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;


namespace szakmajDusza
{
	public class Harc2
	{
		public static Random random = new Random();
		//vizuáls gotta make it work
		public static double playSpeedMultiplier = 1d;
		public static double basePlaySpeed = 750;//in miliseconds

		public static int kazDamage(float damage, int difficulty)
		{
			double roll = random.NextDouble(); // 0.0 .. 1.0 inclusive
			return (int)Math.Round(damage * (1 + (roll * difficulty / 10)));
		}
		public static int plyDamage(float damage, int difficulty)
		{
			double roll = random.NextDouble(); // 0.0 .. 1.0 inclusive
			return (int)Math.Round(damage * (1 - (roll * difficulty / 20)));
		}
		static int respawnTimesPlayer = 0;
		static string respawnedNamePlayer = "";
		static int respawnTimesKazamata = 0;
		static string respawnedNameKazamata = "";
		public static async Task DamageReductions(Card attacker, Card defender, int damage, string damageType,double critMult=0)
		{
			string reductionType = "";
			double armorReduction = 0;
			foreach (var item in defender.Items)//apply all damage reductions
			{
				if (!MagicRes(attacker))
				{
					if (item.Name == "Páncél")
					{
						armorReduction = item.Level * item.BaseVariable*0.01;
						if (reductionType != "dodge") reductionType = "shield";
						//armor animation
					}
					else if (item.Name == "Kikerülés")
					{
						if (random.Next(0, 100) < item.Level * item.BaseVariable)
						{
							damage = 0;
							reductionType = "dodge";
							damageType = "";
							//dodge animation
							break;
						}
					}
					else if (item.Name == "Tüskék")
					{
						int damage2 = (int)Math.Round(item.BaseVariable * item.Level * item.BaseVariable * 0.01, 0);
						await AnimationManager(attacker, "normal", "", damage2);
						attacker.HP -= damage2;
						/*int damage2 = (int)(damage * item.Level * 0.01 * item.BaseVariable);*/

						//thorns animation
					}
				}
			}
			if (reductionType != "dodge")
			{
				foreach (var item in defender.Items)
				{
					if (!MagicRes(attacker))
					{
						if (item.Name == "Tüskék")
						{
							//damage = (int)Math.Round(damage*(1-(item.BaseVariable * item.Level * 0.01)), 0);
							int damage2 = (int)Math.Round(damage*item.BaseVariable * item.Level * 0.01, 0);
							await AnimationManager(attacker, "normal", "", damage2);
							attacker.HP -= damage2;
							reductionType = "shield";
							attacker.UpdateVisual();
							/*int damage2 = (int)(damage * item.Level * 0.01 * item.BaseVariable);*/

							//thorns animation
						}
					}
				}
			}
			
			await AnimationManager(defender, damageType, reductionType, damage, (int)Math.Round(damage * (1+critMult) * armorReduction, 0), 1+critMult);
			defender.HP -= (int)Math.Round(damage*(1+critMult)*(1-armorReduction),0);
		}
		public static async Task AnimationManager(Card card, string damageType, string reductionType, int damageParam = int.MinValue, int reductionParam = int.MinValue, double bonusParam1 = double.MinValue/*, int bonusParam2 = int.MinValue*/)
		{
			/*while (card.animation)
			{

			}*/
			string damagetype2 = damageType;
			if (damageType == "normal")
			{
				if (reductionType == "")
				{
					await card.UpdateVisualDamage(damageParam);
				}
				else if (reductionType == "shield")
				{
					await card.NormalShieldAnim(damageParam, reductionParam);
				}
			}
			else if (damageType == "strength")
			{
				if (reductionType == "")
				{
					await card.StrengthAnim(damageParam);
				}
				else if (reductionType == "shield")
				{
					await card.StrengthShieldAnim(damageParam, reductionParam);
				}
			}
			else if (damageType == "crit")
			{
				if (reductionType == "")
				{
					await card.CritAnim(damageParam, bonusParam1);
				}
				else if (reductionType == "shield")
				{
					await card.CritShieldAnim(damageParam, bonusParam1, reductionParam);
				}
			}
			else if (damageType == "")
			{
				if (reductionType == "dodge")
				{
					await card.DodgeAnim();
				}
				else if (reductionType == "heal")
				{
					await card.HealAnim(reductionParam);
				}
				else if (reductionType == "heal")
				{
					await card.HealAnim(reductionParam);
				}
				else if (reductionType == "revive")
				{
					await card.ReviveAnim(reductionParam);
					card.Disabled = false;
				}
				else if (reductionType == "magic")
				{
					await card.MagicAnim();
				}
			}
			card.HideAllLabels();
			card.UpdateVisual();
		}
		public static async Task calculateDamage(Card attacker, Card defender, bool attackerIsPlayer, int difficulty)
		{
			double roll = random.NextDouble();
			double damageMultiplier = 1;
			if (attackerIsPlayer)
			{
				damageMultiplier = 1 - (roll * difficulty / 20);
			}
			else
			{
				damageMultiplier = 1 + (roll * difficulty / 10);
			}

			//int damage = attacker.Damage;
			bool critHappened = false;
			int critLevel = 0;
			foreach (var item in attacker.Items)//apply all damage boosts
			{
				if (MagicRes(defender))
				{
					continue;
				}
				if (item.Name == "Életerőlopás")
				{
					int damage = (int)Math.Round(item.BaseVariable * item.Level * damageMultiplier, 0);
					await DamageReductions(attacker, defender, damage, "normal");
					await AnimationManager(attacker, "", "heal", 0, damage);
					attacker.HP += damage;
					attacker.UpdateVisual();
					//lifestealLevel += item.Level;
				}
				else if (item.Name == "Erő")
				{
					int damage = (int)Math.Round(item.BaseVariable * item.Level*damageMultiplier,0);
					await DamageReductions(attacker,defender,damage,"strength");
					//strength animation
				}
				else if (item.Name == "Krit csapás")
				{
					if (random.Next(0, 100) < item.Level * item.BaseVariable)
					{
						critHappened=true;
						critLevel = item.Level;
						//int damage = (int)Math.Round(attacker.Damage, 0);
						//damage += (int)(item.Level * 0.4 * attacker.Damage);
						//crit animation
					}
				}
			}
			if (critHappened)
			{
				int damage = (int)Math.Round(attacker.Damage*Multiplier(attacker,defender) * damageMultiplier, 0);
				await DamageReductions(attacker, defender, damage, "crit",(0.4*critLevel));
			}
			else
			{
				int damage = (int)Math.Round(attacker.Damage * Multiplier(attacker, defender) * damageMultiplier, 0);
				await DamageReductions(attacker, defender, damage, "normal");
			}
			/*foreach (var item in defender.Items)//apply all damage reductions
			{
				if (!MagicRes(attacker))
				{
					if (item.Name == "Páncél")
					{
						damage /= (item.Level * item.BaseVariable);
						//armor animation
					}
					else if (item.Name == "Kikerülés")
					{
						if (random.Next(0, 100) < item.Level * item.BaseVariable)
						{
							damage = 0;
							//dodge animation
							break;
						}
					}
				}
			}*/
			/*if (attackerIsPlayer)
			{
				damage = (int)Math.Round(damage * (1 - (roll * difficulty / 20)));
			}
			else
			{
				damage = (int)Math.Round(damage * (1 + (roll * difficulty / 10)));
			}
			defender.HP -= damage;*/
			//damage animation
			if (defender.HP <= 0)
			{
				await RespawnItem(attacker, defender, attackerIsPlayer);
			}

			if (defender.HP > 0)
			{
				foreach (var item in defender.Items)
				{
					if (MagicRes(attacker)) continue;
					if (item.Name == "Tüskék")
					{
						int damage = (int)Math.Round(item.BaseVariable * item.Level * item.BaseVariable * 0.01, 0);
						await AnimationManager(attacker, "normal", "", damage);
						attacker.HP -= damage;
						attacker.UpdateVisual();
						/*int damage2 = (int)(damage * item.Level * 0.01 * item.BaseVariable);*/

						//thorns animation
					}
				}
			}
			if (attacker.HP <= 0)
			{
				await RespawnItem(defender, attacker, !attackerIsPlayer);
			}
			if (attacker.HP <= 0) return;
			foreach (var item in attacker.Items)
			{
				if (defender.HP>0&&MagicRes(defender)) continue;
				else if (item.Name == "Gyógyítás")
				{
					int healAmmount = item.Level*item.BaseVariable;
					await AnimationManager(attacker, "", "heal", 0, healAmmount);
					attacker.HP += healAmmount;
					attacker.UpdateVisual();
				}
			}
		}
		static async Task RespawnItem(Card attacker, Card defender, bool attackerIsPlayer)
		{
			foreach (var item in defender.Items)
			{
				if (MagicRes(attacker)) continue;
				if (item.Name == "Újraéledés")
				{
					int respawntimes = 0;
					if (attackerIsPlayer && respawnedNameKazamata == defender.Name)
					{
						respawntimes = respawnTimesKazamata;
					}
					else if (!attackerIsPlayer && respawnedNamePlayer == defender.Name)
					{
						respawntimes = respawnTimesPlayer;
					}
					if (random.Next(0, 100) < (item.Level - respawntimes) * item.BaseVariable)
					{
						int maxHP = 0;
						for (int i = 0; i < MainWindow.Gyujtemeny.Count; i++)
						{
							if (MainWindow.Gyujtemeny[i].Name==defender.Name)
							{
								maxHP = MainWindow.Gyujtemeny[i].HP;
								break;
							}
						}
						await AnimationManager(defender,"","revive",0,maxHP);
						defender.HP= maxHP;
						defender.UpdateVisual();
						//respawn animation
						return;
					}
				}
			}
		}
		public static bool MagicRes(Card resistor)
		{
			int magicResistLevel = 0;
			int baseVariable = 0;
			foreach (var item in resistor.Items)
			{
				if (item.Name == "Mágikus")
				{
					magicResistLevel += item.Level;
					baseVariable = item.BaseVariable;
				}
			}
			bool magicResist = random.Next(0, 100) < magicResistLevel * baseVariable;
			//magicres animation
			if(magicResist)AnimationManager(resistor,"","magic");

			return magicResist;
		}
		public static async Task StartFight(Grid grid, Button vissza, List<Card> gyujt, Kazamata k, List<Card> pakli, WrapPanel player, WrapPanel kazamata, Label attack, Label defend, Label attackDeploy, Label defendDeploy, WrapPanel fightPlayer, WrapPanel fightKazamata, int difficulty)
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
			await Task.Delay((int)(basePlaySpeed / playSpeedMultiplier));
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
			while ((kazamataCopies.Count != 0 || kaz != null) && (playerCopies.Count != 0 || play != null))
			{
				//player action
				if (play == null)
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
					await Task.Delay((int)(basePlaySpeed / playSpeedMultiplier));
				}
				else if (kaz != null)
				{
					attack.Visibility = Visibility.Visible;
					defend.Visibility = Visibility.Collapsed;
					attackDeploy.Visibility = Visibility.Collapsed;
					defendDeploy.Visibility = Visibility.Collapsed;
					//kaz.HP -= plyDamage((play.Damage * Multiplier(play, kaz)), difficulty);
					int beforeHPKaz = kaz.HP;
					int beforeHPPlay = play.HP;
					await calculateDamage(play,kaz,true,difficulty);
					int afterHPKaz = kaz.HP;
					int afterHPPlay = play.HP;
					//await kaz.UpdateVisualDamage(plyDamage((play.Damage * Multiplier(play, kaz)), difficulty));
					await Task.Delay((int)(basePlaySpeed / (2 * playSpeedMultiplier)));
					play.UpdateVisual();
					kaz.UpdateVisual();
					

					if (kaz.HP <= 0)
					{

						kaz.HP = 0;
						await Task.Delay((int)(basePlaySpeed / (2 * playSpeedMultiplier)));
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
					await Task.Delay((int)(basePlaySpeed / (2 * playSpeedMultiplier)));
				}
				else
				{
					MessageBox.Show("KYS player action");
					//this shouldnt have happened xd
				}



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
					await Task.Delay((int)(basePlaySpeed / playSpeedMultiplier));
				}
				else if (play != null)
				{
					attack.Visibility = Visibility.Collapsed;
					defend.Visibility = Visibility.Visible;
					attackDeploy.Visibility = Visibility.Collapsed;
					defendDeploy.Visibility = Visibility.Collapsed;
					int beforeHPKaz = kaz.HP;
					int beforeHPPlay=play.HP;
					await calculateDamage(kaz, play, false, difficulty);
					int afterHPKaz = kaz.HP;
					int afterHPPlay = play.HP;
					//play.HP -= kazDamage((int)Math.Floor(kaz.Damage * Multiplier(kaz, play)), difficulty);
					//await play.UpdateVisualDamage(kazDamage((int)Math.Floor(kaz.Damage * Multiplier(kaz, play)), difficulty));
					await Task.Delay((int)(basePlaySpeed / (2 * playSpeedMultiplier)));
					kaz.UpdateVisual();
					play.UpdateVisual();
					

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
					await Task.Delay((int)(basePlaySpeed / (2 * playSpeedMultiplier)));
				}
				else
				{
					MessageBox.Show("KYS kazamata action");
					//this shouldnt have happened xd
				}
			}

			Label l = MainWindow.CreateJutalom(grid);
			vissza.Visibility = Visibility.Visible;
			if (playerCopies.Count == 0 && play == null)
			{
				l.Content = "Vesztettél!";
				MainWindow.se.Close();
				MainWindow.se.Open(new Uri("Sounds/Lose.wav", UriKind.Relative));
				MainWindow.se.Play();
				MainWindow.sp.Stop();
               

                //MessageBox.Show("Játékos veszített!");
                kazamata.Children.Clear();
				player.Children.Clear();
				fightPlayer.Children.Clear();
				fightKazamata.Children.Clear();
                await Task.Delay(MainWindow.se.NaturalDuration.TimeSpan);
                MainWindow.se.Stop();
                MainWindow.sp.Play();
            }
			else
			{
				Label l2 = MainWindow.CreateJutalom(grid);
				l2.Margin = new Thickness(0, 305 + 100, 0, 0);
				kazamata.Children.Clear();
				player.Children.Clear();
				fightPlayer.Children.Clear();
				fightKazamata.Children.Clear();
				MainWindow.se.Open(new Uri("Sounds/Win.wav", UriKind.Relative));
				MainWindow.se.Play();
				MainWindow.sp.Stop();

				switch (k.reward)
				{
					case KazamataReward.eletero:
						//MessageBox.Show($"Játékos nyert! Nyeremény: +2 HP {pakli[index].Name}");
						l.Content = $"Nyertél!";
						l2.Content = $"+2❤ {pakli[index].Name}";

						pakli[index].HP += 2;
						pakli[index].UpdateVisual();
						break;
					case KazamataReward.sebzes:
						//MessageBox.Show($"Játékos nyert! Nyeremény: +1 sebzés {pakli[index].Name}");
						l.Content = $"Nyertél!";
						l2.Content = $"+1⚔ {pakli[index].Name}";
						pakli[index].Damage += 1;
						pakli[index].UpdateVisual();
						break;
					case KazamataReward.arany:
                        l.Content = $"Nyertél!";
                        l2.Content = $"+3 arany";
						Item.GoldOwned += 3;
                        break;
					case KazamataReward.newcard:
						foreach (var item in MainWindow.AllCardsDict.Values)
						{
							bool found = false;
							for (int i = 0; i < gyujt.Count; i++)
							{
								if (item.Name == gyujt[i].Name)
								{
									found = true; break;
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
								//MessageBox.Show($"Játékos nyert! Nyeremény: {item.Name} kártya hozzáadva a gyűjteményhez!");
								l.Content = $"Nyertél!";
								//l2.Content =item.Name;
								WrapPanel rewardCard = MainWindow.CreateCenteredWrapPanel(160, 200, 5, item);
								grid.Children.Add(rewardCard);
								rewardCard.Name = "rewardCard";


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
			List<Card> kazPak = k.GetCopy().Defenders;
			List<Card> playPak = Card.GetListCopy(pakli);
			Card? kaz = null;
			Card? play = null;
			bool kazWin = false;
			int kor = 1;
			w.WriteLine($"harc kezdodik;{k.Name}");
			while ((kazPak.Count != 0 || kaz != null))
			{
				w.WriteLine();
				if (kaz == null)
				{
					kaz = kazPak[0];
					w.WriteLine($"{kor}.kor;kazamata;kijatszik;{kaz.Name};{kaz.Damage};{kaz.HP};{kaz.Tipus.ToString()}");
					kazPak.RemoveAt(0);
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

				if (play == null && playPak.Count != 0)
				{
					play = playPak[0];
					w.WriteLine($"{kor}.kor;jatekos;kijatszik;{play.Name};{play.Damage};{play.HP};{play.Tipus.ToString()}");

					playPak.RemoveAt(0);
				}
				else if (play == null && playPak.Count == 0)
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
