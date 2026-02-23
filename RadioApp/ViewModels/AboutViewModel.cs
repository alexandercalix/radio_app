using System;
using System.Windows.Input;
using RadioApp.Core.Interfaces;

namespace RadioApp.ViewModels;

public class AboutViewModel : BaseViewModel
{
    private readonly IAppConfigService _configService;

    public string Description { get; private set; } = "";
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public string DeveloperName { get; private set; } = "";
    public string DeveloperLinkedIn { get; private set; } = "";

    public ICommand OpenLinkedInCommand { get; }
    public ICommand OpenMapCommand { get; }

    public ICommand GoToPlayerCommand => new Command(async () =>
{
    await Shell.Current.GoToAsync("PlayerPage");
});

    public ICommand GoToAboutCommand => new Command(async () =>
    {
        await Shell.Current.GoToAsync("AboutPage", true);
    });

    private string _currentRoute = "PlayerPage";

    public Color PlayerTabColor =>
        _currentRoute == "PlayerPage"
            ? Color.FromArgb("#dd5e3b")
            : Colors.Gray;

    public Color AboutTabColor =>
        _currentRoute == "AboutPage"
            ? Color.FromArgb("#dd5e3b")
            : Colors.Gray;

    public void SetCurrentRoute(string route)
    {
        _currentRoute = route;
        OnPropertyChanged(nameof(PlayerTabColor));
        OnPropertyChanged(nameof(AboutTabColor));
    }

    public ICommand GoBackCommand => new Command(async () =>
{
    await Shell.Current.GoToAsync("..");
});


    public string MapImageUrl =>
    $"https://maps.googleapis.com/maps/api/staticmap?center={Latitude},{Longitude}" +
    $"&zoom=15&size=600x300" +
    $"&markers=color:red%7C{Latitude},{Longitude}" +
    $"&key=YOUR_API_KEY";

    public AboutViewModel(IAppConfigService configService)
    {
        _configService = configService;

        OpenLinkedInCommand = new Command(async () =>
        {
            if (!string.IsNullOrWhiteSpace(DeveloperLinkedIn))
                await Launcher.Default.OpenAsync(DeveloperLinkedIn);
        });

        OpenMapCommand = new Command(async () =>
    {
        var url = $"https://www.google.com/maps?q={Latitude},{Longitude}";
        await Launcher.Default.OpenAsync(url);
    });

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var config = await _configService.GetConfigAsync();

        var about = config.About;

        Description = about.Description ?? "Default radio description.";
        Latitude = about.Latitude ?? 0;
        Longitude = about.Longitude ?? 0;

        DeveloperName = about.DeveloperName ?? "Oscar Calix";
        DeveloperLinkedIn = about.DeveloperLinkedIn ?? "";

        OnPropertyChanged(nameof(Description));
        OnPropertyChanged(nameof(Latitude));
        OnPropertyChanged(nameof(Longitude));
        OnPropertyChanged(nameof(DeveloperName));
        OnPropertyChanged(nameof(DeveloperLinkedIn));
    }
}