using Mauidoro.Controls;
using Mauidoro.Services;
using Mauidoro.ViewModel;

namespace Mauidoro;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var vm = (MainViewModel)BindingContext;
		await vm.RefreshCommand.ExecuteAsync(null);
	}
	
	
}

