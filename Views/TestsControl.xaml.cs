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
    public partial class TestsControl : UserControl
    {
        public TestsControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AddWindow() { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            if (addWin.ShowDialog().Value)
            {
                TestList.Items.Add(new Test() { Title = addWin.tbName.Text });
            }
        }

        private void btAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            var ti = new TestItemControl() { };
            pnTestList.Children.Add(ti);
        }

        private void TestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (var i = pnTestList.Children.Count - 1; i >= 0; i--)
                if (pnTestList.Children[i] is TestItemControl ti)
                    pnTestList.Children.Remove(ti);
        }
    }
}
