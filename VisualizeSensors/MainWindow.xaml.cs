using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

using Newtonsoft.Json;

namespace VisualizeSensors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WindowState previousWindowState = WindowState.Normal;

        SensorConfig sensorConfig;

        string jsonFile = "SensorConfigs/sensor.config";

        bool swapLineColor = false;

        Brush defaultBackground = (Brush)(new BrushConverter().ConvertFrom("#FF1C1C1C"));

        bool input_ctrlHeld = false;

        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists(jsonFile)) {
                if (!SelectConfig()) { 
                    Print($"Could not find {jsonFile}\nExiting...");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
                            
            sensorConfig = ReadJson();
            if (sensorConfig != null) 
                DrawAllSensors();
        } 

        void Print(string text) => Console.WriteLine(text);
        void Print(int value) => Console.WriteLine(value);

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
                ToggleFullscreen();
            else
                DragMove();
        }

        void ToggleFullscreen()
        {
            if (WindowState == WindowState.Normal || WindowState == WindowState.Maximized && WindowStyle == WindowStyle.SingleBorderWindow)
            {
                previousWindowState = WindowState;
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
            }
            else
            {
                WindowState = previousWindowState;
                //WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }

        private void SetWindowPosition()
        {
            Top = 0;
            Left = 0;
            Width = sensorConfig.ref_screen.width;
            Height = sensorConfig.ref_screen.height;
        }

        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (input_ctrlHeld) { 
                Point mousePos = Window.PointToScreen(Mouse.GetPosition(Window));
                Print($"mX {mousePos.X}");
                Print($"mY {mousePos.Y}");
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            input_ctrlHeld = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);

            switch (e.Key.ToString())
            {
                case "Escape":
                    Environment.Exit(0);
                    break;
                case "Space":
                    DrawAllSensors();
                    break;
                case "F1":
                    Topmost = !Topmost;
                    Print($"Always on top: {Topmost}");
                    break;
                case "F2":
                    if (SelectConfig()) {
                        sensorConfig = ReadJson();
                        if (sensorConfig != null)
                            DrawAllSensors();
                    }
                    break;
                case "F5":
                    sensorConfig = ReadJson();
                    break;
                case "F11":
                    if (sensorConfig != null)
                        SetWindowPosition();
                    break;
                case "M":
                    MoveCanvas();
                    break;
                case "Delete":
                    Canvas01.Children.Clear();
                    break;
                case "R":
                    if (input_ctrlHeld) {
                        Canvas01.RenderTransform = new TranslateTransform() {X = 0, Y = 0 };
                    }
                    break;
                case "T":
                    if (Background == Brushes.Transparent) {
                        Background = defaultBackground;
                        Canvas01.Background = defaultBackground;
                    }
                    else
                    {
                        Background = Brushes.Transparent;
                        Canvas01.Background = Brushes.Transparent;
                    }
                    break;
                case "D1":
                    if (!input_ctrlHeld)
                        DrawSensor(SensorRegions.A1_REGION);
                    else
                        DrawSensor(SensorRegions.B1_REGION);
                    break;
                case "D2":
                    if (!input_ctrlHeld)
                        DrawSensor(SensorRegions.A2_REGION);
                    else
                        DrawSensor(SensorRegions.B2_REGION);
                    break;
                case "D3":
                    if (!input_ctrlHeld)
                        DrawSensor(SensorRegions.A3_REGION);
                    else
                        DrawSensor(SensorRegions.B3_REGION);
                    break;
                case "D4":
                    if (!input_ctrlHeld)
                        DrawSensor(SensorRegions.A4_REGION);
                    else
                        DrawSensor(SensorRegions.B4_REGION);
                    break;
                case "D5":
                    if (!input_ctrlHeld)
                        DrawSensor(SensorRegions.A5_REGION);
                    else
                        DrawSensor(SensorRegions.B5_REGION);
                    break;
                case "D6":
                    if (!input_ctrlHeld)
                        DrawSensor(SensorRegions.A6_REGION);
                    else
                        DrawSensor(SensorRegions.B6_REGION);
                    break;
                case "D7":
                    if (!input_ctrlHeld)
                        DrawSensor(SensorRegions.A7_REGION);
                    else
                        DrawSensor(SensorRegions.B7_REGION);
                    break;
                case "D8":
                    if (!input_ctrlHeld)
                        DrawSensor(SensorRegions.A8_REGION);
                    else
                        DrawSensor(SensorRegions.B8_REGION);
                    break;
                case "D0":
                    DrawSensor(SensorRegions.C_REGION);
                    break;
            }
        }

        SensorConfig ReadJson()
        {
            string jsonString = File.ReadAllText(jsonFile);
            SensorConfig tempConfig;

            try
            {
                tempConfig = JsonConvert.DeserializeObject<SensorConfig>(jsonString);
            }
            catch (JsonReaderException)
            {
                Print("Unable to read the config. Config might be malformed.");
                return null;
            }

            return tempConfig;
        }

        void DrawSensor(SensorRegions sensorRegion)
        {
            if (sensorConfig == null)
                return;

            PointCollection pointCollection = new PointCollection();

            foreach (List<int> valuePairs in sensorConfig.regions[sensorRegion.ToString()])
            {
                pointCollection.Add(new Point (valuePairs[0], valuePairs[1]));

                Print(valuePairs[0]);
                Print(valuePairs[1]);

            }

            // DrawPolygon(pointCollection);
            DrawLine(pointCollection);

        }

        void DrawAllSensors()
        {
            if (sensorConfig == null)
                return;

            foreach (SensorRegions region in (SensorRegions[])Enum.GetValues(typeof(SensorRegions)))
                DrawSensor(region);
        }

        void DrawLine(PointCollection pc)
        {
            Print("Drawing Line");
            // Print($"{X1} {Y1} {X2} {Y2}");

            Line myLine;

            foreach (int i in Enumerable.Range(0, pc.Count - 1)){
                myLine = new Line();
                myLine.Stroke = swapLineColor ? Brushes.Red : Brushes.Blue;
                myLine.X1 = pc[i].X;
                myLine.X2 = pc[i + 1].X;
                myLine.Y1 = pc[i].Y;
                myLine.Y2 = pc[i + 1].Y;
                myLine.HorizontalAlignment = HorizontalAlignment.Left;
                myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = 2;
                Canvas01.Children.Add(myLine);
            }

            myLine = new Line();
            myLine.Stroke = swapLineColor ? Brushes.Red : Brushes.Blue;
            myLine.X1 = pc[pc.Count - 1].X;
            myLine.X2 = pc[0].X;
            myLine.Y1 = pc[pc.Count - 1].Y;
            myLine.Y2 = pc[0].Y;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
            Canvas01.Children.Add(myLine);

            swapLineColor = !swapLineColor;

        }

        void DrawPolygon(PointCollection pc)
        {
            Print("Drawing Polygon");

            Polygon myPolygon = new Polygon();
            myPolygon.Points = pc;
            myPolygon.Fill = Brushes.Blue;
            // myPolygon.Width = 200;
            // myPolygon.Height = 200;
            myPolygon.Stretch = Stretch.Fill;
            myPolygon.Stroke = Brushes.Black;
            myPolygon.StrokeThickness = 2;

            myPolygon.Margin = new Thickness(pc[0].X, pc[0].Y, 0, 0);

            Canvas01.Children.Add(myPolygon);
        }

        bool SelectConfig()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(jsonFile);
            openFileDialog.Filter = "config files (*.config)|*.config|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 0;

            if (openFileDialog.ShowDialog() == true) { 
                if (File.Exists(openFileDialog.FileName)) { 
                    jsonFile = openFileDialog.FileName;
                    return true;
                }
            }

            return false;
        }

        void MoveCanvas()
        {
            Point mousePos = Window.PointToScreen(Mouse.GetPosition(Window));

            TranslateTransform move = new TranslateTransform
            {
                X = mousePos.X,
                Y = mousePos.Y - SystemParameters.PrimaryScreenHeight
            };

            Canvas01.RenderTransform = move;
        }
    }
}
