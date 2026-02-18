using TaskManagerApp.Models;

namespace TaskManagerApp.Services;

public interface IStorageService
{
    Task<List<Project>> LoadDataAsync();
    Task SaveDataAsync(List<Project> projects);
}
