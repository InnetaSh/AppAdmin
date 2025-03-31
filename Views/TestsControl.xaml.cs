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
        private Category _category;
        public TestsControl(Category category)
        {
            InitializeComponent();
            _category = category;
            btAddQuestion.Visibility = Visibility.Collapsed;
            bthangeNameTest.Visibility = Visibility.Collapsed;
            btdelTest.Visibility = Visibility.Collapsed;

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




        private void btnAddTest_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AddWindow() { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            addWin.Title = "Добавить тест";
            addWin.tbName.Text = "Название теста";
            if (addWin.ShowDialog().Value)
            {
                var newTest = new Models.Test() { Title = addWin.tbName.Text };
                TestList.Items.Add(newTest);
                _category.Tests.Add(newTest);
            }
        }

        private void btAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            var newQuestion = new Question();
            var itemChangeWin = new TestItemChange(newQuestion) { WindowStartupLocation = WindowStartupLocation.CenterScreen };


            if (itemChangeWin.ShowDialog().Value)
            {
                var currentTest = TestList.SelectedItem as Test;
                var questText = itemChangeWin.tbTest.Text;
                var answerBtns = itemChangeWin.pnAnswers.Children.Cast<UIElement>().Where(x => x is RadioButton).Select(x => x as RadioButton).ToList();

                
                
                newQuestion.QuestionText = questText;
                currentTest.Questions.Add(newQuestion);

                var ti = new QuestionControl(newQuestion);
                ti.tbTest.Text = newQuestion.QuestionText;

                foreach (var ans in newQuestion.Answers)
                    ti.pnAnswers.Children.Add(new RadioButton() 
                    { 
                        Content = ans.AnswerText, 
                        Margin = new Thickness(5, 0, 0, 0), 
                        Background =  ans.IsCorrect ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red)
                    });

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
                if (pnTestList.Children[i] is QuestionControl ti)
                    pnTestList.Children.Remove(ti);

            var currentTest = TestList.SelectedItem as Test;
            if (currentTest !=null)
            {
                tbTime.Inlines.Clear();
                tbTime.Inlines.Add(new Run("Время:") { FontWeight = FontWeights.Bold });
                tbTime.Inlines.Add(" " + currentTest.TimeSec.ToString());


                foreach (var q in currentTest.Questions)
                {
                    var ti = new QuestionControl(q);
                    ti.tbTest.Text = q.QuestionText;
                    foreach (var answ in q.Answers)
                    {
                        var radioButton = new RadioButton()
                        {
                            Content = answ.AnswerText,
                            Margin = new Thickness(5, 0, 0, 0) 
                        };

                        if (answ.IsCorrect == true)
                        {
                            radioButton.Background = new SolidColorBrush(Colors.Green);  
                        }
                        else
                        {
                            radioButton.Background = new SolidColorBrush(Colors.Red); 
                        }
                          


                        ti.pnAnswers.Children.Add(radioButton);
                    }
                    pnTestList.Children.Add(ti);
                }
            }
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
           
           
            if (TestList.SelectedItem != null)
            {
                var selectedTest = (Test)TestList.SelectedItem;
                var addWin = new ChangeInfoTest( selectedTest) { WindowStartupLocation = WindowStartupLocation.CenterScreen };
               
                if (addWin.ShowDialog().Value)
                {
                    selectedTest.Title = addWin.tbChangeName.Text;
                    tbTime.Inlines.Clear();
                    tbTime.Inlines.Add(new Run("Время:") { FontWeight = FontWeights.Bold });
                    tbTime.Inlines.Add(" " + addWin.tbTime.Text);

                   
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
