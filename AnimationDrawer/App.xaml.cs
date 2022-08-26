using System.Collections.Generic;
using System.Windows;
using System.Windows.Ink;

namespace AnimationDrawer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static List<StrokeCollection> strokes = new();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            strokes.Add(new());
        }
    }
}
