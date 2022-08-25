using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Path = System.IO.Path;

namespace AnimationDrawer.Pages
{
    /// <summary>
    /// OutputPage.xaml 的交互逻辑
    /// </summary>
    public partial class OutputPage : Page
    {
        List<List<StrokeCollection>> Strokes = new();
        List<StrokeCollection> allStrokes = new();
        public OutputPage()
        {
            InitializeComponent();
        }

        private void JsonButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = DateTime.Now.ToString();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AniDrawer\\" + fileName + ".json";
            string json = JsonConvert.SerializeObject(App.strokes);

            File.WriteAllText(path, json);
#pragma warning disable CS8602 // 解引用可能出现空引用。
            Process.Start("explorer.exe", Directory.GetParent(path).FullName);
#pragma warning restore CS8602 // 解引用可能出现空引用。
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OverTop\\HangerWindows\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                InitialDirectory = path,
                Filter = "JSON 文件|*.json",
                FilterIndex = 1
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
                    List<StrokeCollection> strokes = JsonConvert.DeserializeObject<List<StrokeCollection>>(File.ReadAllText(filePath));
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
                    if (strokes is null)
                        continue;

                    Strokes.Add(strokes);
                    MessageTextBlock.Text += "\n" + Path.GetFileNameWithoutExtension(filePath) + " 加载完成";
                }
            }
        }

        private void MergeButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (List<StrokeCollection> stroke in Strokes)
            {
                allStrokes.AddRange(stroke);
            }
            MessageTextBlock.Text += "\n合并完成，切换页面以清空缓存";
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            App.strokes = allStrokes;
        }
    }
}
