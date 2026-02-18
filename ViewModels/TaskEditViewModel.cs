using System.Windows.Input;
using TaskManagerApp.Helpers;
using TaskManagerApp.Models;
using TaskManagerApp.Services;

namespace TaskManagerApp.ViewModels;

[QueryProperty(nameof(ProjectId), NavigationConstants.ProjectIdParameter)]
[QueryProperty(nameof(TaskId), NavigationConstants.TaskIdParameter)]
public class TaskEditViewModel : BaseViewModel
{
    private readonly IStorageService _storageService;
    private string _projectId;
    private string _taskId;
    private TaskItem _currentTask;
    private Project _currentProject;
    private string _title;
    private string _description;
    private TaskStatus _selectedStatus;
    private TaskPriority _selectedPriority;

    public string ProjectId
    {
        get => _projectId;
        set => SetProperty(ref _projectId, value);
    }

    public string TaskId
    {
        get => _taskId;
        set
        {
            if (SetProperty(ref _taskId, value))
            {
                LoadTask();
            }
        }
    }

    public TaskItem CurrentTask
    {
        get => _currentTask;
        set => SetProperty(ref _currentTask, value);
    }

    public Project CurrentProject
    {
        get => _currentProject;
        set => SetProperty(ref _currentProject, value);
    }

    public string TaskTitle
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public TaskStatus SelectedStatus
    {
        get => _selectedStatus;
        set => SetProperty(ref _selectedStatus, value);
    }

    public TaskPriority SelectedPriority
    {
        get => _selectedPriority;
        set => SetProperty(ref _selectedPriority, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand LoadTaskCommand { get; }

    public List<TaskStatus> StatusOptions { get; }
    public List<TaskPriority> PriorityOptions { get; }

    public TaskEditViewModel(IStorageService storageService)
    {
        _storageService = storageService;
        Title = "Редактирование задачи";

        StatusOptions = Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>().ToList();
        PriorityOptions = Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>().ToList();

        SaveCommand = new Command(SaveTask);
        CancelCommand = new Command(CancelEdit);
        LoadTaskCommand = new Command(LoadTask);
    }

    public async void LoadTask()
    {
        IsLoading = true;
        try
        {
            if (string.IsNullOrWhiteSpace(ProjectId) || string.IsNullOrWhiteSpace(TaskId))
                return;

            var projects = await _storageService.LoadDataAsync();
            CurrentProject = projects.FirstOrDefault(p => p.Id == ProjectId);

            if (CurrentProject != null)
            {
                var task = CurrentProject.Tasks.FirstOrDefault(t => t.Id == TaskId);
                if (task != null)
                {
                    CurrentTask = task;
                    TaskTitle = task.Title;
                    Description = task.Description;
                    SelectedStatus = task.Status;
                    SelectedPriority = task.Priority;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading task: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async void SaveTask()
    {
        if (CurrentTask == null || CurrentProject == null)
            return;

        if (string.IsNullOrWhiteSpace(TaskTitle))
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Название задачи не может быть пустым", "OK");
            return;
        }

        try
        {
            var projectsData = await _storageService.LoadDataAsync();
            var project = projectsData.FirstOrDefault(p => p.Id == CurrentProject.Id);

            if (project != null)
            {
                var taskIndex = project.Tasks.FindIndex(t => t.Id == CurrentTask.Id);
                if (taskIndex >= 0)
                {
                    project.Tasks[taskIndex].Title = TaskTitle;
                    project.Tasks[taskIndex].Description = Description;
                    project.Tasks[taskIndex].Status = SelectedStatus;
                    project.Tasks[taskIndex].Priority = SelectedPriority;
                }

                await _storageService.SaveDataAsync(projectsData);
            }

            await Shell.Current.GoToAsync($"..?{NavigationConstants.ProjectIdParameter}={CurrentProject.Id}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving task: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось сохранить задачу", "OK");
        }
    }

    private async void CancelEdit()
    {
        await Shell.Current.GoToAsync("..");
    }
}
