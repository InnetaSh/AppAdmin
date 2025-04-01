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
using AppAdmin.Views;

namespace AppAdmin
{
    /// <summary>
    /// Interaction logic for TestsControl.xaml
    /// </summary>
    public partial class TestsUsersControl : UserControl
    {
        private Category _category;
        private User _user;
        public TestsUsersControl(Category category, User user)
        {
            InitializeComponent();
            _category = category;
            _user = user;   
        }


        private void TestsControl_Loaded(object sender, RoutedEventArgs e)
        {
            TestList.Items.Clear();

            if (_category != null && _category.Tests.Count > 0)
            {
                foreach (var t in _category.Tests)
                {
                    TestList.Items.Add(t);
                }
            }
        }

        private void TestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentTest = TestList.SelectedItem as Test;
            if (currentTest != null)
            {
                var ti = new TestDescriptionControl(currentTest,_user);
                  
                pnTestList.Children.Add(ti);
                
            }
        }
    }
}
