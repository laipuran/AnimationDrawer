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
        int index = 1;

        public DrawerPage()
        {
            InitializeComponent();

            DrawerCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Ellipse;
            DrawerCanvas.DefaultDrawingAttributes.Height = 3;
            DrawerCanvas.DefaultDrawingAttributes.Width = 3;
            DrawerCanvas.DefaultDrawingAttributes.FitToCurve = true;

            piece = new();
            piece.Frames.Add(new());
            piece = AnimationPiece.MergeAnimationPieces(piece, App.piece);

            if (piece.Frames.Count == 1)
            {
                piece.Frames.Add(new());
                ImageBrush brush = new(source);
                DrawerCanvas.Background = brush;
            }
            else
            {
                FrameCounter.Text = "第 " + index + " 帧 / 共 " + (App.piece.Frames.Count - 1) + " 帧";
                DrawerCanvas.Strokes = SingleFrame.GetStrokes(piece.Frames[index]);
            }


            DrawerCanvas.Focus();
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
            index = 1;
            piece = new();
            DrawerCanvas.Strokes = new();
            PreviewCanvas.Strokes = new();
            piece.Frames.Add(new());
            piece.Frames.Add(new());
            PreviousButton.IsEnabled = false;
            FrameCounter.Text = "第 " + index + " 帧 / 共 " + (piece.Frames.Count - 1) + "帧";
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
                DrawerCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                ChooseButton.Content = "橡皮";
            }
            else if (DrawerCanvas.EditingMode == InkCanvasEditingMode.EraseByStroke)
            {
                DrawerCanvas.EditingMode = InkCanvasEditingMode.Select;
                ChooseButton.Content = "选择";
            }
            else
            {
                DrawerCanvas.EditingMode = InkCanvasEditingMode.Ink;
                ChooseButton.Content = "画笔";
            }
            DrawerCanvas.Focus();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveStrokes();
        }

        private void SaveStrokes()
        {
            piece.Frames[index] = SingleFrame.GetSingleFrame(DrawerCanvas.Strokes, source);

            AnimationPiece tempPiece = new();
            for (int i = 1; i < piece.Frames.Count; i++)
            {
                tempPiece.Frames.Add(piece.Frames[i]);
            }

            App.piece = tempPiece;

            DrawerCanvas.Focus();
        }

        private void PreviousPage()
        {
            piece.Frames[index] = SingleFrame.GetSingleFrame(DrawerCanvas.Strokes, source);
            index--;
            if (index <= 1)
            {
                PreviousButton.IsEnabled = false;
            }
            DisplayStrokes();
        }

        private void NextPage()
        {
            PreviousButton.IsEnabled = true;
            piece.Frames[index] = SingleFrame.GetSingleFrame(DrawerCanvas.Strokes, source);
            index++;
            if (index > piece.Frames.Count - 1)
            {
                piece.Frames.Add(new());
            }
            DisplayStrokes();
        }

        private void DisplayStrokes()
        {
            if (index <= 0)
            {
                return;
            }
            DrawerCanvas.Strokes = SingleFrame.GetStrokes(piece.Frames[index]);
            try
            {
                PreviewCanvas.Strokes = SingleFrame.GetStrokes(piece.Frames[index - 1]);
            }
            catch { }

            DrawerCanvas.Background = new ImageBrush(source);

            FrameCounter.Text = "第 " + index + " 帧 / 共 " + (piece.Frames.Count - 1) + " 帧";
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
            ImageBrush brush = new();
            DrawerCanvas.Background = brush;
        }
    }
}
