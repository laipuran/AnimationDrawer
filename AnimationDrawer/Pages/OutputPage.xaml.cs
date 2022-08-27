using AnimationDrawer.Ink;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AnimationDrawer.Pages
{
    /// <summary>
    /// OutputPage.xaml 的交互逻辑
    /// </summary>
    public partial class OutputPage : Page
    {
        List<AnimationPiece> pieces = new();
        AnimationPiece piece = new();
        public OutputPage()
        {
            InitializeComponent();
        }

        private void JsonButton_Click(object sender, RoutedEventArgs e)
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AniDrawer\\";
            string fileName = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() +
                "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Hour.ToString() +
                "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
            string path = directory + fileName + ".json";

            string json = JsonConvert.SerializeObject(App.piece);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, json);
#pragma warning disable CS8602 // 解引用可能出现空引用。
            Process.Start("explorer.exe", Directory.GetParent(directory).FullName);
#pragma warning restore CS8602 // 解引用可能出现空引用。
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AniDrawer\\";
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
                    AnimationPiece objectPiece = JsonConvert.DeserializeObject<AnimationPiece>(File.ReadAllText(filePath));
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。

                    if (objectPiece is null)
                        return;

                    pieces.Add(objectPiece);
                    MessageTextBlock.Text += "\n" + Path.GetFileNameWithoutExtension(filePath) + " 加载完成";
                }
            }
        }

        private void MergeButton_Click(object sender, RoutedEventArgs e)
        {
            piece = AnimationPiece.MergeAnimationPieces(pieces);
            MessageTextBlock.Text += $"\n合并完成，总帧数 = {piece.Frames.Count}";
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            App.piece = piece;
            MessageTextBlock.Text += "\n 导入完成";
        }
    }
}
