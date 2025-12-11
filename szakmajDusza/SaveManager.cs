using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace szakmajDusza
{
	public partial class MainWindow : Window
	{
		public void LoadSave(string path)
		{
			if (Path.GetDirectoryName(path) == "saves") fileName = Path.GetFileName(path);
			Item.ResetItems();
			DynamicButtonsPanel.Children.Clear();
			Gyujtemeny.Clear();
			Jatekos.Clear();
			AllCardsDict.Clear();
			AllLeadersDict.Clear();
			AllKazamataDict.Clear();
			Cards_Wrap.Children.Clear();
			PlayerCards_Wrap.Children.Clear();
			StreamReader sr = new StreamReader(path);
			while (!sr.EndOfStream)
			{
				string? line = sr.ReadLine();
				if (line == null || line == "")
				{
					continue;
				}

				string[] data = line.Split(';');
				if (data[0] == "difficulty" && Path.GetDirectoryName(path) == "saves")
				{
					Difficulty = int.Parse(data[1]);
				}
				else if (data[0] == "uj kartya")
				{
					AllCardsDict.Add(data[1], new Card(data[1], int.Parse(data[2]), int.Parse(data[3]), data[4], false));
				}
				else if (data[0] == "uj vezer")
				{
					Card vezer = AllCardsDict[data[2]].GetCopy();
					if (data[3] == "sebzes") vezer.Damage *= 2;
					else if (data[3] == "eletero") vezer.HP *= 2;
					vezer.Vezer = true;
					vezer.Name = data[1];
					vezer.Bonus = data[3];
					vezer.OriginName = data[2];
					vezer.UpdateAllVisual();
					AllLeadersDict.Add(data[1], vezer);
					AllLeadersDict[data[1]].UpdateAllVisual();
				}
				else if (data[0] == "uj kazamata")
				{
					if (data[1] == "egyszeru")
					{
						List<Card> defender = new List<Card>();
						Card card = AllCardsDict[data[3].Split(':')[0]].GetCopy(true);
						defender.Add(card);
						string[] itemNames = data[3].Split(':')[1].Split(',');
						if (itemNames.Length != 1 || itemNames[0] != "")
						{
							for (int i = 0; i < itemNames.Length; i++)
							{
								Item item = Item.Items[itemNames[i].Split('-')[0]].GetCopy();
								int level = int.Parse(itemNames[i].Split('-')[1]);
								item.Level = level;
								defender[0].Items.Add(item);
							}
						}
						Kazamata kazamata = new Kazamata(data[2], "egyszeru", data[4], defender);
						AllKazamataDict.Add(data[2], kazamata);
					}
					else if (data[1] == "kis")
					{
						List<Card> defenders = new List<Card>();
						for (int i = 3; i < data.Length - 1; i++)
						{
							string cardName = data[i].Split(':')[0];
							string[] cardItems = data[i].Split(":")[1].Split(',');
							if (AllCardsDict.ContainsKey(cardName))
							{
								defenders.Add(AllCardsDict[cardName].GetCopy(true));
							}
							else
							{
								defenders.Add(AllLeadersDict[cardName].GetCopy(true));
							}
							if (cardItems.Length == 1 && cardItems[0] == "") continue;
							for (int j = 0; j < cardItems.Length; j++)
							{
								string itemName = cardItems[j].Split('-')[0];
								int itemLevel = int.Parse(cardItems[j].Split('-')[1]);
								Item item = Item.Items[itemName];
								item.Level = itemLevel;
								defenders[defenders.Count - 1].Items.Add(item);
							}
						}
						Kazamata kazamata = new Kazamata(data[2], "kis", data[data.Length - 1], defenders);
						AllKazamataDict.Add(data[2], kazamata);
					}
					else if (data[1] == "nagy")
					{
						List<Card> defenders = new List<Card>();
						for (int i = 3; i < data.Length; i++)
						{
							string cardName = data[i].Split(':')[0];
							string[] cardItems = data[i].Split(":")[1].Split(',');
							if (AllCardsDict.ContainsKey(cardName))
							{
								defenders.Add(AllCardsDict[cardName].GetCopy(true));
							}
							else
							{
								defenders.Add(AllLeadersDict[cardName].GetCopy(true));
							}
							if (cardItems.Length == 1 && cardItems[0] == "") continue;
							for (int j = 0; j < cardItems.Length; j++)
							{
								string itemName = cardItems[j].Split('-')[0];
								int itemLevel = int.Parse(cardItems[j].Split('-')[1]);
								Item item = Item.Items[itemName];
								item.Level = itemLevel;
								defenders[defenders.Count - 1].Items.Add(item);
							}
						}
						Kazamata kazamata = new Kazamata(data[2], "nagy", "", defenders);
						AllKazamataDict.Add(data[2], kazamata);
					}


				}
				else if (data[0] == "felvetel gyujtemenybe")
				{
					if (AllCardsDict.ContainsKey(data[1]))
					{
						Gyujtemeny.Add(AllCardsDict[data[1]].GetCopy());
					}
					else if (AllLeadersDict.ContainsKey(data[1]))
					{
						Gyujtemeny.Add(AllLeadersDict[data[1]].GetCopy());
					}
				}
				else if (data[0] == "kartyafejlesztes")
				{
					if (data[2]=="sebzes")
					{
						foreach (var item in Gyujtemeny)
						{
							if (item.Name == data[1])
							{
								item.Damage += int.Parse(data[3]);
								break;
							}
						}
					}
					else if (data[2] == "eletero")
					{
						foreach (var item in Gyujtemeny)
						{
							if (item.Name == data[1])
							{
								item.HP += int.Parse(data[3]);
								break;
							}
						}
					}
				}
				else if (data[0] == "uj pakli")
				{
					string[] kartyanevek = data[1].Split(',');
					for (int i = 0; i < kartyanevek.Length; i++)
					{
						foreach (var item in Gyujtemeny)
						{
							if (item.Name == kartyanevek[i])
							{
								Jatekos.Add(item);
								break;
							}
						}
					}
				}
				else if (data[0] == "arany")
				{
					Item.GoldOwned = int.Parse(data[1]);
				}
				else if (data[0] == "shopRefreshPrice")
				{
					Item.shopRefreshPrice = int.Parse(data[1]);
				}
				else if (data[0] == "shopItemCount")
				{
					Item.shopItemCount = int.Parse(data[1]);
				}
				else if (data[0] == "item")
				{
					Item.Items[data[1]].Buyable = bool.Parse(data[2]);
					Item.Items[data[1]].MaxLevel = int.Parse(data[3]);
					Item.Items[data[1]].Price = int.Parse(data[4]);
					Item.Items[data[1]].Level = int.Parse(data[5]);
					Item.Items[data[1]].OwnedCount = int.Parse(data[6]);
					Item.Items[data[1]].BaseVariable = int.Parse(data[7]);
					Item.Items[data[1]].InRotation = bool.Parse(data[8]);
				}
				else if (data[0] == "jatekos kartya items")
				{
					int ID = -1;
					for (int i = 0; i < Gyujtemeny.Count; i++)
					{
						if (Gyujtemeny[i].Name == data[1])
						{
							ID = i;
							break;
						}
					}
					string[] itemNames = data[2].Split(',');
					if (itemNames.Length == 1 && itemNames[0] == "") continue;
					for (int i = 0; i < itemNames.Length; i++)
					{
						Item item = Item.Items[itemNames[i]];
						Gyujtemeny[ID].Items.Add(item);
					}
				}
			}
			sr.Close();
			foreach (var item in Gyujtemeny)
			{
				item.UpdateAllVisual();
				bool found = false;
				foreach (var item2 in Jatekos)
				{
					if (item.Name == item2.Name)
					{
						found = true;
						break;
					}
				}
				if (found) continue;
				item.Clicked += AddToPakli;
				item.RightClicked -= RightClick;
				item.RightClicked += RightClick;
				Cards_Wrap.Children.Add(item.GetVisual());
			}
			foreach (var item in Jatekos)
			{
				item.UpdateAllVisual();
				item.Clicked += RemoveFromPakli;
				item.RightClicked -= RightClick;
				item.RightClicked += RightClick;
				PlayerCards_Wrap.Children.Add(item.GetVisual());
			}
			foreach (var item in AllKazamataDict.Values)
			{
				Button b = new Button();
				b.Click += (s, e) =>
				{

					ShowKazamata(AllKazamataDict[item.Name]);

				};
				b.Content = item.Name;
				b.Margin = new Thickness(10, 0, 0, 0);
				DynamicButtonsPanel.Children.Add(b);
			}
			if (DynamicButtonsPanel.ActualWidth != 0)
			{
				AutoResizeButtons(DynamicButtonsPanel);
			}

			SelectableCounter_Label.Content = $"/ {Math.Ceiling((float)Gyujtemeny.Count / 2f)}";
			SelectedCards_Label.Content = Jatekos.Count;

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
			if (fileName == null || fileName == "")
			{
				GoToGrid(Save_Grid);
			}
			else
			{
				SaveProgress();
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
					fileName = $"{FileName_TextBox.Text}.txt";
					Kornyezetrogress();
					KornyezetekMester_List.ItemsSource = Directory.GetFiles("kornyezet").Select(x => x.Split('\\')[1].Split('.')[0]);
					var k1 = Directory.GetFiles("kornyezet")
	.Select(x => Path.GetFileNameWithoutExtension(x) + " (új)");

					var k2 = Directory.GetFiles("saves")
						.Select(x => Path.GetFileNameWithoutExtension(x) + " (mentett)");

					KornyezetekJatekos_List.ItemsSource = k1.Concat(k2).ToList();
					Back(sender, e);
					fileName = "";
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
					fileName = $"{FileName_TextBox.Text}.txt";
					SaveProgress();
					Back(sender, e);

				}
			}

		}
		public static void SaveProgress()
		{
			if (fileName == null || fileName == "")
			{
				//ask for a unique filename
				return;
			}
			Directory.CreateDirectory("saves");
			StreamWriter sw = new StreamWriter("saves/" + fileName);
			sw.WriteLine($"difficulty;{MainWindow.Difficulty}");
			sw.WriteLine();
			foreach (var item in MainWindow.AllCardsDict.Values)
			{
				sw.WriteLine($"uj kartya;{item.Name};{item.Damage};{item.HP};{Card.TipusToString(item.Tipus)}");
			}
			sw.WriteLine();
			foreach (var item in MainWindow.AllLeadersDict.Values)
			{
				sw.WriteLine($"uj vezer;{item.Name};{item.OriginName};{item.Bonus}");
			}
			sw.WriteLine();
			foreach (var kazamata in MainWindow.AllKazamataDict.Values)
			{
				string kazamataString = $"uj kazamata;{Kazamata.KazamataTypeToString(kazamata.Tipus)};{kazamata.Name};";
				foreach (var defender in kazamata.Defenders)
				{
					string defenderItems = "";
					foreach (var item in defender.Items)
					{
						defenderItems += $"{item.Name}-{item.Level},";
					}
					if (defenderItems.Length != 0) defenderItems = defenderItems.Substring(0, defenderItems.Length - 1);
					kazamataString += $"{defender.Name}:{defenderItems};";
				}
				if (kazamata.Tipus == KazamataType.egyszeru || kazamata.Tipus == KazamataType.kis)
				{
					kazamataString += $"{Kazamata.KazamataRewardToString(kazamata.reward)}";
				}
				else
				{
					kazamataString = kazamataString.Substring(0, kazamataString.Length - 1);
				}
				sw.WriteLine(kazamataString);
			}
			sw.WriteLine();
			foreach (var item in MainWindow.Gyujtemeny)
			{
				sw.WriteLine($"felvetel gyujtemenybe;{item.Name}");
			}
			sw.WriteLine();
			foreach (var item in MainWindow.Gyujtemeny)
			{
				if (item.Vezer)
				{
					int eleteroAmmount = item.HP - MainWindow.AllLeadersDict[item.Name].HP;
					if (eleteroAmmount > 0)
					{
						sw.WriteLine($"kartyafejlesztes;{item.Name};eletero;{eleteroAmmount}");
					}
					int sebzesAmmount = item.Damage - MainWindow.AllLeadersDict[item.Name].Damage;
					if (sebzesAmmount > 0)
					{
						sw.WriteLine($"kartyafejlesztes;{item.Name};sebzes;{sebzesAmmount}");
					}
				}
				else
				{
					int eleteroAmmount = item.HP - MainWindow.AllCardsDict[item.Name].HP;
					if (eleteroAmmount > 0)
					{
						sw.WriteLine($"kartyafejlesztes;{item.Name};eletero;{eleteroAmmount}");
					}
					int sebzesAmmount = item.Damage - MainWindow.AllCardsDict[item.Name].Damage;
					if (sebzesAmmount > 0)
					{
						sw.WriteLine($"kartyafejlesztes;{item.Name};sebzes;{sebzesAmmount}");
					}
				}
			}
			sw.WriteLine();
			sw.WriteLine($"arany;{Item.GoldOwned}");
			sw.WriteLine($"shopRefreshPrice;{Item.shopRefreshPrice}");
			sw.WriteLine($"shopItemCount;{Item.shopItemCount}");
			sw.WriteLine();
			foreach (var item in Item.Items.Values)
			{
				sw.WriteLine($"item;{item.Name};{item.Buyable};{item.MaxLevel};{item.Price};{item.Level};{item.OwnedCount};{item.BaseVariable};{item.InRotation}");
			}
			sw.WriteLine();
			foreach (var item in MainWindow.Gyujtemeny)
			{
				string itemString = "";
				foreach (var item2 in item.Items)
				{
					itemString += item2.Name + ",";
				}
				if (itemString.Length > 0) itemString = itemString.Substring(0, itemString.Length - 1);

				sw.WriteLine($"jatekos kartya items;{item.Name};{itemString}");
			}
			sw.WriteLine();
			string pakliString = "";
			foreach (var item in MainWindow.Jatekos)
			{
				pakliString += item.Name + ",";
			}
			if (pakliString.Length != 0)
			{
				pakliString = pakliString.Substring(0, pakliString.Length - 1);
				sw.WriteLine($"uj pakli;{pakliString}");
			}


			sw.Close();
		}
		public static void Kornyezetrogress()
		{
			if (fileName == null || fileName == "")
			{
				return;
			}
			Directory.CreateDirectory("kornyezet");
			StreamWriter sw = new StreamWriter("kornyezet/" + fileName);

			foreach (var item in MainWindow.AllCardsDict.Values)
			{
				sw.WriteLine($"uj kartya;{item.Name};{item.Damage};{item.HP};{Card.TipusToString(item.Tipus)}");
			}
			sw.WriteLine();
			foreach (var item in MainWindow.AllLeadersDict.Values)
			{
				sw.WriteLine($"uj vezer;{item.Name};{item.OriginName};{item.Bonus}");
			}
			sw.WriteLine();
			foreach (var kazamata in MainWindow.AllKazamataDict.Values)
			{
				string kazamataString = $"uj kazamata;{Kazamata.KazamataTypeToString(kazamata.Tipus)};{kazamata.Name};";
				foreach (var defender in kazamata.Defenders)
				{
					string defenderItems = "";
					foreach (var item in defender.Items)
					{
						defenderItems += $"{item.Name}-{item.Level},";
					}
					if (defenderItems.Length != 0) defenderItems = defenderItems.Substring(0, defenderItems.Length - 1);
					kazamataString += $"{defender.Name}:{defenderItems};";
				}
				if (kazamata.Tipus == KazamataType.egyszeru || kazamata.Tipus == KazamataType.kis)
				{
					kazamataString += $"{Kazamata.KazamataRewardToString(kazamata.reward)}";
				}
				else
				{
					kazamataString = kazamataString.Substring(0, kazamataString.Length - 1);
				}
				sw.WriteLine(kazamataString);
			}
			sw.WriteLine();
			foreach (var item in MainWindow.Gyujtemeny)
			{
				sw.WriteLine($"felvetel gyujtemenybe;{item.Name}");
			}
			sw.WriteLine();
			foreach (var item in MainWindow.Gyujtemeny)
			{
				if (item.Vezer)
				{
					int eleteroAmmount = item.HP - MainWindow.AllLeadersDict[item.Name].HP;
					if (eleteroAmmount > 0)
					{
						sw.WriteLine($"kartyafejlesztes;{item.Name};eletero;{eleteroAmmount}");
					}
					int sebzesAmmount = item.Damage - MainWindow.AllLeadersDict[item.Name].Damage;
					if (sebzesAmmount > 0)
					{
						sw.WriteLine($"kartyafejlesztes;{item.Name};sebzes;{sebzesAmmount}");
					}
				}
				else
				{
					int eleteroAmmount = item.HP - MainWindow.AllCardsDict[item.Name].HP;
					if (eleteroAmmount > 0)
					{
						sw.WriteLine($"kartyafejlesztes;{item.Name};eletero;{eleteroAmmount}");
					}
					int sebzesAmmount = item.Damage - MainWindow.AllCardsDict[item.Name].Damage;
					if (sebzesAmmount > 0)
					{
						sw.WriteLine($"kartyafejlesztes;{item.Name};sebzes;{sebzesAmmount}");
					}
				}

			}
			sw.WriteLine();
			sw.WriteLine($"arany;{Item.GoldOwned}");
			sw.WriteLine($"shopRefreshPrice;{Item.shopRefreshPrice}");
			sw.WriteLine($"shopItemCount;{Item.shopItemCount}");
			sw.WriteLine();
			if (Item.Items != null)
			{
				foreach (var item in Item.Items.Values)
				{
					sw.WriteLine($"item;{item.Name};{item.Buyable};{item.MaxLevel};{item.Price};{item.Level};{item.OwnedCount};{item.BaseVariable};{item.InRotation}");
				}
			}

			sw.WriteLine();


			sw.Close();
		}
		/*private void HomeButton(object sender, RoutedEventArgs e)
		{
			while (elozoGrid.Count != 0)
			{
				Back(null, null);
			}
		}*/
	}
}
