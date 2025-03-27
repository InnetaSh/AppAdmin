using AppAdmin.Models;
using AppAdmin.Views;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace AppAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public App App => ((App)Application.Current);
        public AdminWindow()
        {
            InitializeComponent();
            var admin = new Models.Admin();
            admin.Name= "admin";

            admin.Categories = AllCategories();
            App.Admins.Add(admin);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Admins != null && App.Admins.Count > 0 && App.Admins[0] != null)
            {
                
                if (App.Admins[0].Categories != null && App.Admins[0].Categories.Count > 0)
                {
                    foreach (var c in App.Admins[0].Categories)
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

        //-----------------Server-----------------------------------------------

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




        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    SaveDataToDatabase();
        //}
        private void SaveDataToDatabase(object sender, RoutedEventArgs e)
        {
            string url = "http://localhost:5228/api/Admin/allTests";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var admin = App.Admins[0];
                    //var admin = new Admin
                    //{
                    //    Name = "авававав",
                    //    Categories = new List<Category>
                    //        {
                    //            new Category("qqq", true)
                    //            {
                    //                Tests = new List<Test>
                    //                {
                    //                    new Test()
                                        
                                        
                                        
                    //                }
                    //            }
                    //        }
                    //};

                    string json = JsonConvert.SerializeObject(admin, Formatting.Indented);
                    //MessageBox.Show(json);

                    var jsonContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    var response = client.PostAsync(url, jsonContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = response.Content.ReadAsStringAsync().Result; // Получаем ответ синхронно
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

        //-----------------Server-----------------------------------------------




        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AddWindow() { Owner = this };
            addWin.Title = "Добавить категорию";
            addWin.tbName.Text = "Название категории";
            if (addWin.ShowDialog().Value)
            {
                var newCategory = new Models.Category() { Title = addWin.tbName.Text };
                App.Admins[0].Categories.Add(newCategory);

                var text = addWin.tbName.Text;

                CreateNewTab(text, App.Admins[0].Categories.Last());

                if (Categories.Items.Count > 0)
                {
                    App.Admins[0].SetSelectedCategory(Categories.Items.Count - 1);
                }
            }
        }

        

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in Categories.Items)
            {
                if (item is TabItem tabItem)
                {
                    var headerPanel = tabItem.Header as StackPanel;
                    if (headerPanel != null)
                    {
                        var deleteButton = headerPanel.Children.OfType<Button>().FirstOrDefault();
                        if (deleteButton != null)
                        {
                            if (tabItem.IsSelected)
                            {
                                deleteButton.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                deleteButton.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                }
            }

            if (Categories.SelectedIndex >= 0 && Categories.SelectedIndex < Categories.Items.Count)
            {
                App.Admins[0].SetSelectedCategory(Categories.SelectedIndex);
            }
        }

        private void DeleteTab_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
               
                TabItem tabItem = btn.TemplatedParent as TabItem;
                if (tabItem != null)
                {
                    Categories.Items.Remove(tabItem);
                }
            }
        }




        private void CreateNewTab(string text, Category category)
        {
            var ti = new TabItem()
            {
                Header = text,
                Content = new TestsControl(category),
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
            var btnDelete = new Button()
            {
                Width = 24,
                Height = 24,
                VerticalAlignment = VerticalAlignment.Center,
                BorderBrush = new SolidColorBrush(Colors.Transparent),
                Background = new SolidColorBrush(Colors.Transparent),
                Visibility = Visibility.Collapsed
            };

            btnDelete.Content = new Image()
            {
                Source = new BitmapImage(new Uri("img/icon_delete.png", UriKind.Relative)),

                Stretch = Stretch.Fill
            };

            btnDelete.Click += (s, e) =>
            {
                Categories.Items.Remove(ti);
            };

            headerPanel.Children.Add(headerText);
            headerPanel.Children.Add(btnDelete);

            ti.Header = headerPanel;


            Categories.Items.Add(ti);
        }

      
    }

}