using AppAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class ChangeInfoTest : Window
    {
        Test _test;
        public ChangeInfoTest(Test selectedTest)
        {
            InitializeComponent();

            _test = selectedTest;
            tbChangeName.Text = _test.Title;
            tbTime.Text = _test.TimeSec.ToString();

        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            _test.Title = tbChangeName.Text;
            _test.TimeSec = int.TryParse(tbTime.Text, out var time) ? time : 0;
            // _test.IsCorrect = trueRB.IsChecked ?? false;

            Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
