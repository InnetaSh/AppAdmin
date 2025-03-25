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
            addWin.Title = "Добавить тест";
            addWin.tbName.Text = "Название теста";
            if (addWin.ShowDialog().Value)
            {
                TestList.Items.Add(new Test() { Title = addWin.tbName.Text });
            }
        }

        private void btAddQuestion_Click(object sender, RoutedEventArgs e)
        {

            ImageSource imageSource = null;
            List<RadioButton> radioButtons = new List<RadioButton>();
            string text = "";

            var itemChangeWin = new TestItemChange(text, imageSource, radioButtons) { WindowStartupLocation = WindowStartupLocation.CenterScreen };


            if (itemChangeWin.ShowDialog().Value)
            {
                var ti = new TestItemControl() { };

                ti.tbTest.Text = itemChangeWin.tbTest.Text;
                var answerBtns = itemChangeWin.pnAnswers.Children.Cast<UIElement>().Where(x => x is RadioButton).Select(x => x as RadioButton).ToList();
                foreach (var rb in answerBtns)
                    ti.pnAnswers.Children.Add(new RadioButton() { Content = rb.Content, Margin = new Thickness(5, 0, 0, 0) });

                if (itemChangeWin.imageInTextBox.Visibility == Visibility.Visible)
                {
                    ti.imageInTextBox.Source = itemChangeWin.imageInTextBox.Source;
                    ti.imageInTextBox.Visibility = Visibility.Visible;
                    ti.tbTest.Width = 250;
                }

                var delBtn = ti.FindName("delTestBtn") as Button;  

               
                if (delBtn != null)
                {
                    delBtn.Click += (sender, e) =>
                    {
                        pnTestList.Children.Remove(ti);
                    };
                }
                pnTestList.Children.Add(ti);
            }
        }

        private void TestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btAddQuestion.Visibility = TestList.Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            btdelTest.Visibility = TestList.Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            bthangeNameTest.Visibility = TestList.Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            for (var i = pnTestList.Children.Count - 1; i >= 0; i--)
                if (pnTestList.Children[i] is TestItemControl ti)
                    pnTestList.Children.Remove(ti);
        }

        private void btnDelTest_Click(object sender, RoutedEventArgs e)
        {
            if (TestList.SelectedItem != null)
            {
                TestList.Items.Remove(TestList.SelectedItem);
            }
            else
            {
                MessageBox.Show("Выберите элемент для удаления.");
            }
        }

        private void btnChangeNameTest_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AddWindow() { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            addWin.Title = "Изменить название теста";
            if (TestList.SelectedItem != null)
            {
                var selectedTest = (Test)TestList.SelectedItem;
                addWin.tbName.Text = selectedTest.Title;  
                if (addWin.ShowDialog().Value)
                {
                    selectedTest.Title = addWin.tbName.Text;
                    TestList.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Выберите тест для изменения!");
            }
        }
    }
}
