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
using System.Xml.Linq;

namespace AppAdmin.Views
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddAnswer : Window
    {
        Question _quest;
        public AddAnswer(Question quest)
        {
            InitializeComponent();

            _quest = quest;
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            _quest.Answers.Add(new Answer() { Id = _quest.Answers.Count, AnswerText = tbName.Text, IsCorrect = trueRB.IsChecked ?? false });
            Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
