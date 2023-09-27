using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mauidoro.Controls;
using Mauidoro.Model;
using Mauidoro.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace Mauidoro.ViewModel;


public partial class MainViewModel : BaseViewModel // Vprod: ObservableObject
{
    public ObservableRangeCollection<TaskTodo> TaskTodoList { get; set; }
    [ObservableProperty]
    private string _taskTodoName;
    
    private ITaskTodoService _taskTodoService;

    public MainViewModel(ITaskTodoService taskTodoService)
    {
        Title = "TodoList";
        TaskTodoList = new ObservableRangeCollection<TaskTodo>();
        _taskTodoService = taskTodoService;
        TimerView.TimerFocusFinishedEvent += TimerFinished;
    }
    private void TimerFinished(object sender, EventArgs e)
    {
        if(!TaskTodoList.Any())
            return;
        
        SubPomodoro(TaskTodoList.First()).Wait();
    }
    
    [RelayCommand]
    private async Task Refresh()
    {
        IsBusy = true;

// #if DEBUG
//         await Task.Delay(500);
// #endif

        TaskTodoList.Clear();

        var taskTodos = await _taskTodoService.GetTaskTodo();

        TaskTodoList.AddRange(taskTodos);

        IsBusy = false;

        // DependencyService.Get<IToast>()?.MakeToast("Refreshed!"); Voir le projet github sur le café
    }
    [RelayCommand]
    async Task Add()
    {
        if(String.IsNullOrWhiteSpace(TaskTodoName))
            return;
        
        var taskTodo = new TaskTodo()
        {
            Name = TaskTodoName,
        };
        await _taskTodoService.AddTaskTodo(taskTodo);
        await Refresh();
        TaskTodoName = string.Empty;
    }
    [RelayCommand]
    async Task Taped(TaskTodo taskTodo)
    {
        if(taskTodo is null)
            return;
        
        await Shell.Current.GoToAsync(nameof(DetailTaskPage), true, 
            new Dictionary<string, object>
        {
            {"TaskTodo", taskTodo }
        });
    }
    [RelayCommand]
    async Task Remove(TaskTodo taskTodo)
    {
        if (taskTodo is null)
            return;
        await _taskTodoService.RemoveTaskTodo(taskTodo.Id);
        await Refresh();
    }

    async Task Update(TaskTodo taskTodo)
    {
        if (taskTodo is null)
            return;
        if (taskTodo.NbrPomodoro > 0)
            await _taskTodoService.UpdateTaskTodo(taskTodo);
        else
            await _taskTodoService.RemoveTaskTodo(taskTodo.Id);
        await Refresh(); 
    }
    [RelayCommand]
    async Task AddPomodoro(TaskTodo taskTodo)
    {
        if (taskTodo is null)
            return;
        taskTodo.NbrPomodoro++;
        await Update(taskTodo);
    }
    [RelayCommand]
    async Task SubPomodoro(TaskTodo taskTodo)
    {
        if (taskTodo is null)
            return;
        taskTodo.NbrPomodoro--;
        await Update(taskTodo);
    }
}