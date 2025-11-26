
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        static WrapPanel FightPlayer_Wrap = new WrapPanel();

        static WrapPanel FightKazamata_Wrap = new WrapPanel();

       static  Label Defend_Label = new Label();

        static Label Attack_Label = new Label();

        static Label DefendDeploy_Label = new Label();

       static  Label AttackDeploy_Label = new Label();

       static  WrapPanel Fight_Wrap = new WrapPanel();

        static WrapPanel FightPlayerAttacker_Wrap = new WrapPanel();

        static WrapPanel FightKazamataAttacker_Wrap = new WrapPanel();

        static Button Harc = new Button();
        static Button Vissza = new Button();

        static Button ChangeSpeed = new Button();

        static Label Speed_Label = new Label();






        public static List<Card> Gyujtemeny = new List<Card>();
        public static List<Card> Jatekos = new List<Card>();
        public static List<Card> AllCards = new List<Card>();
        public static List<Card> AllLeaders=new List<Card>();
        public static Dictionary<string,Card> AllLeadersDict = new Dictionary<string, Card>();
        public static Dictionary<string,Card> AllCardsDict = new Dictionary<string,Card>();
        public static List<Kazamata> AllKazamata = new List<Kazamata>();
        public static Dictionary<string, Kazamata> AllKazamataDict = new Dictionary<string, Kazamata>();
        //public static Kazamata AllKazamata["Barlangi portya"] = new Kazamata("Barlangi portya", "egyszeru", "sebzes", new List<Card>());
        //public static Kazamata AllKazamata["Osi szentely"] = new Kazamata("Osi szentely", "kis", "eletero", new List<Card>());
        //public static Kazamata AllKazamata["A melyseg kiralynoje"] = new Kazamata("A melyseg kiralynoje", "nagy", "", new List<Card>());

        public static MediaPlayer sp = new MediaPlayer();
        public static MediaPlayer se = new MediaPlayer();

        public static Grid FightGrid = new Grid();

        public static float spVolume = 0.3f;
        public MainWindow()
        {
            InitializeComponent();
            string iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  "playing-card.ico");
            this.Icon = new BitmapImage(new Uri(iconPath, UriKind.Absolute));
            Menu_Grid.Visibility = Visibility.Visible;
            Options_Grid.Visibility = Visibility.Collapsed;
            PakliOssze_Grid.Visibility = Visibility.Collapsed;
            MainRoom_Grid.Visibility = Visibility.Collapsed;
            JatekMester_Grid.Visibility = Visibility.Collapsed;
            ChooseKornyezet_Grid.Visibility = Visibility.Collapsed;
            FightGrid.Visibility = Visibility.Collapsed;

            idk.Children.Add(FightGrid);
            sp.Volume = spVolume;
            sp.Open(new Uri("Sounds/Menu.wav", UriKind.Relative));
            sp.MediaEnded += (s, e) =>
            {
                sp.Position = TimeSpan.Zero;
                sp.Play();
            };
            sp.Play();

            KornyezetekJatekos_List.ItemsSource = Directory.GetFiles("kornyezet").Select(x => x.Split('\\')[1].Split('.')[0]);
        }
        public void LoadData(string path)
        {
            Gyujtemeny.Clear();
            Jatekos.Clear();
            AllCards.Clear();
            AllLeaders.Clear();
            AllCardsDict.Clear();
            AllLeadersDict.Clear();
            AllKazamata.Clear();
            AllKazamataDict.Clear();
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
                    AllCards.Add(new Card(data[1], int.Parse(data[2]), int.Parse(data[3]), data[4], false));
                    AllCardsDict.Add(data[1], AllCards[AllCards.Count - 1]);
                }
                else if (data[0] == "uj vezer")
                {
                    Card vezer = AllCardsDict[data[2]].GetCopy();
                    if (data[3] == "sebzes") vezer.Damage *= 2;
                    else if (data[3] == "eletero") vezer.HP *= 2;
                    vezer.Vezer = true;
                    vezer.Name = data[1];
                    AllLeaders.Add(vezer);
                    AllLeadersDict.Add(data[1], AllLeaders[AllLeaders.Count - 1]);
                }
                else if (data[0] == "uj kazamata")
                {
                    if (data[1] == "egyszeru")
                    {
                        AllKazamata.Add(new Kazamata(data[2], data[1], data[4], new List<Card>() { AllCardsDict[data[3]] }));
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
                        AllKazamata.Add(new Kazamata(data[2], data[1], data[5], defenders));
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
                        AllKazamata.Add(new Kazamata(data[2], data[1], "newcard", defenders));
                    }
                    AllKazamataDict.Add(AllKazamata[AllKazamata.Count - 1].Name, AllKazamata[AllKazamata.Count - 1]);


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
                Cards_Wrap.Children.Add(item.GetVisual());
            }

            foreach (var item in AllKazamata)
            {
                Button b = new Button();
                b.Click += (s, e) =>
                {
                    ShowKazamata(AllKazamataDict[item.Name]);
                };
                b.Content = item.Name;
                b.Margin = new Thickness(10,0,0,0);
                DynamicButtonsPanel.Children.Add(b);
            }

            SelectableCounter_Label.Content = $"/ {Math.Ceiling((float)Gyujtemeny.Count / 2f)}";
        }
        
        private void ShowKazamata(Kazamata k)
        {
             FightPlayer_Wrap = new WrapPanel()
            {
                Name = "FightPlayer_Wrap",
                Margin = new Thickness(115, 263, 1343, 37),
                Width = 400
            };

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
            Vissza.Click += Vissza_Button_Click;

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
                Content = "1x",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(1746, 71, 0, 0),
                VerticalAlignment = VerticalAlignment.Top
            };

            FightGrid.Height = 1080;
            FightGrid.Width = 1920;
            FightGrid.Visibility = Visibility.Visible;
            MainRoom_Grid.Visibility = Visibility.Collapsed;


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

            Harc.Click += (s, e) =>
            {
                Harc.Visibility = Visibility.Collapsed;
                Vissza.Visibility = Visibility.Collapsed;
                Harc2.StartFight(FightGrid,Vissza, Gyujtemeny, k, Jatekos, FightPlayer_Wrap, FightKazamata_Wrap, Attack_Label, Defend_Label,AttackDeploy_Label,DefendDeploy_Label,FightPlayerAttacker_Wrap, FightKazamataAttacker_Wrap, 0);
                
            };

        }

        public static void RemoveShitFomrShit()
        {
            FightGrid.Children.Remove(FightPlayer_Wrap);
            FightGrid.Children.Remove(FightKazamata_Wrap);
            FightGrid.Children.Remove(Defend_Label);
            FightGrid.Children.Remove(Attack_Label);
            FightGrid.Children.Remove(DefendDeploy_Label);
            FightGrid.Children.Remove(AttackDeploy_Label);
            FightGrid.Children.Remove(Fight_Wrap);
            FightGrid.Children.Remove(FightPlayerAttacker_Wrap);
            FightGrid.Children.Remove(FightKazamataAttacker_Wrap);
            FightGrid.Children.Remove(Harc);
            FightGrid.Children.Remove(Vissza);
            FightGrid.Children.Remove(ChangeSpeed);
            FightGrid.Children.Remove(Speed_Label);
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
                if (soveg[i]=='_')
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
            if (Jatekos.Count != 0)
            {
                PakliOssze_Grid.Visibility = Visibility.Collapsed;
                MainRoom_Grid.Visibility = Visibility.Visible;
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
            MainRoom_Grid.Visibility = Visibility.Collapsed;
            PakliOssze_Grid.Visibility = Visibility.Visible;

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
            Jutalom.Margin = new Thickness(0,305,0,0);
            Jutalom.VerticalAlignment = VerticalAlignment.Top;
            Jutalom.Width = 450;
            Jutalom.FontSize = 60;
            Jutalom.HorizontalAlignment = HorizontalAlignment.Center;
            Jutalom.HorizontalContentAlignment= HorizontalAlignment.Center;
            return Jutalom;

        }

        

        private  void Vissza_Button_Click(object sender, RoutedEventArgs e)
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
            MainRoom_Grid.Visibility = Visibility.Visible;
            FightGrid.Visibility = Visibility.Collapsed;

            sp.Stop();
            sp.Open(new Uri("Sounds/Menu.wav", UriKind.Relative));
            sp.Play();
            RemoveShitFomrShit();
            ShowPakli();
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
            wrap.Children.Add(kartya.GetVisual());
            return wrap;
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GoToGame_Button_Click(object sender, RoutedEventArgs e)
        {
            Menu_Grid.Visibility = Visibility.Collapsed;
            ChooseKornyezet_Grid.Visibility = Visibility.Visible;
        }

        private void GoToOptions_Button_Click(object sender, RoutedEventArgs e)
        {
            Menu_Grid.Visibility = Visibility.Collapsed;
            PakliOssze_Grid.Visibility = Visibility.Collapsed;
            Options_Grid.Visibility = Visibility.Visible;
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            Menu_Grid.Visibility = Visibility.Visible;
            Options_Grid.Visibility = Visibility.Collapsed;
            ChooseKornyezet_Grid.Visibility = Visibility.Collapsed;
        }

        private void SFX_On(object sender, RoutedEventArgs e)
        {
            se.Volume = 1;
        }

        private void SFX_Off(object sender, RoutedEventArgs e)
        {
            se.Volume = 0;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            spVolume = (float)Sl.Value * 0.006f;
            sp.Volume = spVolume;
        }

        private void GoToMaster_Button_Click(object sender, RoutedEventArgs e)
        {
            Menu_Grid.Visibility = Visibility.Collapsed;
            JatekMester_Grid.Visibility = Visibility.Visible;
        }

        private void DeleteKornyezet_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ModifyKornyezet_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddKornyezet_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlayInKornyezet_Click(object sender, RoutedEventArgs e)
        {
            LoadData($"kornyezet/{KornyezetekJatekos_List.SelectedItem.ToString()}.txt");
            PakliOssze_Grid.Visibility = Visibility.Visible;
            ChooseKornyezet_Grid.Visibility = Visibility.Collapsed;
        }

        private void DynamicButtonsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AutoResizeButtons();
        }

        private void AutoResizeButtons()
        {
            if (DynamicButtonsPanel.Children.Count == 0)
                return;

            double panelWidth = DynamicButtonsPanel.ActualWidth;
            double minButtonWidth = 250; // minimális gombszélesség
            int buttonsPerRow = Math.Max(1, (int)(panelWidth / minButtonWidth));

            double buttonWidth = (panelWidth / buttonsPerRow) - 20; // 20 = margin

            foreach (Button b in DynamicButtonsPanel.Children)
            {
                b.Width = buttonWidth;
                b.Height = buttonWidth * 0.6; // arányos magasság
                b.FontSize = buttonWidth / 15; // automatikus fontsize skálázás
            }
        }

        // <Label Name = "Jutalom" Content="" Height="340" Margin="0,305,0,0" VerticalAlignment="Top" Width="450" FontSize="60" HorizontalAlignment="Center" HorizontalContentAlignment="Center"/>

    }
}