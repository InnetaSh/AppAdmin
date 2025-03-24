using AppAdmin.Views;
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

namespace AppAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            App.Admins.Add(new Models.Admin());
        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AddWindow() { Owner = this };
            if (addWin.ShowDialog().Value)
            {
                var newCategory = new Models.Category() { Title = addWin.tbName.Text };
                App.Admins[0].Categories.Add(newCategory);

                var ti = new TabItem() { Header = addWin.tbName.Text, Content = new TestsControl(), Tag = newCategory };
                Categories.Items.Add(ti);
                Categories.SelectedIndex = Categories.Items.Count - 1;
            }
        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.Admins[0].SetSelectedCategory(Categories.SelectedIndex - 1);
        }
    }
}