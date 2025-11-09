using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace szakmajDusza
{
    public class Card
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int HP { get; set; }
        public KartyaTipus Tipus { get; set; }
        public bool Vezer { get; set; }

        public Rectangle Rec { get; private set; }
        public Button But { get; private set; }

        public Label NameLabel;
        public Label Name2Label;
        public Label DamageAndHPLabel;
        private Label TypeLabel;

        public Grid visualGroup;

        public event EventHandler<Card> Clicked;

        public Card(string n, int d, int h, string tipus, bool vezer)
        {
            Name = n;
            Damage = d;
            HP = h;
            Vezer = vezer;

            Tipus=StringToTipus(tipus);


            visualGroup = new Grid
            {
                Width = 140,
                Height = 180,
                Margin = new Thickness(10),
                /*Background = new LinearGradientBrush(
        Color.FromRgb(25, 20, 30),
        Color.FromRgb(45, 35, 55),
        90)*/
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
                visualGroup.Background = new LinearGradientBrush(
                    Color.FromRgb(92, 71, 161),
                    Color.FromRgb(58, 42, 96),
                    90);
                visualGroup.Effect = new DropShadowEffect
                {
                    BlurRadius = 25,
                    ShadowDepth = 2,
                    Color = Color.FromRgb(217, 166, 0),
                    Opacity = 0.9
                };
            };

            visualGroup.MouseLeave += (s, e) =>
            {
                visualGroup.Background = new LinearGradientBrush(
                    Color.FromRgb(25, 20, 30),
                    Color.FromRgb(45, 35, 55),
                    90);
                visualGroup.Effect = new DropShadowEffect
                {
                    BlurRadius = 15,
                    ShadowDepth = 5,
                    Color = Colors.Black,
                    Opacity = 0.7
                };
            };

            // keret
            var border = new Border
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

            // típus színezés
            Brush typeColor = Tipus switch
            {
                KartyaTipus.tuz => new SolidColorBrush(Color.FromRgb(200, 50, 30)),
                KartyaTipus.fold => new SolidColorBrush(Color.FromRgb(60, 140, 70)),
                KartyaTipus.viz => new SolidColorBrush(Color.FromRgb(50, 100, 200)),
                KartyaTipus.levego => new SolidColorBrush(Color.FromRgb(180, 200, 255)),
                _ => Brushes.Gray
            };

            // cím
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
                MaxWidth = 90,                // max szélesség
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
            


            // statok
            DamageAndHPLabel = new Label
            {
                Content = $"{Damage} ⚔ / {HP} ❤",
                Foreground = Brushes.WhiteSmoke,
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 10),
                IsHitTestVisible = false
            };

            // típus indikátor
            var ellipse = new Ellipse
            {

                Width = 30,
                Height = 30,
                Fill = typeColor,
                Stroke = Brushes.Gold,
                StrokeThickness = 1.5,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10, 0, 0),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 5,
                    Color = Colors.Black,
                    Opacity = 0.6
                },
                IsHitTestVisible = false
            };
            var brush = new ImageBrush();
            switch (Tipus)
            {
                case KartyaTipus.fold:
                    brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images\\earth.jpg"));

                    break;
                case KartyaTipus.levego:
                    brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images\\wind.jpg"));

                    break;
                case KartyaTipus.tuz:
                    brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images\\fire.jpg"));

                    break;
                case KartyaTipus.viz:
                    brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images\\water.jpg"));

                    break;
                
            }
            brush.Stretch = Stretch.UniformToFill;

            ellipse.Fill = brush;

            // vezér aura
            if (Vezer)
            {
                border.BorderBrush = new LinearGradientBrush(
                    Color.FromRgb(255, 220, 100),
                    Color.FromRgb(255, 255, 200),
                    45);
                border.BorderThickness = new Thickness(4);
                visualGroup.Background = new LinearGradientBrush(
                    Color.FromRgb(45, 35, 60),
                    Color.FromRgb(80, 60, 100),
                    90);
            }

            // gomb (láthatatlan, de kattintható)
            But = new Button
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                
            };
            But.Click += (sender, e) => Clicked?.Invoke(this, this);

            visualGroup.Children.Add(ellipse);
            visualGroup.Children.Add(NameLabel);
            visualGroup.Children.Add(DamageAndHPLabel);
            visualGroup.Children.Add(But);
        }

        public void UpdateVisual()
        {
            // Frissítjük a statisztikát kiíró címkét
            DamageAndHPLabel.Content = $"{Damage} ⚔ / {HP} ❤";

            // Opcionális: ha a HP <= 0, halványítsuk el a kártyát
            if (HP <= 0)
            {
                visualGroup.Opacity = 0.4;
            }
        }
		public async Task UpdateVisualDamage(int dmg)
		{
			// Frissítjük a statisztikát kiíró címkét
			DamageAndHPLabel.Content = $"{Damage} ⚔ / {HP+dmg} - {dmg} ❤";
			// Opcionális: ha a HP <= 0, halványítsuk el a kártyát
			/*if (HP <= 0)
			{
				visualGroup.Opacity = 0.4;
                DamageAndHPLabel.Content = $"{Damage} ⚔ / 0 ❤";

            }*/
		}


		public UIElement GetVisual()
        {
            return visualGroup;
        }

        public bool Equals(Card? obj)
        {
            if (obj.Name == this.Name && obj.Damage == this.Damage && obj.HP == this.HP && obj.Tipus == this.Tipus)
            {
                return true;
            }
            return false;
        }

        
        public void SetPosition(double x, double y)
        {
            Canvas.SetLeft(visualGroup, x);
            Canvas.SetTop(visualGroup, y);
        }
		public static string TipusToString(KartyaTipus tipus)
		{
			string Tipus;
			Tipus = tipus switch
			{
				KartyaTipus.tuz => "tuz",
				KartyaTipus.fold => "fold",
				KartyaTipus.viz => "viz",
				KartyaTipus.levego => "levego",
				_ => "hiba"
			};
			return Tipus;
		}
        public static KartyaTipus StringToTipus(string tipus)
        {
            KartyaTipus Tipus;
			Tipus = tipus switch
			{
				"tuz" => KartyaTipus.tuz,
				"fold" => KartyaTipus.fold,
				"viz" => KartyaTipus.viz,
				"levego" => KartyaTipus.levego,
				_ => KartyaTipus.fold
			};
            return Tipus;
		}
        public Card GetCopy()
        {
            return new Card(Name, Damage, HP, TipusToString(Tipus), Vezer);
		}
        public static List<Card> GetListCopy(List<Card>cards)
        {
			List<Card> cardsCopy = new List<Card>();
			for (int i = 0; i < cards.Count; i++)
			{
				cardsCopy.Add(cards[i].GetCopy());
			}
            return cardsCopy;
		}
	}
    public enum KartyaTipus : byte
    {
        fold,
        levego,
        tuz,
        viz
    }
}
