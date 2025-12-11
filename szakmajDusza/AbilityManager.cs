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
		private void RightClick(object sender, Card c)
		{
			if (!editor)
			{
				lastCard = c;
				SelectedCardForAbility.Children.Clear();
				Ability_Wrap.Children.Clear();
				GoToGrid(AddAbility_grid);
				SelectedCardForAbility.Children.Add(c.GetCopy().GetVisual());
				foreach (var item in Item.Items.Values)
				{
					if (item.OwnedCount > 0)
					{
						Item i = item;
						i.Clicked -= AddAbility;

						i.Clicked += AddAbility;
						Ability_Wrap.Children.Add(i.GetVisual(false));
					}
				}
			}
			else
			{
				lastCard = c;
				SelectedCardForAbility.Children.Clear();
				Ability_Wrap.Children.Clear();
				GoToGrid(AddAbility_grid);
				SelectedCardForAbility.Children.Add(c.GetCopy().GetVisual());
				foreach (var item in Item.Items.Values)
				{

					Item i = item;
					i.Clicked -= AddAbility;

					i.Clicked += AddAbility;
					Ability_Wrap.Children.Add(i.GetVisual(false));

				}
			}

		}
		private void AddAbility(object sender, Item item)
		{


			if (!editor)
			{
				Card c = lastCard;
				if (!c.Items.Contains(item))
				{
					if (c.Vezer && c.Items.Count <= 1)
					{
						c.Items.Add(item);
						c.UpdateAllVisual();
					}

					else if (!c.Vezer && c.Items.Count == 0)
					{
						c.Items.Add(item);
						c.UpdateAllVisual();
					}
					else
					{
						c.Items.Remove(c.Items[0]);
						c.Items.Add(item);
						c.UpdateAllVisual();
					}
				}
				else
				{
					c.Items.Remove(item);
					c.UpdateAllVisual();
				}

				item.Clicked -= AddAbility;

				SelectedCardForAbility.Children.Clear();
				Ability_Wrap.Children.Clear();

				Back(null, null);
				PlayerCards_Wrap.Children.Clear();
				Cards_Wrap.Children.Clear();

				foreach (var a in Gyujtemeny)
				{
					bool found = false;
					foreach (var a2 in Jatekos)
					{
						if (a.Name == a2.Name)
						{
							found = true;
							break;
						}
					}
					if (!found)
					{
						Cards_Wrap.Children.Add(a.GetVisual());
					}

				}
				foreach (var a in Jatekos)
				{
					PlayerCards_Wrap.Children.Add(a.GetVisual());
				}
			}
			else
			{
				Card c = lastCard;
				bool found = false;
				foreach (var x in c.Items)
				{
					if (x.Name == item.Name)
					{
						found = true;
					}
				}
				if (!found)
				{
					if (c.Vezer && c.Items.Count <= 1)
					{
						string e = Interaction.InputBox($"Add meg a szintet 1 és {item.MaxLevel} között", "Add meg a szintet", "0", 200, 200);
						while (!int.TryParse(e, out int number) || number < 1 || number > item.MaxLevel)
						{
							e = Interaction.InputBox($"Add meg a szintet 1 és {item.MaxLevel} között", "Add meg a szintet", "0", 200, 200);
						}
						Item i = item.GetCopy();
						i.Level = int.Parse(e);

						c.Items.Add(i);
						c.UpdateAllVisual();
					}

					else if (!c.Vezer && c.Items.Count == 0)
					{
						string e = Interaction.InputBox($"Add meg a szintet 1 és {item.MaxLevel} között", "Add meg a szintet", "0", 200, 200);
						while (!int.TryParse(e, out int number) || number < 1 || number > item.MaxLevel)
						{
							e = Interaction.InputBox($"Add meg a szintet 1 és {item.MaxLevel} között", "Add meg a szintet", "0", 200, 200);
						}
						Item i = item.GetCopy();
						i.Level = int.Parse(e);

						c.Items.Add(i);
						c.UpdateAllVisual();
					}
					else
					{
						string e = Interaction.InputBox($"Add meg a szintet 1 és {item.MaxLevel} között", "Add meg a szintet", "0", 200, 200);
						while (!int.TryParse(e, out int number) || number < 1 || number > item.MaxLevel)
						{
							e = Interaction.InputBox($"Add meg a szintet 1 és {item.MaxLevel} között", "Add meg a szintet", "0", 200, 200);
						}
						Item i = item.GetCopy();
						c.Items.Remove(c.Items[0]);
						c.Items.Add(i);
						c.UpdateAllVisual();
					}
				}
				else
				{

					c.Items.Remove(item);
					c.UpdateAllVisual();
				}
				item.Clicked -= AddAbility;

				SelectedCardForAbility.Children.Clear();
				Ability_Wrap.Children.Clear();


				//popup for level
				Back(null, null);

				KazamataDeffenders_List.Children.Clear();
				MindenKartya_ListKazamata.Children.Clear();
				Kazamata k = AllKazamataDict[kazamataEditNmae];
				foreach (Card ck in k.Defenders)
				{
					if (ck.Name == lastCard.Name)
					{
						ck.Items = lastCard.Items;
					}
				}
				foreach (var item2 in AllCardsDict.Keys)
				{
					if (k.GetDefenderNames().Contains(item2))
					{
						//Card c = AllCardsDict[item].GetCopy(true);
						foreach (Card ck in k.Defenders)
						{
							if (ck.Name == item2)
							{
								ck.RightClicked -= RightClick;
								ck.RightClicked += RightClick;
								ck.Clicked += RemoveFromKazamata;
								ck.UpdateAllVisual();

								KazamataDeffenders_List.Children.Add(ck.GetVisual());
								break;
							}
						}

					}
					else
					{
						Card ck = AllCardsDict[item2].GetCopy(true);
						c.RightClicked -= RightClick;
						//c.RightClicked += RightClick;

						ck.Clicked += AddToKazamata;
						MindenKartya_ListKazamata.Children.Add(ck.GetVisual());
					}
				}
				int vezerCount = 0;
				foreach (var item2 in AllLeadersDict.Keys)
				{
					if (k.GetDefenderNames().Contains(item2))
					{
						//Card c = AllLeadersDict[item].GetCopy(true);
						foreach (Card ck in k.Defenders)
						{
							if (ck.Name == item2)
							{
								ck.RightClicked -= RightClick;
								ck.RightClicked += RightClick;
								ck.Clicked += RemoveFromKazamata;
								ck.UpdateAllVisual();
								KazamataDeffenders_List.Children.Add(ck.GetVisual());

								break;
							}
						}
						vezerCount++;
					}
					else
					{
						Card ck = AllLeadersDict[item2].GetCopy(true);
						ck.RightClicked -= RightClick;
						//c.RightClicked += RightClick;

						ck.Clicked += AddToKazamata;
						MindenKartya_ListKazamata.Children.Add(ck.GetVisual());
					}
				}
			}

		}
	}
}
