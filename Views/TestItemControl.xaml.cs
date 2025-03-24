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

namespace AppAdmin
{
    /// <summary>
    /// Interaction logic for TestItemControl.xaml
    /// </summary>
    public partial class TestItemControl : UserControl
    {
        private Boolean _ForAdmin;
        public Boolean ForAdmin { get => _ForAdmin; set 
            {
                _ForAdmin = value;
                delTestBtn.Visibility = _ForAdmin ? Visibility.Visible : Visibility.Collapsed;
                addAnswerBtn.Visibility = _ForAdmin ? Visibility.Visible : Visibility.Collapsed;
                delAnswerBtn.Visibility = _ForAdmin ? Visibility.Visible : Visibility.Collapsed;
                tbTest.IsEnabled = _ForAdmin;
                SaveOrNextBtn.Content = _ForAdmin ? "Сохранить" : "Далее";
            }
        }
        public TestItemControl()
        {
            InitializeComponent();
        }

        private void SaveOrNextBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void delTestBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            pnAnswers.Children.Add(new RadioButton());
        }

        private void delAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            var checkRB = pnAnswers.Children.Cast<UIElement>().FirstOrDefault(x => x is RadioButton rb && (rb?.IsChecked ?? false));
            if (checkRB != null)
                pnAnswers.Children.Remove(checkRB);
        }
    }
}
