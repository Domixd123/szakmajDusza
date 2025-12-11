using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace szakmajDusza
{
    public class Card
    {
        //Kartya tulajdonságok
        public string Name { get; set; }
        public int Damage { get; set; }
        public int HP { get; set; }
        public KartyaTipus Tipus { get; set; }
        public List<Item> Items { get; set; }
        public bool Vezer { get; set; }
        public string Bonus { get; set; }
        public string OriginName { get; set; }
        //Vizuális Tulajdonságok
        public bool Disabled { get; set; } = false;
        public Rectangle Rec { get; private set; }
        public Button But { get; private set; }
        public bool animation { get; set; } = false;
        public Label NameLabel;
        public Label Name2Label;
        public Label DamageLabel;
        public Label HPLabel;
        private Label TypeLabel;
        public Label DisLabel;
        public Label ImportantLabel;
        public Label ImportantLabel2;
        public Border border;
        public Grid visualGroup;
        //Click eventek
        public event EventHandler<Card> Clicked;
        public event EventHandler<Card> RightClicked;

        public Card(string n, int d, int h, string tipus, bool vezer, string originName = "", List<Item> items = null)
        {
            Name = n;
            Damage = d;
            HP = h;
            Vezer = vezer;
            OriginName = originName;
            if (items == null) items = new List<Item>();
            Items = items;
            Tipus = StringToTipus(tipus);

            UpdateAllVisual();

        }
        public void UpdateAllVisual() //Vizuálisan újra rajzolja a kártyát
        {
            visualGroup = new Grid
            {
                Width = 140,
                Height = 180,
                Margin = new Thickness(10),
                /*Background = new LinearGradientBrush(
        Color.FromRgb(25, 20, 30),
        Color.FromRgb(45, 35, 55),
        90)*/
            }; //kártya alapo
            visualGroup.RenderTransformOrigin = new Point(0.5, 0.5);
            visualGroup.RenderTransform = new TransformGroup
            {
                Children =
    {
        new ScaleTransform(),
        new TranslateTransform()
    }
            };

            //Árnyékok, hover effectek
            visualGroup.Effect = new System.Windows.Media.Effects.DropShadowEffect
            {
                BlurRadius = 15,
                ShadowDepth = 5,
                Color = Colors.Black,
                Opacity = 0.7
            };
            visualGroup.MouseEnter += (s, e) =>
            {
                if (!Disabled)
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
                }

            };
            visualGroup.MouseLeave += (s, e) =>
            {
                if (!Disabled)
                {
                    if (Vezer)
                    {
                        visualGroup.Background = new LinearGradientBrush(
                            Color.FromRgb(45, 35, 60),
                            Color.FromRgb(80, 60, 100),
                            90);
                    }
                    else
                    {
                        visualGroup.Background = new LinearGradientBrush(
                        Color.FromRgb(25, 20, 30),
                        Color.FromRgb(45, 35, 55),
                        90);
                    }

                    visualGroup.Effect = new DropShadowEffect
                    {
                        BlurRadius = 15,
                        ShadowDepth = 5,
                        Color = Colors.Black,
                        Opacity = 0.7
                    };

                }

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



            // A kártya neve
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
            if (Name.Split(' ').Length > 1) //Második név, ha vezér
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
            if (Disabled) //Vezér merge-hez a kártyák "kikapcsolása"
            {
                DisLabel = new Label
                {
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 0, 67),
                    Foreground = Brushes.Red,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Content = "Pakliban van!"
                };
                visualGroup.Children.Add(DisLabel);
                if (Vezer)
                {
                    DisLabel.Content = "Már vezér!";
                }
            }

            //Sebzés ikon+Label
            var aBG = new ImageBrush();
            aBG.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images\\attackbg.png"));
            aBG.Stretch = Stretch.UniformToFill;
            var DMGEllipse = new Ellipse
            {
                Width = 90,
                Height = 90,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(-12, 0, 0, -5),
                IsHitTestVisible = false
            };
            DMGEllipse.Fill = aBG;
            visualGroup.Children.Add(DMGEllipse);
            DamageLabel = new Label
            {
                Content = Damage,
                Foreground = Brushes.WhiteSmoke,
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(25, 0, 0, 17),
                IsHitTestVisible = false,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                FontWeight = FontWeights.Bold

            };
            DamageLabel.Margin = new Thickness(25 - 5.5 * DamageLabel.Content.ToString().Length, 0, 0, 17);
            visualGroup.Children.Add(DamageLabel);

            //Élet ikon+Label
            var hBG = new ImageBrush();
            hBG.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images\\heartbg.png"));
            hBG.Stretch = Stretch.UniformToFill;
            var HPEllipse = new Ellipse
            {
                Width = 65,
                Height = 65,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 00, 0),
                IsHitTestVisible = false
            };
            HPEllipse.Fill = hBG;
            visualGroup.Children.Add(HPEllipse);
            HPLabel = new Label
            {
                Content = HP,
                Foreground = Brushes.WhiteSmoke,
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 25, 17),
                IsHitTestVisible = false,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                FontWeight = FontWeights.Bold

            };
            HPLabel.Margin = new Thickness(0, 0, 25 - 5.5 * HPLabel.Content.ToString().Length, 17);
            visualGroup.Children.Add(HPLabel);




            //Animációkhoz használt label-ek
            ImportantLabel = new Label
            {
                Content = "",
                Foreground = Brushes.IndianRed,
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 25, 50),
                IsHitTestVisible = false,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                FontWeight = FontWeights.Bold,
                Visibility = Visibility.Hidden

            };
            visualGroup.Children.Add(ImportantLabel);
            ImportantLabel2 = new Label
            {
                Content = "",
                Foreground = Brushes.IndianRed,
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(25, 0, 0, 50),
                IsHitTestVisible = false,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                FontWeight = FontWeights.Bold,
                Visibility = Visibility.Hidden

            };
            visualGroup.Children.Add(ImportantLabel2);




            // típus indikátor
            var ellipse = new Ellipse
            {

                Width = 30,
                Height = 30,
                //Fill = typeColor,
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

            // vezér aura+itemek
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
                //Item karikák
                var ellips1 = new Ellipse
                {
                    Width = 30,
                    Height = 30,
                    Stroke = Brushes.Gold,
                    StrokeThickness = 1.5,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(30, 0, 0, 90),
                    Effect = new System.Windows.Media.Effects.DropShadowEffect
                    {
                        BlurRadius = 5,
                        Color = Colors.Black,
                        Opacity = 0.6
                    },
                    IsHitTestVisible = false
                };
                var ellips2 = new Ellipse
                {
                    Width = 30,
                    Height = 30,
                    Stroke = Brushes.Gold,
                    StrokeThickness = 1.5,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(0, 0, 30, 90),
                    Effect = new System.Windows.Media.Effects.DropShadowEffect
                    {
                        BlurRadius = 5,
                        Color = Colors.Black,
                        Opacity = 0.6
                    },
                    IsHitTestVisible = false
                };
                visualGroup.Children.Add(ellips2);
                visualGroup.Children.Add(ellips1);


                //Itemek vizuális elhelyezése
                if (Items.Count <= 0)
                {
                    ellips1.Fill = Brushes.Transparent;
                    ellips2.Fill = Brushes.Transparent;

                }
                else if (Items.Count == 1)
                {
                    var brush1 = new ImageBrush();
                    brush1.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/images\\{Items[0].IconPath}"));
                    brush1.Stretch = Stretch.UniformToFill;
                    ellips1.Fill = brush1;
                    ellips2.Fill = Brushes.Transparent;
                }
                if (Items.Count >= 2)
                {
                    var brush1 = new ImageBrush();
                    brush1.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/images\\{Items[0].IconPath}"));
                    brush1.Stretch = Stretch.UniformToFill;
                    ellips1.Fill = brush1;

                    var brush2 = new ImageBrush();
                    brush2.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/images\\{Items[1].IconPath}"));
                    brush2.Stretch = Stretch.UniformToFill;
                    ellips2.Fill = brush2;
                }



            }
            else
            {
                //Item karika
                var ellips1 = new Ellipse
                {
                    Width = 30,
                    Height = 30,
                    Stroke = Brushes.Gold,
                    StrokeThickness = 1.5,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(0, 0, 0, 90),
                    Effect = new System.Windows.Media.Effects.DropShadowEffect
                    {
                        BlurRadius = 5,
                        Color = Colors.Black,
                        Opacity = 0.6
                    },
                    IsHitTestVisible = false
                };
                visualGroup.Children.Add(ellips1);
                //Itemek vizuális elhelyezése
                if (Items.Count <= 0)
                {
                    ellips1.Fill = Brushes.Transparent;

                }
                if (Items.Count >= 1)
                {
                    var brush1 = new ImageBrush();
                    brush1.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/images\\{Items[0].IconPath}"));
                    brush1.Stretch = Stretch.UniformToFill;
                    ellips1.Fill = brush1;
                }
            }

            // gomb (láthatatlan, de kattintható)
            But = new Button
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,

            };
            But.Click += (sender, e) => Clicked?.Invoke(this, this);
            But.MouseRightButtonUp += (sender, e) => RightClicked?.Invoke(this, this);
            if (Disabled)
            {
                visualGroup.Background = new LinearGradientBrush(Color.FromRgb(10, 5, 15), Color.FromRgb(30, 20, 40), 90);
            }
            visualGroup.Children.Add(ellipse);
            visualGroup.Children.Add(NameLabel);
            
            visualGroup.Children.Add(But);
        }
        //Sebzés és gyógyítás esetén életerő változtatása
        public void UpdateVisual()
        {
            if (HP <= 0)
            {
                HP = 0;
                visualGroup.Opacity = 0.4;
            }
            else
            {
                visualGroup.Opacity = 1;
            }
            HPLabel.Content = HP;
            DamageLabel.Content = Damage;
            HPLabel.Margin = new Thickness(0, 0, 25 - 5.5 * HPLabel.Content.ToString().Length, 17);
        }
        public Task UpdateVisualHeal(int heal)
        {
            var tcs = new TaskCompletionSource<bool>();
            int previousHP = HP - heal;
            if (HP - heal <= 0)
            {
                previousHP = 0;
            }

            HPLabel.Content = $"{previousHP} + {heal}";
            HPLabel.Margin = new Thickness(0, 0, 25 - 5.5 * HPLabel.Content.ToString().Length, 17);
            tcs.SetResult(true);
            return tcs.Task;
        }

        //Animációk
        public Task StrengthAnim(int dmg)
        {
            animation = true;
            var tcs = new TaskCompletionSource<bool>();
            ImportantLabel.Visibility = Visibility.Visible;
            Panel.SetZIndex(ImportantLabel, -1);
            ImportantLabel.FontWeight = FontWeights.ExtraBold;
            ImportantLabel.Foreground = Brushes.OrangeRed;
            ImportantLabel.Content = $"-{dmg}";

            var anim = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 80),
                To = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 5),
                Duration = TimeSpan.FromSeconds(0.8f / Harc2.playSpeedMultiplier),
                FillBehavior = FillBehavior.Stop
            };
            anim.Completed += (s, e) =>
            {
                // hide once it’s slid away
                ImportantLabel.Visibility = Visibility.Hidden;
                animation = false;
                tcs.SetResult(true);


            };
            ImportantLabel.BeginAnimation(Label.MarginProperty, anim);
            return tcs.Task;
        }
        public Task CritAnim(int dmg, double mult)
        {
            animation = true;
            var tcs = new TaskCompletionSource<bool>();
            ImportantLabel.Visibility = Visibility.Visible;
            Panel.SetZIndex(ImportantLabel, -1);
            ImportantLabel.FontWeight = FontWeights.ExtraBold;
            ImportantLabel.Foreground = Brushes.DarkGoldenrod;
            ImportantLabel.Content = $"-{dmg}×{mult}";

            var anim = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 70),
                To = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 5),
                Duration = TimeSpan.FromSeconds(1.0f / Harc2.playSpeedMultiplier),
                FillBehavior = FillBehavior.Stop
            };
            anim.Completed += (s, e) =>
            {
                // hide once it’s slid away
                ImportantLabel.Visibility = Visibility.Hidden;
                animation = false;
                tcs.SetResult(true);
            };
            ImportantLabel.BeginAnimation(Label.MarginProperty, anim);
            return tcs.Task;
        }
        public Task StrengthShieldAnim(int dmg, int blocked)
        {
            animation = true;
            var tcs = new TaskCompletionSource<bool>();
            ImportantLabel.Visibility = Visibility.Visible;
            Panel.SetZIndex(ImportantLabel, -1);
            ImportantLabel.FontWeight = FontWeights.ExtraBold;
            ImportantLabel.Foreground = Brushes.DimGray;
            ImportantLabel.Content = $"-({dmg}-{blocked})";

            var anim = new ThicknessAnimation
            {

                From = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 80),
                To = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 5),
                Duration = TimeSpan.FromSeconds(0.8f / Harc2.playSpeedMultiplier),
                FillBehavior = FillBehavior.Stop
            };
            anim.Completed += (s, e) =>
            {
                animation = false;
                // hide once it’s slid away
                ImportantLabel.Visibility = Visibility.Hidden;
                tcs.SetResult(true);
            };
            ImportantLabel.BeginAnimation(Label.MarginProperty, anim);
            return tcs.Task;
        }
        public Task CritShieldAnim(int dmg, double mult, int blocked)
        {
            animation = true;
            var tcs = new TaskCompletionSource<bool>();
            ImportantLabel.Visibility = Visibility.Visible;
            Panel.SetZIndex(ImportantLabel, -1);
            ImportantLabel.FontWeight = FontWeights.Bold;
            ImportantLabel.Foreground = Brushes.Gray;
            ImportantLabel.Content = $"-({dmg}×{mult}-{blocked})";

            var anim = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 70),
                To = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 5),
                Duration = TimeSpan.FromSeconds(1.0f / Harc2.playSpeedMultiplier),
                FillBehavior = FillBehavior.Stop
            };
            anim.Completed += (s, e) =>
            {
                animation = false;
                // hide once it’s slid away
                ImportantLabel.Visibility = Visibility.Hidden;
                tcs.SetResult(true);
            };
            ImportantLabel.BeginAnimation(Label.MarginProperty, anim);
            return tcs.Task;
        }
        public Task DodgeAnim()
        {
            animation = true;
            var tcs = new TaskCompletionSource<bool>();
            ImportantLabel.Visibility = Visibility.Visible;
            Panel.SetZIndex(ImportantLabel, -1);
            ImportantLabel.FontWeight = FontWeights.Bold;
            ImportantLabel.Foreground = Brushes.White;
            ImportantLabel.Content = $"Kitérés";

            var anim = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 130),
                To = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 65),
                Duration = TimeSpan.FromSeconds(1.0f / Harc2.playSpeedMultiplier),
                FillBehavior = FillBehavior.Stop
            };
            anim.Completed += (s, e) =>
            {
                // hide once it’s slid away
                animation = false;
                ImportantLabel.Visibility = Visibility.Hidden;
                tcs.SetResult(true);
            };
            ImportantLabel.BeginAnimation(Label.MarginProperty, anim);
            return tcs.Task;
        }
        public Task HealAnim(int amount)//same as lifesteal 
        {
            animation = true;
            var tcs = new TaskCompletionSource<bool>();
            ImportantLabel.Visibility = Visibility.Visible;
            Panel.SetZIndex(ImportantLabel, -1);
            ImportantLabel.FontWeight = FontWeights.Bold;
            ImportantLabel.Foreground = Brushes.Green;
            ImportantLabel.Content = $"+{amount}";
            ImportantLabel.Margin = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 5);

            var anim = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 5),
                To = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 70),
                Duration = TimeSpan.FromSeconds(1.0f / Harc2.playSpeedMultiplier),
                FillBehavior = FillBehavior.Stop
            };
            anim.Completed += (s, e) =>
            {
                animation = false;
                // hide once it’s slid away
                ImportantLabel.Visibility = Visibility.Hidden;
                tcs.SetResult(true);
            };
            ImportantLabel.BeginAnimation(Label.MarginProperty, anim);
            return tcs.Task;

        }
        public Task ReviveAnim(int amount)
        {
            bool anim1comp = false;
            bool anim2comp = false;

            animation = true;
            var tcs = new TaskCompletionSource<bool>();
            ImportantLabel2.Visibility = Visibility.Visible;
            ImportantLabel.Visibility = Visibility.Visible;
            Panel.SetZIndex(ImportantLabel2, -1);
            Panel.SetZIndex(ImportantLabel, -1);
            ImportantLabel2.FontWeight = FontWeights.Bold;
            ImportantLabel2.Foreground = Brushes.White;
            ImportantLabel.FontWeight = FontWeights.Bold;
            ImportantLabel.Foreground = Brushes.Green;
            ImportantLabel.Content = $"+{amount}";
            ImportantLabel2.Content = $"Újraéledve";

            var anim = new ThicknessAnimation
            {
                From = new Thickness(25 - 5.5 * ImportantLabel2.Content.ToString().Length, 0, 0, 70),
                To = new Thickness(25 - 5.5 * ImportantLabel2.Content.ToString().Length, 0, 0, 5),
                Duration = TimeSpan.FromSeconds(1.0f / Harc2.playSpeedMultiplier),
                FillBehavior = FillBehavior.Stop
            };
            var anim2 = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 5),
                To = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 70),
                Duration = TimeSpan.FromSeconds(1.0f / Harc2.playSpeedMultiplier),
                FillBehavior = FillBehavior.Stop
            };
            anim.Completed += (s, e) =>
            {

                // hide once it’s slid away
                ImportantLabel2.Visibility = Visibility.Hidden;
                if (anim2comp)
                {
                    Disabled = false;
                    animation = false;
                    tcs.SetResult(true);

                }
                else
                {
                    anim1comp = true;
                }

            };
            anim2.Completed += (s, e) =>
            {

                // hide once it’s slid away
                ImportantLabel.Visibility = Visibility.Hidden;
                if (anim1comp)
                {
                    Disabled = false;
                    animation = false;
                    tcs.SetResult(true);

                }
                else
                {
                    anim2comp = true;
                }

            };

            ImportantLabel2.BeginAnimation(Label.MarginProperty, anim);
            ImportantLabel.BeginAnimation(Label.MarginProperty, anim2);
            return tcs.Task;

        }
        public Task MagicAnim()
        {
            animation = true;
            var tcs = new TaskCompletionSource<bool>();
            ImportantLabel.Visibility = Visibility.Visible;
            Panel.SetZIndex(ImportantLabel, -1);
            ImportantLabel.FontWeight = FontWeights.Bold;
            ImportantLabel.Foreground = Brushes.White;
            ImportantLabel.Content = $"Befagyasztva";

            var anim = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 130),
                To = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 65),
                Duration = TimeSpan.FromSeconds(1.0f / Harc2.playSpeedMultiplier),
                FillBehavior = FillBehavior.Stop
            };
            anim.Completed += (s, e) =>
            {
                animation = false;
                // hide once it’s slid away
                ImportantLabel.Visibility = Visibility.Hidden;
                tcs.SetResult(true);
            };
            ImportantLabel.BeginAnimation(Label.MarginProperty, anim);
            return tcs.Task;
        }
        public Task NormalShieldAnim(int dmg, int blocked)
        {
            animation = true;
            var tcs = new TaskCompletionSource<bool>();
            ImportantLabel.Visibility = Visibility.Visible;
            Panel.SetZIndex(ImportantLabel, -1);
            ImportantLabel.FontWeight = FontWeights.ExtraBold;
            ImportantLabel.Foreground = Brushes.DimGray;
            ImportantLabel.Content = $"-({dmg}-{blocked})";

            var anim = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 80),
                To = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 5),
                Duration = TimeSpan.FromSeconds(0.8f / Harc2.playSpeedMultiplier),
                FillBehavior = FillBehavior.Stop
            };
            anim.Completed += (s, e) =>
            {
                animation = false;
                // hide once it’s slid away
                ImportantLabel.Visibility = Visibility.Hidden;
                tcs.SetResult(true);

            };
            ImportantLabel.BeginAnimation(Label.MarginProperty, anim);
            return tcs.Task;
        }
        public Task UpdateVisualDamage(int dmg)
        {
            animation = true;
            var tcs = new TaskCompletionSource<bool>();
            ImportantLabel.Visibility = Visibility.Visible;
            ImportantLabel.Foreground = Brushes.Red;
            ImportantLabel.FontWeight = FontWeights.Bold;
            Panel.SetZIndex(ImportantLabel, -1);

            ImportantLabel.Content = $"-{dmg}";

            var anim = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 70),
                To = new Thickness(0, 0, 25 - 5.5 * ImportantLabel.Content.ToString().Length, 5),
                Duration = TimeSpan.FromSeconds(0.8f / Harc2.playSpeedMultiplier),
                FillBehavior = FillBehavior.Stop
            };
            anim.Completed += (s, e) =>
            {
                animation = false;
                tcs.SetResult(true);
                ImportantLabel.Visibility = Visibility.Hidden;
                HPLabel.Content = HP;
            };


            ImportantLabel.BeginAnimation(Label.MarginProperty, anim);

            return tcs.Task;

        }
        //Animációk utána Labelek elrejtése
        public void HideAllLabels()
        {
            ImportantLabel.Visibility = Visibility.Hidden;
            ImportantLabel2.Visibility = Visibility.Hidden;
        }
       

        public UIElement GetVisual()//A kártya vizualizálása
        {
            return visualGroup;
        }


        //egyéb funct.
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
        public Card GetCopy(bool kazamataCard = false)
        {
            if (kazamataCard) return new Card(Name, Damage, HP, TipusToString(Tipus), Vezer, OriginName, Items.Select(x => x.GetCopy()).ToList());
            else return new Card(Name, Damage, HP, TipusToString(Tipus), Vezer, OriginName, Items);
        }
        public static List<Card> GetListCopy(List<Card> cards, bool kazamataCard = false)
        {
            List<Card> cardsCopy = new List<Card>();
            for (int i = 0; i < cards.Count; i++)
            {
                cardsCopy.Add(cards[i].GetCopy(kazamataCard));
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
