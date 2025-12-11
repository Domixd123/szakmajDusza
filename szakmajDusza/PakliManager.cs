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
				bool found = false;
				foreach (var item2 in Jatekos)
				{
					if (item.Name == item2.Name)
					{
						found = true; break;
					}
				}
				if (!found)
				{
					Cards_Wrap.Children.Add(item.GetVisual());
					item.Clicked += AddToPakli;
					item.Clicked -= RemoveFromPakli;
				}

			}

			SelectableCounter_Label.Content = $"/ {Math.Ceiling((float)Gyujtemeny.Count / 2f)}";


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
	}
}
