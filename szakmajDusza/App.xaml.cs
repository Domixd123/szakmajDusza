using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;

namespace szakmajDusza
{
    //copied from dusza github, we have to use this - TKD
    public partial class App : Application
    {
		protected override void OnStartup(StartupEventArgs e)
		{
			AttachConsole(-1);
			base.OnStartup(e);

			if (e.Args.Length == 0)
			{
				Console.WriteLine("Használat: szakmajDusza.exe [--ui | <test_dir_path>]");
				Shutdown(1);
				return;
			}

			if (e.Args[0] == "--ui")
			{
				// Játék mód
				return;
			}

			// Teszt mód
			try
			{
				RunAutomatedTest(e.Args[0]);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				Shutdown();
			}
		}

		private void RunAutomatedTest(string v)
		{
			// ...
		}

		[DllImport("Kernel32.dll")]
		private static extern bool AttachConsole(int processId);
	}

}
