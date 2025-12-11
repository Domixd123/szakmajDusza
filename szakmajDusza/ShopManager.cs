
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
		private void GoToShop_Button_Click(object sender, RoutedEventArgs e)
		{

			All_Vezer_Obtained.Visibility = Visibility.Collapsed;
			CardMerge_Wrap.Children.Clear();
			Shop_Merging_Cards.Children.Clear();
			GoToGrid(Shop_Grid);
			int vezCount = 0;
			foreach (Card item in Gyujtemeny)
			{

				if (item.Vezer)
				{
					vezCount++;
				}
			}
			if (vezCount == AllLeadersDict.Count)
			{
				Shop_Merge.IsEnabled = false;
				All_Vezer_Obtained.Visibility = Visibility.Visible;
				Obtained_Label.Visibility = Visibility.Collapsed;
			}
			else
			{
				foreach (var item in Gyujtemeny)
				{
					var card = item.GetCopy();
					/*bool found = false;
					foreach (var item2 in Jatekos)
					{
						if (item.Name==item2.Name)
						{
							found = true;
							break;
						}
					}*/
					if (card.Vezer || Jatekos.Contains(item as Card))
					{

						card.Disabled = true;
						card.UpdateAllVisual();
						CardMerge_Wrap.Children.Add(card.GetVisual());
					}
					else
					{
						card.Clicked += CardToMerge_Card_Click;
						CardMerge_Wrap.Children.Add(card.GetVisual());
					}
				}
			}
			bool isShopEmpty = true;
			foreach (var item in Item.Items.Values)
			{
				if (item.InRotation)
				{
					isShopEmpty = false;
					break;
				}
			}
			if (isShopEmpty)
			{
				Item.RefreshShop(true);


			}


			ShopRerollPrice_Label.Content = $"Ár: {Item.shopRefreshPrice}";
			if (Item.shopRefreshPrice > Item.GoldOwned)
			{
				ShopRerollPrice_Label.Foreground = Brushes.Red;
				Shop_Refresh.IsEnabled = false;
			}
			else
			{
				ShopRerollPrice_Label.Foreground = Brushes.LightGreen;
			}
			UpdateGoldOwnedLabel();
			UpdateShopWrapChildren();

		}
		public void UpdateShopWrapChildren()
		{
			Shop_Wrap.Children.Clear();
			bool isAnythingBuyable = false;
			foreach (var item in Item.Items.Values)
			{
				if (item.InRotation)
				{
					item.Clicked -= BuyItem;
					item.Clicked += BuyItem;
					if (item.Price > Item.GoldOwned)
					{
						item.Clicked -= BuyItem;

					}
					item.UpdateAllVisual();
					Shop_Wrap.Children.Add(item.GetVisual(true));
					isAnythingBuyable = true;
				}
			}
			if (isAnythingBuyable) AllItemsMaxedError_Label.Visibility = Visibility.Collapsed;
			else AllItemsMaxedError_Label.Visibility = Visibility.Visible;
		}
		public void UpdateGoldOwnedLabel()
		{
			GoldOwned_Label.Content = $"Arany: {Item.GoldOwned}";
		}
		public void BuyItem(object? sender, Item selected)
		{
			//kristof do anymation here
			selected.Buy();
			UpdateShopWrapChildren();
			UpdateGoldOwnedLabel();
		}
		private void RefreshShop_Button_Click(object sender, RoutedEventArgs e)
		{
			Shop_Wrap.Children.Clear();
			Item.RefreshShop(false);

			foreach (var item in Item.Items.Values)
			{
				if (item.InRotation)
				{
					Shop_Wrap.Children.Add(item.GetVisual(true));
				}
			}

			UpdateGoldOwnedLabel();
			UpdateShopWrapChildren();
		}
		private void CardToMerge_Card_Click(object? sender, Card clicked)
		{
			//clicked.GetVisual().IsEnabled = false;
			for (int i = 0; i < Jatekos.Count; i++)
			{
				if (Jatekos[i].Name == clicked.Name)
				{
					//Implement error mnessage
					return;
				}
			}
			if (Shop_Merging_Cards.Children.Count < 3)
			{
				CardMerge_Wrap.Children.Remove(clicked.GetVisual());
				clicked.Clicked -= CardToMerge_Card_Click;
				clicked.Clicked += CardToMergeRemove_Card_Click;
				Shop_Merging_Cards.Children.Add(clicked.GetVisual());
				Merging.Add(clicked);

			}
			if (Shop_Merging_Cards.Children.Count == 3)
			{
				Shop_Merge.IsEnabled = true;
			}
			//Button b = sender as Button;

		}
		private void CardToMergeRemove_Card_Click(object? sender, Card clicked)
		{

			Shop_Merging_Cards.Children.Remove(clicked.GetVisual());
			CardMerge_Wrap.Children.Add(clicked.GetVisual());
			clicked.Clicked -= CardToMergeRemove_Card_Click;
			clicked.Clicked += CardToMerge_Card_Click;
			Merging.Remove(clicked);


			Shop_Merge.IsEnabled = false;

			//Button b = sender as Button;

		}
		private void Merge_To_Vezer(object sender, RoutedEventArgs e)
		{
			MergeToVezet.haromToVezer(Merging[0], Merging[1], Merging[2]);
			Shop_Merge.IsEnabled = false;
			Gyujtemeny[Gyujtemeny.Count - 1].Clicked += AddToPakli;
			Gyujtemeny[Gyujtemeny.Count - 1].RightClicked += RightClick;

			Shop_Merging_Cards.Children.Clear();
			Cards_Wrap.Children.Clear();
			//DynamicButtonsPanel.Children.Clear();
			PlayerCards_Wrap.Children.Clear();
			PakliCards_Wrap.Children.Clear();
			foreach (Card item in Gyujtemeny)
			{
				bool found = false;
				foreach (var item2 in Jatekos)
				{
					if (item.Name == item2.Name)
					{
						found = true;
						break;
					}
				}
				if (!found)
				{
					Cards_Wrap.Children.Add(item.GetVisual());
					/*item.Clicked -= RemoveFromPakli;
                    item.Clicked += AddToPakli;*/
				}
				else
				{
					PlayerCards_Wrap.Children.Add(item.GetVisual());
					/*item.Clicked += RemoveFromPakli;
                    item.Clicked -= AddToPakli;*/
				}

			}
			//Gyujtemeny[Gyujtemeny.Count - 1].Clicked += AddToPakli;
			Card card = Gyujtemeny[Gyujtemeny.Count - 1].GetCopy();
			/*card.RightClicked -= RightClick;
            card.RightClicked += RightClick;
            Card c = card.GetCopy();*/

			card.Disabled = true;
			card.UpdateAllVisual();
			CardMerge_Wrap.Children.Add(card.GetVisual());


			SelectableCounter_Label.Content = $"/ {Math.Ceiling((float)Gyujtemeny.Count / 2f)}";
			int vezCount = 0;
			foreach (Card item in Gyujtemeny)
			{

				if (item.Vezer)
				{
					vezCount++;
				}
			}
			if (vezCount == AllLeadersDict.Count)
			{
				Shop_Merge.IsEnabled = false;
				Obtained_Label.Visibility = Visibility.Collapsed;
				All_Vezer_Obtained.Visibility = Visibility.Visible;
				CardMerge_Wrap.Children.Clear();
			}
			Merging.Clear();
		}
	}
}
