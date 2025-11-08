using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                Cards_Wrap.Children.Add(item.GetVisual());
            }
        }

        private void Egyszeri_Button_Click(object sender, RoutedEventArgs e)
        {
            MainRoom_Grid.Visibility = Visibility.Collapsed;
            EgyszeriKazamata_Grid.Visibility = Visibility.Visible;
            StartFight(EgyszeruKazamata);
        }

        private void Kis_Button_Click(object sender, RoutedEventArgs e)
        {
            MainRoom_Grid.Visibility = Visibility.Collapsed;
            KisKazamata_Grid.Visibility = Visibility.Visible;
            StartFight(KisKazamata);
        }

        private void Nagy_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in NagyKazamata.Defenders)
            {
                if (!Jatekos.Contains(item))
                {
                    MainRoom_Grid.Visibility = Visibility.Collapsed;
                    NagyKazamata_Grid.Visibility = Visibility.Visible;
                    StartFight(NagyKazamata);
                }
            }
        }

        private void StartFight(Kazamata k)
        {

        }
    }
}