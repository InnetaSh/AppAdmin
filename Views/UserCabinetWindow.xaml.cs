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
    /// Interaction logic for CabinetWindow.xaml
    /// </summary>
    public partial class UserCabinetWindow : Window
    {
        private User _user;
        public UserCabinetWindow(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbLogin.Text = _user.Name;

            var cabinetControlTitle = new UserCabitetControl(_user);

            cabinetControlTitle.tbTestTitle.Text = "Название теста";
            cabinetControlTitle.tbCountAnswers.Text = "Правильныx ответов";
            cabinetControlTitle.tbCountPoints.Text = "Количество очков";
            cabinetControlTitle.tbCountTime.Text = "Время(сек)";
            UserListBox.Items.Add(cabinetControlTitle);

            foreach ( var ui in _user.UserInfos)
            {
                var cabinetControl = new UserCabitetControl(_user);
                var totalTestCount = _user.Categories
                                    .Where(c => c.Tests != null)
                                    .SelectMany(c => c.Tests)
                                    .Where(t => t.Title == ui.TestTitle)
                                    .SelectMany(t => t.Questions)
                                    .Count();

                cabinetControl.tbTestTitle.Text = ui.TestTitle;
                cabinetControl.tbCountAnswers.Text = $"{ui.CorrectAnswerCount} из {totalTestCount}";
                cabinetControl.tbCountPoints.Text = ui.Points.ToString(); 
                cabinetControl.tbCountTime.Text = ui.Time.ToString(); 
                UserListBox.Items.Add(cabinetControl);
            }
        }
    }
}
