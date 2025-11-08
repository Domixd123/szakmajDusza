using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        private Label NameLabel;
        private Label DamageAndHPLabel;
        private Label TypeLabel;

        private Grid visualGroup;

        public Card(string n, int d, int h, string tipus, bool vezer)
        {
            Name = n;
            Damage = d;
            HP = h;
            Vezer = vezer;

            Tipus=StringToTipus(tipus);

            
            Rec = new Rectangle
            {
                Height = 150,
                
            };

            But = new Button
            {
                Height = Rec.Height,
                Width = Rec.Width
            };

            
            NameLabel = new Label
            {
                Content = Name,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 0)
            };

            DamageAndHPLabel = new Label
            {
                Content = $"{Damage}/{HP}",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 5)
            };

            TypeLabel = new Label
            {
                Content = Tipus.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Foreground = Brushes.DarkBlue
            };

            
            visualGroup = new Grid();
            visualGroup.Width = Rec.Width;
            visualGroup.Height = Rec.Height;
            visualGroup.Margin = new Thickness(10, 10, 10, 10);
            visualGroup.Background = Brushes.LightGray;
            visualGroup.Children.Add(Rec);
            visualGroup.Children.Add(But);
            visualGroup.Children.Add(NameLabel);
            visualGroup.Children.Add(TypeLabel);
            visualGroup.Children.Add(DamageAndHPLabel);
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
	}
    public enum KartyaTipus : byte
    {
        fold,
        levego,
        tuz,
        viz
    }
}
