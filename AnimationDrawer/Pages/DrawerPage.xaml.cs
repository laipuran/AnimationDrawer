﻿using AnimationDrawer.Ink;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AnimationDrawer.Pages
{
    /// <summary>
    /// DrawerPage.xaml 的交互逻辑
    /// </summary>
    public partial class DrawerPage : Page
    {
        List<StrokeCollection> strokes = new();
        int index = 1;

        public DrawerPage()
        {
            InitializeComponent();

            DrawerCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Ellipse;
            DrawerCanvas.DefaultDrawingAttributes.Height = 3;
            DrawerCanvas.DefaultDrawingAttributes.Width = 3;
            DrawerCanvas.DefaultDrawingAttributes.FitToCurve = true;

            strokes = new();
            strokes.Add(new());
            strokes.AddRange(App.strokes);

            if (strokes.Count == 1)
            {
                strokes.Add(new());
            }
            else
            {
                FrameCounter.Text = "第 " + index + " 帧 / 共 " + (App.strokes.Count - 1) + " 帧";
                DrawerCanvas.Strokes = strokes[index];
            }

            ImageBrush brush = new(App.source);
            DrawerCanvas.Background = brush;

            DrawerCanvas.Focus();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            DrawerCanvas.Strokes = new();
            DrawerCanvas.Focus();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            PreviousPage();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NextPage();
        }

        private void ClearButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            index = 1;
            strokes = new();
            DrawerCanvas.Strokes = new();
            PreviewCanvas.Strokes = new();
            strokes.Add(new());
            strokes.Add(new());
            PreviousButton.IsEnabled = false;
            FrameCounter.Text = "第 " + index + " 帧 / 共 " + (strokes.Count - 1) + "帧";
            DrawerCanvas.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveStrokes();
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

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveStrokes();
        }

        private void SaveStrokes()
        {
            strokes[index] = DrawerCanvas.Strokes;

            List<StrokeCollection> strokesList = new();
            for (int i = 1; i < strokes.Count; i++)
            {
                strokesList.Add(strokes[i]);
            }

            App.strokes = strokesList;

            DrawerCanvas.Focus();
        }

        private void PreviousPage()
        {
            strokes[index] = DrawerCanvas.Strokes;
            index--;
            if (index <= 1)
            {
                PreviousButton.IsEnabled = false;
            }
            DrawerCanvas.Strokes = strokes[index];
            PreviewCanvas.Strokes = strokes[index - 1];

            FrameCounter.Text = "第 " + index + " 帧 / 共 " + (strokes.Count - 1) + " 帧";
            DrawerCanvas.Focus();
        }

        private void NextPage()
        {
            PreviousButton.IsEnabled = true;
            strokes[index] = DrawerCanvas.Strokes;
            index++;
            if (index > strokes.Count - 1)
            {
                strokes.Add(new());
            }
            DrawerCanvas.Strokes = strokes[index];
            PreviewCanvas.Strokes = strokes[index - 1];
            FrameCounter.Text = "第 " + index + " 帧 / 共 " + (strokes.Count - 1) + " 帧";
            DrawerCanvas.Focus();
        }

        private void DrawerCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.PageUp)
            {
                PreviousPage();
            }
            else if (e.Key == Key.Right || e.Key == Key.PageDown)
            {
                NextPage();
            }
        }

        private void BackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AniDrawer\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = false,
                InitialDirectory = path,
                Filter = "图片|*.png, *.jpg",
                FilterIndex = 1
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    App.source = new BitmapImage(new Uri(openFileDialog.FileName));
                    ImageBrush brush = new(App.source);
                    DrawerCanvas.Background = brush;
                }
                catch (UriFormatException)
                {
                    return;
                }
            }
        }

        private void BackgroundButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            App.source = null;
            ImageBrush brush = new();
            DrawerCanvas.Background = brush;
        }
    }
}
