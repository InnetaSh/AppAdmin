using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AppAdmin.Models
{
    public class Admin : User
    {
     
        public List<Category> Categories { get; set; } = new List<Category>();

        public Admin() { }
        public void SetSelectedCategory(int index)
        {
            if(Categories == null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Сервер не запущен");
                }), DispatcherPriority.Normal);
                return;
            }
            if (index >= 0 && index < Categories.Count)
            {
                Categories.ForEach(x => x.IsSelected = false);
                Categories[index].IsSelected = true;
            }
            else
            {
                Console.WriteLine("Индекс выходит за пределы списка Categories.");
            }
        }
    }
}
