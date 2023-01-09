using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace AnimationDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // TODO: Interaction logic needed
    public partial class MainWindow : Window
    {
        readonly Uri DrawerUri = new("Pages/DrawerPage.xaml", UriKind.Relative);
        readonly Uri PreviewUri = new("Pages/PreviewPage.xaml", UriKind.Relative);
        readonly Uri OutputUri = new("Pages/OutputPage.xaml", UriKind.Relative);
        ResourceDictionary Chinese = new();
        ResourceDictionary English = new();
        bool MenuClosed = true;
        Languages currentLang = Languages.Chinese;
        public MainWindow()
        {
            InitializeComponent();
            Icon = GetIcon();
            Chinese.Source = new Uri("Resources/Languages/zh-cn.xaml", UriKind.Relative);
            English.Source = new Uri("Resources/Languages/en-us.xaml", UriKind.Relative);
        }

        public enum Languages
        {
            Chinese,
            English
        }

        private static ImageSource GetIcon()
        {
            ResourceManager Loader = AnimationDrawer.Resources.Resource.ResourceManager;
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
#pragma warning disable CS8604 // 引用类型参数可能为 null。
            Bitmap icon = new((Image)Loader.GetObject("icon"));
#pragma warning restore CS8604 // 引用类型参数可能为 null。
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            return Imaging.CreateBitmapSourceFromHBitmap(icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static string GetString(string name)
        {
            return (string)Application.Current.FindResource(name);
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuClosed)
            {
                Task.Run(() => Dispatcher.BeginInvoke(new Action(() => { MenuOpen(); })));
            }
            else
            {
                Task.Run(() => Dispatcher.BeginInvoke(new Action(() => { MenuClose(); })));
            }
            MenuClosed = !MenuClosed;
        }

        private async void MenuOpen()
        {
            DateTime start = DateTime.Now;
            TimeSpan span = new();
            double k = (120 - 45) / Math.Sin(GetX(175, 135));
            while (span.TotalMilliseconds < 175)
            {
                span = DateTime.Now - start;
                await Task.Run(() => Dispatcher.BeginInvoke(new Action(() => {
                    double x = span.TotalMilliseconds;
                    double value = Math.Sin(GetX(x, 135)) * k + 45;
                    MenuStackPanel.Width = value;
                })));
            }
            MenuStackPanel.Width = 120;
        }

        private async void MenuClose()
        {
            DateTime start = DateTime.Now;
            TimeSpan span = new();
            double k = (120 - 45) / Math.Sin(GetX(175, 135));
            while (span.TotalMilliseconds < 175)
            {
                span = DateTime.Now - start;
                await Task.Run(() => Dispatcher.BeginInvoke(new Action(() => {
                    double x = span.TotalMilliseconds;
                    double value = 120 - Math.Sin(GetX(x, 135)) * k;
                    MenuStackPanel.Width = value;
                })));
            }
            MenuStackPanel.Width = 45;
        }

        private static double GetX(double time, int T)
        {
            double x = Math.PI * time / T / 2;

            return x;
        }

        private void ContentListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ContentListBox.SelectedItem == DrawerListBoxItem)
            {
                ContentFrame.NavigationService.Navigate(DrawerUri);
                TitleTextBlock.Text = GetString("DrawerPage");
            }
            else if (ContentListBox.SelectedItem == PreviewListBoxItem)
            {
                ContentFrame.NavigationService.Navigate(PreviewUri);
                TitleTextBlock.Text = GetString("PreviewPage");
            }
            else if (ContentListBox.SelectedItem == OutputListBoxItem)
            {
                ContentFrame.NavigationService.Navigate(OutputUri);
                TitleTextBlock.Text = GetString("OutputPage");
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }

        private void ContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if ("/" + ContentFrame.Source.ToString() == DrawerUri.ToString())
            {
                TitleTextBlock.Text = "绘图";
                DrawerListBoxItem.IsSelected = true;
            }
            else if ("/" + ContentFrame.Source.ToString() == PreviewUri.ToString())
            {
                TitleTextBlock.Text = "预览";
                PreviewListBoxItem.IsSelected = true;
            }
            else if ("/" + ContentFrame.Source.ToString() == OutputUri.ToString())
            {
                TitleTextBlock.Text = "导出";
                OutputListBoxItem.IsSelected = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentLang == Languages.Chinese)
            {
                currentLang = Languages.English;
                Application.Current.Resources.MergedDictionaries.Add(English);
                Application.Current.Resources.MergedDictionaries.Remove(Chinese);
            }
            else
            {
                currentLang = Languages.Chinese;
                Application.Current.Resources.MergedDictionaries.Add(Chinese);
                Application.Current.Resources.MergedDictionaries.Remove(English);
            }
            ContentFrame.NavigationService.Refresh();
        }
    }
}