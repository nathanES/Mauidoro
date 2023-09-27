using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace Mauidoro.Model;

public partial class TaskTodo
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public int NbrPomodoro { get; set; } = 1;
    public string? Description { get; set; }
    public DateTime? PlannedDate { get; set; }
}



// public partial class TaskTodo  : ObservableObject
// {
//     [ObservableProperty] 
//     private int _id;
//     [ObservableProperty] 
//     private string _name;
//     [ObservableProperty] 
//     private int _nbrPomodoro;
//     [ObservableProperty] 
//     private string? _description;
//     [ObservableProperty] 
//     private DateTime? _plannedDate;
//
//     private static int iId = 0;
//     public TaskTodo()
//     {
//         Id = iId;
//         PlannedDate = DateTime.Today;
//         iId++;
//     }
// }