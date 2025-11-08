using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;
using System.IO;

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
		public static List<Card> Cards = new List<Card>();
		public static Dictionary<string,Card>CardsDict = new Dictionary<string,Card>();
		public static List<Card> Leaders = new List<Card>();
		public static Dictionary<string, Card> LeadersDict = new Dictionary<string, Card>();

		public static List<Card> Jatekos = new List<Card>();
		public static List<Card>Pakli = new List<Card>();

		public static List<Kazamata>Kazamatak=new List<Kazamata>();
		private void RunAutomatedTest(string v)
		{
			StreamReader sr=new StreamReader(v);
			while (!sr.EndOfStream)
			{
				string? line = sr.ReadLine();
				if (line == null || line == "")
				{
					continue;
				}
				string[] data = line.Split(';');

				if (data[0] == "uj kartya")
				{
					Cards.Add(new Card(data[1], int.Parse(data[2]), int.Parse(data[3]), data[4],false));
					CardsDict.Add(data[1], Cards[Cards.Count - 1]);
				}
				else if (data[0] == "uj vezer")
				{
					Leaders.Add(new Card(data[1], int.Parse(data[2]), int.Parse(data[3]), data[4], true));
					LeadersDict.Add(data[1], Leaders[Leaders.Count - 1]);
				}
				else if (data[0]=="uj kazamata")
				{
					if(data[1] == "egyszeru")
					{
						Kazamatak.Add(new Kazamata(data[2], data[1], data[4],new List<Card>() { CardsDict[data[3]] }));
					}
					else if(data[1] == "kis")
					{
						//gotta finish this
					}


					

				}
				else if (data[0]=="uj jatekos")
				{
					//i dont think we need to add anything into here
				}
				else if (data[0]=="felvetel gyujtemenybe")
				{
					if (CardsDict.ContainsKey(data[1]))
					{
						Jatekos.Add(CardsDict[data[1]]);
					}
					else if (LeadersDict.ContainsKey(data[1]))
					{
						Jatekos.Add(LeadersDict[data[1]]);
					}
				}
				else if (data[0] =="uj pakli")
				{
					string[]kartyanevek=data[1].Split(',');
					for (int i = 0; i < kartyanevek.Length; i++)
					{
						if (CardsDict.ContainsKey(kartyanevek[i]))
						{
							Pakli.Add(CardsDict[kartyanevek[i]]);
						}
						else if (LeadersDict.ContainsKey(kartyanevek[i]))
						{
							Pakli.Add(LeadersDict[kartyanevek[i]]);
						}
					}
				}
				else if(data[0] =="export vilag")
				{

				}
				else if (data[0] == "export jatekos")
				{

				}
				else if (data[0] == "harc")
				{

				}
			}
		}

		[DllImport("Kernel32.dll")]
		private static extern bool AttachConsole(int processId);
	}

}
