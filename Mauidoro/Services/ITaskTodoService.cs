using Mauidoro.Model;

namespace Mauidoro.Services;

public interface ITaskTodoService
{
    Task AddTaskTodo(TaskTodo taskTodo);
    Task<IEnumerable<TaskTodo>> GetTaskTodo();
    Task<TaskTodo> GetTaskTodo(int id);
    Task RemoveTaskTodo(int id);
    Task UpdateTaskTodo(TaskTodo taskTodo);
}