using TaskList.ViewModel;
using TaskList.Views;

namespace TaskList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(EntryItem), typeof(EntryItem));
            Routing.RegisterRoute(nameof(MainItemPage), typeof(MainItemPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

        }
    }
}
