﻿using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MainDesktop.UserProfilePage
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private SheldueLogic.Sheldue sheldue;


        public ProfilePage(SheldueLogic.Sheldue sheldue)
        {
            InitializeComponent();

            this.sheldue = sheldue;

            LabelLoginName.Content = sheldue.GetProfileName;
            if (sheldue.ImageIcon == null)
            {
                sheldue.ImageIcon = "https://interactive-examples.mdn.mozilla.net/media/cc0-images/grapefruit-slice-332-332.jpg";
            }

            UpdateImage(sheldue.ImageIcon);
        }

        private void UpdateImage(string path)
        {
            sheldue.ImageIcon = path;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bi3.EndInit();
            ImageOfUser.Stretch = Stretch.Fill;
            ImageOfUser.Source = bi3;
        }

        private void UpdateImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openImage = new OpenFileDialog
            {
                Filter = "png | *.png | jpg | *.jpg",
                Multiselect = false
            };

            if (openImage.ShowDialog() == true)
            {
                try
                {
                    UpdateImage(openImage.FileName);
                }
                catch (Exception ex)
                {
                    MainWindow.Error(ex.Message);
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UpdateImage(Clipboard.GetText());
        }

        private void LoadNewSheldueButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = " Excel file | *.xlsx",
                Multiselect = false
            };

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    sheldue.LoadSheldueFromExcel(new SheldueLogic.SheldueObj.ExcelSheldueConverter(), openDialog.FileName);
                }
                catch (Exception ex)
                {
                    MainWindow.Error(ex.Message);
                }
            }
        }
    }
}
