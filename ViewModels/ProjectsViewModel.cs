using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManagerApp.Helpers;
using TaskManagerApp.Models;
using TaskManagerApp.Services;

namespace TaskManagerApp.ViewModels;

public class ProjectsViewModel : BaseViewModel
{
    private readonly IStorageService _storageService;
    private ObservableCollection<Project> _projects;
    public ObservableCollection<Project> Projects
    {
        get => _projects;
        set => SetProperty(ref _projects, value);
    }

    public ICommand AddProjectCommand { get; }
    public ICommand SelectProjectCommand { get; }
    public ICommand DeleteProjectCommand { get; }
    public ICommand LoadProjectsCommand { get; }

    public ProjectsViewModel(IStorageService storageService)
    {
        _storageService = storageService;
        _projects = new ObservableCollection<Project>();
        Title = "Проекты";

        AddProjectCommand = new Command(AddProject);
        SelectProjectCommand = new Command<Project>(SelectProject);
        DeleteProjectCommand = new Command<Project>(DeleteProject);
        LoadProjectsCommand = new Command(LoadProjects);
    }

    public async void LoadProjects()
    {
        IsLoading = true;
        try
        {
            var projects = await _storageService.LoadDataAsync();
            Projects.Clear();
            foreach (var project in projects)
            {
                Projects.Add(project);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading projects: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async void AddProject()
    {
        var projectName = await Application.Current.MainPage.DisplayPromptAsync("Новый проект", "Введите название проекта");
        
        if (!string.IsNullOrWhiteSpace(projectName))
        {
            var newProject = new Project(projectName);
            Projects.Add(newProject);
            await SaveProjects();
        }
    }

    private async void SelectProject(Project project)
    {
        if (project != null)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { NavigationConstants.ProjectIdParameter, project.Id }
            };
            await Shell.Current.GoToAsync($"{NavigationConstants.ProjectTasksPageRoute}?{NavigationConstants.ProjectIdParameter}={project.Id}");
        }
    }

    private async void DeleteProject(Project project)
    {
        if (project != null)
        {
            var result = await Application.Current.MainPage.DisplayAlert(
                "Удалить проект", 
                $"Вы уверены, что хотите удалить проект '{project.Name}'?", 
                "Да", 
                "Нет");
            
            if (result)
            {
                Projects.Remove(project);
                await SaveProjects();
            }
        }
    }

    private async Task SaveProjects()
    {
        await _storageService.SaveDataAsync(Projects.ToList());
    }
}
