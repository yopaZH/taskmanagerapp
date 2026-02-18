using System.Windows.Input;
using TaskManagerApp.Helpers;

namespace TaskManagerApp.ViewModels;

public class MainViewModel : BaseViewModel
{
    public ICommand NavigateToProjectsCommand { get; }

    public MainViewModel()
    {
        Title = "Управление задачами";
        NavigateToProjectsCommand = new Command(NavigateToProjects);
    }

    private async void NavigateToProjects()
    {
        await Shell.Current.GoToAsync(NavigationConstants.ProjectsPageRoute);
    }
}
