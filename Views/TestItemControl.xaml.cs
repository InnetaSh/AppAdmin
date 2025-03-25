using AppAdmin.Models;
using AppAdmin.Views;
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
                tbTest.IsEnabled = _ForAdmin;
                changeOrNextBtn.Content = _ForAdmin ? "Изменить" : "Далее";
            }
        }
        public TestItemControl()
        {
            InitializeComponent();
        }

        private void ChangeOrNextBtn_Click(object sender, RoutedEventArgs e)
        {
            var imageSource = imageInTextBox.Source != null ? imageInTextBox.Source : null;

            List<RadioButton> radioButtons = new List<RadioButton>();
            foreach (var child in pnAnswers.Children)
            {
                if (child is RadioButton radioButton)  
                {
                    radioButtons.Add(radioButton);
                }
            }

            var itemChangeWin = new TestItemChange(tbTest.Text, imageSource, radioButtons) { WindowStartupLocation = WindowStartupLocation.CenterScreen };
           

            if (itemChangeWin.ShowDialog().Value)
            {
                tbTest.Text = itemChangeWin.tbTest.Text;
                var answerBtns = itemChangeWin.pnAnswers.Children.Cast<UIElement>().Where(x => x is RadioButton).Select(x=> x as RadioButton).ToList();
                foreach(var rb in answerBtns)
                    pnAnswers.Children.Add(new RadioButton(){Content = rb.Content, Margin = new Thickness(5,0,0,0)});
            }
            if (itemChangeWin.imageInTextBox.Visibility == Visibility.Visible)
            {
                imageInTextBox.Source = itemChangeWin.imageInTextBox.Source;
                imageInTextBox.Visibility = Visibility.Visible;
                tbTest.Width = 250;
            }
            changeOrNextBtn.Visibility = Visibility.Collapsed;
            delTestBtn.Visibility = Visibility.Collapsed;
        }



        private void delTestBtn_Click(object sender, RoutedEventArgs e)
        {
            var parentControl = this;  

            var parentPanel = (Panel)this.Parent;  
            parentPanel.Children.Remove(parentControl);  
        }

        private void GridContent_GotFocus(object sender, RoutedEventArgs e)
        {
            changeOrNextBtn.Visibility = Visibility.Visible;
            delTestBtn.Visibility = Visibility.Visible;
        }

        private void GridContent_LostFocus(object sender, RoutedEventArgs e)
        {
            changeOrNextBtn.Visibility = Visibility.Collapsed;
            delTestBtn.Visibility = Visibility.Collapsed;
        }
    }
}
