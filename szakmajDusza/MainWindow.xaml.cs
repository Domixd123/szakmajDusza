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
		/*private void HomeButton(object sender, RoutedEventArgs e)
		{
			while (elozoGrid.Count != 0)
			{
				Back(null, null);
			}
		}*/
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