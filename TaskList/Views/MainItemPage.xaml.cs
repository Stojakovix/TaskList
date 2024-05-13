using TaskList.ViewModel;
using TaskList.Model;
using System.Diagnostics;

namespace TaskList.Views;


public partial class MainItemPage : ContentPage
{
	MainPageViewModel viewModel;
	public MainItemPage(MainPageViewModel _viewModel)
	{
		try
		{
			viewModel = _viewModel;
			this.BindingContext = viewModel;
			InitializeComponent();
		}
		catch (Exception ex)
		{

			Debug.WriteLine(ex.Message);
		}
	}
}