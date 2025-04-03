using System;
using System.Collections.Generic;
using System.IO;
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
using AppAdmin.Models;

namespace AppAdmin.Views
{
    /// <summary>
    /// Interaction logic for TestDescriptionControl.xaml
    /// </summary>
    public partial class UserTestListItemControl : UserControl
    {
        private Test _test;
        private UserQuestionWindow _userQuestionWindow;
        private User _user;
        public UserTestListItemControl(Test test, User user)
        {
            InitializeComponent();
            _test = test;
            _user = user;
            tbDescription.Text = _test.Description;

            string imagesPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "img\\");
            string imgSrc = string.IsNullOrEmpty(_test.ImgSrc) ? System.IO.Path.Combine(imagesPath, "back\\back_default_small.png")
                : System.IO.Path.Combine(imagesPath, $"back\\{_test.ImgSrc}");

            if (System.IO.File.Exists(imgSrc))
                tbImgSrc.Source = new BitmapImage(new Uri(imgSrc, UriKind.RelativeOrAbsolute));
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
           
            _userQuestionWindow = new UserQuestionWindow(_test,_user);
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                _userQuestionWindow.Owner = parentWindow; 
                _userQuestionWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            var question = _test.Questions[0];
            if (!string.IsNullOrEmpty(question.QuestionText))
            {
                _userQuestionWindow.QuestionText.Text = question.QuestionText;
            }
            else
            {
                _userQuestionWindow.QuestionText.Text = "Вопрос отсутствует";
            }
            if (!string.IsNullOrEmpty(question.ImagePath))
            {
                string imagesPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, $"img\\back\\{question.ImagePath}");
                var image = new Image()
                {

                    Source = new BitmapImage(new Uri(imagesPath, UriKind.RelativeOrAbsolute)),
                    Width = 300, 
                    Height =250, 
                    Margin = new Thickness(10) 
                };

               
                _userQuestionWindow.QuestionPanel.Children.Add(image);
            }


            if (question.Answers != null && question.Answers.Count > 0)
            {

                foreach (var answer in question.Answers)
                {
                    var border = new Border()
                    {
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB0B0B0")),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(2),
                        Padding = new Thickness(5),
                        Margin = new Thickness(50, 7, 50, 7)
                    };


                    border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#80FFFFFF"));

                    if (question.IsMultiAnswers)
                    {
                        var cb = new CheckBox()
                        {
                            Content = answer.AnswerText,
                            Margin = new Thickness(10),
                            Tag = answer
                        };
                        border.Child = cb;
                        cb.Checked += Cb_Checked;
                    }
                    else
                    {
                        var rb = new RadioButton()
                        {
                            Content = answer.AnswerText,
                            GroupName = "AnswerGroup",
                            Margin = new Thickness(10),
                            Tag = answer
                        };
                        border.Child = rb;
                        rb.Checked += Cb_Checked;
                    }


                    _userQuestionWindow.AnswersPanel.Children.Add(border);
                }
            }
            else
            {
                var border = new Border()
                {
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB0B0B0")),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(2),
                    Padding = new Thickness(5),
                    Margin = new Thickness(50, 7, 50, 7)
                };

                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#80FFFFFF"));
                var textBlock = new TextBlock()
                {
                    Text = "Нет доступных ответов.",
                    Foreground = new SolidColorBrush(Colors.Red),
                    Margin = new Thickness(10)
                };

                border.Child = textBlock;
                _userQuestionWindow.AnswersPanel.Children.Add(border);
            }

            _userQuestionWindow.StartTest();
            _userQuestionWindow.ShowDialog();
            //this.Visibility = Visibility.Collapsed;
        }

        private void Cb_Checked(object sender, RoutedEventArgs e)
        {
            _userQuestionWindow.NextQuestionButton.IsEnabled = true;
        }
    }
}
