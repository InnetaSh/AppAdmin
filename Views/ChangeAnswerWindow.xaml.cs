using AppAdmin.Models;
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
using System.Windows.Shapes;

namespace AppAdmin.Views
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class ChangeAnswerWindow : Window
    {
        Question _quest;
        private Answer _selectedAnswer;
        public ChangeAnswerWindow(Question quest, Answer selectedAnswer)
        {
            InitializeComponent();

            _quest = quest;
            _selectedAnswer = selectedAnswer;

            if (_selectedAnswer != null)
            {
                tbChangeName.Text = _selectedAnswer.AnswerText;
               
                if (_selectedAnswer.IsCorrect)
                {
                    trueRB.IsChecked = true; 
                }
                else
                {
                    falseRB.IsChecked = true; 
                }
            }
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            var fitAnswr = _quest.Answers.FirstOrDefault(a => a.Id == _selectedAnswer.Id); 

            if (fitAnswr != null)
            {

                _selectedAnswer.AnswerText = tbChangeName.Text;
                _selectedAnswer.IsCorrect = trueRB.IsChecked ?? false;
            }
            else
            {
                Console.WriteLine("Ответ не найден.");
            }

            Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
