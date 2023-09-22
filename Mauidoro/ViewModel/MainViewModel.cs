using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mauidoro.Controls;

namespace Mauidoro.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private IConnectivity connectivity;
    public MainViewModel(IConnectivity connectivity)
    {
        TimerView.TimerFocusFinishedEvent += TimerFinished;
        items = new ObservableCollection<string>();
        this.connectivity = connectivity;
    }

    private void TimerFinished(object sender, EventArgs e)
    {
        //Todo doit enlever un pomodoro à la tache du dessus et la supprimer si elle ne possède plus de pomodoro
        Console.WriteLine("J'ai bien recu l'evenement");
    }
    [ObservableProperty]
    private ObservableCollection<string> items;
    
    [ObservableProperty]
    private string text;
    
    [RelayCommand]
    async Task Add()
    {
        if (string.IsNullOrEmpty(Text))
            return;
        if (connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("Uh Oh", "No Internet", "OK");
            return;
        }
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
        await Shell.Current.GoToAsync($"{nameof(DetailTaskPage)}?Text={s}");
        //On peut passer des objets complex dans le deuxième paramètre de la méthode dans un dictionnaire
        // , new Dictionary<string, object>()
        // {
        //     nameof(DetailPage), new object()
        // };
    }
}