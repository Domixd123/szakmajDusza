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
		private void KartyaSzerkeszto_TextChange(object sender, RoutedEventArgs e)
		{
			if (AllCardsDict.ContainsKey(cardEditName))
			{
				string edits = KartyaSzerkesztoCardName.Text;
				if (!AllCardsDict.ContainsKey(edits) && (bool)!VezerCheck.IsChecked)
				{
					AllCardsDict.Add(edits, AllCardsDict[cardEditName]);
					AllCardsDict.Remove(cardEditName);
					AllCardsDict[edits].Name = edits;
					AllCardsDict[edits].UpdateAllVisual();
					foreach (Card item in Gyujtemeny)
					{
						if (item.Name == cardEditName)
						{
							item.Name = edits;
							break;
						}
					}
					cardEditName = edits;
					SelectedCard_Wrap.Children.Clear();
					SelectedCard_Wrap.Children.Add(AllCardsDict[cardEditName.ToString()].GetCopy().GetVisual());
				}
			}
			else
			{
				string edits = VezerNev.Text + " " + VezerAlapKartya.SelectedItem.ToString();
				if (!AllLeadersDict.ContainsKey(edits) && (bool)!VezerCheck.IsChecked)
				{
					AllLeadersDict.Add(edits, AllLeadersDict[cardEditName]);
					AllLeadersDict.Remove(cardEditName);
					AllLeadersDict[edits].Name = edits;
					if (VezerBonusTipus.SelectedIndex == 0)
					{
						AllLeadersDict[edits].Bonus = "eletero";

					}
					else
					{
						AllLeadersDict[edits].Bonus = "sebzes";
					}
					foreach (Card item in Gyujtemeny)
					{
						if (item.Name == cardEditName)
						{
							item.Name = edits;
							break;
						}
					}
					AllLeadersDict[edits].UpdateAllVisual();
					cardEditName = edits;
					SelectedCard_Wrap.Children.Clear();
					SelectedCard_Wrap.Children.Add(AllLeadersDict[cardEditName.ToString()].GetCopy().GetVisual());
				}
			}


			//AllCardsDict[cardEditName].Name = KartyaSzerkesztoCardName.Text;
		}
		private void KartyaSzerkeszto_TextChange(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (AllCardsDict.ContainsKey(cardEditName))
			{
				if (e.Key == System.Windows.Input.Key.Enter)
				{
					string edits = KartyaSzerkesztoCardName.Text;
					if (!AllCardsDict.ContainsKey(edits))
					{
						AllCardsDict.Add(edits, AllCardsDict[cardEditName]);
						AllCardsDict.Remove(cardEditName);
						AllCardsDict[edits].Name = edits;
						AllCardsDict[edits].UpdateAllVisual();
						foreach (Card item in Gyujtemeny)
						{
							if (item.Name == cardEditName)
							{
								item.Name = edits;
								break;
							}
						}
						cardEditName = edits;
						SelectedCard_Wrap.Children.Clear();
						SelectedCard_Wrap.Children.Add(AllCardsDict[cardEditName.ToString()].GetCopy().GetVisual());
					}
				}
			}
			else
			{
				if (e.Key == System.Windows.Input.Key.Enter)
				{
					string edits = VezerNev.Text + " " + VezerAlapKartya.SelectedItem.ToString();
					if (!AllLeadersDict.ContainsKey(edits))
					{
						AllLeadersDict.Add(edits, AllLeadersDict[cardEditName]);
						AllLeadersDict.Remove(cardEditName);
						AllLeadersDict[edits].Name = edits;
						AllLeadersDict[edits].UpdateAllVisual();
						foreach (Card item in Gyujtemeny)
						{
							if (item.Name == cardEditName)
							{
								item.Name = edits;
								break;
							}
						}
						cardEditName = edits;
						SelectedCard_Wrap.Children.Clear();
						SelectedCard_Wrap.Children.Add(AllLeadersDict[cardEditName.ToString()].GetCopy().GetVisual());
					}
				}
			}

		}
		private void CreateNewCard_Button_Click(object sender, RoutedEventArgs e)
		{
			internalEdits = true;
			LeaderCardPanel.Visibility = Visibility.Collapsed;
			BasicCardPanel.Visibility = Visibility.Visible;
			SelectedCard_Wrap.Children.Clear();
			VezerCheck.IsChecked = false;

			internalEdits = false;
			GoToGrid(KartyaSzerkeszto_Grid);
			string cardName = "ÚjKártya";
			bool goodName = false;
			int bonusIndex = -1;
			while (!goodName)
			{
				bonusIndex++;
				goodName = true;
				foreach (Card item in AllCardsDict.Values)
				{
					if (bonusIndex <= 0)
					{
						if (item.Name == cardName)
						{
							//bonusIndex++;
							goodName = false;
						}
					}
					else if (item.Name == cardName + bonusIndex)
					{
						//bonusIndex++;
						goodName = false;
					}
				}
			}
			if (bonusIndex > 0)
			{
				cardName += bonusIndex;
			}
			Card k = new Card(cardName, 1, 1, "fold", false);
			internalEdits = true;
			cardEditName = k.Name;
			Gyujtemeny_Check.IsChecked = false;
			foreach (Card item in Gyujtemeny)
			{
				if (item.Name == k.Name)
				{
					Gyujtemeny_Check.IsChecked = true;
					break;
				}
			}
			//GoToGrid(KartyaSzerkeszto_Grid);
			KartyaSzerkesztoCardName.Text = k.Name;
			SelectType.ItemsSource = new string[] { "Föld", "Víz", "Levegő", "Tűz" };
			TypeAttack.Text = k.Damage.ToString();
			TypeDefense.Text = k.HP.ToString();
			if (k.Tipus == KartyaTipus.fold) SelectType.SelectedIndex = 0;
			else if (k.Tipus == KartyaTipus.viz) SelectType.SelectedIndex = 1;
			else if (k.Tipus == KartyaTipus.levego) SelectType.SelectedIndex = 2;
			else if (k.Tipus == KartyaTipus.tuz) SelectType.SelectedIndex = 3;
			AllCardsDict.Add(cardName, k);
			if (AllCardsDict.Count > 1)
			{
				VezerCheck.IsEnabled = true;
			}
			else
			{
				VezerCheck.IsEnabled = false;
			}
			internalEdits = false;
			UpdateKartyaSelectionCard(null, null);

		}
		private void VezerCheck_Checked(object sender, RoutedEventArgs e)
		{
			if (internalEdits)
			{

			}
			else
			{
				internalEdits = true;
				AllCardsDict.Remove(cardEditName);
				for (int i = 0; i < Gyujtemeny.Count; i++)
				{
					if (Gyujtemeny[i].Name == cardEditName)
					{
						Gyujtemeny.RemoveAt(i);
						break;
					}
				}

				SelectedCard_Wrap.Children.Clear();
				BasicCardPanel.Visibility = Visibility.Collapsed;
				LeaderCardPanel.Visibility = Visibility.Visible;
				Gyujtemeny_Check.IsChecked = false;
				Gyujtemeny_Check1.IsChecked = false;
				VezerAlapKartya.ItemsSource = AllCardsDict.Keys;
				VezerAlapKartya.SelectedIndex = 0;
				VezerBonusTipus.ItemsSource = new string[] { "Életerő", "Sebzés" };
				VezerBonusTipus.SelectedIndex = 0;
				foreach (var item in VezerAlapKartya.Items)
				{
					if (item.ToString() == cardEditName)
					{
						VezerAlapKartya.SelectedIndex = 1;
						break;
					}
					break;
				}
				Card k = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].GetCopy();

				k.Vezer = true;
				k.OriginName = VezerAlapKartya.SelectedItem.ToString();
				VezerNev.Text = "Vezér";
				cardEditName = VezerNev.Text + " " + VezerAlapKartya.SelectedItem.ToString();

				AllLeadersDict.Add(cardEditName, k);
				AllLeadersDict[cardEditName].Name = cardEditName;
				SelectedCard_Wrap.Children.Add(k.GetVisual());
				if (VezerBonusTipus.SelectedIndex == 0)
				{
					AllLeadersDict[cardEditName].HP *= 2;

				}
				else
				{
					AllLeadersDict[cardEditName].Damage *= 2;

				}
				AllLeadersDict[cardEditName].UpdateAllVisual();
				SelectedCard_Wrap.Children.Clear();
				SelectedCard_Wrap.Children.Add(AllLeadersDict[cardEditName].GetVisual());
				internalEdits = false;
			}


		}
		private void VezerCheck_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!internalEdits)
			{
				SelectedCard_Wrap.Children.Clear();
				AllLeadersDict.Remove(cardEditName);
				BasicCardPanel.Visibility = Visibility.Visible;
				LeaderCardPanel.Visibility = Visibility.Collapsed;
				Gyujtemeny_Check.IsChecked = false;
				Gyujtemeny_Check1.IsChecked = false;
				for (int i = 0; i < Gyujtemeny.Count; i++)
				{
					if (Gyujtemeny[i].Name == cardEditName)
					{
						Gyujtemeny.RemoveAt(i);
						break;
					}
				}
				string cardName = "ÚjKártya";
				bool goodName = false;
				int bonusIndex = -1;
				while (!goodName)
				{
					bonusIndex++;
					goodName = true;
					foreach (Card item in AllCardsDict.Values)
					{
						if (bonusIndex <= 0)
						{
							if (item.Name == cardName)
							{
								//bonusIndex++;
								goodName = false;
							}
						}
						else if (item.Name == cardName + bonusIndex)
						{
							//bonusIndex++;
							goodName = false;
						}
					}
				}
				if (bonusIndex > 0)
				{
					cardName += bonusIndex;
				}
				Card k = new Card(cardName, 1, 1, "fold", false);
				internalEdits = true;
				cardEditName = k.Name;
				Gyujtemeny_Check.IsChecked = false;
				foreach (Card item in Gyujtemeny)
				{
					if (item.Name == k.Name)
					{
						Gyujtemeny_Check.IsChecked = true;
						break;
					}
				}
				//GoToGrid(KartyaSzerkeszto_Grid);
				KartyaSzerkesztoCardName.Text = k.Name;
				SelectType.ItemsSource = new string[] { "Föld", "Víz", "Levegő", "Tűz" };
				TypeAttack.Text = k.Damage.ToString();
				TypeDefense.Text = k.HP.ToString();
				if (k.Tipus == KartyaTipus.fold) SelectType.SelectedIndex = 0;
				else if (k.Tipus == KartyaTipus.viz) SelectType.SelectedIndex = 1;
				else if (k.Tipus == KartyaTipus.levego) SelectType.SelectedIndex = 2;
				else if (k.Tipus == KartyaTipus.tuz) SelectType.SelectedIndex = 3;
				AllCardsDict.Add(cardName, k);
				internalEdits = false;
				UpdateKartyaSelectionCard(null, null);
			}

		}
		private void VezerAlapKartya_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!internalEdits)
			{
				string edits = VezerNev.Text + " " + VezerAlapKartya.SelectedItem.ToString();
				AllLeadersDict.Remove(cardEditName);
				SelectedCard_Wrap.Children.Clear();
				Card c = AllCardsDict[VezerAlapKartya.SelectedItem.ToString()].GetCopy();
				c.Vezer = true;
				c.Name = edits;
				c.OriginName = VezerAlapKartya.SelectedItem.ToString();
				if (VezerBonusTipus.SelectedIndex == 1)
				{
					c.Damage *= 2;
				}
				else
				{
					c.HP *= 2;
				}
				if ((bool)Gyujtemeny_Check1.IsChecked)
				{
					bool isIngyujt = false;
					foreach (Card item in Gyujtemeny)
					{
						if (item.Name == cardEditName)
						{
							isIngyujt = true;
							break;
						}
					}
					if (!isIngyujt)
					{
						Gyujtemeny.Add(AllLeadersDict[cardEditName].GetCopy());
					}

				}
				else
				{
					for (int i = 0; i < Gyujtemeny.Count; i++)
					{
						if (Gyujtemeny[i].Name == cardEditName)
						{
							Gyujtemeny.RemoveAt(i);
							break;
						}
					}
				}
				c.UpdateAllVisual();
				AllLeadersDict.Add(edits, c);
				SelectedCard_Wrap.Children.Add(c.GetVisual());
				cardEditName = edits;
			}



		}
		private void CreateNewKazamata_Button_Click(object sender, RoutedEventArgs e)
		{
			List<Card> c = new List<Card>();
			string kazName = "új kazamata";
			bool goodName = false;
			int bonusIndex = -1;
			while (!goodName)
			{
				bonusIndex++;
				goodName = true;
				foreach (Kazamata item in AllKazamataDict.Values)
				{
					if (bonusIndex <= 0)
					{
						if (item.Name == kazName)
						{
							//bonusIndex++;
							goodName = false;
						}
					}
					else if (item.Name == kazName + bonusIndex)
					{
						//bonusIndex++;
						goodName = false;
					}
				}
			}
			if (bonusIndex > 0)
			{
				kazName += bonusIndex;
			}
			Kazamata k = new Kazamata(kazName, "egyszeru", "eletero", c);
			AllKazamataDict.Add(kazName, k);
			CreateNewCard_Button.Visibility = Visibility.Collapsed;
			CreateNewKazamata_Button.Visibility = Visibility.Visible;
			MindenKazamata_List.Children.Clear();
			MindenKartya_List.Children.Clear();
			if (sender != null)
			{
				MindenKazamata_List.Visibility = Visibility.Visible;
			}
			foreach (var item in AllKazamataDict.Values)
			{
				Button b = new Button();
				b.Click += EditKazamata;

				b.Content = item.Name;
				b.Margin = new Thickness(10, 0, 0, 0);
				MindenKazamata_List.Children.Add(b);
			}

			AutoResizeButtons(MindenKazamata_List);
			EditKazamata(MindenKazamata_List.Children[MindenKazamata_List.Children.Count - 1], null);
		}
		private void KornyezetMentes_Button_Click(object sender, RoutedEventArgs e)
		{
			if (Save.fileName == null || Save.fileName == "")
			{
				GoToGrid(Save_Grid);
				return;
			}
			Save.Kornyezetrogress();
			Back(null, null);
		}
		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			string kazName = kazamataEditNmae;
			AllKazamataDict.Remove(kazName);
			Kazamata_Button_Click(null, null);
			Back(sender, e);
		}
		private void UpdateKartyaSelectionCards(object sender, SelectionChangedEventArgs e)
		{
			if (!internalEdits)
			{
				if (VezerBonusTipus.SelectedIndex == 0)
				{
					AllLeadersDict[cardEditName].Bonus = "eletero";

				}
				else
				{
					AllLeadersDict[cardEditName].Bonus = "sebzes";
				}
			}
			UpdateKartyaSelectionCard(null, null);

		}
		private void Kazamata_Button_Click(object sender, RoutedEventArgs e)
		{
			KazMent.IsEnabled = false;
			CreateNewCard_Button.Visibility = Visibility.Collapsed;
			CreateNewKazamata_Button.Visibility = Visibility.Visible;
			MindenKazamata_List.Children.Clear();
			MindenKartya_List.Children.Clear();
			if (sender != null)
			{
				MindenKazamata_List.Visibility = Visibility.Visible;
			}
			foreach (var item in AllKazamataDict.Values)
			{
				Button b = new Button();
				b.Click += EditKazamata;

				b.Content = item.Name;
				b.Margin = new Thickness(10, 0, 0, 0);
				MindenKazamata_List.Children.Add(b);
			}

			AutoResizeButtons(MindenKazamata_List);

			/*foreach (var item in AllCardsDict.Values)
            {
                Card c = item.GetCopy();
                MindenKartya_List.Children.Add(c.GetVisual());
                // if (sender!=null)
                {
                    c.Clicked += (s, e) => { SelectForModify(c); };
                }

            }
            foreach (var item in AllLeadersDict.Values)
            {
                Card c = item.GetCopy();
                MindenKartya_List.Children.Add(c.GetVisual());
                //if (sender != null)
                {
                    c.Clicked += (s, e) => { SelectForModify(c); };
                }
                //c.UpdateAllVisual();

            }
            CreateNewCard_Button.Visibility = Visibility.Visible;*/

		}
		public void EditKazamata(object sender, RoutedEventArgs e)
		{
			internalEdits = true;
			GoToGrid(KazamataSzerkeszto_Grid);
			MindenKartya_ListKazamata.Children.Clear();
			KazamataDeffenders_List.Children.Clear();
			Kazamata k = AllKazamataDict[(sender as Button).Content.ToString()];
			KazamataName.Text = k.Name;
			foreach (var item in AllCardsDict.Keys)
			{
				if (k.GetDefenderNames().Contains(item))
				{
					//Card c = AllCardsDict[item].GetCopy(true);
					foreach (Card c in k.Defenders)
					{
						if (c.Name == item)
						{
							c.RightClicked -= RightClick;
							c.RightClicked += RightClick;
							c.Clicked += RemoveFromKazamata;
							KazamataDeffenders_List.Children.Add(c.GetVisual());
							break;
						}
					}

				}
				else
				{
					Card c = AllCardsDict[item].GetCopy(true);
					c.RightClicked -= RightClick;
					//c.RightClicked += RightClick;

					c.Clicked += AddToKazamata;
					MindenKartya_ListKazamata.Children.Add(c.GetVisual());
				}
			}
			int vezerCount = 0;
			foreach (var item in AllLeadersDict.Keys)
			{
				if (k.GetDefenderNames().Contains(item))
				{
					//Card c = AllLeadersDict[item].GetCopy(true);
					foreach (Card c in k.Defenders)
					{
						if (c.Name == item)
						{
							c.RightClicked -= RightClick;
							c.RightClicked += RightClick;
							c.Clicked += RemoveFromKazamata;
							KazamataDeffenders_List.Children.Add(c.GetVisual());
							break;
						}
					}
					vezerCount++;
				}
				else
				{
					Card c = AllLeadersDict[item].GetCopy(true);
					c.RightClicked -= RightClick;
					//c.RightClicked += RightClick;

					c.Clicked += AddToKazamata;
					MindenKartya_ListKazamata.Children.Add(c.GetVisual());
				}
			}
			if (k.Tipus == KazamataType.egyszeru)
			{
				KazamataJutalom.Visibility = Visibility.Visible;
				KazamataJutalom_Label.Visibility = Visibility.Visible;
				KazamataTipus.SelectedIndex = 0;
				KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count}/1";
			}
			else if (k.Tipus == KazamataType.kis)
			{
				KazamataJutalom.Visibility = Visibility.Visible;
				KazamataJutalom_Label.Visibility = Visibility.Visible;
				KazamataTipus.SelectedIndex = 1;
				KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count - vezerCount}/3 és {vezerCount}/1 vezér";

			}
			else
			{
				KazamataJutalom.Visibility = Visibility.Collapsed;
				KazamataJutalom_Label.Visibility = Visibility.Collapsed;
				KazamataTipus.SelectedIndex = 2;
				KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count - vezerCount}/5 és {vezerCount}/1 vezér";

			}
			if (k.reward == KazamataReward.eletero)
			{
				KazamataJutalom.SelectedIndex = 0;
			}
			else if (k.reward == KazamataReward.sebzes)
			{
				KazamataJutalom.SelectedIndex = 1;
			}
			else if (k.reward == KazamataReward.arany)
			{
				KazamataJutalom.SelectedIndex = 2;
			}
			kazamataEditNmae = k.Name;



			internalEdits = false;
		}
		public void UpdateKazamata()
		{
			if (!internalEdits)
			{
				MindenKartya_ListKazamata.Children.Clear();
				KazamataDeffenders_List.Children.Clear();
				Kazamata k = AllKazamataDict[kazamataEditNmae];
				foreach (var item in AllCardsDict.Keys)
				{
					if (k.GetDefenderNames().Contains(item))
					{
						Card c = AllCardsDict[item].GetCopy(true);
						c.RightClicked -= RightClick;
						c.RightClicked += RightClick;

						c.Clicked -= AddToKazamata;
						c.Clicked += RemoveFromKazamata;
						KazamataDeffenders_List.Children.Add(c.GetVisual());
					}
					else
					{
						Card c = AllCardsDict[item].GetCopy(true);
						c.RightClicked -= RightClick;
						//c.RightClicked += RightClick;

						c.Clicked -= RemoveFromKazamata;
						c.Clicked += AddToKazamata;
						MindenKartya_ListKazamata.Children.Add(c.GetVisual());
					}
				}
				int vezerCount = 0;
				foreach (var item in AllLeadersDict.Keys)
				{
					if (k.GetDefenderNames().Contains(item))
					{
						Card c = AllLeadersDict[item].GetCopy(true);
						c.RightClicked -= RightClick;
						c.RightClicked += RightClick;

						c.Clicked -= AddToKazamata;
						c.Clicked += RemoveFromKazamata;
						KazamataDeffenders_List.Children.Add(c.GetVisual());
						vezerCount++;
					}
					else
					{
						Card c = AllLeadersDict[item].GetCopy(true);
						c.RightClicked -= RightClick;
						//c.RightClicked += RightClick;

						c.Clicked -= RemoveFromKazamata;
						c.Clicked += AddToKazamata;
						MindenKartya_ListKazamata.Children.Add(c.GetVisual());
					}
				}



				if (k.Tipus == KazamataType.egyszeru)
				{
					KazamataTipus.SelectedIndex = 0;
					KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count}/1";
				}
				else if (k.Tipus == KazamataType.kis)
				{
					KazamataTipus.SelectedIndex = 1;
					KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count - vezerCount}/3 és {vezerCount}/1 vezér";

				}
				else
				{
					KazamataTipus.SelectedIndex = 2;
					KazamataKartya.Content = $"{KazamataDeffenders_List.Children.Count - vezerCount}/5 és {vezerCount}/1 vezér";

				}
			}


		}
		private void KazamataTipus_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!internalEdits)
			{
				AllKazamataDict[kazamataEditNmae].Tipus = Kazamata.StringToKazamataType(KazamataTipus.SelectedItem.ToString());
				foreach (Card item in AllKazamataDict[kazamataEditNmae].Defenders)
				{
					item.Items.Clear();
				}
				AllKazamataDict[kazamataEditNmae].Defenders.Clear();
				MindenKartya_ListKazamata.Children.Clear();
				KazamataDeffenders_List.Children.Clear();
				Kazamata k = AllKazamataDict[kazamataEditNmae];
				if (k.Tipus == KazamataType.egyszeru)
				{
					KazamataJutalom.Visibility = Visibility.Visible;
					KazamataJutalom_Label.Visibility = Visibility.Visible;
					KazamataTipus.SelectedIndex = 0;
					KazamataKartya.Content = $"{0}/1";
				}
				else if (k.Tipus == KazamataType.kis)
				{
					KazamataJutalom.Visibility = Visibility.Visible;
					KazamataJutalom_Label.Visibility = Visibility.Visible;

					KazamataTipus.SelectedIndex = 1;
					KazamataKartya.Content = $"{0}/3 és {0}/1 vezér";

				}
				else
				{
					KazamataJutalom.Visibility = Visibility.Collapsed;
					KazamataJutalom_Label.Visibility = Visibility.Collapsed;
					AllKazamataDict[kazamataEditNmae].reward = KazamataReward.newcard;
					KazamataTipus.SelectedIndex = 2;
					KazamataKartya.Content = $"{0}/5 és {0}/1 vezér";

				}
				UpdateKazamata();

			}

		}
		private void AddToKazamata(object? sender, Card clicked)
		{

			int vezer = 0;
			int kazamataDef = KazamataDeffenders_List.Children.Count;
			int kazamataMaxDef = 0;
			foreach (Card item in AllKazamataDict[kazamataEditNmae].Defenders)
			{
				if (item.Vezer)
				{
					vezer++;
				}
			}
			if (AllKazamataDict[kazamataEditNmae].Tipus == KazamataType.egyszeru)
			{
				kazamataMaxDef = 1;
			}
			else if (AllKazamataDict[kazamataEditNmae].Tipus == KazamataType.kis)
			{
				kazamataMaxDef = 3;
			}
			else
			{
				kazamataMaxDef = 5;
			}
			if (clicked.Vezer && vezer == 0 && AllKazamataDict[kazamataEditNmae].Tipus != KazamataType.egyszeru)
			{
				vezer++;
				kazamataDef++;
				clicked.Clicked -= AddToKazamata;
				clicked.Clicked += RemoveFromKazamata;
				AllKazamataDict[kazamataEditNmae].Defenders.Add(clicked);
				UpdateKazamata();
			}
			else if (!clicked.Vezer && kazamataDef - vezer < kazamataMaxDef)
			{
				kazamataDef++;
				clicked.Clicked -= AddToKazamata;
				clicked.Clicked += RemoveFromKazamata;
				AllKazamataDict[kazamataEditNmae].Defenders.Add(clicked);
				UpdateKazamata();
			}
			if (AllKazamataDict[kazamataEditNmae].Tipus != KazamataType.egyszeru && (vezer == 1 && kazamataDef - vezer == kazamataMaxDef))
			{
				KazMent.IsEnabled = true;
			}
			else if (AllKazamataDict[kazamataEditNmae].Tipus == KazamataType.egyszeru && kazamataDef - vezer == kazamataMaxDef)
			{
				KazMent.IsEnabled = true;

			}
			else
			{
				KazMent.IsEnabled = false;

			}


		}
		private void RemoveFromKazamata(object? sender, Card clicked)
		{


			clicked.Clicked += AddToKazamata;
			clicked.Clicked -= RemoveFromKazamata;
			//AllKazamataDict[kazamataEditNmae].Defenders.Remove(clicked);
			foreach (Card item in AllKazamataDict[kazamataEditNmae].Defenders)
			{
				if (item.Name == clicked.Name)
				{
					AllKazamataDict[kazamataEditNmae].Defenders.Remove(item);
					break;
				}
			}
			UpdateKazamata();



			KazMent.IsEnabled = false;


		}
		private void KazamataName_LostFocus(object sender, RoutedEventArgs e)
		{
			if (!internalEdits)
			{
				string edits = KazamataName.Text;
				Kazamata k = AllKazamataDict[kazamataEditNmae];
				k.Name = edits;
				AllKazamataDict.Remove(kazamataEditNmae);
				AllKazamataDict.Add(edits, k);

				kazamataEditNmae = edits;
			}
		}
		private void KazamataName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
			{
				if (!internalEdits)
				{
					string edits = KazamataName.Text;
					Kazamata k = AllKazamataDict[kazamataEditNmae];
					k.Name = edits;
					AllKazamataDict.Remove(kazamataEditNmae);
					AllKazamataDict.Add(edits, k);

					kazamataEditNmae = edits;
				}
			}
		}
		private void KazamataJutalom_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!internalEdits)
			{
				if (KazamataJutalom.SelectedIndex == 0)
				{
					AllKazamataDict[kazamataEditNmae].reward = KazamataReward.eletero;
				}
				else if (KazamataJutalom.SelectedIndex == 1)
				{
					AllKazamataDict[kazamataEditNmae].reward = KazamataReward.sebzes;
				}
				else
				{
					AllKazamataDict[kazamataEditNmae].reward = KazamataReward.arany;
				}
			}
		}
	}
}
