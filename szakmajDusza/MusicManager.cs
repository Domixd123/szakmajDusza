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
		private void SFX_On(object sender, RoutedEventArgs e)
		{
			se.Volume = seVolume;
			se.IsMuted = false;
		}
		private void SFX_Off(object sender, RoutedEventArgs e)
		{
			se.IsMuted = true;
		}
		private void MUSIC_On(object sender, RoutedEventArgs e)
		{
			sp.Volume = spVolume;
			sp.IsMuted = false;
		}
		private void MUSIC_Off(object sender, RoutedEventArgs e)
		{
			sp.IsMuted = true;
		}
		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{

			spVolume = (float)Sl.Value * spMult;
			sp.Volume = spVolume;


			seVolume = (float)Sl.Value * seMult;

			se.Volume = seVolume;



		}
	}
}
