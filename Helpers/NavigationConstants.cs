namespace TaskManagerApp.Helpers;

public static class NavigationConstants
{
    // Shell routes
    public const string MainPageRoute = nameof(MainPageRoute);
    public const string ProjectsPageRoute = nameof(ProjectsPageRoute);
    public const string ProjectTasksPageRoute = nameof(ProjectTasksPageRoute);
    public const string TaskEditPageRoute = nameof(TaskEditPageRoute);

    // Query parameters
    public const string ProjectIdParameter = "projectId";
    public const string TaskIdParameter = "taskId";
}
