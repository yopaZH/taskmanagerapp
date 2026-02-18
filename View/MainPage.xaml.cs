using TaskManagerApp.ViewModels;

namespace TaskManagerApp.View;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
