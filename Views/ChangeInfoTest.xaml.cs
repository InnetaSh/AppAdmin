using AppAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppAdmin.Views
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class ChangeInfoTest : Window
    {
        Test _test;
        public ChangeInfoTest(Test selectedTest)
        {
            InitializeComponent();

            _test = selectedTest;
            tbChangeName.Text = _test.Title;
            tbTime.Text = _test.TimeSec.ToString();
            tbChangeDescription.Text = _test.Description;

            string fullImagesPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, $"img\\back\\{_test.ImgSrc}");
            if (!string.IsNullOrEmpty(_test.ImgSrc) && System.IO.File.Exists(fullImagesPath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                string fileUri = new Uri(fullImagesPath).AbsoluteUri;
                bitmap.UriSource = new Uri(fileUri, UriKind.Absolute);

                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                imageInTextBox.Source = bitmap;

            }
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            _test.Title = tbChangeName.Text;
            _test.TimeSec = int.TryParse(tbTime.Text, out var time) ? time : 0;
            _test.Description = tbChangeDescription.Text;
            _test.ImgSrc = System.IO.Path.GetFileName((imageInTextBox.Source as BitmapImage)?.UriSource?.OriginalString);
            // _test.IsCorrect = trueRB.IsChecked ?? false;

            Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void addImgBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.png;*.bmp;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                _test.ImgSrc = System.IO.Path.GetFileName( filePath);
                var image = new BitmapImage(new Uri(filePath));

                imageInTextBox.Source = image;


                imageInTextBox.Visibility = Visibility.Visible;
                this.Height = 350;
            }
        }
    }
}
