using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace szakmajDusza
{
	public class Item
	{
		public static int GoldOwned = 100000;
		public static int shopItemCount = 3;
		public static int shopRefreshPrice = 2;
		public static Dictionary<string, Item> Items = new Dictionary<string, Item>()
		{
			{ "Életerőlopás",new Item("Életerőlopás","(Szint*5)% életerőt elvesz ellenfelétől és magát gyógyítja ugyanennyivel",true,5,5)},
			{"Gyógyítás", new Item("Gyógyítás","(Szint*2) életerővel gyógyítja magát minden kör végén",true,5,5) },
			{"Erő", new Item("Erő","(Szint*2)-vel erősebben üt",true,5,5) },
			{"Páncél", new Item("Páncél","(Szint*8)%-kal kevesebbet sebződik",true,5,5) },
			{"Újraéledés", new Item("Újraéledés","(Szint*15)% eséllyel újraéled, ez minden újraéledés után csökken 15%-kal",true,5,5) },
			{"Krit ütés", new Item("Krit ütés","(Szint*10)% eséllyel a támadása (Szint*40)%-kal többet sebez",true,5,5) },
			{"Tüskék", new Item("Tüskék","A kapott sebzés (Szint*5)%-át visszaüti ellenfelére (ez csak Mágikussal hárítható)",true,5,5) },
			{"Kikerülés", new Item("Kikerülés","(Szint*5)% eséllyel immunis lesz az ellenfél következő támadására",true,5,5) },
			{"Mágikus", new Item("Mágikus","(Szint*8)% eséllyel blokkolja az ellenfél következő képességét (kivétel a Mágikus-t)",true,5,5) },
		};
		public bool Disabled {  get; set; }=false;
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Buyable { get; set; }
		public bool InRotation { get; set; }
		public int Price { get; set; }
		public int OwnedCount { get; set; }
		public int Level { get; set; }
		public int MaxLevel { get; set; }
		public Rectangle Rec { get; private set; }
		public Button But { get; private set; }

		public Label NameLabel;
		public Label Name2Label;
		public Label LevelLabel;
		public Label MaxLevelLabel;
		public Label PriceLabel;
		public Label InfoLabel;      // price / gold  VAGY  level / maxlevel
		public Label OwnedLabel;     // ownedcount
		public Label DescLabel;      // description hover alatt
		public Label NewLabel;
		public Label LevelUpLabel;
		public Border border;

		public Grid visualGroup;

		public event EventHandler<Item> Clicked;
		public Item(string name, string description, bool buyable, int maxlevel, int price, int level = 0, int ownedcount = 0, bool inRotation = false)
		{
			Name = name;
			Description = description;
			Buyable = buyable;
			MaxLevel = maxlevel;
			Price = price;
			Level = level;
			OwnedCount = ownedcount;
			InRotation = inRotation;
		}
		public void Buy()
		{
			GoldOwned -= Price;
			OwnedCount++;
			InRotation = false;
			//UpdateAllVisual();
			bool isShopEmpty = true;
			foreach (var item in Items.Values)
			{
				if (item.InRotation)
				{
					isShopEmpty = false;
					break;
				}
			}
			if(isShopEmpty)RefreshShop(true);
		}
		public static void RefreshShop(bool isFree)
		{
			if (!isFree)
			{
				GoldOwned -= shopRefreshPrice;
			}
			List<string> itemPool = new List<string>();
			foreach (var item in Items.Values)
			{
				item.TryLevelUp();
				if (item.Buyable && !item.InRotation && item.Level != item.MaxLevel)
				{
					itemPool.Add(item.Name);
				}
				if (item.InRotation) item.InRotation = false;
			}
			Random r = new Random();
			int itemPoolCount=itemPool.Count;
			for (int i = 0; i < Math.Min(itemPoolCount, shopItemCount); i++)
			{
				int randomID = r.Next(0, itemPool.Count);
				Items[itemPool[randomID]].InRotation = true;
				itemPool.RemoveAt(randomID);
			}
			if (itemPoolCount<shopItemCount)
			{
				int needed=shopItemCount-itemPoolCount;
				itemPool = new List<string>();
				foreach (var item in Items.Values)
				{
					if (item.Buyable && !item.InRotation && item.Level != item.MaxLevel)
					{
						itemPool.Add(item.Name);
					}
				}
				for (int i = 0; i < Math.Min(itemPool.Count, needed); i++)
				{
					int randomID = r.Next(0, itemPool.Count);
					Items[itemPool[randomID]].InRotation = true;
					itemPool.RemoveAt(randomID);
				}
			}
		}
		public static int LevelUpRequirement(int level)
		{
			return (int)Math.Pow(1.5, level);
		}
		public int CurrentLevelOwned()
		{
			int owned = OwnedCount;
			int x = 1;
			while (owned > LevelUpRequirement(x))
			{
				owned -= LevelUpRequirement(x);
				x++;
			}
			return owned;
		}
		public void TryLevelUp()
		{
			if(OwnedCount==0) return;
			int owned = OwnedCount;
			int x = 1;
			while (owned > LevelUpRequirement(x))
			{
				owned -= LevelUpRequirement(x);
				x++;
			}
			Level = x;
			UpdateAllVisual();
		}
		public Item GetCopy()
		{
			return new Item(Name, Description, Buyable, MaxLevel, Price, Level, OwnedCount, InRotation);
		}
		public void UpdateAllVisual()
		{
			visualGroup = new Grid
			{
				Width = 140,
				Height = 180,
				Margin = new Thickness(10),

			};
			visualGroup.Effect = new System.Windows.Media.Effects.DropShadowEffect
			{
				BlurRadius = 15,
				ShadowDepth = 5,
				Color = Colors.Black,
				Opacity = 0.7
			};
			visualGroup.MouseEnter += (s, e) =>
			{
				if (NewLabel != null) NewLabel.Visibility = Visibility.Collapsed;
				visualGroup.Background = new LinearGradientBrush(
					Color.FromRgb(92, 71, 161),
					Color.FromRgb(58, 42, 96),
					90);

				// --- UI elrejtése hover alatt ---
				if (InfoLabel != null) InfoLabel.Visibility = Visibility.Collapsed;
				if (OwnedLabel != null) OwnedLabel.Visibility = Visibility.Collapsed;
				if (LevelLabel != null) LevelLabel.Visibility = Visibility.Collapsed;
				if (MaxLevelLabel != null) MaxLevelLabel.Visibility = Visibility.Collapsed;

				// csak name + description
				if (DescLabel != null) DescLabel.Visibility = Visibility.Visible;
			};

			visualGroup.MouseLeave += (s, e) =>
			{
				if (NewLabel != null) NewLabel.Visibility = Visibility.Visible;
				visualGroup.Background = new LinearGradientBrush(
					Color.FromRgb(25, 20, 30),
					Color.FromRgb(45, 35, 55),
					90);

				// --- UI visszaállítása ---
				if (InfoLabel != null) InfoLabel.Visibility = Visibility.Visible;
				if (OwnedLabel != null) OwnedLabel.Visibility = Visibility.Visible;
				if (LevelLabel != null) LevelLabel.Visibility = Visibility.Visible;
				if (MaxLevelLabel != null) MaxLevelLabel.Visibility = Visibility.Visible;

				if (DescLabel != null) DescLabel.Visibility = Visibility.Collapsed;
			};


			// keret
			border = new Border
			{
				CornerRadius = new CornerRadius(12),
				BorderBrush = new LinearGradientBrush(
					Color.FromRgb(255, 215, 100),
					Color.FromRgb(180, 120, 30),
					45),
				BorderThickness = new Thickness(3),
				Background = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255))
			};
			visualGroup.Children.Add(border);

			NameLabel = new Label
			{
				Content = Name.Split(' ')[0],
				Foreground = Brushes.Gold,
				FontWeight = FontWeights.Bold,
				FontSize = 16,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top,
				Margin = new Thickness(10, 8, 0, 0),
				IsHitTestVisible = false,
				MaxWidth = 120,                // max szélesség
				HorizontalContentAlignment = HorizontalAlignment.Center,

			};
			if (Name.Split(' ').Length > 1)
			{
				Name2Label = new Label
				{

					Content = Name.Split(' ')[1],
					Foreground = Brushes.Gold,
					FontWeight = FontWeights.Bold,
					FontSize = 16,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Top,
					Margin = new Thickness(10, 24, 0, 0),
					IsHitTestVisible = false,
					MaxWidth = 90,                // max szélesség
					HorizontalContentAlignment = HorizontalAlignment.Center,

				};
				visualGroup.Children.Add(Name2Label);
			}
			if (Items[Name].Level == 0)
			{
				border.BorderBrush = new LinearGradientBrush(
		Color.FromRgb(180, 80, 255),   // világos lila
		Color.FromRgb(110, 40, 180),   // sötétebb lila
		45);

				border.BorderThickness = new Thickness(4);

				visualGroup.Background = new LinearGradientBrush(
					Color.FromRgb(45, 35, 60),
					Color.FromRgb(80, 60, 100),
					90);

				// NEW címke
				NewLabel = new Label
				{
					Content = "NEW",
					Foreground = Brushes.Gold,
					FontWeight = FontWeights.ExtraBold,
					FontSize = 18,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
					IsHitTestVisible = false
				};


				visualGroup.Children.Add(NewLabel);
			}
			/*else if(LevelUpRequirement(Level+1)==CurrentLevelOwned()+1)
			{
				border.BorderBrush = new LinearGradientBrush(
	Color.FromRgb(100, 180, 255),   // világos kék
	Color.FromRgb(30, 90, 180),     // sötétebb kék
	45);
				border.BorderThickness = new Thickness(4);

				visualGroup.Background = new LinearGradientBrush(
					Color.FromRgb(45, 35, 60),
					Color.FromRgb(80, 60, 100),
					90);
				LevelUpLabel = new Label
				{
					Content = "Level Up",
					Foreground = Brushes.Gold,
					FontWeight = FontWeights.ExtraBold,
					FontSize = 18,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
					IsHitTestVisible = false
				};
				visualGroup.Children.Add(LevelUpLabel);
			}*/
				But = new Button
				{
					Background = Brushes.Transparent,
					BorderThickness = new Thickness(0),
					Cursor = Cursors.Hand,

				};
			But.Click += (sender, e) => Clicked?.Invoke(this, this);
			if (Disabled)
			{
				visualGroup.Background = new LinearGradientBrush(Color.FromRgb(10, 5, 15), Color.FromRgb(30, 20, 40), 90);
			}
			//visualGroup.Children.Add(ellipse);
			visualGroup.Children.Add(NameLabel);
			//visualGroup.Children.Add(DamageAndHPLabel);
			visualGroup.Children.Add(But);
		}
		public UIElement GetVisual(bool shopMode)
		{
			UpdateAllVisual();
			// név már létre lett hozva az UpdateAllVisual-ban
			// Itt hozzuk létre a shop vagy non-shop info label-eket

			// Description (hover alatt jelenjen meg)
			DescLabel = new Label
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(5),
				Visibility = Visibility.Collapsed,
				MaxWidth = 120,
				IsHitTestVisible = false
			};

			DescLabel.Content = new TextBlock
			{
				Text = Description,
				TextWrapping = TextWrapping.Wrap,
				Foreground = Brushes.White,
				FontSize = 12,
				IsHitTestVisible = false,
				TextAlignment = TextAlignment.Center
			};

			visualGroup.Children.Add(DescLabel);


			if (shopMode)
			{
				// ár kiírása
				InfoLabel = new Label
				{
					FontSize = 16,
					Content = $"Ár: {Price}",
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Bottom,
					Margin = new Thickness(0, 0, 0, 10),
					FontWeight = FontWeights.Bold,
					IsHitTestVisible = false,
					Foreground = (GoldOwned >= Price) ? Brushes.LightGreen : Brushes.Red
				};

				visualGroup.Children.Add(InfoLabel);
				if (LevelUpRequirement(Level + 1) == CurrentLevelOwned() + 1)
				{
					border.BorderBrush = new LinearGradientBrush(
		Color.FromRgb(100, 180, 255),   // világos kék
		Color.FromRgb(30, 90, 180),     // sötétebb kék
		45);
					border.BorderThickness = new Thickness(4);

					visualGroup.Background = new LinearGradientBrush(
						Color.FromRgb(45, 35, 60),
						Color.FromRgb(80, 60, 100),
						90);
					LevelUpLabel = new Label
					{
						Content = "Level Up",
						Foreground = Brushes.Gold,
						FontWeight = FontWeights.ExtraBold,
						FontSize = 18,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center,
						IsHitTestVisible = false
					};
					visualGroup.Children.Add(LevelUpLabel);
				}
			}
			else
			{
				// level / maxlevel
				InfoLabel = new Label
				{
					FontSize = 10,
					Content = $"Szint: {Level} / {MaxLevel}",
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Bottom,
					Margin = new Thickness(0, 0, 0, 28),
					FontWeight = FontWeights.Bold,
					IsHitTestVisible = false,
					Foreground = Brushes.Gold
				};
				visualGroup.Children.Add(InfoLabel);

				// ownedcount
				OwnedLabel = new Label
				{
					FontSize = 12,
					Content = $"Szintlépés: {OwnedCount} / {LevelUpRequirement(Level + 1)}",
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Bottom,
					Margin = new Thickness(0, 0, 0, 8),
					IsHitTestVisible = false,
					Foreground = Brushes.LightGray
				};
				visualGroup.Children.Add(OwnedLabel);
			}

			return visualGroup;
		}
	}
}
