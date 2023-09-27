using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mauidoro.Model;
using Mauidoro.Services;

namespace Mauidoro.ViewModel;
[QueryProperty("TaskTodo","TaskTodo")] //queryId est le nom qu'il y a dans l'url
public partial class DetailTaskViewModel : ObservableObject
{
    [ObservableProperty]
    private TaskTodo _taskTodo;
    private ITaskTodoService _taskTodoService;
    public DetailTaskViewModel(ITaskTodoService taskTodoService)
    {
        _taskTodoService = taskTodoService;
    }
    
    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
    
    [RelayCommand]
    async Task SaveTask(TaskTodo taskTodo)
    { 
        if(taskTodo is null)
            return;
        
        if (taskTodo.NbrPomodoro > 0)
            await _taskTodoService.UpdateTaskTodo(taskTodo);
        else
            await _taskTodoService.RemoveTaskTodo(taskTodo.Id);

        await Shell.Current.GoToAsync("..");

        // MainViewModel.TaskTodoList
        //Comment faire pour sauvegarder les changements
    }
    
}