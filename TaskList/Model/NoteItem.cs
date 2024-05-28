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
    public class NoteItem : INotifyPropertyChanged
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string? title;
        public string? Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    NotifyPropertyChanged(nameof(Title));
                }
            }
        }
        private string? noteText;
        public string? NoteText
        {
            get { return noteText; }
            set
            {
                if (noteText != value)
                {
                    noteText = value;
                    NotifyPropertyChanged(nameof(NoteText));
                }
            }
        }

        private bool? isExtended;

        public bool? IsExtended
        {
            get { return isExtended; }
            set
            {
                if (isExtended != value)
                {
                    isExtended = value;
                    NotifyPropertyChanged(nameof(IsExtended));
                }
            }
        }

        public DateTime DateCreated { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }


}
