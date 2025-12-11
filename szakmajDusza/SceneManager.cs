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
		private void Exit_Button_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
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
		private void GoToMaster_Button_Click(object sender, RoutedEventArgs e)
		{
			editor = true;
			GoToGrid(JatekMester_Grid);
		}
		private void Back(object sender, RoutedEventArgs e)
		{
			if (menuMusic != "menu")
			{
				MainWindow.sp.Open(new Uri("Sounds/Menu.wav", UriKind.Relative));
				MainWindow.sp.Play();
				menuMusic = "menu";
			}
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
			if (vissza == PakliOssze_Grid)
			{
				foreach (Card item in Gyujtemeny)
				{
					item.Disabled = false;
				}
				foreach (Card item in Jatekos)
				{
					item.Disabled = false;

				}
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

			if (kovetkezo == Shop_Grid)
			{
				foreach (var item in Item.Items.Values)
				{

					Item i = item;
					i.Clicked -= AddAbility;


				}
			}

			if (kovetkezo == MainRoom_Grid)
			{
				DisableNagyKazamata();

			}
		}
	}
}
