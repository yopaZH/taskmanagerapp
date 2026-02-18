using System.Text.Json;
using TaskManagerApp.Models;

namespace TaskManagerApp.Services;

public class FileStorageService : IStorageService
{
    private readonly string _filePath;
    private const string FileName = "tasks_data.json";

    public FileStorageService()
    {
        _filePath = Path.Combine(FileSystem.AppDataDirectory, FileName);
    }

    public async Task<List<Project>> LoadDataAsync()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return new List<Project>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Project>();
            }

            var projects = JsonSerializer.Deserialize<List<Project>>(json, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            return projects ?? new List<Project>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading data: {ex.Message}");
            return new List<Project>();
        }
    }

    public async Task SaveDataAsync(List<Project> projects)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(projects, options);
            await File.WriteAllTextAsync(_filePath, json);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving data: {ex.Message}");
        }
    }
}
