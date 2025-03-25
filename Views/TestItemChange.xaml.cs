using AppAdmin.Models;
using AppAdmin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppAdmin
{
    /// <summary>
    /// Interaction logic for TestItemControl.xaml
    /// </summary>
    public partial class TestItemChange : Window
    {
        private string _text;
        private ImageSource _imageSource;
        private List<RadioButton> _radioButtons;
        public TestItemChange(string text, ImageSource imageSource, List<RadioButton> radioButtons)
        {
            InitializeComponent();

            _text = text;
            _imageSource = imageSource;
            _radioButtons = radioButtons;

            if (_imageSource != null)
            {
                imageInTextBox.Source = _imageSource;
                imageInTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                imageInTextBox.Visibility = Visibility.Collapsed;
            }
            foreach (var rb in _radioButtons)
            {
                RadioButton newRadioButton = new RadioButton() { Margin = new Thickness(5, 0, 0, 0) };
                newRadioButton.Content = rb.Content;
                newRadioButton.Checked += RadioButton_Checked;
                pnAnswers.Children.Add(newRadioButton);
            }
            tbTest.Text = _text;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

      

        private void addAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AddWindow() { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            addWin.Title = "Добавить ответ";
            addWin.tbName.Text = "";

            if (addWin.ShowDialog().Value)
            {
                RadioButton newRadioButton = new RadioButton() { Margin = new Thickness(5, 0, 0, 0) };
                newRadioButton.Content = addWin.tbName.Text;
                newRadioButton.Checked += RadioButton_Checked;

                pnAnswers.Children.Add(newRadioButton);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateVisibleBtnState();
        }

        private void UpdateVisibleBtnState()
        {
            var btnVisibleState = pnAnswers.Children.Cast<UIElement>().Any(x => x is RadioButton rb && (rb.IsChecked ?? false));
            delAnswerBtn.Visibility = btnVisibleState ? Visibility.Visible : Visibility.Collapsed;
            changeAnswerBtn.Visibility = btnVisibleState ? Visibility.Visible : Visibility.Collapsed;
        }


        private void delAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            var checkRB = pnAnswers.Children.Cast<UIElement>().FirstOrDefault(x => x is RadioButton rb && (rb?.IsChecked ?? false));
            if (checkRB != null)
                pnAnswers.Children.Remove(checkRB);
            UpdateVisibleBtnState();
        }

        private void changeAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AddWindow() { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            addWin.Title = "Изменить ответ";
            addWin.btAdd.Content = "Изменить";
            addWin.tbName.Text = "";

            if (addWin.ShowDialog().Value)
            {
                var checkRB = pnAnswers.Children.Cast<UIElement>().FirstOrDefault(x => x is RadioButton rb && (rb.IsChecked ?? false)) as RadioButton;
                if (checkRB != null)
                    checkRB.Content = addWin.tbName.Text;
            }
        }

        private void cancelTestBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                var image = new BitmapImage(new Uri(filePath));

                
                imageInTextBox.Source = image;

                imageInTextBox.Visibility = Visibility.Visible;
                tbTest.Width = 250; 
            }
        }
    }
}
