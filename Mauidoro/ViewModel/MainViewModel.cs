using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Mauidoro.ViewModel;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        items = new ObservableCollection<string>();
    }
    [ObservableProperty]
    private ObservableCollection<string> items;
    
    [ObservableProperty]
    private string text;
    
    [RelayCommand]
    void Add()
    {
        if (string.IsNullOrEmpty(Text))
            return;
        Items.Add(Text);
        Text = string.Empty;
    }
    
    [RelayCommand]
    void Delete(string s)
    {
        if (Items.Contains(s))
            Items.Remove(s);
    }
    
    [RelayCommand]
    async Task Tap(string s)
    {
        await Shell.Current.GoToAsync($"{nameof(DetailPage)}?Text={s}");
        //On peut passer des objets complex dans le deuxième paramètre de la méthode dans un dictionnaire
        // , new Dictionary<string, object>()
        // {
        //     nameof(DetailPage), new object()
        // };
    }
}