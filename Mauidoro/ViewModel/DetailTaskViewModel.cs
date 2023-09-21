using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Mauidoro.ViewModel;
[QueryProperty("Text","Text")] //queryId est le nom qu'il y a dans l'url
public partial class DetailTaskViewModel : ObservableObject
{
    [ObservableProperty]
    private string text;

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}