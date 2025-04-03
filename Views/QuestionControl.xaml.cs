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
    public partial class QuestionControl : UserControl
    {
        private Question _quest;
        private Test _test;
        private Boolean _ForAdmin;
        public Boolean ForAdmin { get => _ForAdmin; set 
            {
                _ForAdmin = value;
                delTestBtn.Visibility = _ForAdmin ? Visibility.Visible : Visibility.Collapsed;
                tbTest.IsEnabled = _ForAdmin;
                changeTestBtn.Content = _ForAdmin ? "Изменить" : "Далее";
            }
        }
        public QuestionControl(Question quest, Test test)
        {
            InitializeComponent();
            changeTestBtn.Visibility = Visibility.Collapsed;
            delTestBtn.Visibility = Visibility.Collapsed;
            btnColumn.Visibility = Visibility.Collapsed;
            _quest = quest;
            _test = test;
            tblWeight.Text = "Вес: " + _quest.Weight.ToString(); 
        }

        private void ChangeTestBtn_Click(object sender, RoutedEventArgs e)
        {
            List<RadioButton> radioButtons = new List<RadioButton>();
            foreach (var child in pnAnswers.Children)
            {
                if (child is RadioButton radioButton)  
                {
                    radioButtons.Add(radioButton);
                }
            }

            var itemChangeWin = new TestItemChange(_quest) { WindowStartupLocation = WindowStartupLocation.CenterScreen };
           

            if (itemChangeWin.ShowDialog().Value)
            {
                _quest.QuestionText = itemChangeWin.tbTest.Text;
                _quest.Weight = int.TryParse(itemChangeWin.tbWeight.Text, out var weight) ? weight : 0;
                _quest.IsMultiAnswers = itemChangeWin.tbIsMultiAnswers.IsChecked == true;

                tblWeight.Text ="Вес: " + itemChangeWin.tbWeight.Text;
                tbTest.Text = _quest.QuestionText; 


                for (var i = pnAnswers.Children.Count - 1; i >= 0; i--)
                    if (pnAnswers.Children[i] is RadioButton rb)
                        pnAnswers.Children.Remove(rb);

                foreach (var answ in _quest.Answers)
                {
                    RadioButton newRadioButton = new RadioButton() { Margin = new Thickness(5, 0, 0, 0) };
                    newRadioButton.Content = answ.AnswerText;
                    if (answ.IsCorrect == true)
                    {
                        newRadioButton.Background = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        newRadioButton.Background = new SolidColorBrush(Colors.Red);
                    }

                    pnAnswers.Children.Add(newRadioButton);
                }
            }
       
            if (!string.IsNullOrEmpty(_quest.ImagePath))
            {
                try
                {
                    string imagesPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "img\\back");
                    string imgSrc = System.IO.Path.Combine(imagesPath, $"{_quest.ImagePath}");

                    imageInTextBox.Source = new BitmapImage(new Uri(imgSrc));
                    imageInTextBox.Visibility = Visibility.Visible;
               
                    this.Height = 300;
                }
                catch (UriFormatException ex)
                {
                    MessageBox.Show("Неверный формат пути к изображению: " + ex.Message);
                }
            }
            else
            {
                imageInTextBox.Visibility = Visibility.Collapsed;
                this.Height = 200;
            }
        }



        private void delTestBtn_Click(object sender, RoutedEventArgs e)
        {
            var parentControl = this;  

            var parentPanel = (Panel)this.Parent;  
            parentPanel.Children.Remove(parentControl);
            _test.Questions.Remove(_quest);
        }

        private void GridContent_GotFocus(object sender, RoutedEventArgs e)
        {
            changeTestBtn.Visibility = Visibility.Visible;
            delTestBtn.Visibility = Visibility.Visible;
            btnColumn.Visibility = Visibility.Visible;
        }

        private void GridContent_LostFocus(object sender, RoutedEventArgs e)
        {
            changeTestBtn.Visibility = Visibility.Collapsed;
            delTestBtn.Visibility = Visibility.Collapsed;
            btnColumn.Visibility = Visibility.Collapsed;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustHeight();
        }

        private void AdjustHeight()
        {
           
            double contentHeight = 0;

            contentHeight += tbTest.ActualHeight;
            contentHeight += pnAnswers.ActualHeight;
            if (imageInTextBox.Visibility == Visibility.Visible)
            {
                contentHeight += imageInTextBox.ActualHeight;
                contentHeight -= tbTest.ActualHeight;
            }

            this.Height = contentHeight + 20;
        }
    }
}
