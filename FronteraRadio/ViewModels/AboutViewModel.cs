using System;
using System.Windows.Input;
using FronteraRadio.Core.Interfaces;
using System.Globalization;
using System.Diagnostics;
namespace FronteraRadio.ViewModels;

public class AboutViewModel : BaseViewModel
{
    private readonly IAppConfigService _configService;

    // Propiedades de Información
    public string Description { get; private set; } = "Cargando información...";
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string DeveloperName { get; private set; } = "Cargando...";
    public string DeveloperLinkedIn { get; private set; } = "";

    // Propiedades de Redes Sociales (Para los comandos)
    public string? FacebookUrl { get; private set; }
    public string? InstagramUrl { get; private set; }
    public string? TikTokUrl { get; private set; }

    // Comandos
    public ICommand OpenLinkedInCommand { get; }
    public ICommand OpenMapCommand { get; }
    public ICommand OpenFacebookCommand { get; }
    public ICommand OpenInstagramCommand { get; }
    public ICommand OpenTiktokCommand { get; }

    public string AppVersion => $"{AppInfo.Current.VersionString} ({AppInfo.Current.BuildString})";

    private string _mapImageUrl = "";
    public string MapImageUrl
    {
        get => _mapImageUrl;
        private set
        {
            _mapImageUrl = value;
            OnPropertyChanged(nameof(MapImageUrl));
        }
    }

    public AboutViewModel(IAppConfigService configService)
    {
        _configService = configService;

        // COMANDOS DINÁMICOS
        OpenLinkedInCommand = new Command(async () => await OpenBrowser(DeveloperLinkedIn));
        OpenFacebookCommand = new Command(async () => await OpenBrowser(FacebookUrl));
        OpenInstagramCommand = new Command(async () => await OpenBrowser(InstagramUrl));
        OpenTiktokCommand = new Command(async () => await OpenBrowser(TikTokUrl));

        OpenMapCommand = new Command(async () =>
        {
            var location = new Location(Latitude, Longitude);
            var options = new MapLaunchOptions { Name = "Frontera Radio 95.1 FM" };
            try
            {
                await Map.Default.OpenAsync(location, options);
            }
            catch
            {
                var url = DeviceInfo.Platform == DevicePlatform.Android
                    ? $"http://maps.google.com/?q={Latitude},{Longitude}"
                    : $"https://maps.apple.com/?q={Latitude},{Longitude}";
                await Launcher.Default.OpenAsync(url);
            }
        });

        // DISPARAMOS LA CARGA
        _ = LoadAsync();
    }

    private async Task OpenBrowser(string? url)
    {
        if (!string.IsNullOrWhiteSpace(url))
            await Launcher.Default.OpenAsync(url);
    }

    private async Task LoadAsync()
    {
        try
        {
            var config = await _configService.GetConfigAsync();
            var about = config.About;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Asignamos coordenadas
                Latitude = about.Latitude ?? 13.996781;
                Longitude = about.Longitude ?? -88.375490;

                // Generamos la URL con el truco del .png al final
                // NOTA: Yandex usa Longitud {0} primero, luego Latitud {1}
                MapImageUrl = string.Format(CultureInfo.InvariantCulture,
                    "https://static-maps.yandex.ru/1.x/?ll={0},{1}&z=16&l=sat,skl&pt={0},{1},pm2rdl&size=450,300&ext=.png",
                    Longitude, Latitude);

                // Otros datos
                Description = about.Description ?? "";
                DeveloperName = about.DeveloperName ?? "Oscar Calix";
                DeveloperLinkedIn = about.DeveloperLinkedIn ?? "";

                // LOG PARA DEBUG (Míralo en la consola de Visual Studio/Rider)
                Debug.WriteLine($"[MAPA DEBUG] URL Generada: {MapImageUrl}");

                // Notificamos el resto
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(DeveloperName));
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[MAPA ERROR] {ex.Message}");
        }
    }
}