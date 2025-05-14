using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Powerscan
{
    public partial class EntryWindow : Window
    {
        private Random rand = new Random();
        private DispatcherTimer lightningTimer;
        public ViewModel VM { get; set; }
        public EntryWindow()
        {
            InitializeComponent();
            InitLightningEffect();
            VM = new ViewModel();
            this.DataContext = VM;
            VM.LoadData();
        }

        private void InitLightningEffect()
        {
            lightningTimer = new DispatcherTimer();
            lightningTimer.Interval = TimeSpan.FromMilliseconds(3000); // Lightning Every 5 sec
            lightningTimer.Tick += (s, e) => GenerateLightning();
            lightningTimer.Start();
        }

        private void GenerateLightning() //Method for generating Lightnich effects
        {
            LightningCanvas.Children.Clear();

            double canvasWidth = LightningCanvas.ActualWidth;
            double canvasHeight = LightningCanvas.ActualHeight;

            if (canvasWidth == 0 || canvasHeight == 0)
                return; // If the window has not fully rendered yet

            Point start = new Point(rand.Next(0, (int)canvasWidth), 0); // Generate a vertical line with random horizontal position
            Point end = new Point(rand.Next(0, (int)canvasWidth), canvasHeight); // Starting on the top and ending at the bottom of the canvas

            DrawLightningBranch(start, end, 5, true);
        }

        private void DrawLightningBranch(Point start, Point end, int depth, bool mainBranch)
        {
            if (depth <= 0)
            {
                DrawLine(start, end, mainBranch);
                return;
            }

            // Realistic zigzag shape
            Point mid = new Point(
                (start.X + end.X) / 2 + rand.Next(-20, 20),
                (start.Y + end.Y) / 2 + rand.Next(-20, 20));

            DrawLightningBranch(start, mid, depth - 1, mainBranch);
            DrawLightningBranch(mid, end, depth - 1, mainBranch);

            // Strong branching on the main libhtning bolt
            if (mainBranch && depth >= 2)
            {
                int branchCount = rand.Next(1, 2); // Side branches

                for (int i = 0; i < branchCount; i++)
                {
                    if (rand.NextDouble() < 0.8) // 80 % Probability
                    {
                        Point branchEnd = new Point(
                            mid.X + rand.Next(-60, 60),
                            mid.Y + rand.Next(30, 100));

                        DrawLightningBranch(mid, branchEnd, depth - 2, false);
                    }
                }
            }
        }

        private void DrawLine(Point start, Point end, bool mainBranch)
        {
            var line = new Line
            {
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                Stroke = Brushes.LightBlue,
                StrokeThickness = mainBranch ? 2.5 : 1.8,
                Opacity = mainBranch ? 1.0 : 0.5,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.Blue,        // Color for shadow or glow effects
                    BlurRadius = 30,            // Visibly wide glow
                    ShadowDepth = 0,            // Even glow around the line
                    Direction = 0,
                    Opacity = 1.0               // Highly visible
                }
            };

            LightningCanvas.Children.Add(line);
        }

        private void speichernbtn_Click(object sender, RoutedEventArgs e) 
        {
            if (!string.IsNullOrWhiteSpace(plztxtbx.Text) && !string.IsNullOrWhiteSpace(orttxtbx.Text) && !string.IsNullOrWhiteSpace(strassetxtbx.Text) && !string.IsNullOrWhiteSpace(hausnummertextbox.Text) && !string.IsNullOrWhiteSpace(gebaeudetextbox.Text) && !string.IsNullOrWhiteSpace(etagtextbox.Text) && !string.IsNullOrWhiteSpace(raumtxtbx.Text) && !string.IsNullOrWhiteSpace(zaehleridtxtbx.Text) && !string.IsNullOrWhiteSpace(kostentxtbx.Text))
            {
                VM.ZipCode = plztxtbx.Text;
                VM.City = orttxtbx.Text;
                VM.Street = strassetxtbx.Text;
                VM.Housenumber = hausnummertextbox.Text;
                VM.Building = gebaeudetextbox.Text;
                VM.Floor = etagtextbox.Text;
                VM.Room = raumtxtbx.Text;
                VM.MeterId = zaehleridtxtbx.Text;
                
                

                if (double.TryParse(kostentxtbx.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double kosten))
                {
                    VM.Costs = kosten;
                    VM.SaveData();
                }
                else
                {
                    MessageBox.Show("Bitte gib eine gültige Zahl für die Kosten ein (z. B. 0,25 oder 1.50).", "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Alle Felder müssen ausgefüllt sein!!");
            }
        }

        private void abbrechenbtn_Click(object sender, RoutedEventArgs e)
        {
            Window mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        private void CostsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;

            // Allow digits
            if (char.IsDigit(e.Text, 0))
            {
                e.Handled = false;
                return;
            }

            // There is only one comma or dot allowed. not both and not multiple
            if ((e.Text == "," || e.Text == ".") &&
                !textBox.Text.Contains(",") &&
                !textBox.Text.Contains("."))
            {
                e.Handled = false;
                return;
            }

            // All other inputs is blocked
            e.Handled = true;
        }

        private void CostsTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
        }
    }
}