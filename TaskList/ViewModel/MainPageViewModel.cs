using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskList.Model;
using TaskList.Data;
using System.Diagnostics;

namespace TaskList.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        Conn Db;
        public ICommand SaveTask { get; set; }

        private string textEntry;
        public string TextEntry
        {
            get { return textEntry; }
            set
            {
                if (textEntry != value)
                {
                    textEntry = value;
                    NotifyPropertyChanged(nameof(TextEntry));
                }
            }
        }
        private int urgency;
        public int Urgency
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


        private ObservableCollection<List<TaskItem>> items;
        public ObservableCollection<List<TaskItem>> Items
        {
            get { return items; }
            set
            {
                if (items != value)
                {
                    items = value;
                    NotifyPropertyChanged(nameof(Items));
                }
            }
        }
        public MainPageViewModel(Conn conn)
        {
            Debug.WriteLine("View model succesfully initialized");
            //SaveTask = new Command(saveItem);
            SaveTask = new Command(async () => await saveItem());


            Db = conn;
            Items = new ObservableCollection<List<TaskItem>>();
            GetItems();
        }

        public async void GetItems()
        {
            items.Clear();
            await Db.GetItemsAsync();
            foreach(List<TaskItem> item in Items)
            {
                Debug.WriteLine(
            }
        }

        public async Task saveItem()
        {
            if (!string.IsNullOrEmpty(TextEntry))
            {
                var newItemList = new List<TaskItem>()
                {
                    new TaskItem()
                    {
                        Name = TextEntry,
                        Description = null,
                        DateTime = DateTime.UtcNow,
                        urgency = Urgency,
                        IsCompleted = false
                    }
                   
                };


                Items.Add(newItemList);
                foreach(TaskItem item in newItemList)
                {
                   await Db.SaveItemAsync(item);
                   Debug.WriteLine("Item name is " + item.Name);
                }
                Debug.WriteLine("Number of items in items is " + Items.Count);
                GetItems();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
