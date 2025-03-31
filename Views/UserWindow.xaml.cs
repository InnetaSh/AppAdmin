using AppAdmin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace AppAdmin.Views
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public App App => ((App)Application.Current);
        private User _user;
      
        public UserWindow(User user)
        {
            InitializeComponent();
            _user = user;

           _user.Categories = AllCategories();
            App.Users.Add(_user);
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



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Categories.Items.Clear();

         if (App.Users != null && App.Users.Count > 0 && App.Users[0] != null)
            {

                if (App.Users[0].Categories != null && App.Users[0].Categories.Count > 0)
                {
                    foreach (var c in App.Users[0].Categories)
                    {
                        CreateNewTab(c.Title, c);
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


        private void CreateNewTab(string text, Category category)
        {
            var ti = new TabItem()
            {
                Header = text,
                Content = new TestsUsersControl(category),
            
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
    }
}
