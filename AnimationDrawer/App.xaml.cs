using System.Collections.Generic;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;
using AnimationDrawer.Ink;

namespace AnimationDrawer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static  AnimationPiece piece = new();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            piece.Frames.Add(new());
        }
    }
}
