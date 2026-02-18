using TaskManagerApp.View;
using TaskManagerApp.ViewModels;
using TaskManagerApp.Services;

namespace TaskManagerApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterServices()
            .RegisterViewModels()
            .RegisterPages();

        return builder.Build();
    }

    static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IStorageService, FileStorageService>();
        return builder;
    }

    static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<ProjectsViewModel>();
        builder.Services.AddTransient<ProjectTasksViewModel>();
        builder.Services.AddTransient<TaskEditViewModel>();
        return builder;
    }

    static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<ProjectsPage>();
        builder.Services.AddTransient<ProjectTasksPage>();
        builder.Services.AddTransient<TaskEditPage>();
        return builder;
    }
}
