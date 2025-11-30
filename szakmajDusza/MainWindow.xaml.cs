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


		public static float spVolume = 0.25f;
		public static float seVolume = 0.5f;
		public static float spMult = 0f;
		public static float seMult = 0f;
		public MainWindow()
		{
			InitializeComponent();
			internalEdits = true;
			KazamataTipus.ItemsSource = new string[] { "egyszerű", "kis", "nagy" }; KazamataTipus.SelectedIndex = 0;
			KazamataJutalom.ItemsSource = new string[] { "életerő", "sebzés" }; KazamataJutalom.SelectedIndex = 0;
			internalEdits = false;

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
			//KornyezetekJatekos_List.ItemsSource = Directory.GetFiles("kornyezet").Select(x => /*"kornyezet/"+*/x.Split('\\')[1].Split('.')[0]);
			KornyezetekMester_List.ItemsSource = Directory.GetFiles("kornyezet").Select(x => x.Split('\\')[1].Split('.')[0]);
			//KornyezetekJatekos_List.ItemsSource = Directory.GetFiles("saves").Select(x => "saves/" + x.Split('\\')[1].Split('.')[0]);
			var k1 = Directory.GetFiles("kornyezet")
	.Select(x =>  Path.GetFileNameWithoutExtension(x)+" (új)");

			var k2 = Directory.GetFiles("saves")
				.Select(x => Path.GetFileNameWithoutExtension(x)+" (mentett)");

			KornyezetekJatekos_List.ItemsSource = k1.Concat(k2).ToList();

		}


		private void RightClick(object sender, Card c)
		{
            SelectedCardForAbility.Children.Clear();
            Ability_Wrap.Children.Clear();
            GoToGrid(AddAbility_grid);
			SelectedCardForAbility.Children.Add(c.GetCopy().GetVisual());
			foreach (var item in Item.Items.Values)
			{
				if (item.OwnedCount > 0)
				{
					Item i = item.GetCopy();
					i.Clicked += (s, e) => AddAbility(i, c);
					Ability_Wrap.Children.Add(i.GetVisual(false));
				}
			}
		}

		private void AddAbility(Item item, Card c)
		{
			c.Items.Add(item);
			c.UpdateAllVisual();
			SelectedCardForAbility.Children.Clear();
			Ability_Wrap.Children.Clear();
			Back(null, null);

			PlayerCards_Wrap.Children.Clear();
			Cards_Wrap.Children.Clear();

			foreach (var a in Gyujtemeny)
			{
				if (!Jatekos.Contains(a))
				{
                    Cards_Wrap.Children.Add(a.GetVisual());
                }
				
			}
			foreach (var a in Jatekos)
			{
				PlayerCards_Wrap.Children.Add(a.GetVisual());
			}
		}
		public void DisableNagyKazamata()
		{
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
				if (!found) return;//there are cards that can still be acquired
			}
			foreach (var item in DynamicButtonsPanel.Children)//only runs if all normal cards are already acquired
			{
				if (item.GetType() == typeof(Button) && AllKazamataDict.ContainsKey((item as Button).Content.ToString()) && AllKazamataDict[(item as Button).Content.ToString()].Tipus == KazamataType.nagy)
				{
					(item as Button).IsEnabled = false;
				}
			}
		}
		public void LoadSave(string path)
		{
			Item.ResetItems();
			DynamicButtonsPanel.Children.Clear();
			Gyujtemeny.Clear();
			Jatekos.Clear();
			AllCardsDict.Clear();
			AllLeadersDict.Clear();
			AllKazamataDict.Clear();
			Cards_Wrap.Children.Clear();
			PlayerCards_Wrap.Children.Clear();
			StreamReader sr = new StreamReader(path);
			while (!sr.EndOfStream)
			{
				string? line = sr.ReadLine();
				if (line == null || line == "")
				{
					continue;
				}

				string[] data = line.Split(';');
				if (data[0] == "difficulty")
				{
					Difficulty=int.Parse(data[1]);
				}
				else if (data[0] == "uj kartya")
				{
					AllCardsDict.Add(data[1], new Card(data[1], int.Parse(data[2]), int.Parse(data[3]), data[4], false));
				}
				else if (data[0] == "uj vezer")
				{
					Card vezer = AllCardsDict[data[2]].GetCopy();
					if (data[3] == "sebzes") vezer.Damage *= 2;
					else if (data[3] == "eletero") vezer.HP *= 2;
					vezer.Vezer = true;
					vezer.Name = data[1];
					vezer.Bonus = data[3];
					vezer.OriginName = data[2];
					vezer.UpdateAllVisual();
					AllLeadersDict.Add(data[1], vezer);
					AllLeadersDict[data[1]].UpdateAllVisual();
				}
				else if (data[0] == "uj kazamata")
				{
					if (data[1] == "egyszeru")
					{
						List<Card> defender = new List<Card>();
						Card card = AllCardsDict[data[3].Split(':')[0]].GetCopy(true);
						defender.Add(card);
						string[] itemNames = data[3].Split(':')[1].Split(',');
						if (itemNames.Length != 1 || itemNames[0] != "")
						{
							for (int i = 0; i < itemNames.Length; i++)
							{
								Item item = Item.Items[itemNames[i].Split('-')[0]].GetCopy();
								int level = int.Parse(itemNames[i].Split('-')[1]);
								item.Level = level;
								defender[0].Items.Add(item);
							}
						}
						Kazamata kazamata = new Kazamata(data[2], "egyszeru", data[4],defender);
						AllKazamataDict.Add(data[2],kazamata);
					}
					else if (data[1] == "kis")
					{
						List<Card>defenders=new List<Card>();
						for (int i = 3; i < data.Length-1; i++)
						{
							string cardName = data[i].Split(':')[0];
							string[] cardItems= data[i].Split(":")[1].Split(',');
							if (AllCardsDict.ContainsKey(cardName))
							{
								defenders.Add(AllCardsDict[cardName].GetCopy(true));
							}
							else
							{
								defenders.Add(AllLeadersDict[cardName].GetCopy(true));
							}
							if (cardItems.Length == 1 && cardItems[0] == "") continue;
							for (int j = 0; j < cardItems.Length; j++)
							{
								string itemName = cardItems[j].Split('-')[0];
								int itemLevel = int.Parse(cardItems[j].Split('-')[1]);
								Item item = Item.Items[itemName];
								item.Level = itemLevel;
								defenders[defenders.Count - 1].Items.Add(item);
							}
						}
						Kazamata kazamata = new Kazamata(data[2], "kis", data[data.Length-1], defenders);
						AllKazamataDict.Add(data[2], kazamata);
					}
					else if (data[1] == "nagy")
					{
						List<Card> defenders = new List<Card>();
						for (int i = 3; i < data.Length; i++)
						{
							string cardName = data[i].Split(':')[0];
							string[] cardItems = data[i].Split(":")[1].Split(',');
							if (AllCardsDict.ContainsKey(cardName))
							{
								defenders.Add(AllCardsDict[cardName].GetCopy(true));
							}
							else
							{
								defenders.Add(AllLeadersDict[cardName].GetCopy(true));
							}
							if (cardItems.Length == 1 && cardItems[0] == "") continue;
							for (int j = 0; j < cardItems.Length; j++)
							{
								string itemName = cardItems[j].Split('-')[0];
								int itemLevel = int.Parse(cardItems[j].Split('-')[1]);
								Item item = Item.Items[itemName];
								item.Level = itemLevel;
								defenders[defenders.Count - 1].Items.Add(item);
							}
						}
						Kazamata kazamata = new Kazamata(data[2], "nagy", "", defenders);
						AllKazamataDict.Add(data[2], kazamata);
					}


				}
				else if (data[0] == "felvetel gyujtemenybe")
				{
					if (AllCardsDict.ContainsKey(data[1]))
					{
						Gyujtemeny.Add(AllCardsDict[data[1]].GetCopy());
					}
					else if (AllLeadersDict.ContainsKey(data[1]))
					{
						Gyujtemeny.Add(AllLeadersDict[data[1]].GetCopy());
					}
				}
				else if (data[0] == "uj pakli")
				{
					string[] kartyanevek = data[1].Split(',');
					for (int i = 0; i < kartyanevek.Length; i++)
					{
						if (AllCardsDict.ContainsKey(kartyanevek[i]))
						{
							Jatekos.Add(AllCardsDict[kartyanevek[i]]);
						}
						else if (AllLeadersDict.ContainsKey(kartyanevek[i]))
						{
							Jatekos.Add(AllCardsDict[kartyanevek[i]]);
						}
					}
				}
				else if (data[0]=="arany")
				{
					Item.GoldOwned = int.Parse(data[1]);
				}
				else if (data[0] == "shopRefreshPrice")
				{
					Item.shopRefreshPrice= int.Parse(data[1]);
				}
				else if (data[0] == "shopItemCount")
				{
					Item.shopItemCount= int.Parse(data[1]);
				}
				else if (data[0] == "item")
				{
					Item.Items[data[1]].Buyable = bool.Parse(data[2]);
					Item.Items[data[1]].MaxLevel = int.Parse(data[3]);
					Item.Items[data[1]].Price = int.Parse(data[4]);
					Item.Items[data[1]].Level = int.Parse(data[5]);
					Item.Items[data[1]].OwnedCount = int.Parse(data[6]);
					Item.Items[data[1]].BaseVariable = int.Parse(data[7]);
					Item.Items[data[1]].Buyable = bool.Parse(data[8]);
				}
				else if (data[0]=="jatekos kartya items")
				{
					int ID = -1;
					for (int i = 0; i < Gyujtemeny.Count; i++)
					{
						if (Gyujtemeny[i].Name == data[1])
						{
							ID = i;
							break;
						}
					}
					string[]itemNames= data[2].Split(',');
					if (itemNames.Length == 1 && itemNames[0] == "") continue;
					for (int i = 0; i < itemNames.Length; i++)
					{
						Gyujtemeny[i].Items.Add(Item.Items[itemNames[i]]);
					}
				}
			}
			foreach (var item in Gyujtemeny)
			{
				item.UpdateAllVisual();
				bool found = false;
				foreach (var item2 in Jatekos)
				{
					if (item.Name == item2.Name)
					{
						found=true;
						break;
					}
				}
				if (found) continue;
				item.Clicked += AddToPakli;
                item.RightClicked -= RightClick;
                item.RightClicked += RightClick;
				Cards_Wrap.Children.Add(item.GetVisual());
			}
			foreach (var item in Jatekos)
			{
				item.UpdateAllVisual();
				item.Clicked += RemoveFromPakli;
                item.RightClicked -= RightClick;
                item.RightClicked += RightClick;
                PlayerCards_Wrap.Children.Add(item.GetVisual());
			}
			foreach (var item in AllKazamataDict.Values)
			{
				Button b = new Button();
				b.Click += (s, e) =>
				{

					ShowKazamata(AllKazamataDict[item.Name]);

				};
				b.Content = item.Name;
				b.Margin = new Thickness(10, 0, 0, 0);
				DynamicButtonsPanel.Children.Add(b);
			}
			if (DynamicButtonsPanel.ActualWidth != 0)
			{
				AutoResizeButtons(DynamicButtonsPanel);
			}

			SelectableCounter_Label.Content = $"/ {Math.Ceiling((float)Gyujtemeny.Count / 2f)}";
			SelectedCards_Label.Content = Jatekos.Count;

		}
		public void LoadData(string path)
		{
			Item.ResetItems();
			DynamicButtonsPanel.Children.Clear();
			Gyujtemeny.Clear();
			Jatekos.Clear();
			AllCardsDict.Clear();
			AllLeadersDict.Clear();
			AllKazamataDict.Clear();
			Cards_Wrap.Children.Clear();
			PlayerCards_Wrap.Children.Clear();
			StreamReader sr = new StreamReader(path);
			while (!sr.EndOfStream)
			{
				string? line = sr.ReadLine();
				if (line == null || line == "")
				{
					continue;
				}
				string[] data = line.Split(';');

				if (data[0] == "uj kartya")
				{
					AllCardsDict.Add(data[1], new Card(data[1], int.Parse(data[2]), int.Parse(data[3]), data[4], false));
				}
				else if (data[0] == "uj vezer")
				{
					Card vezer = AllCardsDict[data[2]].GetCopy();
					if (data[3] == "sebzes") vezer.Damage *= 2;
					else if (data[3] == "eletero") vezer.HP *= 2;
					vezer.Vezer = true;
					vezer.Name = data[1];
					vezer.Bonus = data[3];
					vezer.OriginName = data[2];
					vezer.UpdateAllVisual();
					AllLeadersDict.Add(data[1], vezer);
					AllLeadersDict[data[1]].UpdateAllVisual();
				}
				else if (data[0] == "uj kazamata")
				{
					if (data[1] == "egyszeru")
					{
						AllKazamataDict.Add(data[2], new Kazamata(data[2], data[1], data[4], new List<Card>() { AllCardsDict[data[3]] }));
					}
					else if (data[1] == "kis")
					{
						List<Card> defenders = new List<Card>();
						string[] def = data[3].Split(",");
						for (int i = 0; i < def.Length; i++)
						{
							defenders.Add(AllCardsDict[def[i]]);
						}
						defenders.Add(AllLeadersDict[data[4]]);
						AllKazamataDict.Add(data[2], new Kazamata(data[2], data[1], data[5], defenders));
					}
					else if (data[1] == "nagy")
					{
						List<Card> defenders = new List<Card>();
						string[] def = data[3].Split(",");
						for (int i = 0; i < def.Length; i++)
						{
							defenders.Add(AllCardsDict[def[i]]);
						}
						defenders.Add(AllLeadersDict[data[4]]);
						AllKazamataDict.Add(data[2], new Kazamata(data[2], data[1], "newcard", defenders));
					}


				}
				else if (data[0] == "felvetel gyujtemenybe")
				{
					if (AllCardsDict.ContainsKey(data[1]))
					{
						Gyujtemeny.Add(AllCardsDict[data[1]].GetCopy());
					}
					else if (AllLeadersDict.ContainsKey(data[1]))
					{
						Gyujtemeny.Add(AllLeadersDict[data[1]].GetCopy());
					}
				}
				else if (data[0] == "uj pakli")
				{
					string[] kartyanevek = data[1].Split(',');
					for (int i = 0; i < kartyanevek.Length; i++)
					{
						if (AllCardsDict.ContainsKey(kartyanevek[i]))
						{
							Gyujtemeny.Add(AllCardsDict[kartyanevek[i]]);
						}
						else if (AllLeadersDict.ContainsKey(kartyanevek[i]))
						{
							Gyujtemeny.Add(AllCardsDict[kartyanevek[i]]);
						}
					}
				}
			}
			foreach (var item in Gyujtemeny)
			{
				item.Clicked += AddToPakli;
                item.RightClicked -= RightClick;
                item.RightClicked += RightClick;
                Cards_Wrap.Children.Add(item.GetVisual());
			}

			foreach (var item in AllKazamataDict.Values)
			{
				Button b = new Button();
				b.Click += (s, e) =>
				{

					ShowKazamata(AllKazamataDict[item.Name]);

				};
				b.Content = item.Name;
				b.Margin = new Thickness(10, 0, 0, 0);
				DynamicButtonsPanel.Children.Add(b);
			}
			if (DynamicButtonsPanel.ActualWidth != 0)
			{
				AutoResizeButtons(DynamicButtonsPanel);
			}

			SelectableCounter_Label.Content = $"/ {Math.Ceiling((float)Gyujtemeny.Count / 2f)}";
			SelectedCards_Label.Content = Jatekos.Count;
		}

		private void ShowKazamata(Kazamata k)
		{
			Jutalom = new Label()
			{
				Content = k.reward == KazamataReward.eletero ? "Jutalom:+2❤" : k.reward == KazamataReward.sebzes ? "Jutalom:+1⚔" : "Jutalom:",
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
				sp.Open(new Uri("Sounds/Nehez.wav", UriKind.Relative));
				sp.Play();
			}
			else
			{
				sp.Open(new Uri("Sounds/Kozepes.wav", UriKind.Relative));
				sp.Play();
			}

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


		private void AddToPakli(object? sender, Card clicked)
		{
			if (Jatekos.Count >= Math.Ceiling((float)Gyujtemeny.Count / 2f) || Jatekos.Contains(clicked))
			{
				se.Open(new Uri("Sounds/Decline.wav", UriKind.Relative));
				se.Play();
				int flashCount = 0;
				bool isRed = false;
				DispatcherTimer timer = new DispatcherTimer();
				timer.Interval = TimeSpan.FromMilliseconds(100);

				timer.Tick += (s, e) =>
				{
					if (isRed)
					{
						SelectedCards_Label.Foreground = Brushes.Gold;
						SelectableCounter_Label.Foreground = Brushes.Gold;
					}
					else
					{
						SelectedCards_Label.Foreground = Brushes.Red;
						SelectableCounter_Label.Foreground = Brushes.Red;
					}

					isRed = !isRed;
					flashCount++;

					if (flashCount >= 4)
						timer.Stop();
				};

				timer.Start();


			}
			else
			{
				Cards_Wrap.Children.Remove(clicked.GetVisual());
				Jatekos.Add(clicked);
				PlayerCards_Wrap.Children.Add(clicked.GetVisual());
				clicked.Clicked -= AddToPakli;
				clicked.Clicked += RemoveFromPakli;
                clicked.RightClicked -= RightClick;
                clicked.RightClicked += RightClick;

                SelectedCards_Label.Content = Jatekos.Count;

				se.Open(new Uri("Sounds/KartyaClick.wav", UriKind.Relative));
				se.Play();
			}
		}

		private void RemoveFromPakli(object? sender, Card clicked)
		{
			PlayerCards_Wrap.Children.Remove(clicked.GetVisual());
			Jatekos.Remove(clicked);
			Cards_Wrap.Children.Add(clicked.GetVisual());
			clicked.Clicked -= RemoveFromPakli;
			clicked.Clicked += AddToPakli;

			SelectedCards_Label.Content = Jatekos.Count;
		}




		static private string Alsovonas(string soveg)
		{
			string o = "";
			for (int i = 0; i < soveg.Length; i++)
			{
				if (soveg[i] == '_')
				{
					o += " ";
				}
				else
				{
					o += soveg[i];
				}
			}
			return o;
		}


		static public float Multiplier(Card attack, Card def)
		{
			if (attack.Tipus == def.Tipus)
			{
				return 1;
			}
			/*else if (attack.Tipus==)
            {

            }*/
			return 0;
		}


		private void ConfirmPakli_Button_Click(object sender, RoutedEventArgs e)
		{
			if (Jatekos.Count != 0 && Jatekos.Count <= Math.Ceiling((float)Gyujtemeny.Count / 2f))
			{
				//implement hiba
				GoToGrid(MainRoom_Grid);
				DisableNagyKazamata();
				se.Open(new Uri("Sounds/Comfirm.wav", UriKind.Relative));
				se.Play();
				ShowPakli();
			}

			else
			{
				se.Open(new Uri("Sounds/Decline.wav", UriKind.Relative));
				se.Play();
				int flashCount = 0;
				bool isRed = false;
				DispatcherTimer timer = new DispatcherTimer();
				timer.Interval = TimeSpan.FromMilliseconds(100);

				timer.Tick += (s, e) =>
				{
					if (isRed)
					{
						SelectedCards_Label.Foreground = Brushes.Gold;
						SelectableCounter_Label.Foreground = Brushes.Gold;
					}
					else
					{
						SelectedCards_Label.Foreground = Brushes.Red;
						SelectableCounter_Label.Foreground = Brushes.Red;
					}

					isRed = !isRed;
					flashCount++;

					if (flashCount >= 4)
						timer.Stop();
				};

				timer.Start();
			}

		}

		private void ShowPakli()
		{
			PakliCards_Wrap.Children.Clear();
			foreach (var item in Gyujtemeny)
			{

				item.Clicked -= RemoveFromPakli;
				item.Clicked -= AddToPakli;

			}

			foreach (var item in Jatekos)
			{
				item.Clicked -= RemoveFromPakli;
				item.Clicked -= AddToPakli;
				PlayerCards_Wrap.Children.Remove(item.GetVisual());
				PakliCards_Wrap.Children.Add(item.GetVisual());
			}

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

		private void PakliChange_Button_Click(object sender, RoutedEventArgs e)
		{
			//GoToGrid(PakliOssze_Grid);
			Back(sender, e);
			foreach (var item in Jatekos)
			{
				PakliCards_Wrap.Children.Remove(item.GetVisual());
				PlayerCards_Wrap.Children.Add(item.GetVisual());
				item.Clicked += RemoveFromPakli;
				item.Clicked -= AddToPakli;
			}

			Cards_Wrap.Children.Clear();

			foreach (var item in Gyujtemeny)
			{
				if (!Jatekos.Contains(item))
				{
					Cards_Wrap.Children.Add(item.GetVisual());
					item.Clicked += AddToPakli;
					item.Clicked -= RemoveFromPakli;
				}

			}

			SelectableCounter_Label.Content = $"/ {Math.Ceiling((float)Gyujtemeny.Count / 2f)}";


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

		private void Exit_Button_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void GoToGame_Button_Click(object sender, RoutedEventArgs e)
		{
			//Menu_Grid.Visibility = Visibility.Collapsed;
			//ChooseKornyezet_Grid.Visibility = Visibility.Visible;
			GoToGrid(ChooseKornyezet_Grid);
		}

		private void GoToOptions_Button_Click(object sender, RoutedEventArgs e)
		{

			GoToGrid(Options_Grid);
		}

		private void BackToMenu_Click(object sender, RoutedEventArgs e)
		{
			/*Menu_Grid.Visibility = Visibility.Visible;
            Options_Grid.Visibility = Visibility.Collapsed;
            ChooseKornyezet_Grid.Visibility = Visibility.Collapsed;
            JatekMester_Grid.Visibility = Visibility.Collapsed;*/
		}

		private void SFX_On(object sender, RoutedEventArgs e)
		{
			se.Volume = seVolume;
			se.IsMuted = false;
		}

		private void SFX_Off(object sender, RoutedEventArgs e)
		{
			se.IsMuted = true;
		}

		private void MUSIC_On(object sender, RoutedEventArgs e)
		{
			sp.Volume = spVolume;
			sp.IsMuted = false;
		}

		private void MUSIC_Off(object sender, RoutedEventArgs e)
		{
			sp.IsMuted = true;
		}

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{

			spVolume = (float)Sl.Value * spMult;
			sp.Volume = spVolume;


			seVolume = (float)Sl.Value * seMult;

			se.Volume = seVolume;



		}

		private void GoToMaster_Button_Click(object sender, RoutedEventArgs e)
		{

			GoToGrid(JatekMester_Grid);
		}

		private void DeleteKornyezet_Button_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ModifyKornyezet_Button_Click(object sender, RoutedEventArgs e)
		{

		}

		private void AddKornyezet_Button_Click(object sender, RoutedEventArgs e)
		{

			GoToGrid(KornyezetSzerkeszto_Grid);
		}

		private void PlayInKornyezet_Click(object sender, RoutedEventArgs e)
		{
			LoadData($"kornyezet/{KornyezetekJatekos_List.SelectedItem.ToString()}.txt");

			GoToGrid(PakliOssze_Grid);
		}





		private void KornyezetekMester_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (KornyezetekMester_List.SelectedItem == null)
			{
				return;
			}
			LoadData($"kornyezet/{KornyezetekMester_List.SelectedItem.ToString()}.txt");

			GoToGrid(KornyezetSzerkeszto_Grid);
			KornyezetekMester_List.SelectedItem = null;
		}

		private void BackToJatekMester(object sender, RoutedEventArgs e)
		{
			/*JatekMester_Grid.Visibility = Visibility.Visible;
            KornyezetSzerkeszto_Grid.Visibility = Visibility.Collapsed;
            KartyaSzerkeszto_Grid.Visibility = Visibility.Collapsed;*/
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
		}
		private void DeleteCard_Click(object sender, RoutedEventArgs e)
		{
			string cardName = cardEditName;

			foreach (var item in AllLeadersDict.Values)
			{

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


			AllCardsDict.Remove(cardName);
			AllLeadersDict.Remove(cardName);
			MessageBox.Show("Sikeres törlés!", "", MessageBoxButton.OK, MessageBoxImage.Information);
			Back(sender, e);
		}
		private void BackToKornyezetSzerkeszto_Click(object sender, RoutedEventArgs e)
		{
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
			VezerAlapKartya.ItemsSource = AllCardsDict.Keys;
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
			}
			GoToGrid(KartyaSzerkeszto_Grid);

			internalEdits = false;
			UpdateKartyaSelectionCard(null, null);
		}
		private void UpdateKartyaSelectionCard(object sender, RoutedEventArgs es)
		{
			if (!internalEdits)
			{
				if ((bool)VezerCheck.IsChecked)
				{

					if ((bool)Gyujtemeny_Check.IsChecked)
					{
						Gyujtemeny.Add(AllLeadersDict[cardEditName].GetCopy());
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
					/*try
                    {
                        if (int.Parse(TypeAttack.Text) > 0) AllLeadersDict[cardEditName.ToString()].Damage = int.Parse(TypeAttack.Text);
                        else
                        {
                            MessageBox.Show("Szöveg", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        } //kristof make error message
                    }
                    catch { }
                    try
                    {
                        if (int.Parse(TypeDefense.Text) > 0) AllLeadersDict[cardEditName.ToString()].HP = int.Parse(TypeDefense.Text);
                        else
                        {
                            MessageBox.Show("Szöveg", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }//kristof make error message
                    }
                    catch { }*/
					/*if (SelectType.SelectedIndex == 0) AllLeadersDict[cardEditName.ToString()].Tipus = KartyaTipus.fold;
                    else if (SelectType.SelectedIndex == 1) AllLeadersDict[cardEditName.ToString()].Tipus = KartyaTipus.viz;
                    else if (SelectType.SelectedIndex == 2) AllLeadersDict[cardEditName.ToString()].Tipus = KartyaTipus.levego;
                    else if (SelectType.SelectedIndex == 3) AllLeadersDict[cardEditName.ToString()].Tipus = KartyaTipus.tuz;*/


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

				}
				else
				{
					if ((bool)Gyujtemeny_Check.IsChecked)
					{
						Gyujtemeny.Add(AllCardsDict[cardEditName].GetCopy());
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
				LoadSave($"saves/{fileName}.txt");
				KornyezetekJatekos_List.SelectedItem = null;
				GoToGrid(PakliOssze_Grid);
			}
			else
			{
				Difficulty_Stack.Visibility = Visibility.Visible;
			}
		}

		private void ConfirmDif_Button_Click(object sender, RoutedEventArgs e)
		{
			string fileName = KornyezetekJatekos_List.SelectedItem.ToString().Split('(')[0][..^1];
			LoadData($"kornyezet/{fileName}.txt");

			Difficulty = int.Parse((string)Dif_Label.Content);
			KornyezetekJatekos_List.SelectedItem = null;
			GoToGrid(PakliOssze_Grid);
		}

		private void DifPlus_Button_Click(object sender, RoutedEventArgs e)
		{
			if (int.Parse(Dif_Label.Content.ToString()) == 0)
			{

				DifMinus_Button.IsEnabled = true;
			}
			else if (int.Parse(Dif_Label.Content.ToString()) == 9)
			{
				DifPlus_Button.IsEnabled = false;
			}

			Dif_Label.Content = (int.Parse(Dif_Label.Content.ToString()) + 1).ToString();
		}

		private void DifMinus_Button_Click(object sender, RoutedEventArgs e)
		{
			if (int.Parse(Dif_Label.Content.ToString()) == 1)
			{

				DifMinus_Button.IsEnabled = false;
			}
			else if (int.Parse(Dif_Label.Content.ToString()) == 10)
			{
				DifPlus_Button.IsEnabled = true;
			}

			Dif_Label.Content = (int.Parse(Dif_Label.Content.ToString()) - 1).ToString();
		}

		private void Back(object sender, RoutedEventArgs e)
		{
			if (elozoGrid.Count == 0)
				return;

			/*if (elozoGrid.Peek().Name== "Shop_Grid")
            {
                CardMerge_Wrap.Children.Clear();
                Shop_Merging_Cards.Children.Clear();
            }*/

			Grid vissza = elozoGrid.Pop();

			// minden grid elrejtése
			foreach (var g in idk.Children.OfType<Grid>())
				g.Visibility = Visibility.Collapsed;

			vissza.Visibility = Visibility.Visible;
			

			// --- SPECIÁLIS LOGIKA ---

			// 1) Menü → zenét visszakapcsol


			// 2) Visszatérés a harcból → harc elemeinek törlése
			if (vissza == MainRoom_Grid || vissza == PakliOssze_Grid)
			{
				DisableNagyKazamata();

			}

			if (vissza != ChooseKornyezet_Grid)
			{
				Difficulty_Stack.Visibility = Visibility.Collapsed;
				KornyezetekJatekos_List.SelectedItem = null;
			}
			// 3) Ha a HARCBÓL lépünk vissza
			/*if (vissza != FightGrid)
            {
                ();   // harc UI eltakarítása
            }*/
		}


		public void GoToGrid(Grid kovetkezo)
		{
			// aktuális grid
			Grid akt = idk.Children
						   .OfType<Grid>()
						   .FirstOrDefault(g => g.Visibility == Visibility.Visible);

			if (akt != null)
				elozoGrid.Push(akt);  // mentés

			// minden grid elrejtése
			foreach (var g in idk.Children.OfType<Grid>())
				g.Visibility = Visibility.Collapsed;

			// új grid megjelenítése
			kovetkezo.Visibility = Visibility.Visible;

			// --- SPECIÁLIS LOGIKA ---

			if (kovetkezo == Menu_Grid)
			{
				sp.Stop();
				sp.Open(new Uri("Sounds/Menu.wav", UriKind.Relative));
				sp.Play();
			}



			if (kovetkezo == MainRoom_Grid)
			{
				DisableNagyKazamata();

			}
		}

		private void GoToShop_Button_Click(object sender, RoutedEventArgs e)
		{
			All_Vezer_Obtained.Visibility = Visibility.Collapsed;
			CardMerge_Wrap.Children.Clear();
			Shop_Merging_Cards.Children.Clear();
			GoToGrid(Shop_Grid);
			int vezCount = 0;
			foreach (Card item in Gyujtemeny)
			{

				if (item.Vezer)
				{
					vezCount++;
				}
			}
			if (vezCount == AllLeadersDict.Count)
			{
				Shop_Merge.IsEnabled = false;
				All_Vezer_Obtained.Visibility = Visibility.Visible;
				Obtained_Label.Visibility = Visibility.Collapsed;
			}
			else
			{
				foreach (var item in Gyujtemeny)
				{
					var card = item.GetCopy();
					if (card.Vezer || Jatekos.Contains(item as Card))
					{

						card.Disabled = true;
						card.UpdateAllVisual();
						CardMerge_Wrap.Children.Add(card.GetVisual());
					}
					else
					{
						card.Clicked += CardToMerge_Card_Click;
						CardMerge_Wrap.Children.Add(card.GetVisual());
					}
				}
			}
			bool isShopEmpty = true;
			foreach (var item in Item.Items.Values)
			{
				if (item.InRotation)
				{
					isShopEmpty = false;
					break;
				}
			}
			if (isShopEmpty) Item.RefreshShop(true);

			ShopRerollPrice_Label.Content = $"Ár: {Item.shopRefreshPrice}";
			if (Item.shopRefreshPrice > Item.GoldOwned)
			{
				ShopRerollPrice_Label.Foreground = Brushes.Red;
				Shop_Refresh.IsEnabled = false;
			}
			else
			{
				ShopRerollPrice_Label.Foreground = Brushes.LightGreen;
			}
			UpdateGoldOwnedLabel();
			UpdateShopWrapChildren();
		}
		public void UpdateShopWrapChildren()
		{
			Shop_Wrap.Children.Clear();
			bool isAnythingBuyable = false;
			foreach (var item in Item.Items.Values)
			{
				if (item.InRotation)
				{
					item.Clicked -= BuyItem;
					item.Clicked += BuyItem;
					if (item.Price > Item.GoldOwned)
					{
						item.Disabled = true;
					}
					item.UpdateAllVisual();
					Shop_Wrap.Children.Add(item.GetVisual(true));
					isAnythingBuyable = true;
				}
			}
			if (isAnythingBuyable) AllItemsMaxedError_Label.Visibility = Visibility.Collapsed;
			else AllItemsMaxedError_Label.Visibility = Visibility.Visible;
		}
		public void UpdateGoldOwnedLabel()
		{
			GoldOwned_Label.Content = $"Arany: {Item.GoldOwned}";
		}

		public void BuyItem(object? sender, Item selected)
		{
			//kristof do anymation here
			selected.Buy();
			UpdateShopWrapChildren();
			UpdateGoldOwnedLabel();
		}
		private void RefreshShop_Button_Click(object sender, RoutedEventArgs e)
		{
			Shop_Wrap.Children.Clear();
			Item.RefreshShop(false);
			foreach (var item in Item.Items.Values)
			{
				if (item.InRotation)
				{
					Shop_Wrap.Children.Add(item.GetVisual(true));
				}
			}

			UpdateGoldOwnedLabel();
			UpdateShopWrapChildren();
		}
		private void CardToMerge_Card_Click(object? sender, Card clicked)
		{
			//clicked.GetVisual().IsEnabled = false;
			for (int i = 0; i < Jatekos.Count; i++)
			{
				if (Jatekos[i].Name == clicked.Name)
				{
					//Implement error mnessage
					return;
				}
			}
			if (Shop_Merging_Cards.Children.Count < 3)
			{
				CardMerge_Wrap.Children.Remove(clicked.GetVisual());
				clicked.Clicked -= CardToMerge_Card_Click;
				clicked.Clicked += CardToMergeRemove_Card_Click;
				Shop_Merging_Cards.Children.Add(clicked.GetVisual());
				Merging.Add(clicked);

			}
			if (Shop_Merging_Cards.Children.Count == 3)
			{
				Shop_Merge.IsEnabled = true;
			}
			//Button b = sender as Button;

		}
		private void CardToMergeRemove_Card_Click(object? sender, Card clicked)
		{

			Shop_Merging_Cards.Children.Remove(clicked.GetVisual());
			CardMerge_Wrap.Children.Add(clicked.GetVisual());
			clicked.Clicked -= CardToMergeRemove_Card_Click;
			clicked.Clicked += CardToMerge_Card_Click;
			Merging.Remove(clicked);


			Shop_Merge.IsEnabled = false;

			//Button b = sender as Button;

		}

		private void Merge_To_Vezer(object sender, RoutedEventArgs e)
		{
			MergeToVezet.haromToVezer(Merging[0], Merging[1], Merging[2]);
			Shop_Merge.IsEnabled = false;
			Gyujtemeny[Gyujtemeny.Count - 1].Clicked += AddToPakli;
			Shop_Merging_Cards.Children.Clear();
			Cards_Wrap.Children.Clear();
			//DynamicButtonsPanel.Children.Clear();
			PlayerCards_Wrap.Children.Clear();
			PakliCards_Wrap.Children.Clear();
			foreach (Card item in Gyujtemeny)
			{
				if (!Jatekos.Contains(item))
				{
					Cards_Wrap.Children.Add(item.GetVisual());
					/*item.Clicked -= RemoveFromPakli;
                    item.Clicked += AddToPakli;*/
				}
				else
				{
					PlayerCards_Wrap.Children.Add(item.GetVisual());
					/*item.Clicked += RemoveFromPakli;
                    item.Clicked -= AddToPakli;*/
				}

			}
			//Gyujtemeny[Gyujtemeny.Count - 1].Clicked += AddToPakli;
			Card card = Gyujtemeny[Gyujtemeny.Count - 1].GetCopy();
			card.Disabled = true;
			card.UpdateAllVisual();
			CardMerge_Wrap.Children.Add(card.GetVisual());

			SelectableCounter_Label.Content = $"/ {Math.Ceiling((float)Gyujtemeny.Count / 2f)}";
			int vezCount = 0;
			foreach (Card item in Gyujtemeny)
			{

				if (item.Vezer)
				{
					vezCount++;
				}
			}
			if (vezCount == AllLeadersDict.Count)
			{
				Shop_Merge.IsEnabled = false;
				Obtained_Label.Visibility = Visibility.Collapsed;
				All_Vezer_Obtained.Visibility = Visibility.Visible;
				CardMerge_Wrap.Children.Clear();
			}

			/*for (int i = 0; i < Gyujtemeny.Count; i++)
            {
                if (Gyujtemeny[i].Name == Merging[0].Name)
                {
                    PlayerCards_Wrap.Children.Remove(Gyujtemeny[i].GetVisual());
                    Cards_Wrap.Children.Remove(Gyujtemeny[i].GetVisual());
                    Gyujtemeny.RemoveAt(i);
                    i--;
                }
                else if (Gyujtemeny[i].Name == Merging[1].Name)
                {
                    PlayerCards_Wrap.Children.Remove(Gyujtemeny[i].GetVisual());
                    Cards_Wrap.Children.Remove(Gyujtemeny[i].GetVisual());
                    Gyujtemeny.RemoveAt(i);
                    i--;

                }
                else if (Gyujtemeny[i].Name == Merging[2].Name)
                {
                    PlayerCards_Wrap.Children.Remove(Gyujtemeny[i].GetVisual());
                    Cards_Wrap.Children.Remove(Gyujtemeny[i].GetVisual());
                    Gyujtemeny.RemoveAt(i);
                    i--;

                }
            }*/
			/*Gyujtemeny.Remove(Merging[0]); 
            Gyujtemeny.Remove(Merging[1]); 
            Gyujtemeny.Remove(Merging[2]);
            for (int i = 0; i < Jatekos.Count; i++)
            {
                if (Jatekos[i] == Merging[0] )
                {
                    PlayerCards_Wrap.Children.Remove(Merging[0].GetVisual());
                    Jatekos.Remove(Merging[0]);
                   
                }
                else if (Jatekos[i] == Merging[1])
                {
                    PlayerCards_Wrap.Children.Remove(Merging[1].GetVisual());
                    Jatekos.Remove(Merging[0]);

                }
                else if (Jatekos[i] == Merging[2])
                {
                    PlayerCards_Wrap.Children.Remove(Merging[2].GetVisual());
                    Jatekos.Remove(Merging[0]);

                }
            }*/
			Merging.Clear();
		}



		private void KartyaSzerkeszto_TextChange(object sender, RoutedEventArgs e)
		{
			if (AllCardsDict.ContainsKey(cardEditName))
			{
				string edits = KartyaSzerkesztoCardName.Text;
				if (!AllCardsDict.ContainsKey(edits) && (bool)!VezerCheck.IsChecked)
				{
					AllCardsDict.Add(edits, AllCardsDict[cardEditName]);
					AllCardsDict.Remove(cardEditName);
					AllCardsDict[edits].Name = edits;
					AllCardsDict[edits].UpdateAllVisual();
					cardEditName = edits;
					SelectedCard_Wrap.Children.Clear();
					SelectedCard_Wrap.Children.Add(AllCardsDict[cardEditName.ToString()].GetCopy().GetVisual());
				}
			}
			else
			{
				string edits = VezerNev.Text + " " + VezerAlapKartya.SelectedItem.ToString();
				if (!AllLeadersDict.ContainsKey(edits) && (bool)!VezerCheck.IsChecked)
				{
					AllLeadersDict.Add(edits, AllLeadersDict[cardEditName]);
					AllLeadersDict.Remove(cardEditName);
					AllLeadersDict[edits].Name = edits;
					AllLeadersDict[edits].UpdateAllVisual();
					cardEditName = edits;
					SelectedCard_Wrap.Children.Clear();
					SelectedCard_Wrap.Children.Add(AllLeadersDict[cardEditName.ToString()].GetCopy().GetVisual());
				}
			}


			//AllCardsDict[cardEditName].Name = KartyaSzerkesztoCardName.Text;
		}

		private void KartyaSzerkeszto_TextChange(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (AllCardsDict.ContainsKey(cardEditName))
			{
				if (e.Key == System.Windows.Input.Key.Enter)
				{
					string edits = KartyaSzerkesztoCardName.Text;
					if (!AllCardsDict.ContainsKey(edits))
					{
						AllCardsDict.Add(edits, AllCardsDict[cardEditName]);
						AllCardsDict.Remove(cardEditName);
						AllCardsDict[edits].Name = edits;
						AllCardsDict[edits].UpdateAllVisual();
						cardEditName = edits;
						SelectedCard_Wrap.Children.Clear();
						SelectedCard_Wrap.Children.Add(AllCardsDict[cardEditName.ToString()].GetCopy().GetVisual());
					}
				}
			}
			else
			{
				if (e.Key == System.Windows.Input.Key.Enter)
				{
					string edits = VezerNev.Text + " " + VezerAlapKartya.SelectedItem.ToString();
					if (!AllLeadersDict.ContainsKey(edits))
					{
						AllLeadersDict.Add(edits, AllLeadersDict[cardEditName]);
						AllLeadersDict.Remove(cardEditName);
						AllLeadersDict[edits].Name = edits;
						AllLeadersDict[edits].UpdateAllVisual();
						cardEditName = edits;
						SelectedCard_Wrap.Children.Clear();
						SelectedCard_Wrap.Children.Add(AllLeadersDict[cardEditName.ToString()].GetCopy().GetVisual());
					}
				}
			}

		}

		private void CreateNewCard_Button_Click(object sender, RoutedEventArgs e)
		{
			internalEdits = true;
			VezerCheck.IsChecked = false;
			internalEdits = false;
			GoToGrid(KartyaSzerkeszto_Grid);
			string cardName = "ÚjKártya";
			bool goodName = false;
			int bonusIndex = -1;
			while (!goodName)
			{
				bonusIndex++;
				goodName = true;
				foreach (Card item in AllCardsDict.Values)
				{
					if (bonusIndex <= 0)
					{
						if (item.Name == cardName)
						{
							//bonusIndex++;
							goodName = false;
						}
					}
					else if (item.Name == cardName + bonusIndex)
					{
						//bonusIndex++;
						goodName = false;
					}
				}
			}
			if (bonusIndex > 0)
			{
				cardName += bonusIndex;
			}
			Card k = new Card(cardName, 1, 1, "fold", false);
			internalEdits = true;
			cardEditName = k.Name;
			Gyujtemeny_Check.IsChecked = false;
			foreach (Card item in Gyujtemeny)
			{
				if (item.Name == k.Name)
				{
					Gyujtemeny_Check.IsChecked = true;
					break;
				}
			}
			//GoToGrid(KartyaSzerkeszto_Grid);
			KartyaSzerkesztoCardName.Text = k.Name;
			SelectType.ItemsSource = new string[] { "Föld", "Víz", "Levegő", "Tűz" };
			TypeAttack.Text = k.Damage.ToString();
			TypeDefense.Text = k.HP.ToString();
			if (k.Tipus == KartyaTipus.fold) SelectType.SelectedIndex = 0;
			else if (k.Tipus == KartyaTipus.viz) SelectType.SelectedIndex = 1;
			else if (k.Tipus == KartyaTipus.levego) SelectType.SelectedIndex = 2;
			else if (k.Tipus == KartyaTipus.tuz) SelectType.SelectedIndex = 3;
			AllCardsDict.Add(cardName, k);
			internalEdits = false;
			UpdateKartyaSelectionCard(null, null);

		}

		private void VezerCheck_Checked(object sender, RoutedEventArgs e)
		{
			if (internalEdits)
			{

			}
			else
			{
				internalEdits = true;
				AllCardsDict.Remove(cardEditName);
				SelectedCard_Wrap.Children.Clear();
				BasicCardPanel.Visibility = Visibility.Collapsed;
				LeaderCardPanel.Visibility = Visibility.Visible;
				VezerAlapKartya.ItemsSource = AllCardsDict.Keys;
				VezerAlapKartya.SelectedIndex = 0;
				VezerBonusTipus.ItemsSource = new string[] { "Életerő", "Sebzés" };
				VezerBonusTipus.SelectedIndex = 0;
				Card k = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].GetCopy();
				k.Vezer = true;
				k.OriginName = VezerAlapKartya.SelectedItem.ToString();
				VezerNev.Text = "Vezér";
				cardEditName = VezerNev.Text + " " + VezerAlapKartya.SelectedItem.ToString();
				AllLeadersDict.Add(cardEditName, k);
				AllLeadersDict[cardEditName].Name = cardEditName;
				SelectedCard_Wrap.Children.Add(k.GetVisual());
				if (VezerBonusTipus.SelectedIndex == 0)
				{
					AllLeadersDict[cardEditName].HP *= 2;

				}
				else
				{
					AllLeadersDict[cardEditName].Damage *= 2;

				}
				AllLeadersDict[cardEditName].UpdateAllVisual();
				SelectedCard_Wrap.Children.Clear();
				SelectedCard_Wrap.Children.Add(AllLeadersDict[cardEditName].GetVisual());
				internalEdits = false;
			}


		}

		private void VezerCheck_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!internalEdits)
			{
			SelectedCard_Wrap.Children.Clear();
			AllLeadersDict.Remove(cardEditName);
			BasicCardPanel.Visibility = Visibility.Visible;
			LeaderCardPanel.Visibility = Visibility.Collapsed;
			string cardName = "ÚjKártya";
			bool goodName = false;
			int bonusIndex = -1;
			while (!goodName)
			{
				bonusIndex++;
				goodName = true;
				foreach (Card item in AllCardsDict.Values)
				{
					if (bonusIndex <= 0)
					{
						if (item.Name == cardName)
						{
							//bonusIndex++;
							goodName = false;
						}
					}
					else if (item.Name == cardName + bonusIndex)
					{
						//bonusIndex++;
						goodName = false;
					}
				}
			}
			if (bonusIndex > 0)
			{
				cardName += bonusIndex;
			}
			Card k = new Card(cardName, 1, 1, "fold", false);
			internalEdits = true;
			cardEditName = k.Name;
			Gyujtemeny_Check.IsChecked = false;
			foreach (Card item in Gyujtemeny)
			{
				if (item.Name == k.Name)
				{
					Gyujtemeny_Check.IsChecked = true;
					break;
				}
			}
			//GoToGrid(KartyaSzerkeszto_Grid);
			KartyaSzerkesztoCardName.Text = k.Name;
			SelectType.ItemsSource = new string[] { "Föld", "Víz", "Levegő", "Tűz" };
			TypeAttack.Text = k.Damage.ToString();
			TypeDefense.Text = k.HP.ToString();
			if (k.Tipus == KartyaTipus.fold) SelectType.SelectedIndex = 0;
			else if (k.Tipus == KartyaTipus.viz) SelectType.SelectedIndex = 1;
			else if (k.Tipus == KartyaTipus.levego) SelectType.SelectedIndex = 2;
			else if (k.Tipus == KartyaTipus.tuz) SelectType.SelectedIndex = 3;
			AllCardsDict.Add(cardName, k);
			internalEdits = false;
			UpdateKartyaSelectionCard(null, null);
			}
			
		}

		private void VezerAlapKartya_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!internalEdits)
			{
				string edits = VezerNev.Text + " " + VezerAlapKartya.SelectedItem.ToString();
				AllLeadersDict.Remove(cardEditName);
				SelectedCard_Wrap.Children.Clear();
				Card c = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].GetCopy();
				c.Vezer = true;
				c.Name = edits;
				c.OriginName = VezerAlapKartya.SelectedItem.ToString();
				if (VezerBonusTipus.SelectedIndex == 1)
				{
					c.Damage *= 2;
				}
				else
				{
					c.HP *= 2;
				}
				c.UpdateAllVisual();
				AllLeadersDict.Add(edits, c);
				SelectedCard_Wrap.Children.Add(c.GetVisual());
				cardEditName = edits;
			}



		}

		private void Kazamata_Button_Click(object sender, RoutedEventArgs e)
		{
			CreateNewCard_Button.Visibility = Visibility.Collapsed;
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
					Card c = AllCardsDict[item].GetCopy();
					c.Clicked += RemoveFromKazamata;
					KazamataDeffenders_List.Children.Add(c.GetVisual());
				}
				else
				{
					Card c = AllCardsDict[item].GetCopy();
					c.Clicked += AddToKazamata;
					MindenKartya_ListKazamata.Children.Add(c.GetVisual());
				}
			}
			int vezerCount = 0;
			foreach (var item in AllLeadersDict.Keys)
			{
				if (k.GetDefenderNames().Contains(item))
				{
					Card c = AllLeadersDict[item].GetCopy();
					c.Clicked += RemoveFromKazamata;
					KazamataDeffenders_List.Children.Add(c.GetVisual());
					vezerCount++;
				}
				else
				{
					Card c = AllLeadersDict[item].GetCopy();
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
						Card c = AllCardsDict[item].GetCopy();
						c.Clicked -= AddToKazamata;
						c.Clicked += RemoveFromKazamata;
						KazamataDeffenders_List.Children.Add(c.GetVisual());
					}
					else
					{
						Card c = AllCardsDict[item].GetCopy();
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
						Card c = AllLeadersDict[item].GetCopy();
						c.Clicked -= AddToKazamata;
						c.Clicked += RemoveFromKazamata;
						KazamataDeffenders_List.Children.Add(c.GetVisual());
						vezerCount++;
					}
					else
					{
						Card c = AllLeadersDict[item].GetCopy();
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
			clicked.Clicked -= AddToKazamata;
			clicked.Clicked += RemoveFromKazamata;
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
			if (clicked.Vezer && vezer == 0)
			{
				AllKazamataDict[kazamataEditNmae].Defenders.Add(clicked);
				UpdateKazamata();
			}
			else if (kazamataDef - vezer < kazamataMaxDef)
			{
				AllKazamataDict[kazamataEditNmae].Defenders.Add(clicked);
				UpdateKazamata();
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
				else
				{
					AllKazamataDict[kazamataEditNmae].reward = KazamataReward.sebzes;
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






		// <Label Name = "Jutalom" Content="" Height="340" Margin="0,305,0,0" VerticalAlignment="Top" Width="450" FontSize="60" HorizontalAlignment="Center" HorizontalContentAlignment="Center"/>

	}
}