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
        readonly Conn Db;
        public ICommand SaveTaskCommand { get; set; }
        public ICommand ToggleTaskCompletedCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand OpenNoteCommand { get; set; }
        public ICommand SaveNoteCommand { get; set; }

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

        private string titleText;
        public string TitleText
        {
            get { return titleText; }
            set
            {
                if (titleText != value)
                {
                    titleText = value;
                    NotifyPropertyChanged(nameof(TitleText));
                }
            }
        }

        private string nText;
        public string NText
        {
            get { return nText; }
            set
            {
                if (nText != value)
                {
                    nText = value;
                    NotifyPropertyChanged(nameof(NText));
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
        private ObservableCollection<NoteItem> noteItems;
        public ObservableCollection<NoteItem> NoteItems
        {
            get { return noteItems; }
            set
            {
                if (noteItems != value)
                {
                    noteItems = value;
                    NotifyPropertyChanged(nameof(NoteItems));
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
            DeleteCommand = new Command<TaskItem>(DeleteTask);
            OpenNoteCommand = new Command(async () => await OpenNote());
            SaveNoteCommand = new Command(async () => await SaveNote());

            var datetime = DateTime.Now;
            Debug.WriteLine(datetime);
            Urgency = "Low";
            Db = conn;
            Items = [];
            NoteItems = [];
            GetItems();
            GetNotes();
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
                    Debug.WriteLine(item.Name + " " + item.ButtonText);
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

        private async void DeleteTask(TaskItem taskItem)
        {
            Debug.WriteLine(taskItem.Name);
            await Db.DeleteItemAsync(taskItem);
            Items.Remove(taskItem);
        }


        public async Task SaveItem()
        {
            try
            {
                // stavi picker is empty check
                if (!string.IsNullOrEmpty(TextEntry))
                {
                    Urgency ??= "Low";

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

        public async void GetNotes()
        {
            try
            {
                var dbItems = await Db.GetNoteItemsAsync();
                dbItems.Reverse();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if(dbItems == null)
                    {
                        
                    }
                    foreach (var dbItem in dbItems)
                    {
                        NoteItems.Clear();
                        NoteItems.Add(dbItem);
                        Debug.WriteLine("DbItems title is " + dbItem.Title);
                    }
                    Debug.WriteLine("Number of items in items is " + NoteItems.Count);

                });
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }
        public async Task OpenNote()
        {
            try
            {
                Debug.WriteLine("Clicked openNote");
                var noteItem = new NoteItem()
                {
                    Title = string.Empty,
                    NoteText = string.Empty,
                    IsExtended = true,
                    DateCreated = DateTime.Now,
                };
                NoteItems.Insert(0, noteItem);
                await Db.SaveNoteItemAsync(noteItem);
                Debug.WriteLine(noteItem.NoteText+" " + noteItem.Title+ " " + noteItem.IsExtended+ " " + noteItem.DateCreated);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }

        public async Task SaveNote()
        {
            try
            {
                // stavi picker is empty check
                if (!string.IsNullOrEmpty(TitleText))
                {

                    var noteItem = new NoteItem()
                    {
                        Title = TitleText,
                        NoteText = nText,
                        IsExtended = false,
                        DateCreated = DateTime.Now,
                    };

                    NoteItems.Insert(0, noteItem);

                    await Db.SaveNoteItemAsync(noteItem);

                    Debug.WriteLine("Number of items in items is " + NoteItems.Count);

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
