using System.Configuration;
using System.Data;
using System.Windows;
using AppAdmin.Models;

namespace AppAdmin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static List<Admin> Admins = new List<Admin>();
    }

}
