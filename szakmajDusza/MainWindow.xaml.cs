using System.Data;
using System.Globalization;
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
        static public bool internalEdits = false;


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


        public static float spVolume = 0.2f;
        public static float spMult = 0.0f;
        public MainWindow()
        {
            InitializeComponent();
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

            elozoGrid.Push(Menu_Grid);

            idk.Children.Add(FightGrid);
            
            sp.Open(new Uri("Sounds/Menu.wav", UriKind.Relative));
            
            sp.MediaEnded += (s, e) =>
            {
                sp.Position = TimeSpan.Zero;
                sp.Play();
            };
            sp.Play();
            sp.Volume = spVolume;
            KornyezetekJatekos_List.ItemsSource = Directory.GetFiles("kornyezet").Select(x => x.Split('\\')[1].Split('.')[0]);
            KornyezetekMester_List.ItemsSource = Directory.GetFiles("kornyezet").Select(x => x.Split('\\')[1].Split('.')[0]);
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
        public void LoadData(string path)
        {
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
                    AllLeadersDict.Add(data[1], vezer);
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

            SelectableCounter_Label.Content = $"/ {Math.Ceiling((float)Gyujtemeny.Count / 2f)}";
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
            Vissza.Click += (s, e) => Back(s, e);

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
            FightGrid.Visibility = Visibility.Visible;
            MainRoom_Grid.Visibility = Visibility.Collapsed;
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
                Harc2.StartFight(FightGrid, Vissza, Gyujtemeny, k, Jatekos, FightPlayer_Wrap, FightKazamata_Wrap, Attack_Label, Defend_Label, AttackDeploy_Label, DefendDeploy_Label, FightPlayerAttacker_Wrap, FightKazamataAttacker_Wrap, 0);

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
            if (Jatekos.Count != 0)
            {
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
            GoToGrid(PakliOssze_Grid);

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
            
            GoToGrid(MainRoom_Grid);
            DisableNagyKazamata();
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
            se.Volume = 1;
        }

        private void SFX_Off(object sender, RoutedEventArgs e)
        {
            se.Volume = 0;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            spVolume = (float)Sl.Value *spMult;
            sp.Volume = spVolume;
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
            MindenKartya_List.Children.Clear();
            if (sender!=null)
            {
                MindenKartya_List.Visibility = Visibility.Visible;
            }
            
            foreach (var item in AllCardsDict.Values)
            {
                MindenKartya_List.Children.Add(item.GetVisual());
                if (sender!=null)
                {
                    item.Clicked += (s, e) => { SelectForModify(item); };
                }
                
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
            MessageBox.Show("Sikeres törlés!", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void BackToKornyezetSzerkeszto_Click(object sender, RoutedEventArgs e)
        {
            ListKartya_Button_Click(null, e);
            Back(sender, e);
            /*KartyaSzerkeszto_Grid.Visibility = Visibility.Collapsed;
            KornyezetSzerkeszto_Grid.Visibility = Visibility.Visible;*/
        }
        private void SelectForModify(Card k)
        {
            internalEdits = true;
            cardEditName = k.Name;
            Gyujtemeny_Check.IsChecked = false;
            foreach (Card item in Gyujtemeny)
            {
                if (item.Name==k.Name)
                {
                    Gyujtemeny_Check.IsChecked = true;
                    break;
                }
            }
            GoToGrid(KartyaSzerkeszto_Grid);
            KartyaSzerkesztoCardName.Text = k.Name;
            SelectType.ItemsSource = new string[] { "Föld", "Víz", "Levegő", "Tűz" };
            TypeAttack.Text = k.Damage.ToString();
            TypeDefense.Text = k.HP.ToString();
            if (k.Tipus == KartyaTipus.fold) SelectType.SelectedIndex = 0;
            else if (k.Tipus == KartyaTipus.viz) SelectType.SelectedIndex = 1;
            else if (k.Tipus == KartyaTipus.levego) SelectType.SelectedIndex = 2;
            else if (k.Tipus == KartyaTipus.tuz) SelectType.SelectedIndex = 3;
            internalEdits = false;
            UpdateKartyaSelectionCard(null, null);
        }
        private void UpdateKartyaSelectionCard(object sender, RoutedEventArgs es)
        {
            if (!internalEdits)
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

        private void KornyezetekJatekos_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KornyezetekJatekos_List.SelectedItem == null)
            {
                return;
            }
            LoadData($"kornyezet/{KornyezetekJatekos_List.SelectedItem.ToString()}.txt");
           

            GoToGrid(PakliOssze_Grid);
           // SelectableCounter_Label.Content = $"/ {Math.Ceiling((float)Gyujtemeny.Count / 2f)}";
            KornyezetekJatekos_List.SelectedItem = null;
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
            if (vissza == Menu_Grid)
            {
                sp.Stop();
                sp.Open(new Uri("Sounds/Menu.wav", UriKind.Relative));
                sp.Play();
            }

            // 2) Visszatérés a harcból → harc elemeinek törlése
            if (vissza == MainRoom_Grid || vissza == PakliOssze_Grid)
            {
                DisableNagyKazamata();
                
            }

            // 3) Ha a HARCBÓL lépünk vissza
            if (vissza != FightGrid)
            {
                RemoveShitFomrShit();   // harc UI eltakarítása
            }
        }


        private void GoToGrid(Grid kovetkezo)
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
            int vezCount=0;
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
               
           
        }
        private void CardToMerge_Card_Click(object? sender, Card clicked)
        {
            //clicked.GetVisual().IsEnabled = false;
            for (int i = 0; i < Jatekos.Count; i++)
            {
                if (Jatekos[i].Name==clicked.Name)
                {
                    //Implement error mnessage
                    return;
                }
            }
            if (Shop_Merging_Cards.Children.Count<3)
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
            string edits = KartyaSzerkesztoCardName.Text;
            if (!AllCardsDict.ContainsKey(edits))
            {
                AllCardsDict.Add(edits, AllCardsDict[cardEditName]);
                AllCardsDict.Remove(cardEditName);
                AllCardsDict[edits].Name=edits;
                AllCardsDict[edits].UpdateAllVisual();
                cardEditName = edits;
                SelectedCard_Wrap.Children.Clear();
                SelectedCard_Wrap.Children.Add(AllCardsDict[cardEditName.ToString()].GetCopy().GetVisual());
            }
            
            //AllCardsDict[cardEditName].Name = KartyaSzerkesztoCardName.Text;
        }

        private void KartyaSzerkeszto_TextChange(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key==System.Windows.Input.Key.Enter)
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

        
        // <Label Name = "Jutalom" Content="" Height="340" Margin="0,305,0,0" VerticalAlignment="Top" Width="450" FontSize="60" HorizontalAlignment="Center" HorizontalContentAlignment="Center"/>

    }
}