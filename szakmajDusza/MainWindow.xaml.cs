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
        public static List<Card> Jatekos = new List<Card>();
        public static List<Card> EgyszeruKazamata = new List<Card>();
        public static List<Card> KisKazamata = new List<Card>();
        public static List<Card> NagyKazamata = new List<Card>();

        public MainWindow()
        {
            InitializeComponent();

            MainRoom_Grid.Visibility = Visibility.Visible;

            UploadCards();
            

        }

        private void UploadCards()
        {
            Jatekos.Add(new Card("Arin", 2, 5, "fold", false));
            Jatekos.Add(new Card("Liora", 2, 4, "levego", false));
            Jatekos.Add(new Card("Sellia", 2, 6, "viz", false));
            Jatekos.Add(new Card("Nerun", 3, 3, "tuz", false));
            Jatekos.Add(new Card("Torak", 3, 4, "fold", false));
            Jatekos.Add(new Card("Emera", 2, 5, "levego", false));
            Jatekos.Add(new Card("Kael", 3, 5, "tuz", false));
            Jatekos.Add(new Card("Myra", 2, 6, "fold", false));
            Jatekos.Add(new Card("Thalen", 3, 5, "levego", false));
            Jatekos.Add(new Card("Isara", 2, 6, "viz", false));


            EgyszeruKazamata.Add(new Card("Nerun", 3, 3, "tuz", false));

            KisKazamata.Add(new Card("Arin", 2, 5, "fold", false));
            KisKazamata.Add(new Card("Emera", 2, 5, "levego", false));
            KisKazamata.Add(new Card("Selia", 2, 6, "viz", false));
            KisKazamata.Add(new Card("Lord Torak", 6, 4, "fold", true));

            NagyKazamata.Add(new Card("Liora", 2, 4, "levego", false));
            NagyKazamata.Add(new Card("Arin", 2, 5, "fold", false));
            NagyKazamata.Add(new Card("Selia", 2, 6, "viz", false));
            NagyKazamata.Add(new Card("Nerun", 3, 3, "tuz", false));
            NagyKazamata.Add(new Card("Torak", 3, 4, "fold", false));
            NagyKazamata.Add(new Card("Priestess Selia", 2, 12, "fold", true));


            foreach (var item in Jatekos)
            {
                Cards_Wrap.Children.Add(item.GetVisual());
            }
        }

        private void Egyszeri_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Kis_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Nagy_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}