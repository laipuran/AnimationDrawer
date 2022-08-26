using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace AnimationDrawer.Pages
{
    /// <summary>
    /// PreviewPage.xaml 的交互逻辑
    /// </summary>
    public partial class PreviewPage : Page
    {
        int FPS = 60;
        int index = 0;
        public PreviewPage()
        {
            InitializeComponent();
            PreviewCanvas.Strokes = new();
            FrameSlider.Maximum = App.strokes.Count - 1;
            FrameSlider.Value = 0;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayButton.IsEnabled = false;
            PreviewStrokes();
            PlayButton.IsEnabled = true;
        }

        private async void PreviewStrokes()
        {
            foreach (StrokeCollection item in App.strokes)
            {
                index = App.strokes.IndexOf(item);
                await Task.Run(() => Dispatcher.BeginInvoke(new Action(() => { FrameSlider.Value = index; })));
                await Task.Delay(1000 / FPS);
            }
        }

        private void FpsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            FPS = (int)FpsSlider.Value;
            FpsTextBlock.Text = "帧率：" + FPS.ToString();
        }
        
        private async void FrameSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            index = (int)FrameSlider.Value;
            await Task.Run(() => Dispatcher.BeginInvoke(new Action(() => { PreviewCanvas.Strokes = App.strokes[index]; })));
            await Task.Run(() => Dispatcher.BeginInvoke(new Action(() => { FrameTextBlock.Text = (index + 1).ToString(); })));
        }
    }
}
