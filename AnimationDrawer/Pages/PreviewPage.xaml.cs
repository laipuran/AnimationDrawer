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
        public PreviewPage()
        {
            InitializeComponent();
            PreviewCanvas.Strokes = new();
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
                await Task.Run(() => Dispatcher.BeginInvoke(new Action(() => { PreviewCanvas.Strokes = item; })));
                await Task.Delay(1000 / FPS);
            }
        }
    }
}
