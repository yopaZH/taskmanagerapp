namespace TaskManagerApp.Models;

public class TaskItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.New;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public TaskItem() { }

    public TaskItem(string projectId, string title)
    {
        ProjectId = projectId;
        Title = title;
    }
}
