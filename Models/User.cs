using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAdmin.Models
{
    public class User : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public string TimeOfLastHeart { get; set; } 

        private int _countHeart;
        public int CountHeart
        {
            get => _countHeart;
            set
            {
                if (_countHeart != value)
                {
                    _countHeart = value;
                    OnPropertyChanged(nameof(CountHeart)); 
                }
            }
        }

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<UserInfo> UserInfos { get; set; } = new List<UserInfo>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class UserInfo
    {
        public string Id { get; set; }
        public string TestTitle { get; set; }
        public int CorrectAnswerCount { get; set; }
        public int Points { get; set; }
        public int Time {  get; set; }
        public string Token { get; set; }

    }
}
