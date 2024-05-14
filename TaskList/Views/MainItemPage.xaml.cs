using TaskList.ViewModel;
using TaskList.Model;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskList.Views;


public partial class MainItemPage : ContentPage, INotifyPropertyChanged
{
	MainPageViewModel viewModel;
	public MainItemPage(MainPageViewModel _viewModel)
	{
		try
		{
			viewModel = _viewModel;
            InitializeComponent();
			BindingContext = viewModel;
		}
		catch (Exception ex)
		{

			Debug.WriteLine(ex.Message);
		}
	}



    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}