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
	}
}
