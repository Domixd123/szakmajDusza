<<<<<<< Updated upstream
﻿using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
=======
﻿using System.Windows;
>>>>>>> Stashed changes
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
            Gyujtemeny.Add(new Card("Arin", 2, 5, "fold", false));
            Gyujtemeny.Add(new Card("Liora", 2, 4, "levego", false));
            Gyujtemeny.Add(new Card("Sellia", 2, 6, "viz", false));
            Gyujtemeny.Add(new Card("Nerun", 3, 3, "tuz", false));
            Gyujtemeny.Add(new Card("Torak", 3, 4, "fold", false));
            Gyujtemeny.Add(new Card("Emera", 2, 5, "levego", false));
            Gyujtemeny.Add(new Card("Kael", 3, 5, "tuz", false));
            Gyujtemeny.Add(new Card("Myra", 2, 6, "fold", false));
            Gyujtemeny.Add(new Card("Thalen", 3, 5, "levego", false));
            Gyujtemeny.Add(new Card("Isara", 2, 6, "viz", false));


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
            if (Jatekos.Count > Gyujtemeny.Count / 2 || Jatekos.Contains(clicked))
            {
                MessageBox.Show("Nem lehet hozzáadni ezt a kártyát");
            }
            else
            {
                Cards_Wrap.Children.Remove(clicked.GetVisual());
                Jatekos.Add(clicked);
                PlayerCards_Wrap.Children.Add(clicked.GetVisual());
                clicked.Clicked -= AddToPakli;
            }
        }

        private void Egyszeri_Button_Click(object sender, RoutedEventArgs e)
        {
            MainRoom_Grid.Visibility = Visibility.Collapsed;
            EgyszeriKazamata_Grid.Visibility = Visibility.Visible;
            Harc.StartFight(EgyszeruKazamata, Jatekos);
        }

        private void Kis_Button_Click(object sender, RoutedEventArgs e)
        {
            MainRoom_Grid.Visibility = Visibility.Collapsed;
            KisKazamata_Grid.Visibility = Visibility.Visible;
            Harc.StartFight(KisKazamata, Jatekos);
        }

        private void Nagy_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in NagyKazamata.Defenders)
            {
                if (!Jatekos.Contains(item))
                {
                    MainRoom_Grid.Visibility = Visibility.Collapsed;
                    NagyKazamata_Grid.Visibility = Visibility.Visible;
                    Harc.StartFight(NagyKazamata, Jatekos);
                }
            }
        }


        private void StartFight(Kazamata k, List<Card> pakli)
        {
            Card kaz = null;
            Card play = null;
            while (k.Defenders.Count != 0 && pakli.Count != 0)
            {
                if (kaz == null)
                {
                    kaz = k.Defenders[0];
                    k.Defenders.RemoveAt(0);
                }
                else
                {

                }

                if (play == null)
                {
                    play = pakli[0];
                    pakli.RemoveAt(0);
                }
                else
                {

                }
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
            PakliOssze_Grid.Visibility = Visibility.Collapsed;
            MainRoom_Grid.Visibility = Visibility.Visible;
            foreach (var item in Jatekos)
            {
                PlayerCards_Wrap.Children.Remove(item.GetVisual());
                PakliCards_Wrap.Children.Add(item.GetVisual());
            }
        }
    }
}