namespace Mauidoro.Model;

public class TaskTodo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NbrPomodoro { get; set; }
    public string? Description { get; set; }
    public DateTime? PlannedDate { get; set; }

    private static int iId = 0;
    public TaskTodo()
    {
        Id = iId;
        iId++;
    }
}