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
		private void ConfirmDif_Button_Click(object sender, RoutedEventArgs e)
		{
			string fileName = KornyezetekJatekos_List.SelectedItem.ToString().Split('(')[0][..^1];
			editor = false;
			LoadSave($"kornyezet/{fileName}.txt");
			//loaddata was here
			Difficulty = int.Parse((string)Dif_Label.Content);
			KornyezetekJatekos_List.SelectedItem = null;
			Difficulty_Stack.Visibility= Visibility.Collapsed;
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
	}
}
