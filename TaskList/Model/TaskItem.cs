using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Model
{
    public class TaskItem : INotifyPropertyChanged
    {
        [PrimaryKey , AutoIncrement]
        public int Id { get; set; }
        private string? name;
        public string? Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        private string? description;
        public string? Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }

        private string? urgency;
        public string? Urgency
        {
            get { return urgency; }
            set
            {
                if (urgency != value)
                {
                    urgency = value;
                    NotifyPropertyChanged(nameof(Urgency));
                }
            }
        }

        private DateTime dateTime;
        public DateTime DateTime
        {
            get { return dateTime; }
            set
            {
                if (dateTime != value)
                {
                    dateTime = value;
                    NotifyPropertyChanged(nameof(DateTime));
                }
            }
        }

        private bool isCompleted;
        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                if (isCompleted != value)
                {
                    isCompleted = value;
                    NotifyPropertyChanged(nameof(IsCompleted));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
