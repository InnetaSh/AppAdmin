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
using AppAdmin.Models;

namespace AppAdmin.Views
{
    /// <summary>
    /// Interaction logic for TestDescriptionControl.xaml
    /// </summary>
    public partial class TestDescriptionControl : UserControl
    {
        private Test _test;
        private TestWindow _testWindow;
        public TestDescriptionControl(Test test)
        {
            InitializeComponent();
            _test = test;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
             _testWindow = new TestWindow(_test);
            var question = _test.Questions[0];
            if (!string.IsNullOrEmpty(question.QuestionText))
            {
                _testWindow.QuestionText.Text = question.QuestionText;
            }
            else
            {
                _testWindow.QuestionText.Text = "Вопрос отсутствует";
            }
            if (!string.IsNullOrEmpty(question.ImagePath))
            {
                var image = new Image()
                {
                    Source = new BitmapImage(new Uri(question.ImagePath, UriKind.RelativeOrAbsolute)),
                    Width = 200, 
                    Height = 150, 
                    Margin = new Thickness(10) 
                };

               
                _testWindow.QuestionPanel.Children.Add(image);
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


                    _testWindow.AnswersPanel.Children.Add(border);
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
                _testWindow.AnswersPanel.Children.Add(border);
            }

            _testWindow.StartTest();
            _testWindow.Show();
            this.Visibility = Visibility.Collapsed;
        }

        private void Cb_Checked(object sender, RoutedEventArgs e)
        {
            _testWindow.NextQuestionButton.IsEnabled = true;
        }
    }
}
