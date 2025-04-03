using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using AppAdmin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppAdmin.Views
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class UserQuestionWindow : Window
    {
        private Test _test;
        private User _user;
        private int i = 0;
        private int _isCorrectAnswer = 0;
        private int _points = 0;
        private int _countHeards = 5;

        private DispatcherTimer _timer; 
        private int _timeRemaining ;

        public UserQuestionWindow(Test test,User user)
        {
            InitializeComponent();
            _test = test;
            _user = user;
            _countHeards = _user.CountHeart;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); 
            _timer.Tick += Timer_Tick;

            _timeRemaining = _test.TimeSec;

            tbHeart.Text = _countHeards.ToString();

            //string imagesPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "img");
            //string imgSrc = string.IsNullOrEmpty(_test.ImgSrc) ? System.IO.Path.Combine(imagesPath, "back\\back_default.png") 
            //    : System.IO.Path.Combine(imagesPath, $"back\\{_test.ImgSrc}");
            //ImgSrc.ImageSource = new BitmapImage(new Uri(imgSrc, UriKind.Relative));

            Answer2.Visibility= Visibility.Collapsed;
            Answer1.Visibility = Visibility.Collapsed;
            brdAnswer2.Visibility = Visibility.Collapsed;
            brdAnswer1.Visibility = Visibility.Collapsed;
            Error.Visibility = Visibility.Collapsed;
            MenuQuestionButton.Visibility = Visibility.Collapsed;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeRemaining--; 

           
            TimerProgressBar.Value = 100 - (_timeRemaining * 100 / _test.TimeSec); 
            TimerText.Text = _timeRemaining.ToString("D2");

           
            if (_timeRemaining == 0)
            {
                _timer.Stop();  
                TimerProgressBar.Visibility = Visibility.Collapsed;

                //AnswersPanel.Visibility = Visibility.Collapsed;
                QuestionText.Visibility = Visibility.Collapsed;
                pnProgressBar.Visibility = Visibility.Collapsed;
                BackQuestionButton.Visibility = Visibility.Collapsed;
                NextQuestionButton.Visibility = Visibility.Collapsed;
                var TimeUpText = "Время вышло!";
                MenuQuestionButton.Visibility = Visibility.Visible;

                var resultTextBlock = new TextBlock()
                {
                    Text = TimeUpText,
                    FontSize = 24,
                    FontFamily = new FontFamily("Arial"),
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.Red),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10)
                };

                AnswersPanel.Children.Add(resultTextBlock);
            }
        }

        public void StartTest()
        {
            _timeRemaining = _test.TimeSec; 
            TimerProgressBar.Value = 0; 
            TimerProgressBar.Visibility = Visibility.Visible; 
            _timer.Start(); 
        }

        private void FinishTestButton_Click(object sender, RoutedEventArgs e)
        {
                _timer.Stop();
                this.Close();
        }
        
        private void MenuhTestButton_Click(object sender, RoutedEventArgs e)
        {
                _timer.Stop();

                var userInfo = new UserInfo();
                userInfo.CorrectAnswerCount = _isCorrectAnswer;
                userInfo.Points = _points;
                userInfo.TestTitle = _test.Title;
                userInfo.Token = _user.Token;
                userInfo.Time = _test.TimeSec - _timeRemaining;
                _user.UserInfos.Add(userInfo);
                SaveUserToBaseTable(userInfo);
                this.Close();
            
        }

        private void GameOver()
        {
            QuestionPanel.Visibility = Visibility.Collapsed;
            QuestionText.Visibility = Visibility.Collapsed;
            pnProgressBar.Visibility = Visibility.Collapsed;
            NextQuestionButton.Visibility = Visibility.Collapsed;
            AnswersPanel.Children.Clear();

            var Text = $"Вы не прошли тест";
            var resultTextBlock = new TextBlock()
            {
                Text = Text,
                FontSize = 24,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Yellow),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(10)
            };
            AnswersPanel.Children.Add(resultTextBlock);

            var resultText = $"У Вас жизни закончились! Подождите восстановления.";
            var resultBlock = new TextBlock()
            {
                Text = resultText,
                FontSize = 20,
                Foreground = new SolidColorBrush(Colors.Green),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(10)
            };
            AnswersPanel.Children.Add(resultBlock);

        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            bool isAnswerSelected = false;
           if (_user.CountHeart == 0)
            {
                GameOver();
            }
            
           
                var question = _test.Questions[i];
                foreach (var child in AnswersPanel.Children)
                {
                    if (child is Border border)
                    {
                        if (border.Child is CheckBox checkBox)
                        {
                            if (checkBox.IsChecked == true)
                            {
                                isAnswerSelected = true;

                                var answer = (Answer)checkBox.Tag;
                                if (answer.IsCorrect)
                                {
                                    _isCorrectAnswer += 1;
                                    _points += question.Weight;
                                }
                                else
                                {
                                    _countHeards -= 1;
                                    _user.CountHeart = _countHeards;
                                    SaveUserCountHeart(_user);
                                    tbHeart.Text = _countHeards.ToString();
                                    if (_user.CountHeart == 0)
                                    {
                                        var currentTime = DateTime.Now;
                                        _user.TimeOfLastHeart = currentTime.ToString();
                                        SaveUserTimeOfLastHeart(_user);
                                       /* GameOver()*/;
                                    }
                                }
                                break; 
                            }
                        }
                  
                    else if (border.Child is RadioButton radioButton)
                    {
                        if (radioButton.IsChecked == true)
                        {
                            isAnswerSelected = true;

                           
                            var answer = (Answer)radioButton.Tag; 
                            if (answer.IsCorrect)
                            {
                                _isCorrectAnswer += 1;
                                _points += question.Weight;
                            }
                            else
                            {
                                _countHeards -= 1;
                                _user.CountHeart = _countHeards;
                                SaveUserCountHeart(_user);
                              
                                tbHeart.Text = _countHeards.ToString();
                                if (_user.CountHeart == 0)
                                {
                                    var currentTime = DateTime.Now;
                                    _user.TimeOfLastHeart = currentTime.ToString();
                                    SaveUserTimeOfLastHeart(_user);
                                    //GameOver();
                                }
                            }

                            break; 
                        }
                    }
                }
            }

           
            if (!isAnswerSelected)
            {
                Error.Text = "Пожалуйста, выберите ответ.";
                Error.Visibility = Visibility.Visible;
                NextQuestionButton.IsEnabled = false;
            }
            else
            {
                    i++;
                if (i == _test.Questions.Count)
                {
                    QuestionPanel.Visibility = Visibility.Collapsed;
                    QuestionText.Visibility = Visibility.Collapsed;
                    pnProgressBar.Visibility = Visibility.Collapsed;
                    NextQuestionButton.Visibility = Visibility.Collapsed;
                    AnswersPanel.Children.Clear();

                    //var userInfo = new UserInfo();
                    //userInfo.CorrectAnswerCount = _isCorrectAnswer;
                    //userInfo.Points = _points;
                    //userInfo.TestTitle = _test.Title;
                    //userInfo.Token = _user.Token;
                    //userInfo.Time = _test.TimeSec - _timeRemaining;
                    //_user.UserInfos.Add(userInfo);
                    //SaveUserToBaseTable(userInfo);

                    BackQuestionButton.Visibility = Visibility.Collapsed;
                    MenuQuestionButton.Visibility = Visibility.Visible;


                    var congratulationText = "";
                    if (_countHeards > 0)
                        congratulationText = "Поздравляем!!! Вы прошли тест";
                    else
                        congratulationText = $"Вы не прошли тест";



                    var resultTextBlock = new TextBlock()
                    {
                        Text = congratulationText,
                        FontSize = 24,
                        FontFamily = new FontFamily("Arial"),
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Colors.Yellow),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(10)
                    };

                    AnswersPanel.Children.Add(resultTextBlock);
                    var resultText = $"Правильных ответов: {_isCorrectAnswer} из {_test.Questions.Count}";

                    var resultBlock = new TextBlock()
                    {
                        Text = resultText,
                        FontSize = 20,
                        Foreground = new SolidColorBrush(Colors.Green),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(10)
                    };

                    var pointText = $"Ваш счёт: {_points}";
                    var pointBlock = new TextBlock()
                    {
                        Text = pointText,
                        FontSize = 20,
                        Foreground = new SolidColorBrush(Colors.Green),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(10)
                    };

                    AnswersPanel.Children.Add(resultBlock);
                    AnswersPanel.Children.Add(pointBlock);
                }
                else
                {
                    question = _test.Questions[i];
                    Error.Text = "";
                    Error.Visibility = Visibility.Collapsed;
                    NextQuestionButton.IsEnabled = true;

                    if (!string.IsNullOrEmpty(question.QuestionText))
                    {
                        QuestionText.Text = question.QuestionText;
                    }
                    else
                    {
                        QuestionText.Text = "Вопрос отсутствует";
                    }
                    if (!string.IsNullOrEmpty(question.ImagePath))
                    {
                        var image = new Image()
                        {
                            Source = new BitmapImage(new Uri(question.ImagePath, UriKind.RelativeOrAbsolute)),
                            Width = 300,
                            Height = 250,
                            Margin = new Thickness(10)
                        };


                        QuestionPanel.Children.Add(image);
                    }

                    AnswersPanel.Children.Clear();
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


                            AnswersPanel.Children.Add(border);
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
                        AnswersPanel.Children.Add(border);
                    }
                }
                   // i++;
                
            }
        }

        private void SaveUserToBaseTable(UserInfo userInfo)
        {
            string url = "http://localhost:5228/api/Admin/userInfo";
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    string json = JsonConvert.SerializeObject(userInfo, Formatting.Indented);
                    //MessageBox.Show(json);

                    var jsonContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    var response = client.PostAsync(url, jsonContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = response.Content.ReadAsStringAsync().Result;

                        MessageBox.Show("Данные успешно сохранены");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка запроса: " + response.StatusCode, "Ошибка");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при запросе: " + ex.Message, "Ошибка");
            }
        }



        private void SaveUserCountHeart(User user)
        {
            string url = "http://localhost:5228/api/Admin/user/heart";
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    string json = JsonConvert.SerializeObject(user, Formatting.Indented);
                    //MessageBox.Show(json);

                    var jsonContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    var response = client.PostAsync(url, jsonContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = response.Content.ReadAsStringAsync().Result;

                        //MessageBox.Show("Данные успешно сохранены");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка запроса: " + response.StatusCode, "Ошибка");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при запросе: " + ex.Message, "Ошибка");
            }
        }

        private void SaveUserTimeOfLastHeart(User user)
        {
            string url = "http://localhost:5228/api/Admin/user/time";
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    string json = JsonConvert.SerializeObject(user, Formatting.Indented);
                    //MessageBox.Show(json);

                    var jsonContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    var response = client.PostAsync(url, jsonContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = response.Content.ReadAsStringAsync().Result;

                        //MessageBox.Show("Данные успешно сохранены");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка запроса: " + response.StatusCode, "Ошибка");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при запросе: " + ex.Message, "Ошибка");
            }
        }



        private void Cb_Checked(object sender, RoutedEventArgs e)
        {
            NextQuestionButton.IsEnabled = true;
        }

    }
}
