using AnimationDrawer.Ink;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

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
            FrameSlider.Maximum = App.piece.Frames.Count - 1;
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
            List<StrokeCollection> collections = AnimationPiece.GetStrokeCollections(App.piece);
            foreach (StrokeCollection item in collections)
            {
                index = collections.IndexOf(item);
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
            await Task.Run(() => Dispatcher.BeginInvoke(new Action(() => { PreviewCanvas.Strokes = SingleFrame.GetStrokes(App.piece.Frames[index]); })));
            await Task.Run(() => Dispatcher.BeginInvoke(new Action(() => { PreviewCanvas.Background = new ImageBrush(App.piece.Frames[index].Background); })));
            await Task.Run(() => Dispatcher.BeginInvoke(new Action(() => { FrameTextBlock.Text = (index + 1).ToString(); })));
        }
    }
}
