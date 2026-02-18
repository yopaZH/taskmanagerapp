namespace TaskManagerApp.Models;

public class Project
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public List<TaskItem> Tasks { get; set; } = new();

    public Project() { }

    public Project(string name)
    {
        Name = name;
    }

    public int GetCompletedTaskCount()
    {
        return Tasks.Count(t => t.Status == TaskStatus.Completed);
    }

    public int GetTotalTaskCount()
    {
        return Tasks.Count;
    }
}
