using AppAdmin.Models;
using AppAdmin.Views;
using System;
using System.Collections;
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
using System.Text.RegularExpressions;

namespace AppAdmin
{
    /// <summary>
    /// Interaction logic for TestItemControl.xaml
    /// </summary>
    public partial class TestItemChange : Window
    {
        private Question _quest;
        public TestItemChange(Question quest)
        {
            InitializeComponent();
            changeAnswerBtn.Visibility = Visibility.Collapsed;
            delAnswerBtn.Visibility = Visibility.Collapsed;



            _quest = quest;

            if (!string.IsNullOrEmpty(_quest.ImagePath))
            {
                try
                {
                    string imagesPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "img\\back");
                    string imgSrc = System.IO.Path.Combine(imagesPath, $"{_quest.ImagePath}");
                    imageInTextBox.Source = new BitmapImage(new Uri(imgSrc));
                    imageInTextBox.Visibility = Visibility.Visible;
                }
                catch (UriFormatException ex)
                {
                    MessageBox.Show("Неверный формат пути к изображению: " + ex.Message);
                }
            }
            else
            {
                imageInTextBox.Visibility = Visibility.Collapsed;
            }
            foreach (var ans in _quest.Answers)
            {

                if (_quest.IsMultiAnswers)
                {
                    CheckBox chBox = new CheckBox() { Margin = new Thickness(5, 0, 0, 0) };
                    chBox.Content = ans.AnswerText;
                    if (ans.IsCorrect)
                    {
                        chBox.Background = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        chBox.Background = new SolidColorBrush(Colors.Red);
                    }
                    chBox.Tag = ans;
                    chBox.IsChecked = ans.IsCorrect;
                    chBox.Checked += Element_Checked;

                    pnAnswers.Children.Add(chBox);
                }
                else
                {

                    RadioButton rbButton = new RadioButton() { Margin = new Thickness(5, 0, 0, 0) };
                    rbButton.Content = ans.AnswerText;
                    if (ans.IsCorrect)
                    {
                        rbButton.Background = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        rbButton.Background = new SolidColorBrush(Colors.Red);
                    }
                    rbButton.Tag = ans;
                    rbButton.IsChecked = ans.IsCorrect;
                    rbButton.Checked += Element_Checked;
                    pnAnswers.Children.Add(rbButton);
                }
            }
            tbTest.Text = _quest.QuestionText;
            tbWeight.Text = _quest.Weight.ToString();
            tbIsMultiAnswers.IsChecked = _quest.IsMultiAnswers ? (bool?)true : (bool?)false;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

      

        private void addAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AddAnswer(_quest) { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            addWin.Title = "Добавить ответ";
            addWin.tbName.Text = "";


            if (addWin.ShowDialog().Value)
            {
                if (tbIsMultiAnswers.IsChecked == true)
                {
                    CheckBox chBox = new CheckBox() { Margin = new Thickness(5, 0, 0, 0) };
                    chBox.Content = addWin.tbName.Text;
                
                    if (addWin.trueRB.IsChecked == true)
                    {
                        chBox.Background = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        chBox.Background = new SolidColorBrush(Colors.Red);
                    }
                    chBox.Checked += Element_Checked;

                    pnAnswers.Children.Add(chBox);
                }
                else { 

                    RadioButton rbButton = new RadioButton() { Margin = new Thickness(5, 0, 0, 0) };
                    rbButton.Content = addWin.tbName.Text;
                    if (addWin.trueRB.IsChecked == true)
                    {
                        rbButton.Background = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        rbButton.Background = new SolidColorBrush(Colors.Red);
                    }
                    rbButton.Checked += Element_Checked;

                    pnAnswers.Children.Add(rbButton);
                 }
            }
        }

        private void Element_Checked(object sender, RoutedEventArgs e)
        {
            UpdateVisibleBtnState();
        }

        private void UpdateVisibleBtnState()
        {
            var btnVisibleState = pnAnswers.Children.Cast<UIElement>()
                                .Any(x => x is FrameworkElement fe && 
                                ((fe is RadioButton rb && (rb.IsChecked ?? false)) || 
                                (fe is CheckBox cb && (cb.IsChecked ?? false))));

            delAnswerBtn.Visibility = btnVisibleState ? Visibility.Visible : Visibility.Collapsed;
            changeAnswerBtn.Visibility = btnVisibleState ? Visibility.Visible : Visibility.Collapsed;
        }




        private void delAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            var checkElement = pnAnswers.Children.Cast<UIElement>()
                              .FirstOrDefault(x => x is FrameworkElement fe &&
                              ((fe is RadioButton rb && (rb.IsChecked ?? false)) ||
                              (fe is CheckBox cb && (cb.IsChecked ?? false))));

            if (checkElement != null)
            {
                pnAnswers.Children.Remove(checkElement);
            }
            UpdateVisibleBtnState();
        }

        private void changeAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedAnswer = pnAnswers.Children.OfType<UIElement>()
                                .FirstOrDefault(x =>
                                (x is RadioButton rb && (rb.IsChecked ?? false)) ||
                                (x is CheckBox cb && (cb.IsChecked ?? false))) as FrameworkElement;

            Answer answer = selectedAnswer?.Tag as Answer;
            if (answer == null && selectedAnswer != null)
            {
                answer = new Answer
                {
                    AnswerText = selectedAnswer is ContentControl contentControl ? contentControl.Content.ToString() : "",
                    IsCorrect = (selectedAnswer as Control)?.Background != new SolidColorBrush(Color.FromRgb(255, 0, 0))
                };
            }
            var changeWin = new ChangeAnswerWindow(_quest, answer) { WindowStartupLocation = WindowStartupLocation.CenterScreen };
       

            if (changeWin.ShowDialog().Value)
            {
                    if (selectedAnswer is RadioButton rb)
                    {
                    rb.Content = changeWin.tbChangeName.Text;
                    rb.IsChecked = changeWin.trueRB.IsChecked;
                    if (changeWin.trueRB.IsChecked == true)
                    {
                        rb.Background = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        rb.Background = new SolidColorBrush(Colors.Red);
                    }

                }
                    else if (selectedAnswer is CheckBox cb)
                    {
                    cb.Content = changeWin.tbChangeName.Text;
                    cb.IsChecked = changeWin.trueRB.IsChecked;
                    if (changeWin.trueRB.IsChecked == true)
                    {
                        cb.Background = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        cb.Background = new SolidColorBrush(Colors.Red);
                    }
                }
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
                string filePath =  openFileDialog.FileName;
                _quest.ImagePath = System.IO.Path.GetFileName(filePath);
                var image = new BitmapImage(new Uri(filePath));

                imageInTextBox.Source = image;
               

                imageInTextBox.Visibility = Visibility.Visible;
                this.Height = 350;
            }
        }
        private void delImgBtn_Click(object sender, RoutedEventArgs e)
        {
            imageInTextBox.Visibility = Visibility.Collapsed;
            _quest.ImagePath = "";
            this.Height = 250;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbIsMultiAnswers_Checked(object sender, RoutedEventArgs e)
        {
            bool isMultiAnswersChecked = tbIsMultiAnswers.IsChecked ?? false;

            var elementsToChange = pnAnswers.Children.Cast<UIElement>().ToList();

            foreach (var element in elementsToChange)
            {
                if (isMultiAnswersChecked && element is RadioButton rb)
                {
                    var content = rb.Content;
                    var background = rb.Background;
                    var isChecked = rb.IsChecked;
                    var tag = rb.Tag;

                    pnAnswers.Children.Remove(rb);

                    var chBox = new CheckBox() { Content = content, Background = background, Margin = new Thickness(5, 0, 0, 0), Tag = tag };
                    chBox.IsChecked = isChecked;
                    chBox.Checked += Element_Checked; 

                    pnAnswers.Children.Add(chBox);
                }
                else if (!isMultiAnswersChecked && element is CheckBox cb)
                {
                    var content = cb.Content;
                    var background = cb.Background;
                    var isChecked = cb.IsChecked;
                    var tag = cb.Tag;

                    pnAnswers.Children.Remove(cb);

                    var rbButton = new RadioButton() { Content = content, Background = background, Margin = new Thickness(5, 0, 0, 0), Tag = tag };
                    rbButton.IsChecked = isChecked;
                    rbButton.Checked += Element_Checked; 

                    pnAnswers.Children.Add(rbButton);
                }
            }
        }
    }
}
