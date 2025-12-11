using Microsoft.VisualBasic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace szakmajDusza
{
	public partial class MainWindow : Window
	{
		public void DisableNagyKazamata()
		{
			bool cardRemains = false;
			foreach (var item in AllCardsDict.Values)
			{
				bool found = false;
				foreach (var item2 in Gyujtemeny)
				{
					if (item.Name == item2.Name)
					{
						found = true;
						break;
					}
				}
				if (!found) cardRemains = true;//there are cards that can still be acquired
			}
			if (!cardRemains)
			{
				foreach (var item in DynamicButtonsPanel.Children)//only runs if all normal cards are already acquired
				{
					if (item.GetType() == typeof(Button) && AllKazamataDict.ContainsKey((item as Button).Content.ToString()) && AllKazamataDict[(item as Button).Content.ToString()].Tipus == KazamataType.nagy)
					{
						(item as Button).IsEnabled = false;
					}
				}
			}
			else
			{
				foreach (var item in DynamicButtonsPanel.Children)//only runs if all normal cards are already acquired
				{
					if (item.GetType() == typeof(Button) && AllKazamataDict.ContainsKey((item as Button).Content.ToString()) && AllKazamataDict[(item as Button).Content.ToString()].Tipus == KazamataType.nagy)
					{
						(item as Button).IsEnabled = true;
					}
				}
			}

		}
		private void ShowKazamata(Kazamata k)
		{
			Jutalom = new Label()
			{
				Content = k.reward == KazamataReward.eletero ? "Jutalom:+2❤" : k.reward == KazamataReward.sebzes ? "Jutalom:+1⚔" : k.reward == KazamataReward.arany ? "Jutalom:+3 arany" : "Jutalom:",
				Name = "Jutalom",
				Height = 100,
				Margin = new Thickness(0, 305, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Width = 450,
				FontSize = 60,
				HorizontalAlignment = HorizontalAlignment.Center,
				HorizontalContentAlignment = HorizontalAlignment.Center
			};
			ScrollViewer scrollViewer1 = new ScrollViewer()
			{
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
				HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
			};

			Grid g1 = new Grid()
			{
				Background = Brushes.Transparent
			};

			g1.Children.Add(FightPlayer_Wrap);
			scrollViewer1.Content = g1;
			FightPlayer_Wrap = new WrapPanel()
			{
				Name = "FightPlayer_Wrap",
				Margin = new Thickness(115, 263, 1343, 37),
				Width = 400
			};

			ScrollViewer scrollViewer = new ScrollViewer()
			{
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
				HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
			};

			Grid g = new Grid()
			{
				Background = Brushes.Transparent
			};

			g.Children.Add(FightKazamata_Wrap);
			scrollViewer.Content = g;
			FightKazamata_Wrap = new WrapPanel()
			{
				Name = "FightKazamata_Wrap",
				Margin = new Thickness(1386, 263, 146, 37),
				Width = 400
			};

			Defend_Label = new Label()
			{
				Name = "Defend_Label",
				Content = "Kazamata támad",
				Foreground = Brushes.Red,
				FontSize = 60,
				HorizontalAlignment = HorizontalAlignment.Center,
				Margin = new Thickness(0, 116, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Visibility = Visibility.Collapsed
			};

			Attack_Label = new Label()
			{
				Name = "Attack_Label",
				Content = "Játékos támad",
				Foreground = Brushes.Green,
				FontSize = 60,
				HorizontalAlignment = HorizontalAlignment.Center,
				Margin = new Thickness(0, 116, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Visibility = Visibility.Collapsed
			};

			DefendDeploy_Label = new Label()
			{
				Name = "DefendDeploy_Label",
				Content = "Kazamata kijátszik",
				Foreground = Brushes.Red,
				FontSize = 60,
				HorizontalAlignment = HorizontalAlignment.Center,
				Margin = new Thickness(0, 116, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Visibility = Visibility.Collapsed
			};

			AttackDeploy_Label = new Label()
			{
				Name = "AttackDeploy_Label",
				Content = "Játékos kijátszik",
				Foreground = Brushes.Green,
				FontSize = 60,
				HorizontalAlignment = HorizontalAlignment.Center,
				Margin = new Thickness(0, 116, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Visibility = Visibility.Collapsed
			};

			Fight_Wrap = new WrapPanel()
			{
				Name = "Fight_Wrap",
				Margin = new Thickness(665, 310, 665, 362)
			};

			FightPlayerAttacker_Wrap = new WrapPanel()
			{
				Name = "FightPlayerAttacker_Wrap",
				Margin = new Thickness(740, 310, 965, 362)
			};

			FightKazamataAttacker_Wrap = new WrapPanel()
			{
				Name = "FightKazamataAttacker_Wrap",
				Margin = new Thickness(960, 310, 746, 362)
			};

			Harc = new Button()
			{
				Name = "Harc",
				Content = "Harc",
				HorizontalAlignment = HorizontalAlignment.Center,
				Height = 100,
				Margin = new Thickness(0, 650, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Width = 300,
				FontSize = 60
			};

			Vissza = new Button()
			{
				Name = "Vissza",
				Content = "Vissza",
				HorizontalAlignment = HorizontalAlignment.Center,
				Height = 100,
				Margin = new Thickness(0, 794, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Width = 300,
				FontSize = 60
			};
			Vissza.Click += (s, e) => Back(s, e);
			Vissza.Click += (s, e) => Vissza_Button_Click(s, e);

			ChangeSpeed = new Button()
			{
				Content = "Harc gyorsítása",
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = 77,
				Margin = new Thickness(1641, 0, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Width = 279,
				FontSize = 30
			};
			ChangeSpeed.Click += ChangeSpeed_Click;

			Speed_Label = new Label()
			{
				Name = "Speed_Label",
				FontStyle = FontStyles.Oblique,
				FontSize = 50,
				Content = $"{Harc2.playSpeedMultiplier}x",
				HorizontalAlignment = HorizontalAlignment.Left,
				Margin = new Thickness(1746, 71, 0, 0),
				VerticalAlignment = VerticalAlignment.Top
			};

			FightGrid.Height = 1080;
			FightGrid.Width = 1920;
			/*FightGrid.Visibility = Visibility.Visible;
            MainRoom_Grid.Visibility = Visibility.Collapsed;*/
			GoToGrid(FightGrid);
			if (k.reward == KazamataReward.newcard)
			{
				foreach (var item in AllCardsDict.Values)
				{
					bool found = false;
					for (int i = 0; i < Gyujtemeny.Count; i++)
					{
						if (item.Name == Gyujtemeny[i].Name)
						{
							found = true; break;
						}
					}
					for (int i = 0; i < Jatekos.Count; i++)
					{
						if (item.Name == Jatekos[i].Name)
						{
							found = true; break;
						}
					}
					if (!found)
					{
						//gyujt.Add(item.GetCopy());
						//MainWindow.Cards_Wrap.Children.Add(gyujt[gyujt.Count-1].GetVisual());
						//ui fix needed
						//MessageBox.Show($"Játékos nyert! Nyeremény: {item.Name} kártya hozzáadva a gyűjteményhez!");

						//l2.Content =item.Name;
						WrapPanel rewardCard = CreateCenteredWrapPanel(160, 200, 5, item);
						FightGrid.Children.Add(rewardCard);
						rewardCard.Name = "rewardCard";


						break;
					}
				}
			}

			foreach (var item in Jatekos)
			{
				FightPlayer_Wrap.Children.Add(item.GetCopy().GetVisual());
			}
			foreach (var item in k.Defenders)
			{
				FightKazamata_Wrap.Children.Add(item.GetCopy().GetVisual());
			}

			FightGrid.Children.Add(FightPlayer_Wrap);
			FightGrid.Children.Add(FightKazamata_Wrap);
			FightGrid.Children.Add(Defend_Label);
			FightGrid.Children.Add(Attack_Label);
			FightGrid.Children.Add(DefendDeploy_Label);
			FightGrid.Children.Add(AttackDeploy_Label);
			FightGrid.Children.Add(Fight_Wrap);
			FightGrid.Children.Add(FightPlayerAttacker_Wrap);
			FightGrid.Children.Add(FightKazamataAttacker_Wrap);
			FightGrid.Children.Add(Harc);
			FightGrid.Children.Add(Vissza);
			FightGrid.Children.Add(ChangeSpeed);
			FightGrid.Children.Add(Speed_Label);
			FightGrid.Children.Add(Jutalom);
			/*FightGrid.Children.Add(scrollViewer);
            FightGrid.Children.Add(scrollViewer1);*/
			Harc.Click += (s, e) =>
			{
				foreach (var item in FightGrid.Children)
				{
					if (item.GetType() == typeof(WrapPanel) && (item as WrapPanel).Name == "rewardCard")
					{
						(item as WrapPanel).Children.RemoveRange(0, (item as WrapPanel).Children.Count);
					}
				}
				Harc.Visibility = Visibility.Collapsed;
				Jutalom.Visibility = Visibility.Collapsed;
				Vissza.Visibility = Visibility.Collapsed;
				Harc2.StartFight(FightGrid, Vissza, Gyujtemeny, k, Jatekos, FightPlayer_Wrap, FightKazamata_Wrap, Attack_Label, Defend_Label, AttackDeploy_Label, DefendDeploy_Label, FightPlayerAttacker_Wrap, FightKazamataAttacker_Wrap, Difficulty);

			};


			if (new Random().Next(2) == 1)
			{
				sp.Open(new Uri("Sounds/NagyHarc.wav", UriKind.Relative));
				sp.Play();
			}
			else
			{
				sp.Open(new Uri("Sounds/KozepesHarc.wav", UriKind.Relative));
				sp.Play();
			}
			menuMusic = "harc";

		}
		private void ChangeSpeed_Click(object sender, RoutedEventArgs e)
		{
			switch (Harc2.playSpeedMultiplier)
			{
				case 1:
					Harc2.playSpeedMultiplier = 2d;
					break;
				case 2:
					Harc2.playSpeedMultiplier = 4d;
					break;
				case 4:
					Harc2.playSpeedMultiplier = 8d;
					break;
				case 8:
					Harc2.playSpeedMultiplier = 0.25d;
					break;
				case 0.25d:
					Harc2.playSpeedMultiplier = 0.5d;
					break;
				case 0.5d:
					Harc2.playSpeedMultiplier = 1d;
					break;
				default:
					break;
			}

			Speed_Label.Content = $"{Harc2.playSpeedMultiplier}x";


		}
		public static Label CreateJutalom(Grid Parent)
		{
			Label Jutalom = new Label();
			Jutalom.Content = "";
			Jutalom.Name = "Jutalom";
			Parent.Children.Add(Jutalom);
			Jutalom.Height = 100;
			Jutalom.Margin = new Thickness(0, 305, 0, 0);
			Jutalom.VerticalAlignment = VerticalAlignment.Top;
			Jutalom.Width = 450;
			Jutalom.FontSize = 60;
			Jutalom.HorizontalAlignment = HorizontalAlignment.Center;
			Jutalom.HorizontalContentAlignment = HorizontalAlignment.Center;
			return Jutalom;

		}
		public void RemoveShitFomrShit()
		{
			// Remove static combat UI
			FightGrid.Children.Remove(FightPlayer_Wrap);
			FightGrid.Children.Remove(FightKazamata_Wrap);
			FightGrid.Children.Remove(Fight_Wrap);
			FightGrid.Children.Remove(FightPlayerAttacker_Wrap);
			FightGrid.Children.Remove(FightKazamataAttacker_Wrap);
			FightGrid.Children.Remove(Attack_Label);
			FightGrid.Children.Remove(Defend_Label);
			FightGrid.Children.Remove(AttackDeploy_Label);
			FightGrid.Children.Remove(DefendDeploy_Label);
			FightGrid.Children.Remove(Harc);
			FightGrid.Children.Remove(Vissza);
			FightGrid.Children.Remove(ChangeSpeed);
			FightGrid.Children.Remove(Speed_Label);
			FightGrid.Children.Remove(Jutalom);

			// Remove reward card if exists
			foreach (var item in FightGrid.Children.OfType<WrapPanel>().ToList())
			{
				if (item.Name == "rewardCard")
					FightGrid.Children.Remove(item);
			}
		}
	}
}
