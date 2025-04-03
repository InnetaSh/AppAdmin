using AppAdmin.Models;
using AppAdmin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Reflection;
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
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows.Interop;

namespace AppAdmin
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        //private List<User> _users = new List<User>();
        private static readonly string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public LoginWindow()
        {
            InitializeComponent();
            ConfirmPasswordLabel.Visibility = Visibility.Collapsed;
            ConfirmPassword.Visibility = Visibility.Collapsed;
            EmailLabel.Visibility = Visibility.Collapsed;
            Email.Visibility = Visibility.Collapsed;
            IsAdmin.Visibility = Visibility.Collapsed;
        }

        private string HashPassword(string plainPassword)
        {

            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }




        private void Hyperlink_RequestNavigate_Register(object sender, RequestNavigateEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            var button = FindName("Button") as Button;
            Error.Text = "";
            Error.Visibility = Visibility.Collapsed;

            if (hyperlink != null)
            {
                var currentText = (hyperlink.Inlines.FirstInline as Run)?.Text.Trim();

                if (currentText == "Register")
                {
                    UserName.Text = "";
                    Password.Password = "";
                    hyperlink.Inlines.Clear();
                    hyperlink.Inlines.Add("Login");

                    ConfirmPasswordLabel.Visibility = Visibility.Visible;
                    ConfirmPassword.Visibility = Visibility.Visible;

                    EmailLabel.Visibility = Visibility.Visible;
                    Email.Visibility = Visibility.Visible;
                    IsAdmin.Visibility = Visibility.Visible;

                    Header.Content = "Sing In";
                    btnLogin.Content = "SingIn";
                }
                else if (currentText == "Login")
                {
                    UserName.Text = "";
                    Password.Password = "";
                    hyperlink.Inlines.Clear();
                    hyperlink.Inlines.Add("Register");

                    ConfirmPasswordLabel.Visibility = Visibility.Collapsed;
                    ConfirmPassword.Visibility = Visibility.Collapsed;

                    EmailLabel.Visibility = Visibility.Collapsed;
                    Email.Visibility = Visibility.Collapsed;
                    IsAdmin.Visibility = Visibility.Collapsed;

                    Header.Content = "Log In";
                    btnLogin.Content = "LogIn";
                }
            }

        }
    
   
        

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Error.Text = "";
            Error.Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(UserName.Text) || string.IsNullOrEmpty(Password.Password))
            {
                Error.Text = "Все поля должны быть заполнены";
                Error.Visibility = Visibility.Visible;
                return;
            }
            if (ConfirmPassword.Visibility == Visibility.Visible && Password.Password != ConfirmPassword.Password)
            {
                Error.Text = "Пароли не совпадают";
                Error.Visibility = Visibility.Visible;
                return;
            }

            if (btnLogin.Content.ToString() == "SingIn" && !IsValidEmail(Email.Text))
            {
                Error.Text = "Введите корректный email";
                Error.Visibility = Visibility.Visible;
                return;
            }
            var user = new User();
            user.Name = UserName.Text;

            user.Password = Password.Password.Trim();
            if (btnLogin.Content.ToString() == "SingIn")
            {
                user.Email = Email.Text;

                if (IsAdmin.IsChecked == true)
                {
                    var msg = CheckNameExist(UserName.Text);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Error.Visibility = Visibility.Visible;
                        Error.Text = msg;
                        return;
                    }
                    msg = CheckEmailExist(Email.Text);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Error.Visibility = Visibility.Visible;
                        Error.Text = msg;
                        return;
                    }
                    var fitAdmin = SingInAdmin(user);
                    if (fitAdmin == null)
                    {
                        Error.Text = "Не удалось добавить нового пользователя";
                        Error.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                    {
                        Hide();
                        var adminWindow = new AdminWindow(fitAdmin);

                        adminWindow.Owner = this; 
                        adminWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        adminWindow.Show();
                    }
                }
                else
                {
                    var msg = CheckNameExist(UserName.Text);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Error.Visibility = Visibility.Visible;
                        Error.Text = msg;
                        return;
                    }
                    msg = CheckEmailExist(Email.Text);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Error.Visibility = Visibility.Visible;
                        Error.Text = msg;
                        return;
                    }
                
                     var fituser = SingInUser(user);
                    if (fituser == null)
                    {
                        Error.Text = "Не удалось добавить нового пользователя";
                        Error.Visibility = Visibility.Visible;
                        return;
                    }
                    Hide();
                    var userWindow = new UserWindow(fituser);
                    userWindow.Owner = this;
                    userWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    userWindow.Show();
                }
            }
            else if (btnLogin.Content.ToString() == "Login")
            {
                    var fitAdmin = FindAdminByName(user);
                    if (fitAdmin == null)
                    {

                        var fituser = FindUserByName(user);
                        if (fituser == null)
                        {
                            Error.Text = "Не удалось войти в систему";
                            Error.Visibility = Visibility.Visible;
                            return;
                        }
                        Hide();
                        var userWindow = new UserWindow(fituser);
                        userWindow.Owner = this;
                        userWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        userWindow.Show();


                        
                    }
                    else
                    {
                        Hide();
                        var adminWindow = new AdminWindow(fitAdmin);
                        adminWindow.Owner = this;
                        adminWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        adminWindow.Show();
                    }

            }
        }


        private Admin SingInAdmin(User user)
        {
            string url = "http://localhost:5228/api/Admin/singin/admin";
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


                        var fitAdmin = JsonConvert.DeserializeObject<Admin>(responseData);
                        MessageBox.Show("Данные успешно сохранены");
                        return fitAdmin;
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
                // MessageBox.Show("Ошибка при запросе: " + ex.Message, "Ошибка");
                Error.Text = "Нет соединения с сервером!";
                Error.Visibility = Visibility.Visible;
                return null;
            }
        }

        private User SingInUser(User user)
        {
            string url = "http://localhost:5228/api/Admin/singin/user";
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
                        
                        var fitUser = JsonConvert.DeserializeObject<User>(responseData);
                        //MessageBox.Show("Данные успешно сохранены");
                        return fitUser;
                    }
                    else
                    {
                       // MessageBox.Show("Ошибка запроса: " + response.StatusCode, "Ошибка");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Ошибка при запросе: " + ex.Message, "Ошибка");
                Error.Text = "Нет соединения с сервером!";
                Error.Visibility = Visibility.Visible;
                return null;
            }
        }

      

        private Admin FindAdminByName(User user)
        {
            string url = "http://localhost:5228/api/Admin/login/admin";
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

                       
                        var fitAdmin = JsonConvert.DeserializeObject<Admin>(responseData);

                       // MessageBox.Show("Данные успешно получены.");
                        return fitAdmin;  
                    }
                    else
                    {
                        //MessageBox.Show("Ошибка запроса: " + response.StatusCode, "Ошибка");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Ошибка при запросе: " + ex.Message, "Ошибка");
                Error.Text = "Нет соединения с сервером!";
                Error.Visibility = Visibility.Visible;
                return null;
            }
        }


        private User FindUserByName(User user)
        {
            string url = "http://localhost:5228/api/Admin/login/user";
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


                        var fitUser = JsonConvert.DeserializeObject<User>(responseData);

                        //MessageBox.Show("Данные успешно получены.");
                        return fitUser;
                    }
                    else
                    {
                        // MessageBox.Show("Ошибка запроса: " + response.StatusCode, "Ошибка");
                        Error.Text = "Нет соединения с сервером!";
                        Error.Visibility = Visibility.Visible;
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Ошибка при запросе: " + ex.Message, "Ошибка");
                Error.Text = "Нет соединения с сервером!";
                Error.Visibility = Visibility.Visible;
                return null;
            }
        }

        private bool IsValidEmail(string email)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(email, EmailPattern);
        }

        private async void UserName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (btnLogin.Content.ToString() == "Login")
            {
                return;
            }
                var msg = CheckNameExist(UserName.Text);
            if (!string.IsNullOrEmpty(msg))
            {
                Error.Visibility = Visibility.Visible;
                Error.Text = msg;
            }
            else Error.Visibility = Visibility.Collapsed;
        }

        private  void EmailLabel_LostFocus(object sender, RoutedEventArgs e)
        {
            var msg =  CheckEmailExist(Email.Text);
            if(!string.IsNullOrEmpty(msg))
            {
                Error.Visibility = Visibility.Visible;
                Error.Text = msg;
            }
            else Error.Visibility = Visibility.Collapsed;
        }

        private string CheckNameExist(string name)
        {
            string url = "http://localhost:5228/api/Admin/checkUsernameExists";
            //string name = UserName.Text;
            var msg = "";

            try
            {
                using (HttpClient client = new HttpClient())
                {

                    var response =  client.GetAsync($"{url}?name={name}").Result;

                    if (response.IsSuccessStatusCode)
                    {

                        string responseContent =  response.Content.ReadAsStringAsync().Result;
                        bool isUsernameTaken = bool.TryParse(responseContent, out bool result) && result;

                        if (isUsernameTaken)
                        {
                            //Error.Visibility = Visibility.Visible;
                            msg = "Это имя пользователя уже занято.";
                        }
                        //else
                        //{
                        //    Error.Visibility = Visibility.Collapsed;
                        //}
                    }
                    else
                    {
                        //Error.Visibility = Visibility.Visible;
                         msg = "Ошибка при проверке email пользователя.";
                    }
                }
            }
            catch (Exception ex)
            {
                //Error.Visibility = Visibility.Visible;
                msg = "Ошибка при запросе: " + ex.Message;
            }
            return msg;
        }

        private string CheckEmailExist(string email)
        {
            var msg = "";
            string url = "http://localhost:5228/api/Admin/checkEmailExists";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = client.GetAsync($"{url}?email={email}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = response.Content.ReadAsStringAsync().Result; 

                        bool isUsernameTaken = bool.TryParse(responseContent, out bool result) && result;

                        if (isUsernameTaken)
                        {
                            //Error.Visibility = Visibility.Visible;
                            msg = "Этот email пользователя уже занят.";
                        }
                        //else
                        //{
                        //    Error.Visibility = Visibility.Collapsed;
                        //}
                    }
                    else
                    {
                        //Error.Visibility = Visibility.Visible;
                        msg = "Ошибка при проверке email пользователя.";
                    }
                }
            }
            catch (Exception ex)
            {
                //Error.Visibility = Visibility.Visible;
                msg = "Ошибка при запросе: " + ex.Message;
            }

            return msg;
        }
    }
    
}
