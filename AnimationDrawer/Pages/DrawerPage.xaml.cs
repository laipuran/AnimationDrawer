using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;

namespace AnimationDrawer.Pages
{
    /// <summary>
    /// DrawerPage.xaml 的交互逻辑
    /// </summary>
    public partial class DrawerPage : Page
    {
        List<StrokeCollection> strokes = new();
        int index = 1;

        public DrawerPage()
        {
            InitializeComponent();
            DrawerCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Ellipse;
            DrawerCanvas.DefaultDrawingAttributes.Height = 3;
            DrawerCanvas.DefaultDrawingAttributes.Width = 3;
            DrawerCanvas.DefaultDrawingAttributes.FitToCurve = true;
            strokes.Add(new());
            strokes.Add(new());
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            DrawerCanvas.Strokes = new();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            strokes[index] = DrawerCanvas.Strokes;
            index--;
            if (index <= 1)
            {
                PreviousButton.IsEnabled = false;
            }
            DrawerCanvas.Strokes = strokes[index];
            PreviewCanvas.Strokes = strokes[index - 1];

            FrameCounter.Text = "第 " + index + " 帧 / 共 " + (strokes.Count - 1) + " 帧";
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            PreviousButton.IsEnabled = true;
            strokes[index] = DrawerCanvas.Strokes;
            index++;
            if (index > strokes.Count - 1)
            {
                strokes.Add(new());
            }
            DrawerCanvas.Strokes = strokes[index];
            PreviewCanvas.Strokes = strokes[index - 1];
            FrameCounter.Text = "第 " + index + " 帧 / 共 " + (strokes.Count - 1) + " 帧";
        }

        private void ClearButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            index = 1;
            strokes = new();
            DrawerCanvas.Strokes = new();
            PreviewCanvas.Strokes = new();
            strokes.Add(new());
            strokes.Add(new());
            FrameCounter.Text = "第 " + index + " 帧 / 共 " + (strokes.Count - 1) + "帧";
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            strokes[index] = DrawerCanvas.Strokes;
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
        }
    }
}
