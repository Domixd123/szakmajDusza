using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace szakmajDusza
{
	//copied from dusza github, we have to use this - TKD
	public partial class App : Application
	{
		/*protected override void OnStartup(StartupEventArgs e)
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
				if (e.Args[0][e.Args[0].Length-1]=='\\') RunAutomatedTest(e.Args[0]);
				else RunAutomatedTest(e.Args[0] + "\\");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				Shutdown();
			}
		}*/
		public static List<Card> Cards = new List<Card>();
		public static Dictionary<string, Card> CardsDict = new Dictionary<string, Card>();
		public static List<Card> Leaders = new List<Card>();
		public static Dictionary<string, Card> LeadersDict = new Dictionary<string, Card>();

		public static List<Card> Jatekos = new List<Card>();
		public static List<Card> Pakli = new List<Card>();

		public static List<Kazamata> Kazamatak = new List<Kazamata>();
		public static Dictionary<string,Kazamata>KazamatakDict=new Dictionary<string,Kazamata>();
		private void RunAutomatedTest(string v)
		{
			StreamReader sr = new StreamReader(v + "in.txt");
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
					Cards.Add(new Card(data[1], int.Parse(data[2]), int.Parse(data[3]), data[4], false));
					CardsDict.Add(data[1], Cards[Cards.Count - 1]);
				}
				else if (data[0] == "uj vezer")
				{
					Card vezer = CardsDict[data[2]].GetCopy();
					if (data[3] == "sebzes") vezer.Damage *= 2;
					else if (data[3] == "eletero") vezer.HP *= 2;
					vezer.Vezer = true;
					vezer.Name = data[1];
					Leaders.Add(vezer);
					LeadersDict.Add(data[1], Leaders[Leaders.Count - 1]);
				}
				else if (data[0] == "uj kazamata")
				{
					if (data[1] == "egyszeru")
					{
						Kazamatak.Add(new Kazamata(data[2], data[1], data[4], new List<Card>() { CardsDict[data[3]] }));
					}
					else if (data[1] == "kis")
					{
						List<Card> defenders = new List<Card>();
						string[] def = data[3].Split(",");
						for (int i = 0; i < def.Length; i++)
						{
							defenders.Add(CardsDict[def[i]]);
						}
						defenders.Add(LeadersDict[data[4]]);
						Kazamatak.Add(new Kazamata(data[2], data[1], data[5], defenders));
					}
					else if (data[1] == "nagy")
					{
						List<Card> defenders = new List<Card>();
						string[] def = data[3].Split(",");
						for (int i = 0; i < def.Length; i++)
						{
							defenders.Add(CardsDict[def[i]]);
						}
						defenders.Add(LeadersDict[data[4]]);
						Kazamatak.Add(new Kazamata(data[2], data[1], "newcard", defenders));
					}
					KazamatakDict.Add(Kazamatak[Kazamatak.Count - 1].Name, Kazamatak[Kazamatak.Count-1]);


				}
				else if (data[0] == "uj jatekos")
				{
					//i dont think we need to add anything into here
				}
				else if (data[0] == "felvetel gyujtemenybe")
				{
					if (CardsDict.ContainsKey(data[1]))
					{
						Jatekos.Add(CardsDict[data[1]].GetCopy());
					}
					else if (LeadersDict.ContainsKey(data[1]))
					{
						Jatekos.Add(LeadersDict[data[1]].GetCopy());
					}
				}
				else if (data[0] == "uj pakli")
				{
					string[] kartyanevek = data[1].Split(',');
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
				else if (data[0] == "export vilag")
				{
					StreamWriter sw = new StreamWriter(v + data[1]);
					for (int i = 0; i < Cards.Count; i++)
					{
						sw.WriteLine($"kartya;{Cards[i].Name};{Cards[i].Damage};{Cards[i].HP};{Card.TipusToString(Cards[i].Tipus)}");

					}
					sw.WriteLine();
					for (int i = 0; i < Leaders.Count; i++)
					{
						if (Leaders[i].Vezer)
						{
							sw.WriteLine($"vezer;{Leaders[i].Name};{Leaders[i].Damage};{Leaders[i].HP};{Card.TipusToString(Leaders[i].Tipus)}");
						}
					}
					sw.WriteLine();
					for (int i = 0; i < Kazamatak.Count; i++)
					{
						if (Kazamatak[i].Tipus == KazamataType.egyszeru)
						{
							sw.WriteLine($"kazamata;egyszeru;{Kazamatak[i].Name};{Kazamatak[i].Defenders[0].Name};{Kazamata.KazamataRewardToString(Kazamatak[i].reward)}");
						}
						else if (Kazamatak[i].Tipus == KazamataType.kis)
						{
							string normalDefenders = "";
							for (int j = 0; j < Kazamatak[i].Defenders.Count; j++)
							{
								if (!Kazamatak[i].Defenders[j].Vezer)
								{
									normalDefenders += $"{Kazamatak[i].Defenders[j].Name},";
								}
							}
							normalDefenders = normalDefenders.Substring(0, normalDefenders.Length - 1);//remove last ','
							string vezer = "";
							for (int j = 0; j < Kazamatak[i].Defenders.Count; j++)
							{
								if (Kazamatak[i].Defenders[j].Vezer)
								{
									vezer = Kazamatak[i].Defenders[j].Name;
									break;
								}
							}
							sw.WriteLine($"kazamata;kis;{Kazamatak[i].Name};{normalDefenders};{vezer};{Kazamata.KazamataRewardToString(Kazamatak[i].reward)}");
						}
						else if (Kazamatak[i].Tipus == KazamataType.nagy)
						{
							string normalDefenders = "";
							for (int j = 0; j < Kazamatak[i].Defenders.Count; j++)
							{
								if (!Kazamatak[i].Defenders[j].Vezer)
								{
									normalDefenders += $"{Kazamatak[i].Defenders[j].Name},";
								}
							}
							normalDefenders = normalDefenders.Substring(0, normalDefenders.Length - 1);//remove last ','
							string vezer = "";
							for (int j = 0; j < Kazamatak[i].Defenders.Count; j++)
							{
								if (Kazamatak[i].Defenders[j].Vezer)
								{
									vezer = Kazamatak[i].Defenders[j].Name;
									break;
								}
							}
							sw.WriteLine($"kazamata;nagy;{Kazamatak[i].Name};{normalDefenders};{vezer}");
						}
					}
					sw.Close();
				}
				else if (data[0] == "export jatekos")
				{
					StreamWriter sw = new StreamWriter(v + data[1]);
					for (int i = 0; i < Jatekos.Count; i++)
					{
						sw.WriteLine($"gyujtemeny;{Jatekos[i].Name};{Jatekos[i].Damage};{Jatekos[i].HP};{Card.TipusToString(Jatekos[i].Tipus)}");
					}
					sw.WriteLine();
					for (int i = 0; i < Pakli.Count; i++)
					{
						sw.WriteLine($"pakli;{Pakli[i].Name}");
					}
					sw.Close();
				}
				else if (data[0] == "harc")
				{
					StreamWriter sw = new StreamWriter(v + data[2]);
					Harc2.StartFight(KazamatakDict[data[1]].GetCopy(), Card.GetListCopy(Pakli), sw);
					sw.Close();
				}
			}
		}

		[DllImport("Kernel32.dll")]
		private static extern bool AttachConsole(int processId);
	}

}
