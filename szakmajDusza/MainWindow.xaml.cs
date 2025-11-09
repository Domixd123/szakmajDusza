
﻿using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace szakmajDusza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public static List<Card> Gyujtemeny = new List<Card>();
        public static List<Card> Jatekos = new List<Card>();
        public static List<Card> AllCards = new List<Card>();
		public static Dictionary<string,Card> AllCardsDict = new Dictionary<string,Card>();
		public static Kazamata EgyszeruKazamata = new Kazamata("Barlangi portya", "egyszeru", "sebzes", new List<Card>());
        public static Kazamata KisKazamata = new Kazamata("Osi szentely", "kis", "eletero", new List<Card>());
        public static Kazamata NagyKazamata = new Kazamata("A melyseg kiralynoje", "nagy", "", new List<Card>());

        public MainWindow()
        {
            InitializeComponent();

            PakliOssze_Grid.Visibility = Visibility.Visible;
            MainRoom_Grid.Visibility = Visibility.Collapsed;
            EgyszeriKazamata_Grid.Visibility = Visibility.Collapsed;
            KisKazamata_Grid.Visibility = Visibility.Collapsed;
            NagyKazamata_Grid.Visibility = Visibility.Collapsed;

            UploadCards();

        }

        
        private void UploadCards()
        {
            AllCardsDict.Add("Arin", new Card("Arin", 2, 5, "fold", false));
			AllCardsDict.Add("Liora", new Card("Liora", 2, 4, "levego", false));
			AllCardsDict.Add("Nerun", new Card("Nerun", 3, 3, "tuz", false));
			AllCardsDict.Add("Selia", new Card("Selia", 2, 6, "viz", false));
			AllCardsDict.Add("Torak", new Card("Torak", 3, 4, "fold", false));
			AllCardsDict.Add("Emera", new Card("Emera", 2, 5, "levego", false));
			AllCardsDict.Add("Vorn", new Card("Vorn", 2, 7, "viz", false));
			AllCardsDict.Add("Kael", new Card("Kael", 3, 5, "tuz", false));
			AllCardsDict.Add("Myra", new Card("Myra", 2, 6, "fold", false));
			AllCardsDict.Add("Thalen", new Card("Thalen", 3, 5, "levego", false));
			AllCardsDict.Add("Isara", new Card("Isara", 2, 6, "viz", false));

            AllCards.Add(AllCardsDict["Arin"]);
			AllCards.Add(AllCardsDict["Liora"]);
			AllCards.Add(AllCardsDict["Nerun"]);
			AllCards.Add(AllCardsDict["Selia"]);
			AllCards.Add(AllCardsDict["Torak"]);
			AllCards.Add(AllCardsDict["Emera"]);
			AllCards.Add(AllCardsDict["Vorn"]);
			AllCards.Add(AllCardsDict["Kael"]);
			AllCards.Add(AllCardsDict["Myra"]);
			AllCards.Add(AllCardsDict["Thalen"]);
			AllCards.Add(AllCardsDict["Isara"]);

            Gyujtemeny.Add(AllCardsDict["Arin"].GetCopy());
			Gyujtemeny.Add(AllCardsDict["Liora"].GetCopy());
			Gyujtemeny.Add(AllCardsDict["Selia"].GetCopy());
			Gyujtemeny.Add(AllCardsDict["Nerun"].GetCopy());
			Gyujtemeny.Add(AllCardsDict["Torak"].GetCopy());
			Gyujtemeny.Add(AllCardsDict["Emera"].GetCopy());
			Gyujtemeny.Add(AllCardsDict["Kael"].GetCopy());
			Gyujtemeny.Add(AllCardsDict["Myra"].GetCopy());
			Gyujtemeny.Add(AllCardsDict["Thalen"].GetCopy());
			Gyujtemeny.Add(AllCardsDict["Isara"].GetCopy());

            //leader cards, kazamata fix would be great
			EgyszeruKazamata.Defenders.Add(new Card("Nerun", 3, 3, "tuz", false));

            KisKazamata.Defenders.Add(new Card("Arin", 2, 5, "fold", false));
            KisKazamata.Defenders.Add(new Card("Emera", 2, 5, "levego", false));
            KisKazamata.Defenders.Add(new Card("Selia", 2, 6, "viz", false));
            KisKazamata.Defenders.Add(new Card("Lord Torak", 6, 4, "fold", true));

            NagyKazamata.Defenders.Add(new Card("Liora", 2, 4, "levego", false));
            NagyKazamata.Defenders.Add(new Card("Arin", 2, 5, "fold", false));
            NagyKazamata.Defenders.Add(new Card("Selia", 2, 6, "viz", false));
            NagyKazamata.Defenders.Add(new Card("Nerun", 3, 3, "tuz", false));
            NagyKazamata.Defenders.Add(new Card("Torak", 3, 4, "fold", false));
            NagyKazamata.Defenders.Add(new Card("Priestess Selia", 2, 12, "fold", true));


            foreach (var item in Gyujtemeny)
            {
                item.Clicked += AddToPakli;
                Cards_Wrap.Children.Add(item.GetVisual());
            }


        }

        private void AddToPakli(object? sender, Card clicked)
        {
            if (Jatekos.Count >= Gyujtemeny.Count / 2 || Jatekos.Contains(clicked))
            {
                MessageBox.Show("Nem lehet hozzáadni ezt a kártyát");
            }
            else
            {
                Cards_Wrap.Children.Remove(clicked.GetVisual());
                Jatekos.Add(clicked);
                PlayerCards_Wrap.Children.Add(clicked.GetVisual());
                clicked.Clicked -= AddToPakli;
                clicked.Clicked += RemoveFromPakli;

                SelectedCards_Label.Content = Jatekos.Count;
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

        private async void Egyszeri_Button_Click(object sender, RoutedEventArgs e)
        {
            MainRoom_Grid.Visibility = Visibility.Collapsed;
            EgyszeriKazamata_Grid.Visibility = Visibility.Visible;
            await Harc2.StartFight(Gyujtemeny, EgyszeruKazamata, Jatekos, FightPlayerEgyszeri_Wrap, FightKazamataEgyszeri_Wrap, AttackEgyszeri_Label, DefendEgyszeri_Label, FightPlayerAttackerEgyszeri_Wrap, FightKazamataAttackerEgyszeri_Wrap);
            MainRoom_Grid.Visibility = Visibility.Visible;
            EgyszeriKazamata_Grid.Visibility = Visibility.Collapsed;
            ShowPakli();
        }

        private async void Kis_Button_Click(object sender, RoutedEventArgs e)
        {
            MainRoom_Grid.Visibility = Visibility.Collapsed;
            KisKazamata_Grid.Visibility = Visibility.Visible;
            await Harc2.StartFight(Gyujtemeny, KisKazamata, Jatekos, FightPlayerKis_Wrap, FightKazamataKis_Wrap, AttackKis_Label, DefendKis_Label,FightPlayerAttackerKis_Wrap, FightKazamataAttackerKis_Wrap);
            MainRoom_Grid.Visibility = Visibility.Visible;
            KisKazamata_Grid.Visibility = Visibility.Collapsed;
            ShowPakli();
        }

        private async void Nagy_Button_Click(object sender, RoutedEventArgs e)
        {
            bool anythingpossible=false;
            foreach (var item in AllCards)
            {
                bool found=false;
                for (int i = 0; i < Gyujtemeny.Count; i++)
                {
                    if (Gyujtemeny[i].Name==item.Name)
                    {
                        found= true; break;
                    }
                }
				for (int i = 0; i < Jatekos.Count; i++)
				{
					if (Jatekos[i].Name == item.Name)
					{
						found = true; break;
					}
				}
				if (!found)
                {
                    anythingpossible= true;
                    MainRoom_Grid.Visibility = Visibility.Collapsed;
                    NagyKazamata_Grid.Visibility = Visibility.Visible;
                    await Harc2.StartFight(Gyujtemeny, NagyKazamata, Jatekos, FightPlayerNagy_Wrap, FightKazamataNagy_Wrap, AttackNagy_Label, DefendNagy_Label, FightPlayerAttackerNagy_Wrap, FightKazamataAttackerNagy_Wrap);
                    MainRoom_Grid.Visibility = Visibility.Visible;
                    NagyKazamata_Grid.Visibility = Visibility.Collapsed;
                    ShowPakli();
                    break;
                }
            }
            if (!anythingpossible)
            {
                MessageBox.Show("Már minden kartyát megszereztél!");
            }
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
                ShowPakli();
            }

            else
            {
                MessageBox.Show("Legalább egy kártyát ki kell választanod!");
            }
            
        }

        private void ShowPakli()
        {
            PakliCards_Wrap.Children.Clear();
            foreach (var item in Jatekos)
            {
                
                item.Clicked -= RemoveFromPakli;
                item.Clicked -= AddToPakli;
                PlayerCards_Wrap.Children.Remove(item.GetVisual());
                PakliCards_Wrap.Children.Add(item.GetVisual());
            }

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
            }
        }
    }
}