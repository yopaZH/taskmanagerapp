using TaskManagerApp.View;
using TaskManagerApp.ViewModels;

namespace TaskManagerApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
