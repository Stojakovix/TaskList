using TaskList.ViewModel;
using TaskList.Model;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskList.Views;


public partial class MainItemPage : ContentPage
{
	private readonly MainPageViewModel viewModel;
	public MainItemPage(MainPageViewModel _viewModel)
	{
        InitializeComponent();
        try
		{
			viewModel = _viewModel;
			this.BindingContext = viewModel;
		}
		catch (Exception ex)
		{

			Debug.WriteLine($"Error setting binding content: {ex.Message}");
		}
	}



}