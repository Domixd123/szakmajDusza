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
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		static Stack<Grid> elozoGrid = new Stack<Grid>();
		static Label Jutalom = new Label();
		static WrapPanel FightPlayer_Wrap = new WrapPanel();

		static WrapPanel FightKazamata_Wrap = new WrapPanel();

		static Label Defend_Label = new Label();

		static Label Attack_Label = new Label();

		static Label DefendDeploy_Label = new Label();

		static Label AttackDeploy_Label = new Label();

		static WrapPanel Fight_Wrap = new WrapPanel();

		static WrapPanel FightPlayerAttacker_Wrap = new WrapPanel();

		static WrapPanel FightKazamataAttacker_Wrap = new WrapPanel();

		static Button Harc = new Button();
		static Button Vissza = new Button();

		static Button ChangeSpeed = new Button();

		static Label Speed_Label = new Label();

		static public string cardEditName = "";
		static public string kazamataEditNmae = "";
		static public bool internalEdits = false;
		static public string menuMusic = "menu";
		public static int Difficulty = 0;

		public static List<Card> Gyujtemeny = new List<Card>();
		public static List<Card> Jatekos = new List<Card>();
		//public static List<Card> AllCards = new List<Card>();
		//public static List<Card> AllLeaders=new List<Card>();
		public static Dictionary<string, Card> AllLeadersDict = new Dictionary<string, Card>();
		public static Dictionary<string, Card> AllCardsDict = new Dictionary<string, Card>();
		//public static List<Kazamata> AllKazamata = new List<Kazamata>();
		public static Dictionary<string, Kazamata> AllKazamataDict = new Dictionary<string, Kazamata>();
		//public static Kazamata AllKazamata["Barlangi portya"] = new Kazamata("Barlangi portya", "egyszeru", "sebzes", new List<Card>());
		//public static Kazamata AllKazamata["Osi szentely"] = new Kazamata("Osi szentely", "kis", "eletero", new List<Card>());
		//public static Kazamata AllKazamata["A melyseg kiralynoje"] = new Kazamata("A melyseg kiralynoje", "nagy", "", new List<Card>());

		public static MediaPlayer sp = new MediaPlayer();
		public static MediaPlayer se = new MediaPlayer();

		public static Grid FightGrid = new Grid();

		public static List<Card> Merging = new List<Card>();

		public static bool editor = false;
		public static float spVolume = 0.5f;
		public static float seVolume = 0.5f;
		public static float spMult = 0.0025f;
		public static float seMult = 0.04f;
		public static Card lastCard;
		public MainWindow()
		{
			InitializeComponent();
			internalEdits = true;
			KazamataTipus.ItemsSource = new string[] { "egyszerű", "kis", "nagy" }; KazamataTipus.SelectedIndex = 0;
			KazamataJutalom.ItemsSource = new string[] { "életerő", "sebzés", "arany" }; KazamataJutalom.SelectedIndex = 0;
			internalEdits = false;
			CreateNewCard_Button.Visibility = Visibility.Collapsed;
			CreateNewKazamata_Button.Visibility = Visibility.Collapsed;

			string iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "playing-card.ico");
			this.Icon = new BitmapImage(new Uri(iconPath, UriKind.Absolute));
			Menu_Grid.Visibility = Visibility.Visible;
			Options_Grid.Visibility = Visibility.Collapsed;
			PakliOssze_Grid.Visibility = Visibility.Collapsed;
			MainRoom_Grid.Visibility = Visibility.Collapsed;
			JatekMester_Grid.Visibility = Visibility.Collapsed;
			ChooseKornyezet_Grid.Visibility = Visibility.Collapsed;
			FightGrid.Visibility = Visibility.Collapsed;
			KornyezetSzerkeszto_Grid.Visibility = Visibility.Collapsed;
			KartyaSzerkeszto_Grid.Visibility = Visibility.Collapsed;
			Shop_Grid.Visibility = Visibility.Collapsed;
			KazamataSzerkeszto_Grid.Visibility = Visibility.Collapsed;
			AddAbility_grid.Visibility = Visibility.Collapsed;

			//elozoGrid.Push(Menu_Grid);



			Difficulty_Stack.Visibility = Visibility.Collapsed;

			idk.Children.Add(FightGrid);

			sp.Open(new Uri("Sounds/Menu.wav", UriKind.Relative));
			sp.Volume = spVolume;

			sp.MediaEnded += (s, e) =>
			{
				sp.Position = TimeSpan.Zero;
				sp.Play();
			};
			sp.Play();
			Save.fileName = "";
			//KornyezetekJatekos_List.ItemsSource = Directory.GetFiles("kornyezet").Select(x => /*"kornyezet/"+*/x.Split('\\')[1].Split('.')[0]);
			KornyezetekMester_List.ItemsSource = Directory.GetFiles("kornyezet").Select(x => x.Split('\\')[1].Split('.')[0]);
			//KornyezetekJatekos_List.ItemsSource = Directory.GetFiles("saves").Select(x => "saves/" + x.Split('\\')[1].Split('.')[0]);
			var k1 = Directory.GetFiles("kornyezet")
	.Select(x => Path.GetFileNameWithoutExtension(x) + " (új)");

			var k2 = Directory.GetFiles("saves")
				.Select(x => Path.GetFileNameWithoutExtension(x) + " (mentett)");

			KornyezetekJatekos_List.ItemsSource = k1.Concat(k2).ToList();

		}
		private void Vissza_Button_Click(object sender, RoutedEventArgs e)
		{
			foreach (var item in ((sender as Button).Parent as Grid).Children)
			{
				if (item.GetType() == typeof(Label))
				{
					if ((item as Label).Name == "Jutalom")
					{
						((sender as Button).Parent as Grid).Children.Remove(item as Label);
						break;
					}
				}
			}
			foreach (var item in ((sender as Button).Parent as Grid).Children)
			{
				if (item.GetType() == typeof(Label))
				{
					if ((item as Label).Name == "Jutalom")
					{
						((sender as Button).Parent as Grid).Children.Remove(item as Label);
						break;
					}
				}
			}
			foreach (var item in ((sender as Button).Parent as Grid).Children)
			{
				if (item.GetType() == typeof(Label))
				{
					if ((item as Label).Name == "Jutalom")
					{
						((sender as Button).Parent as Grid).Children.Remove(item as Label);
						break;
					}
				}
			}
			foreach (var item in FightGrid.Children)
			{
				if (item.GetType() == typeof(WrapPanel) && (item as WrapPanel).Name == "rewardCard")
				{
					(item as WrapPanel).Children.RemoveRange(0, (item as WrapPanel).Children.Count);
				}
			}
			RemoveShitFomrShit();
			//GoToGrid(MainRoom_Grid);
			DisableNagyKazamata();
			/*sp.Stop();
            sp.Open(new Uri("Sounds/Menu.wav", UriKind.Relative));
            sp.Play();
            RemoveShitFomrShit();
            ShowPakli();*/
		}
		public static WrapPanel CreateCenteredWrapPanel(double width, double height, double spacing, Card kartya)
		{
			WrapPanel wrap = new WrapPanel
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Orientation = Orientation.Horizontal, // vízszintes
				Width = width,
				Height = height,
				Margin = new Thickness(spacing)
			};
			wrap.Children.Add(kartya.GetCopy().GetVisual());
			return wrap;
		}
		private void AddKornyezet_Button_Click(object sender, RoutedEventArgs e)
		{
			Gyujtemeny.Clear();
			Jatekos.Clear();
			MindenKartya_List.Children.Clear();
			MindenKazamata_List.Children.Clear();
			AllCardsDict.Clear();
			AllLeadersDict.Clear();
			AllKazamataDict.Clear();
			Item.ResetItems();
			GoToGrid(KornyezetSzerkeszto_Grid);
		}
		private void KornyezetekMester_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (KornyezetekMester_List.SelectedItem == null)
			{
				return;
			}
			LoadSave($"kornyezet/{KornyezetekMester_List.SelectedItem.ToString()}.txt");
			Save.fileName = KornyezetekMester_List.SelectedItem.ToString() + ".txt";
			MindenKartya_List.Children.Clear();
			MindenKazamata_List.Children.Clear();
			//loaddata was here
			GoToGrid(KornyezetSzerkeszto_Grid);
			KornyezetekMester_List.SelectedItem = null;
		}
		private void ListKartya_Button_Click(object sender, RoutedEventArgs e)
		{
			MindenKazamata_List.Children.Clear();
			MindenKartya_List.Children.Clear();
			if (sender != null)
			{
				MindenKartya_List.Visibility = Visibility.Visible;
			}

			foreach (var item in AllCardsDict.Values)
			{
				Card c = item.GetCopy();
				MindenKartya_List.Children.Add(c.GetVisual());
				// if (sender!=null)
				{
					c.Clicked += (s, e) => { SelectForModify(c); };
				}

			}
			foreach (var item in AllLeadersDict.Values)
			{
				Card c = item.GetCopy();
				MindenKartya_List.Children.Add(c.GetVisual());
				//if (sender != null)
				{
					c.Clicked += (s, e) => { SelectForModify(c); };
				}
				//c.UpdateAllVisual();

			}
			CreateNewCard_Button.Visibility = Visibility.Visible;
			CreateNewKazamata_Button.Visibility = Visibility.Collapsed;
		}
		private void DeleteCard_Click(object sender, RoutedEventArgs e)
		{
			string cardName = cardEditName;

			foreach (var item in AllLeadersDict.Values)
			{
				if (item.OriginName == cardEditName)
				{
					MessageBox.Show("A kártya törlése előtt töröld az összes vezért ami belőle következik!");
					return;
				}
			}

			foreach (var item in AllKazamataDict.Values)
			{
				foreach (var item2 in item.Defenders)
				{
					if (item2.Name == cardName)
					{
						MessageBox.Show("A kártya törlése előtt vedd ki az összes kazamatából!");
						return;
					}
				}
			}

			if (Gyujtemeny.FirstOrDefault(x => x.Name == cardName) != null)
			{
				Gyujtemeny.Remove(Gyujtemeny.First(x => x.Name == cardName));
			}

			AllCardsDict.Remove(cardName);
			AllLeadersDict.Remove(cardName);

			MessageBox.Show("Sikeres törlés!", "", MessageBoxButton.OK, MessageBoxImage.Information);
			ListKartya_Button_Click(null, null);
			Back(sender, e);
		}
		private void BackToKornyezetSzerkeszto_Click(object sender, RoutedEventArgs e)
		{
			SelectedCard_Wrap.Children.Clear();
			ListKartya_Button_Click(null, e);
			Back(sender, e);
			/*KartyaSzerkeszto_Grid.Visibility = Visibility.Collapsed;
            KornyezetSzerkeszto_Grid.Visibility = Visibility.Visible;*/
		}
		private void BackToKornyezetSzerkesztoKazamata_Click(object sender, RoutedEventArgs e)
		{
			Kazamata_Button_Click(null, e);
			Back(sender, e);
			/*KartyaSzerkeszto_Grid.Visibility = Visibility.Collapsed;
            KornyezetSzerkeszto_Grid.Visibility = Visibility.Visible;*/
		}
		private void SelectForModify(Card k)
		{
			SelectedCard_Wrap.Children.Clear();
			VezerAlapKartya.ItemsSource = AllCardsDict.Keys;
			if (AllCardsDict.Count > 1 || k.Vezer)
			{
				VezerCheck.IsEnabled = true;
			}
			else
			{
				VezerCheck.IsEnabled = false;
			}
			internalEdits = true;
			cardEditName = k.Name;
			Gyujtemeny_Check.IsChecked = false;
			BasicCardPanel.Visibility = Visibility.Visible;
			LeaderCardPanel.Visibility = Visibility.Collapsed;
			foreach (Card item in Gyujtemeny)
			{
				if (item.Name == k.Name)
				{
					Gyujtemeny_Check.IsChecked = true;
					break;
				}
			}
			if (k.Vezer)
			{
				BasicCardPanel.Visibility = Visibility.Collapsed;
				LeaderCardPanel.Visibility = Visibility.Visible;
				VezerBonusTipus.ItemsSource = new string[] { "Életerő", "Sebzés" };
				VezerCheck.IsChecked = true;
				string vAlap = k.Name2Label.Content.ToString();
				int index = -1;
				foreach (var item in AllCardsDict.Keys)
				{
					index++;
					if (vAlap == item)
					{
						break;
					}
				}
				VezerAlapKartya.SelectedIndex = index;
				VezerNev.Text = k.NameLabel.Content.ToString();
				if (AllLeadersDict[cardEditName].HP != AllCardsDict[vAlap].HP)
				{
					VezerBonusTipus.SelectedIndex = 0;
				}
				else VezerBonusTipus.SelectedIndex = 1;
				SelectedCard_Wrap.Children.Clear();
				SelectedCard_Wrap.Children.Add(k.GetCopy().GetVisual());
			}
			else
			{
				BasicCardPanel.Visibility = Visibility.Visible;
				LeaderCardPanel.Visibility = Visibility.Collapsed;
				VezerCheck.IsChecked = false;
				KartyaSzerkesztoCardName.Text = k.Name;
				SelectType.ItemsSource = new string[] { "Föld", "Víz", "Levegő", "Tűz" };
				TypeAttack.Text = k.Damage.ToString();
				TypeDefense.Text = k.HP.ToString();
				if (k.Tipus == KartyaTipus.fold) SelectType.SelectedIndex = 0;
				else if (k.Tipus == KartyaTipus.viz) SelectType.SelectedIndex = 1;
				else if (k.Tipus == KartyaTipus.levego) SelectType.SelectedIndex = 2;
				else if (k.Tipus == KartyaTipus.tuz) SelectType.SelectedIndex = 3;
				SelectedCard_Wrap.Children.Add(k.GetCopy().GetVisual());

			}
			GoToGrid(KartyaSzerkeszto_Grid);

			internalEdits = false;
			//UpdateKartyaSelectionCard(null, null);
		}
		/*private void HomeButton(object sender, RoutedEventArgs e)
		{
			while (elozoGrid.Count != 0)
			{
				Back(null, null);
			}
		}*/
		private void UpdateKartyaSelectionCard(object sender, RoutedEventArgs es)
		{
			if (!internalEdits)
			{
				if ((bool)VezerCheck.IsChecked)
				{




					if (VezerBonusTipus.SelectedIndex == 0)
					{
						AllLeadersDict[cardEditName].HP = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].HP * 2;
						AllLeadersDict[cardEditName].Damage = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].Damage;

					}
					else
					{
						AllLeadersDict[cardEditName].Damage = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].Damage * 2; AllLeadersDict[cardEditName].HP = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].HP;
					}


					AllLeadersDict[cardEditName.ToString()].UpdateAllVisual();
					SelectedCard_Wrap.Children.Clear();
					SelectedCard_Wrap.Children.Add(AllLeadersDict[cardEditName.ToString()].GetVisual());

					if ((bool)Gyujtemeny_Check1.IsChecked)
					{
						bool isIngyujt = false;
						foreach (Card item in Gyujtemeny)
						{
							if (item.Name == cardEditName)
							{
								isIngyujt = true;
								break;
							}
						}
						if (!isIngyujt)
						{
							Gyujtemeny.Add(AllLeadersDict[cardEditName].GetCopy());
						}
						else
						{
							foreach (Card item in Gyujtemeny)
							{
								if (item.Name == cardEditName)
								{
									if (VezerBonusTipus.SelectedIndex == 0)
									{
										item.HP = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].HP * 2;
										item.Damage = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].Damage;

									}
									else
									{
										item.Damage = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].Damage * 2; item.HP = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].HP;
									}
									break;
								}
							}
						}



					}
					else
					{
						for (int i = 0; i < Gyujtemeny.Count; i++)
						{
							if (Gyujtemeny[i].Name == cardEditName)
							{
								Gyujtemeny.RemoveAt(i);
								break;
							}
						}
					}

				}
				else
				{

					try
					{
						if (int.Parse(TypeAttack.Text) > 0) AllCardsDict[cardEditName.ToString()].Damage = int.Parse(TypeAttack.Text);
						else
						{
							MessageBox.Show("Szöveg", "", MessageBoxButton.OK, MessageBoxImage.Error);
						} //kristof make error message
					}
					catch { }
					try
					{
						if (int.Parse(TypeDefense.Text) > 0) AllCardsDict[cardEditName.ToString()].HP = int.Parse(TypeDefense.Text);
						else
						{
							MessageBox.Show("Szöveg", "", MessageBoxButton.OK, MessageBoxImage.Error);
						}//kristof make error message
					}
					catch { }
					if (SelectType.SelectedIndex == 0) AllCardsDict[cardEditName.ToString()].Tipus = KartyaTipus.fold;
					else if (SelectType.SelectedIndex == 1) AllCardsDict[cardEditName.ToString()].Tipus = KartyaTipus.viz;
					else if (SelectType.SelectedIndex == 2) AllCardsDict[cardEditName.ToString()].Tipus = KartyaTipus.levego;
					else if (SelectType.SelectedIndex == 3) AllCardsDict[cardEditName.ToString()].Tipus = KartyaTipus.tuz;
					AllCardsDict[cardEditName.ToString()].UpdateAllVisual();
					SelectedCard_Wrap.Children.Clear();
					SelectedCard_Wrap.Children.Add(AllCardsDict[cardEditName.ToString()].GetCopy().GetVisual());


					if ((bool)Gyujtemeny_Check.IsChecked)
					{
						bool isIngyujt = false;
						foreach (Card item in Gyujtemeny)
						{
							if (item.Name == cardEditName)
							{
								isIngyujt = true;
								break;
							}
						}
						if (!isIngyujt)
						{
							Gyujtemeny.Add(AllCardsDict[cardEditName].GetCopy());
						}
						else
						{
							foreach (Card item in Gyujtemeny)
							{
								if (item.Name == cardEditName)
								{
									try
									{
										if (int.Parse(TypeAttack.Text) > 0) item.Damage = int.Parse(TypeAttack.Text);
										else
										{
											MessageBox.Show("Szöveg", "", MessageBoxButton.OK, MessageBoxImage.Error);
										} //kristof make error message
									}
									catch { }
									try
									{
										if (int.Parse(TypeDefense.Text) > 0) item.HP = int.Parse(TypeDefense.Text);
										else
										{
											MessageBox.Show("Szöveg", "", MessageBoxButton.OK, MessageBoxImage.Error);
										}//kristof make error message
									}
									catch { }
									if (SelectType.SelectedIndex == 0) item.Tipus = KartyaTipus.fold;
									else if (SelectType.SelectedIndex == 1) item.Tipus = KartyaTipus.viz;
									else if (SelectType.SelectedIndex == 2) item.Tipus = KartyaTipus.levego;
									else if (SelectType.SelectedIndex == 3) item.Tipus = KartyaTipus.tuz;
									break;
								}
							}
						}

					}
					else
					{
						for (int i = 0; i < Gyujtemeny.Count; i++)
						{
							if (Gyujtemeny[i].Name == cardEditName)
							{
								Gyujtemeny.RemoveAt(i);
								break;
							}
						}
					}

				}


			}
		}
		private void DynamicButtonsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			AutoResizeButtons(sender as WrapPanel);
		}
		private void AutoResizeButtons(WrapPanel wrap)
		{
			if (wrap.Children.Count == 0)
				return;

			//double panelWidth = wrap.ActualWidth;
			double panelWidth = 700;
			double minButtonWidth = 250; // minimális gombszélesség
			int buttonsPerRow = Math.Max(1, (int)(panelWidth / minButtonWidth));

			double buttonWidth = (panelWidth / buttonsPerRow) - 20; // 20 = margin


			foreach (Button b in wrap.Children)
			{
				b.Width = buttonWidth;
				b.Height = buttonWidth * 0.6; // arányos magasság
				b.FontSize = buttonWidth / 15; // automatikus fontsize skálázás
			}
		}
		private void KornyezetekJatekos_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (KornyezetekJatekos_List.SelectedItem == null)
				return;

			// Csak megjelenítjük a nehézségi választót
			string fileName = KornyezetekJatekos_List.SelectedItem.ToString().Split('(')[0][..^1];
			if (KornyezetekJatekos_List.SelectedItem.ToString().Split('(')[1] == "mentett)")
			{
				editor = false;
				LoadSave($"saves/{fileName}.txt");
				KornyezetekJatekos_List.SelectedItem = null;
				GoToGrid(PakliOssze_Grid);
			}
			else
			{
				Difficulty_Stack.Visibility = Visibility.Visible;
			}

		}
		private void Kazamata_Button_Click(object sender, RoutedEventArgs e)
		{
			KazMent.IsEnabled = false;
			CreateNewCard_Button.Visibility = Visibility.Collapsed;
			CreateNewKazamata_Button.Visibility = Visibility.Visible;
			MindenKazamata_List.Children.Clear();
			MindenKartya_List.Children.Clear();
			if (sender != null)
			{
				MindenKazamata_List.Visibility = Visibility.Visible;
			}
			foreach (var item in AllKazamataDict.Values)
			{
				Button b = new Button();
				b.Click += EditKazamata;

				b.Content = item.Name;
				b.Margin = new Thickness(10, 0, 0, 0);
				MindenKazamata_List.Children.Add(b);
			}

			AutoResizeButtons(MindenKazamata_List);

			/*foreach (var item in AllCardsDict.Values)
            {
                Card c = item.GetCopy();
                MindenKartya_List.Children.Add(c.GetVisual());
                // if (sender!=null)
                {
                    c.Clicked += (s, e) => { SelectForModify(c); };
                }

            }
            foreach (var item in AllLeadersDict.Values)
            {
                Card c = item.GetCopy();
                MindenKartya_List.Children.Add(c.GetVisual());
                //if (sender != null)
                {
                    c.Clicked += (s, e) => { SelectForModify(c); };
                }
                //c.UpdateAllVisual();

            }
            CreateNewCard_Button.Visibility = Visibility.Visible;*/

		}
		public void EditKazamata(object sender, RoutedEventArgs e)
		{
			internalEdits = true;
			GoToGrid(KazamataSzerkeszto_Grid);
			MindenKartya_ListKazamata.Children.Clear();
			KazamataDeffenders_List.Children.Clear();
			Kazamata k = AllKazamataDict[(sender as Button).Content.ToString()];
			KazamataName.Text = k.Name;
			foreach (var item in AllCardsDict.Keys)
			{
				if (k.GetDefenderNames().Contains(item))
				{
					//Card c = AllCardsDict[item].GetCopy(true);
					foreach (Card c in k.Defenders)
					{
						if (c.Name == item)
						{
							c.RightClicked -= RightClick;
							c.RightClicked += RightClick;
							c.Clicked += RemoveFromKazamata;
							KazamataDeffenders_List.Children.Add(c.GetVisual());
							break;
						}
					}

				}
				else
				{
					Card c = AllCardsDict[item].GetCopy(true);
					c.RightClicked -= RightClick;
					//c.RightClicked += RightClick;

					c.Clicked += AddToKazamata;
					MindenKartya_ListKazamata.Children.Add(c.GetVisual());
				}
			}
			int vezerCount = 0;
			foreach (var item in AllLeadersDict.Keys)
			{
				if (k.GetDefenderNames().Contains(item))
				{
					//Card c = AllLeadersDict[item].GetCopy(true);
					foreach (Card c in k.Defenders)
					{
						if (c.Name == item)
						{
							c.RightClicked -= RightClick;
							c.RightClicked += RightClick;
							c.Clicked += RemoveFromKazamata;
							KazamataDeffenders_List.Children.Add(c.GetVisual());
							break;
						}
					}
					vezerCount++;
				}
				else
				{
					Card c = AllLeadersDict[item].GetCopy(true);
					c.RightClicked -= RightClick;
					//c.RightClicked += RightClick;

					c.Clicked += AddToKazamata;
					MindenKartya_ListKazamata.Children.Add(c.GetVisual());
				}
			}
			if (k.Tipus == KazamataType.egyszeru)
			{
				KazamataJutalom.Visibility = Visibility.Visible;
				KazamataJutalom_Label.Visibility = Visibility.Visible;
				KazamataTipus.SelectedIndex = 0;
				KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count}/1";
			}
			else if (k.Tipus == KazamataType.kis)
			{
				KazamataJutalom.Visibility = Visibility.Visible;
				KazamataJutalom_Label.Visibility = Visibility.Visible;
				KazamataTipus.SelectedIndex = 1;
				KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count - vezerCount}/3 és {vezerCount}/1 vezér";

			}
			else
			{
				KazamataJutalom.Visibility = Visibility.Collapsed;
				KazamataJutalom_Label.Visibility = Visibility.Collapsed;
				KazamataTipus.SelectedIndex = 2;
				KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count - vezerCount}/5 és {vezerCount}/1 vezér";

			}
			if (k.reward == KazamataReward.eletero)
			{
				KazamataJutalom.SelectedIndex = 0;
			}
			else if (k.reward == KazamataReward.sebzes)
			{
				KazamataJutalom.SelectedIndex = 1;
			}
			else if (k.reward == KazamataReward.arany)
			{
				KazamataJutalom.SelectedIndex = 2;
			}
			kazamataEditNmae = k.Name;



			internalEdits = false;
		}
		public void UpdateKazamata()
		{
			if (!internalEdits)
			{
				MindenKartya_ListKazamata.Children.Clear();
				KazamataDeffenders_List.Children.Clear();
				Kazamata k = AllKazamataDict[kazamataEditNmae];
				foreach (var item in AllCardsDict.Keys)
				{
					if (k.GetDefenderNames().Contains(item))
					{
						Card c = AllCardsDict[item].GetCopy(true);
						c.RightClicked -= RightClick;
						c.RightClicked += RightClick;

						c.Clicked -= AddToKazamata;
						c.Clicked += RemoveFromKazamata;
						KazamataDeffenders_List.Children.Add(c.GetVisual());
					}
					else
					{
						Card c = AllCardsDict[item].GetCopy(true);
						c.RightClicked -= RightClick;
						//c.RightClicked += RightClick;

						c.Clicked -= RemoveFromKazamata;
						c.Clicked += AddToKazamata;
						MindenKartya_ListKazamata.Children.Add(c.GetVisual());
					}
				}
				int vezerCount = 0;
				foreach (var item in AllLeadersDict.Keys)
				{
					if (k.GetDefenderNames().Contains(item))
					{
						Card c = AllLeadersDict[item].GetCopy(true);
						c.RightClicked -= RightClick;
						c.RightClicked += RightClick;

						c.Clicked -= AddToKazamata;
						c.Clicked += RemoveFromKazamata;
						KazamataDeffenders_List.Children.Add(c.GetVisual());
						vezerCount++;
					}
					else
					{
						Card c = AllLeadersDict[item].GetCopy(true);
						c.RightClicked -= RightClick;
						//c.RightClicked += RightClick;

						c.Clicked -= RemoveFromKazamata;
						c.Clicked += AddToKazamata;
						MindenKartya_ListKazamata.Children.Add(c.GetVisual());
					}
				}



				if (k.Tipus == KazamataType.egyszeru)
				{
					KazamataTipus.SelectedIndex = 0;
					KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count}/1";
				}
				else if (k.Tipus == KazamataType.kis)
				{
					KazamataTipus.SelectedIndex = 1;
					KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count - vezerCount}/3 és {vezerCount}/1 vezér";

				}
				else
				{
					KazamataTipus.SelectedIndex = 2;
					KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count - vezerCount}/5 és {vezerCount}/1 vezér";

				}
			}


		}
		private void KazamataTipus_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!internalEdits)
			{
				AllKazamataDict[kazamataEditNmae].Tipus = Kazamata.StringToKazamataType(KazamataTipus.SelectedItem.ToString());
				foreach (Card item in AllKazamataDict[kazamataEditNmae].Defenders)
				{
					item.Items.Clear();
				}
				AllKazamataDict[kazamataEditNmae].Defenders.Clear();
				MindenKartya_ListKazamata.Children.Clear();
				KazamataDeffenders_List.Children.Clear();
				Kazamata k = AllKazamataDict[kazamataEditNmae];
				if (k.Tipus == KazamataType.egyszeru)
				{
					KazamataJutalom.Visibility = Visibility.Visible;
					KazamataJutalom_Label.Visibility = Visibility.Visible;
					KazamataTipus.SelectedIndex = 0;
					KazamataKartya.Content = $"{0}/1";
				}
				else if (k.Tipus == KazamataType.kis)
				{
					KazamataJutalom.Visibility = Visibility.Visible;
					KazamataJutalom_Label.Visibility = Visibility.Visible;

					KazamataTipus.SelectedIndex = 1;
					KazamataKartya.Content = $"{0}/3 és {0}/1 vezér";

				}
				else
				{
					KazamataJutalom.Visibility = Visibility.Collapsed;
					KazamataJutalom_Label.Visibility = Visibility.Collapsed;
					AllKazamataDict[kazamataEditNmae].reward = KazamataReward.newcard;
					KazamataTipus.SelectedIndex = 2;
					KazamataKartya.Content = $"{0}/5 és {0}/1 vezér";

				}
				UpdateKazamata();

			}

		}
		private void AddToKazamata(object? sender, Card clicked)
		{

			int vezer = 0;
			int kazamataDef = KazamataDeffenders_List.Children.Count;
			int kazamataMaxDef = 0;
			foreach (Card item in AllKazamataDict[kazamataEditNmae].Defenders)
			{
				if (item.Vezer)
				{
					vezer++;
				}
			}
			if (AllKazamataDict[kazamataEditNmae].Tipus == KazamataType.egyszeru)
			{
				kazamataMaxDef = 1;
			}
			else if (AllKazamataDict[kazamataEditNmae].Tipus == KazamataType.kis)
			{
				kazamataMaxDef = 3;
			}
			else
			{
				kazamataMaxDef = 5;
			}
			if (clicked.Vezer && vezer == 0 && AllKazamataDict[kazamataEditNmae].Tipus != KazamataType.egyszeru)
			{
				vezer++;
				kazamataDef++;
				clicked.Clicked -= AddToKazamata;
				clicked.Clicked += RemoveFromKazamata;
				AllKazamataDict[kazamataEditNmae].Defenders.Add(clicked);
				UpdateKazamata();
			}
			else if (!clicked.Vezer && kazamataDef - vezer < kazamataMaxDef)
			{
				kazamataDef++;
				clicked.Clicked -= AddToKazamata;
				clicked.Clicked += RemoveFromKazamata;
				AllKazamataDict[kazamataEditNmae].Defenders.Add(clicked);
				UpdateKazamata();
			}
			if (AllKazamataDict[kazamataEditNmae].Tipus != KazamataType.egyszeru && (vezer == 1 && kazamataDef - vezer == kazamataMaxDef))
			{
				KazMent.IsEnabled = true;
			}
			else if (AllKazamataDict[kazamataEditNmae].Tipus == KazamataType.egyszeru && kazamataDef - vezer == kazamataMaxDef)
			{
				KazMent.IsEnabled = true;

			}
			else
			{
				KazMent.IsEnabled = false;

			}


		}
		private void RemoveFromKazamata(object? sender, Card clicked)
		{


			clicked.Clicked += AddToKazamata;
			clicked.Clicked -= RemoveFromKazamata;
			//AllKazamataDict[kazamataEditNmae].Defenders.Remove(clicked);
			foreach (Card item in AllKazamataDict[kazamataEditNmae].Defenders)
			{
				if (item.Name == clicked.Name)
				{
					AllKazamataDict[kazamataEditNmae].Defenders.Remove(item);
					break;
				}
			}
			UpdateKazamata();



			KazMent.IsEnabled = false;


		}
		private void KazamataName_LostFocus(object sender, RoutedEventArgs e)
		{
			if (!internalEdits)
			{
				string edits = KazamataName.Text;
				Kazamata k = AllKazamataDict[kazamataEditNmae];
				k.Name = edits;
				AllKazamataDict.Remove(kazamataEditNmae);
				AllKazamataDict.Add(edits, k);

				kazamataEditNmae = edits;
			}
		}
		private void KazamataName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
			{
				if (!internalEdits)
				{
					string edits = KazamataName.Text;
					Kazamata k = AllKazamataDict[kazamataEditNmae];
					k.Name = edits;
					AllKazamataDict.Remove(kazamataEditNmae);
					AllKazamataDict.Add(edits, k);

					kazamataEditNmae = edits;
				}
			}
		}
		private void KazamataJutalom_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!internalEdits)
			{
				if (KazamataJutalom.SelectedIndex == 0)
				{
					AllKazamataDict[kazamataEditNmae].reward = KazamataReward.eletero;
				}
				else if (KazamataJutalom.SelectedIndex == 1)
				{
					AllKazamataDict[kazamataEditNmae].reward = KazamataReward.sebzes;
				}
				else
				{
					AllKazamataDict[kazamataEditNmae].reward = KazamataReward.arany;
				}
			}
		}
		private void SaveProgress_Click(object sender, RoutedEventArgs e)
		{
			if (Save.fileName == null || Save.fileName == "")
			{
				GoToGrid(Save_Grid);
			}
			else
			{
				Save.SaveProgress();
			}
		}
		private async void Save_World_Button_Click(object sender, RoutedEventArgs e)
		{
			if (elozoGrid.Peek().Name == "KornyezetSzerkeszto_Grid")
			{
				if (File.Exists($"kornyezet/{FileName_TextBox.Text}.txt"))
				{
					UsedFileNameError_Label.Visibility = Visibility.Visible;
					await Task.Delay(2000);
					UsedFileNameError_Label.Visibility = Visibility.Collapsed;
				}
				else
				{
					Save.fileName = $"{FileName_TextBox.Text}.txt";
					Save.Kornyezetrogress();
					KornyezetekMester_List.ItemsSource = Directory.GetFiles("kornyezet").Select(x => x.Split('\\')[1].Split('.')[0]);
					var k1 = Directory.GetFiles("kornyezet")
	.Select(x => Path.GetFileNameWithoutExtension(x) + " (új)");

					var k2 = Directory.GetFiles("saves")
						.Select(x => Path.GetFileNameWithoutExtension(x) + " (mentett)");

					KornyezetekJatekos_List.ItemsSource = k1.Concat(k2).ToList();
					Back(sender, e);
					Save.fileName = "";
					Back(sender, e);
				}
			}
			else
			{
				if (File.Exists($"saves/{FileName_TextBox.Text}.txt"))
				{
					UsedFileNameError_Label.Visibility = Visibility.Visible;
					await Task.Delay(2000);
					UsedFileNameError_Label.Visibility = Visibility.Collapsed;
				}
				else
				{
					Save.fileName = $"{FileName_TextBox.Text}.txt";
					Save.SaveProgress();
					Back(sender, e);

				}
			}

		}
	}
}