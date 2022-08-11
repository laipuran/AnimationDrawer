using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using SharpDX.DirectInput;

namespace AnimationDrawer.Pages
{
    /// <summary>
    /// DrawerPage.xaml 的交互逻辑
    /// </summary>
    public partial class DrawerPage : Page
    {
        int index = 1;

        public DrawerPage()
        {
            InitializeComponent();
            if (App.strokes.Count == 0)
            {
                DrawerCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Ellipse;
                DrawerCanvas.DefaultDrawingAttributes.Height = 3;
                DrawerCanvas.DefaultDrawingAttributes.Width = 3;
                DrawerCanvas.DefaultDrawingAttributes.FitToCurve = true;
                App.strokes.Add(new());
                App.strokes.Add(new());
            }
            else
            {
                FrameCounter.Text = "第 " + index + " 帧 / 共 " + (App.strokes.Count - 1) + " 帧";
                DrawerCanvas.Strokes = App.strokes[index];
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
            App.strokes[index] = DrawerCanvas.Strokes;
            index--;
            if (index <= 1)
            {
                PreviousButton.IsEnabled = false;
            }
            DrawerCanvas.Strokes = App.strokes[index];
            PreviewCanvas.Strokes = App.strokes[index - 1];

            FrameCounter.Text = "第 " + index + " 帧 / 共 " + (App.strokes.Count - 1) + " 帧";
            DrawerCanvas.Focus();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            PreviousButton.IsEnabled = true;
            App.strokes[index] = DrawerCanvas.Strokes;
            index++;
            if (index > App.strokes.Count - 1)
            {
                App.strokes.Add(new());
            }
            DrawerCanvas.Strokes = App.strokes[index];
            PreviewCanvas.Strokes = App.strokes[index - 1];
            FrameCounter.Text = "第 " + index + " 帧 / 共 " + (App.strokes.Count - 1) + " 帧";
            DrawerCanvas.Focus();
        }

        private void ClearButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            index = 1;
            App.strokes = new();
            DrawerCanvas.Strokes = new();
            PreviewCanvas.Strokes = new();
            App.strokes.Add(new());
            App.strokes.Add(new());
            PreviousButton.IsEnabled = false;
            FrameCounter.Text = "第 " + index + " 帧 / 共 " + (App.strokes.Count - 1) + "帧";
            DrawerCanvas.Focus();
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            App.strokes[index] = DrawerCanvas.Strokes;
            DrawerCanvas.Focus();
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

        private void GetStatusButton_Click(object sender, RoutedEventArgs e)
        {
            DirectInput directInput = new();
            IList<DeviceInstance> allDevices = directInput.GetDevices();
            List<Joystick> joysticks = new();
            foreach (var deviceInstance in allDevices)
            {
                if (deviceInstance.Type == DeviceType.Joystick)
                {
                    MessageBox.Show(deviceInstance.InstanceGuid.ToString());
                }
            }
        }

    }
}
