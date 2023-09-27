using Mauidoro.Model;
using Mauidoro.Services;
using SQLite;

[assembly:Dependency(typeof(TaskTodoService))]
namespace Mauidoro.Services;

public class TaskTodoService : ITaskTodoService
{
    private SQLiteAsyncConnection db;
    private async Task Init()
    {
        if(db is not null)
            return;
        
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "TaskTodo.db");
        db = new SQLiteAsyncConnection(databasePath);
        await db.CreateTableAsync<TaskTodo>();
    }
    public async Task AddTaskTodo(TaskTodo taskTodo)
    {
        await Init();
        await db.InsertAsync(taskTodo);
    }
    public async Task RemoveTaskTodo(int id)
    {
        await Init();
        await db.DeleteAsync<TaskTodo>(id);
    }
    public async Task<IEnumerable<TaskTodo>> GetTaskTodo()
    {
        await Init();
        return await db.Table<TaskTodo>().ToListAsync();
    }
    public async Task<TaskTodo> GetTaskTodo(int id)
    {
        await Init();
        return await db.Table<TaskTodo>()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateTaskTodo(TaskTodo taskTodo)
    {
        await Init();
        // le contrôle sur le nombre de pomodoro doit être fait avant
        await db.UpdateAsync(taskTodo);
        //TODO a tester
    }
}