using AnimationDrawer.Ink;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AnimationDrawer.Pages
{
    /// <summary>
    /// DrawerPage.xaml 的交互逻辑
    /// </summary>
    public partial class DrawerPage : Page
    {
        private AnimationPiece piece = new();
        private ImageSource? source;
        int index = 0;

        public DrawerPage()
        {
            InitializeComponent();
            InitializeDrawer();

            piece = App.piece;

            if (piece.Count == 0)
            {
                piece.Frames.Add(new());
                DrawerCanvas.Background = new ImageBrush(source);
            }
            else
            {
                DrawerCanvas.Strokes = piece.Frames[index].GetStrokes();
                DrawerCanvas.Background = piece.Frames[index].GetBrush();
            }
            FrameCounter.Text = $"{index + 1}/{piece.Count}";


            DrawerCanvas.Focus();
        }

        private void InitializeDrawer()
        {
            DrawerCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Ellipse;
            DrawerCanvas.DefaultDrawingAttributes.Height = 3;
            DrawerCanvas.DefaultDrawingAttributes.Width = 3;
            DrawerCanvas.DefaultDrawingAttributes.FitToCurve = true;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            DrawerCanvas.Strokes = new();
            DrawerCanvas.Focus();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            PreviousPage();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NextPage();
        }

        private void ClearButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            index = 0;
            piece = new();
            DrawerCanvas.Strokes = new();
            PreviewCanvas.Strokes = new();
            piece.Frames.Add(new());
            PreviousButton.IsEnabled = false;
            FrameCounter.Text = $"{index + 1}/{piece.Count}";
            DrawerCanvas.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveStrokes();
        }

        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrawerCanvas.EditingMode == InkCanvasEditingMode.Ink)
            {
                DrawerCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                DrawerCanvas.DefaultDrawingAttributes.Height = 10;
                DrawerCanvas.DefaultDrawingAttributes.Width = 10;
                ChooseButton.Content = MainWindow.GetString("Eraser");
            }
            else if (DrawerCanvas.EditingMode == InkCanvasEditingMode.EraseByPoint)
            {
                DrawerCanvas.EditingMode = InkCanvasEditingMode.Select;
                ChooseButton.Content = MainWindow.GetString("Select");
            }
            else
            {
                DrawerCanvas.EditingMode = InkCanvasEditingMode.Ink;
                DrawerCanvas.DefaultDrawingAttributes.Height = 3;
                DrawerCanvas.DefaultDrawingAttributes.Width = 3;
                ChooseButton.Content = MainWindow.GetString("Pen");
            }
            DrawerCanvas.Focus();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveStrokes();
        }

        private void SaveStrokes()
        {
            piece.Frames[index] = DrawerCanvas.GetSingleFrame();

            App.piece = piece;

            DrawerCanvas.Focus();
        }

        private void PreviousPage()
        {
            piece.Frames[index] = DrawerCanvas.GetSingleFrame();
            if (index - 1 < 0)
            {
                return;
            }
            index--;
            DisplayFrame();
        }

        private void NextPage()
        {
            PreviousButton.IsEnabled = true;
            piece.Frames[index] = DrawerCanvas.GetSingleFrame();
            index++;
            if (index >= piece.Count)
            {
                piece.Frames.Add(new());
            }
            DisplayFrame();
        }

        private void DisplayFrame()
        {
            DrawerCanvas.Strokes = piece.Frames[index].GetStrokes();
            if (piece.Frames[index].Background is not null)
            {
                DrawerCanvas.Background = piece.Frames[index].GetBrush();
            }
            else
                DrawerCanvas.Background = new ImageBrush(source);
            FrameCounter.Text = $"{index + 1}/{piece.Count}";

            if (index == 0)
            {
                PreviousButton.IsEnabled = false;
                return;
            }

            PreviewCanvas.Strokes = piece.Frames[index - 1].GetStrokes();

            DrawerCanvas.Focus();
        }

        private void DrawerCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.PageUp)
            {
                PreviousPage();
            }
            else if (e.Key == Key.Right || e.Key == Key.PageDown)
            {
                NextPage();
            }
        }

        private void BackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AniDrawer\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = false,
                InitialDirectory = path,
                Filter = "图片|*.png; *.jpg",
                FilterIndex = 1
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    source = new BitmapImage(new Uri(openFileDialog.FileName));
                    ImageBrush brush = new(source);
                    DrawerCanvas.Background = brush;
                }
                catch (UriFormatException)
                {
                    return;
                }
            }
        }

        private void BackgroundButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            source = null;
            DrawerCanvas.Background = new ImageBrush();

            if (Keyboard.IsKeyDown(Key.A))
            {
                foreach (SingleFrame item in piece.Frames)
                {
                    item.Background = null;
                }
            }
        }
    }
}
