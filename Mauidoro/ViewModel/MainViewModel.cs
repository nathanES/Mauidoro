using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mauidoro.Controls;
using Mauidoro.Model;

namespace Mauidoro.ViewModel;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel(IConnectivity connectivity)
    {
        TimerView.TimerFocusFinishedEvent += TimerFinished;
        taskTodoList = new ObservableCollection<TaskTodo>();
    }
    
    [ObservableProperty]
    private ObservableCollection<TaskTodo> taskTodoList;
    
    [ObservableProperty]
    private string taskTodoName;
    
    [RelayCommand]
    async Task AddNewTaskTodo()
    {
        if (string.IsNullOrEmpty(TaskTodoName))
            return;
        if (TaskTodoList.Where(x => x.Name == TaskTodoName).ToList().Count > 0)
            return;// Mettre un message d'alerte ?
        
        var taskTodoToAdd = new TaskTodo()
        {
            Name = TaskTodoName,
            NbrPomodoro = 1
        };
        TaskTodoList.Add(taskTodoToAdd);
        TaskTodoName = string.Empty;
    }
    
    [RelayCommand]
    void DeleteTaskTodo(int taskTodoIdToDelete)
    {
        TaskTodo taskTodoToDelete = TaskTodoList.Where(x => x.Id == taskTodoIdToDelete).ToList().First();
        
        if (taskTodoToDelete is not null)
            TaskTodoList.Remove(taskTodoToDelete);
    }
    
    [RelayCommand]
    void AddPomodoroToTaskTodo(int taskTodoIdToAddPomodoro)
    {
        TaskTodo taskTodoToAddPomodoro = TaskTodoList.Where(x => x.Id == taskTodoIdToAddPomodoro).ToList().First();
        if (taskTodoToAddPomodoro is not null)
        {
            //Todo trouver un meilleur truc pour mettre à jour le compteur de pomodoro
            int iTaskTodoList = TaskTodoList.IndexOf(taskTodoToAddPomodoro);
            TaskTodoList.Remove(taskTodoToAddPomodoro); 
            taskTodoToAddPomodoro.NbrPomodoro++;
            TaskTodoList.Insert(iTaskTodoList, taskTodoToAddPomodoro);

        }
    }
    [RelayCommand]
    void SubPomodoroToTaskTodo(int taskTodoIdTdSubPomodoro)
    {
        TaskTodo taskTodoToSubPomodoro = TaskTodoList.Where(x => x.Id == taskTodoIdTdSubPomodoro).ToList().First();
        if (taskTodoToSubPomodoro is not null)
        {
            //Todo trouver un meilleur truc pour mettre à jour le compteur de pomodoro
            int iTaskTodoList = TaskTodoList.IndexOf(taskTodoToSubPomodoro);
            taskTodoToSubPomodoro.NbrPomodoro--;
            TaskTodoList[iTaskTodoList] = taskTodoToSubPomodoro;
        }
    }
    
    // [RelayCommand]
    // async Task Tap(string s)
    // {
    //     await Shell.Current.GoToAsync($"{nameof(DetailTaskPage)}?Text={s}");
    // }
    
    private void TimerFinished(object sender, EventArgs e)
    {
        if(!TaskTodoList.Any())
            return;
        SubPomodoroToTaskTodo(TaskTodoList.First().Id);
        if(TaskTodoList.First().NbrPomodoro <= 0)
            DeleteTaskTodo(TaskTodoList.First().Id);
    }


    #region Old
    // private IConnectivity connectivity;
    // public MainViewModel(IConnectivity connectivity)
    // {
    //     items = new ObservableCollection<string>();
    //     this.connectivity = connectivity;
    // }
    
    // [ObservableProperty]
    // private ObservableCollection<string> items;
    //
    // [ObservableProperty]
    // private string text;
    //
    // [RelayCommand]
    // async Task Add()
    // {
    //     if (string.IsNullOrEmpty(Text))
    //         return;
    //     if (connectivity.NetworkAccess != NetworkAccess.Internet)
    //     {
    //         await Shell.Current.DisplayAlert("Uh Oh", "No Internet", "OK");
    //         return;
    //     }
    //     Items.Add(Text);
    //     Text = string.Empty;
    // }
    //
    // [RelayCommand]
    // void Delete(string s)
    // {
    //     if (Items.Contains(s))
    //         Items.Remove(s);
    // }
    //
    // [RelayCommand]
    // async Task Tap(string s)
    // {
    //     await Shell.Current.GoToAsync($"{nameof(DetailTaskPage)}?Text={s}");
    //     //On peut passer des objets complex dans le deuxième paramètre de la méthode dans un dictionnaire
    //     // , new Dictionary<string, object>()
    //     // {
    //     //     nameof(DetailPage), new object()
    //     // };
    // }
    #endregion

}