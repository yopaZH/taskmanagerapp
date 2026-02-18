using System.Text.Json;

// Простая минимальная реализация моделей и файлового хранилища

enum TaskStatus { New, InProgress, Completed }
enum TaskPriority { Low, Medium, High }

class TaskItem
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

class Project
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public List<TaskItem> Tasks { get; set; } = new();

    public Project() { }
    public Project(string name) { Name = name; }

    public int GetCompletedTaskCount() => Tasks.Count(t => t.Status == TaskStatus.Completed);
    public int GetTotalTaskCount() => Tasks.Count;
}

class SimpleFileStorage
{
    private readonly string _path;
    public SimpleFileStorage(string fileName = "tasks_data.json")
    {
        _path = Path.Combine(Environment.CurrentDirectory, fileName);
    }

    public List<Project> Load()
    {
        try
        {
            if (!File.Exists(_path)) return new List<Project>();
            var json = File.ReadAllText(_path);
            if (string.IsNullOrWhiteSpace(json)) return new List<Project>();
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Project>>(json, opts) ?? new List<Project>();
        }
        catch
        {
            return new List<Project>();
        }
    }

    public void Save(List<Project> projects)
    {
        try
        {
            var opts = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(projects, opts);
            File.WriteAllText(_path, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
        }
    }
}

// --- Программа ---

class Program
{
    static void Main(string[] args)
    {
        var storage = new SimpleFileStorage();
        var projects = storage.Load();
        Console.WriteLine($"📁 Загружено проектов: {projects.Count}");

        var project = new Project("Тестовый проект");
        project.Tasks.Add(new TaskItem(project.Id, "Купить молоко") { Description = "Молоко 2л", Priority = TaskPriority.High });
        project.Tasks.Add(new TaskItem(project.Id, "Написать отчет") { Priority = TaskPriority.Medium, Status = TaskStatus.InProgress });
        project.Tasks.Add(new TaskItem(project.Id, "Позвонить") { Priority = TaskPriority.Low, Status = TaskStatus.Completed });

        projects.Add(project);
        storage.Save(projects);

        Console.WriteLine("\n✅ Проект добавлен и сохранён. Текущая статистика:");
        foreach (var p in projects)
        {
            Console.WriteLine($"- {p.Name}: {p.GetCompletedTaskCount()}/{p.GetTotalTaskCount()} выполнено");
        }

        Console.WriteLine($"\nФайл данных: {Path.Combine(Environment.CurrentDirectory, "tasks_data.json")}\n");
        Console.WriteLine("Готово. Закройте окно, или нажмите Enter для выхода...");
        Console.ReadLine();
    }
}
