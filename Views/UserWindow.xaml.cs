using AppAdmin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using System.Xml.Linq;

namespace AppAdmin.Views
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public App App => ((App)Application.Current);
        private User _user;

        private int _heartCount = 5;

      

        public UserWindow(User user)
        {
            InitializeComponent();
            _user = user;

            _user.Categories = AllCategories();
            _user.UserInfos = UserInfo(_user.Token);
            _heartCount = _user.CountHeart;
            App.Users.Add(_user);

            DataContext = _user;
        }


        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in Categories.Items)
            {
                if (item is TabItem tabItem)
                {
                    var headerPanel = tabItem.Header as StackPanel;
                   
                }
            }

            if (Categories.SelectedIndex >= 0 && Categories.SelectedIndex < Categories.Items.Count)
            {
                //App.Admins[0].SetSelectedCategory(Categories.SelectedIndex);
            }
        }




        private List<Category> AllCategories()
        {
            string url = "http://localhost:5228/api/Admin/allTests";
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    var webRequest = new HttpRequestMessage(HttpMethod.Get, url);
                    var response = client.Send(webRequest);
                    if (response.IsSuccessStatusCode)
                    {

                        string responseData = response.Content.ReadAsStringAsync().Result;


                        var categoryList = JsonConvert.DeserializeObject<List<Category>>(responseData);

                        return categoryList;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка запроса: " + response.StatusCode, "Ошибка");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при запросе: " + ex.Message, "Ошибка");
                return null;
            }
        }


        private List<UserInfo> UserInfo(string Token)
        {
            string url = "http://localhost:5228/api/Admin/user/userInfo";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response =  client.GetAsync($"{url}?Token={Token}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string json =  response.Content.ReadAsStringAsync().Result;
                        var userInfos = JsonConvert.DeserializeObject<List<UserInfo>>(json);

                        return userInfos ?? new List<UserInfo>(); 
                    }
                    else
                    {
                        return new List<UserInfo>(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запросе: {ex.Message}", "Ошибка");
                return new List<UserInfo>();
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int t = DifferentByTime();
            if (t == 0 && _user.CountHeart == 0)
            {

            }
            else 
            {
                if(t == 5)
    {
                    _user.CountHeart = Math.Min(_user.CountHeart + 1, 5);
                }
                else if (t == 10)
                {
                    _user.CountHeart = Math.Min(_user.CountHeart + 2, 5);
                }
                else if (t == 15)
                {
                    _user.CountHeart = Math.Min(_user.CountHeart + 3, 5);
                }
                else if (t == 20)
                {
                    _user.CountHeart = Math.Min(_user.CountHeart + 4, 5);
                }
                else if (t == 25)
                {
                    _user.CountHeart = Math.Min(_user.CountHeart + 5, 5);
                }
            }


            if (Categories.Items.Count > 2)
            {
                for (int i = Categories.Items.Count - 1; i > 1; i--)
                {
                    Categories.Items.RemoveAt(i);
                }
            }
          

            if (App.Users != null && App.Users.Count > 0 && App.Users[0] != null)
            {

                if (App.Users[0].Categories != null && App.Users[0].Categories.Count > 0)
                {
                    foreach (var c in App.Users[0].Categories)
                    {
                        CreateNewTab(c.Title, c, App.Users[0]);
                    }
                }
                else
                {
                    Console.WriteLine("Categories is null or empty.");
                }
            }
            else
            {
                Console.WriteLine("Admins list is null or empty, or Admin[0] is null.");
            }
          
        }

        private int DifferentByTime()
        {
            if (string.IsNullOrEmpty(_user.TimeOfLastHeart))
            {
                Console.WriteLine("Ошибка: Время последнего сердца отсутствует.");
                return 0;
            }

            string timeOfLastHeartString = _user.TimeOfLastHeart; 
            DateTime currentTime = DateTime.Now;
            DateTime timeOfLastHeart;

            
            string format = "dd.MM.yyyy H:mm:ss";
            int t;
            if (DateTime.TryParseExact(timeOfLastHeartString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out timeOfLastHeart))
            {
                TimeSpan difference = currentTime - timeOfLastHeart;
                int minutesPassed = (int)difference.TotalMinutes;

                if (minutesPassed >= 25)
                {
                    Console.WriteLine("Прошло 20 или более минут.");
                    t = 25;
                }
                else if (minutesPassed >= 20)
                {
                    Console.WriteLine("Прошло 20 или более минут.");
                    t = 20;
                }
                else if (minutesPassed >= 15)
                {
                    Console.WriteLine("Прошло 15 минут.");
                    t = 15;
                }
                else if (minutesPassed >= 10)
                {
                    Console.WriteLine("Прошло 10 минут.");
                    t = 10;
                }
                else if (minutesPassed >= 5)
                {
                    Console.WriteLine("Прошло 5 минут.");
                    t = 5;
                }
                else
                {
                    Console.WriteLine("Прошло меньше 5 минут.");
                    t = 0;
                }
            }
            else
            {
                Console.WriteLine("Ошибка: Неверный формат даты.");
                t = 0;
            }
            return t;
        }


        private void CreateNewTab(string text, Category category, User user)
        {
            var ti = new TabItem()
            {
                Header = text,
                Content = new UserTestsListControl(category,user),
            
                //Tag = category
            };
        

            var headerPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };

            var headerText = new TextBlock()
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 5, 0)
            };
           
            headerPanel.Children.Add(headerText);

            ti.Header = headerPanel;


            Categories.Items.Add(ti);
        }

        private void Cabinet_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new UserCabinetWindow(_user) { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            newWindow.Show();
        }
    }
}
