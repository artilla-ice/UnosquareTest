using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace UnosquareTest.Models
{
    public class EmployeeModel : INotifyPropertyChanged
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        private string m_profileImage = DefaultUserImage;
        public string ProfileImage
        {
            get { return m_profileImage; }
            set
            {
                m_profileImage = value;
                OnPropertyChanged();
            }
        }

        private DateTime m_birthDate = DateTime.Today;
        public DateTime Birthdate
        {
            get { return m_birthDate; }
            set
            {
                m_birthDate = value;
                this.Age = this.CalculateAge();

                OnPropertyChanged();
            }
        }

        public int ExperienceYears { get; set; }


        private int m_age = 0;
        public int Age 
        {
            get { return m_age; }
            set
            {
                m_age = value;

                OnPropertyChanged();
            }
        }

        private int CalculateAge()
        {
            var age = 0;

            if (m_birthDate != DateTime.Today)
                age = DateTime.Today.AddTicks(-m_birthDate.Ticks).Year - 1;

            return age;
        }

        private static readonly string DefaultUserImage = "defaultpp.jpg";

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
