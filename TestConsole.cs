using TaskManagerApp.Models;
using TaskManagerApp.Services;

// Создайте сервис хранения
var storage = new FileStorageService();

// Загрузите существующие данные
var projects = await storage.LoadDataAsync();
Console.WriteLine($"📁 Загружено проектов: {projects.Count}");

// Создайте новый проект
var newProject = new Project("Тестовый проект");
Console.WriteLine($"\n➕ Создан проект: {newProject.Name} (ID: {newProject.Id})");

// Добавьте задачи
var task1 = new TaskItem(newProject.Id, "Купить продукты")
{
    Description = "Молоко, хлеб, яйца",
    Priority = TaskPriority.High,
    Status = TaskStatus.New
};

var task2 = new TaskItem(newProject.Id, "Написать код")
{
    Description = "Реализовать функцию X",
    Priority = TaskPriority.Medium,
    Status = TaskStatus.InProgress
};

var task3 = new TaskItem(newProject.Id, "Позвонить другу")
{
    Priority = TaskPriority.Low,
    Status = TaskStatus.Completed
};

newProject.Tasks.AddRange(new[] { task1, task2, task3 });

Console.WriteLine($"\n✅ Добавлено задач: {newProject.Tasks.Count}");

// Выведите статистику
foreach (var task in newProject.Tasks)
{
    Console.WriteLine($"  • {task.Title}");
    Console.WriteLine($"    Приоритет: {task.Priority}, Статус: {task.Status}");
}

// Сохраните проект
projects.Add(newProject);
await storage.SaveDataAsync(projects);
Console.WriteLine($"\n💾 Данные сохранены");

// Показажите статистику проекта
Console.WriteLine($"\n📊 Статистика проекта:");
Console.WriteLine($"  Выполнено: {newProject.GetCompletedTaskCount()}/{newProject.GetTotalTaskCount()}");
