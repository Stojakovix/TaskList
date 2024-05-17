using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskList.Model;
using TaskList.Data;
using System.Diagnostics;
using System.Text;

namespace TaskList.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        Conn Db;
        public ICommand SaveTaskCommand { get; set; }

        public ICommand ToggleTaskCompletedCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        private string buttonText;
        public string ButtonText
        {
            get { return buttonText; }
            set
            {
                if (buttonText != value)
                {
                    buttonText = value;
                    NotifyPropertyChanged(nameof(ButtonText));
                }
            }
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    NotifyPropertyChanged(nameof(SearchText));
                }
            }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    NotifyPropertyChanged(nameof(IsChecked));
                }
            }
        }

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
        private string urgency;
        public string Urgency
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


        private ObservableCollection<TaskItem> items;
        public ObservableCollection<TaskItem> Items
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
            SaveTaskCommand = new Command(async () => await SaveItem());
            ToggleTaskCompletedCommand = new Command<TaskItem>(ToggleTaskCompleted);
            SearchCommand = new Command(GenerateSearchResults);

            Db = conn;
            Items = [];
            GetItems();
        }


        public async void GenerateSearchResults()
        {
            string searchTerm = SearchText;
            string escaped_search_term = searchTerm.Replace("/", "\\/");
            var dbItems = await Db.FindItems(escaped_search_term);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Items.Clear();
                foreach (var dbItem in dbItems)
                    Items.Add(dbItem);

            });

        }

        public async void GetItems()
        {
            try
            {
                
                var dbItems = await Db.GetItemsAsync();
                dbItems.Reverse();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Items.Clear();
                    foreach (var dbItem in dbItems)
                    {
                        if (dbItem.IsCompleted)
                        {
                            dbItem.ButtonText = "✔️";
                        }
                        else
                        {
                            dbItem.ButtonText = "⏳";
                        }
                        Items.Add(dbItem);
                    }
                });


                foreach (TaskItem item in Items)
                {
                    Debug.WriteLine( item.Name + " " + item.ButtonText);
                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }
        private async void ToggleTaskCompleted(TaskItem taskItem)
        {
            taskItem.IsCompleted = !taskItem.IsCompleted;
            await Db.SaveItemAsync(taskItem);

            if (taskItem.IsCompleted)
            {
                taskItem.ButtonText = "✔️";
            }
            else
            {
                taskItem.ButtonText = "⏳";
            }
            Debug.WriteLine("Changed task status to " + taskItem.IsCompleted + " Is the isChecked true? " + isChecked);

        }

        public async Task SaveItem()
        {
            try
            {
                // stavi picker is empty check
                if (!string.IsNullOrEmpty(TextEntry))
                {

                    var taskItem = new TaskItem()
                    {
                        Name = TextEntry,
                        Description = null,
                        DateTime = DateTime.UtcNow,
                        Urgency = Urgency,
                        IsCompleted = false,
                        ButtonText = "⏳", 
                    };

                    Items.Insert(0, taskItem);
                    await Db.SaveItemAsync(taskItem);
                    foreach (TaskItem item in items)
                    {
                        Debug.WriteLine(item.Name + ": " + item.IsCompleted);
                    }
                    Debug.WriteLine("Number of items in items is " + Items.Count);
                    //GetItems();
                    TextEntry = "";
                }

            }
            catch (Exception ex)
            {

                Debug.Write(ex.Message);
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
