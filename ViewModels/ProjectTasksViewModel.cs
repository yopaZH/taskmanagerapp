using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManagerApp.Helpers;
using TaskManagerApp.Models;
using TaskManagerApp.Services;

namespace TaskManagerApp.ViewModels;

[QueryProperty(nameof(ProjectId), NavigationConstants.ProjectIdParameter)]
public class ProjectTasksViewModel : BaseViewModel
{
    private readonly IStorageService _storageService;
    private string _projectId;
    private Project _currentProject;
    private ObservableCollection<TaskItem> _tasks;
    private TaskStatus _selectedStatusFilter = TaskStatus.New;

    public string ProjectId
    {
        get => _projectId;
        set
        {
            if (SetProperty(ref _projectId, value))
            {
                LoadTasks();
            }
        }
    }

    public Project CurrentProject
    {
        get => _currentProject;
        set => SetProperty(ref _currentProject, value);
    }

    public ObservableCollection<TaskItem> Tasks
    {
        get => _tasks;
        set => SetProperty(ref _tasks, value);
    }

    public TaskStatus SelectedStatusFilter
    {
        get => _selectedStatusFilter;
        set
        {
            if (SetProperty(ref _selectedStatusFilter, value))
            {
                FilterTasks();
            }
        }
    }

    public ICommand AddTaskCommand { get; }
    public ICommand EditTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }
    public ICommand ToggleTaskStatusCommand { get; }
    public ICommand LoadTasksCommand { get; }

    public ProjectTasksViewModel(IStorageService storageService)
    {
        _storageService = storageService;
        _tasks = new ObservableCollection<TaskItem>();
        
        AddTaskCommand = new Command(AddTask);
        EditTaskCommand = new Command<TaskItem>(EditTask);
        DeleteTaskCommand = new Command<TaskItem>(DeleteTask);
        ToggleTaskStatusCommand = new Command<TaskItem>(ToggleTaskStatus);
        LoadTasksCommand = new Command(LoadTasks);
    }

    public async void LoadTasks()
    {
        IsLoading = true;
        try
        {
            var projects = await _storageService.LoadDataAsync();
            CurrentProject = projects.FirstOrDefault(p => p.Id == CurrentProject?.Id);
            
            if (CurrentProject != null)
            {
                Title = CurrentProject.Name;
                FilterTasks();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading tasks: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void FilterTasks()
    {
        if (CurrentProject == null)
            return;

        Tasks.Clear();
        var filteredTasks = CurrentProject.Tasks
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.Status);

        foreach (var task in filteredTasks)
        {
            Tasks.Add(task);
        }
    }

    private async void AddTask()
    {
        if (CurrentProject == null)
            return;

        var taskTitle = await Application.Current.MainPage.DisplayPromptAsync(
            "Новая задача", 
            "Введите название задачи");
        
        if (!string.IsNullOrWhiteSpace(taskTitle))
        {
            var newTask = new TaskItem(CurrentProject.Id, taskTitle);
            CurrentProject.Tasks.Add(newTask);
            await SaveProject();
            FilterTasks();
        }
    }

    private async void EditTask(TaskItem task)
    {
        if (task != null)
        {
            await Shell.Current.GoToAsync($"{NavigationConstants.TaskEditPageRoute}?{NavigationConstants.ProjectIdParameter}={CurrentProject.Id}&{NavigationConstants.TaskIdParameter}={task.Id}");
        }
    }

    private async void DeleteTask(TaskItem task)
    {
        if (task != null)
        {
            var result = await Application.Current.MainPage.DisplayAlert(
                "Удалить задачу",
                $"Вы уверены, что хотите удалить задачу '{task.Title}'?",
                "Да",
                "Нет");
            
            if (result)
            {
                CurrentProject.Tasks.Remove(task);
                await SaveProject();
                FilterTasks();
            }
        }
    }

    private void ToggleTaskStatus(TaskItem task)
    {
        if (task == null)
            return;

        var nextStatus = task.Status switch
        {
            TaskStatus.New => TaskStatus.InProgress,
            TaskStatus.InProgress => TaskStatus.Completed,
            TaskStatus.Completed => TaskStatus.New,
            _ => TaskStatus.New
        };

        task.Status = nextStatus;
        SaveProject();
        FilterTasks();
    }

    private async Task SaveProject()
    {
        try
        {
            var projects = await _storageService.LoadDataAsync();
            var projectIndex = projects.FindIndex(p => p.Id == CurrentProject.Id);
            
            if (projectIndex >= 0)
            {
                projects[projectIndex] = CurrentProject;
                await _storageService.SaveDataAsync(projects);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving project: {ex.Message}");
        }
    }
}
