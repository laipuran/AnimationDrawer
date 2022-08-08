using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace AnimationDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // TODO: Interaction logic needed
    public partial class MainWindow : Window
    {
        Uri DrawerUri = new("Pages/DrawerPage.xaml", UriKind.Relative);
        Uri PreviewUri = new("Pages/PreviewPage.xaml", UriKind.Relative);
        bool MenuClosed = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuClosed)
            {
                Storyboard openMenu = (Storyboard)FindResource("MenuOpen");
                openMenu.Begin();
            }
            else
            {
                Storyboard closeMenu = (Storyboard)FindResource("MenuClose");
                closeMenu.Begin();
            }
            MenuClosed = !MenuClosed;
        }

        private void ContentListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ContentListBox.SelectedItem == DrawerListBoxItem)
            {
                ContentFrame.NavigationService.Navigate(DrawerUri);
            }
            else if (ContentListBox.SelectedItem == PreviewListBoxItem)
            {
                ContentFrame.NavigationService.Navigate(PreviewUri);
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
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
        }
    }
}