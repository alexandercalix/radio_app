using System;
using System.Windows.Input;
using FronteraRadio.Core.Enums;
using FronteraRadio.Core.Icons;
using FronteraRadio.Core.Interfaces;
using FronteraRadio.Views;

namespace FronteraRadio.ViewModels;



public class PlayerViewModel : BaseViewModel
{
    private readonly IAudioService _audioService;
    private readonly IAppConfigService _configService;
    public bool IsStopped => State == PlayerState.Stopped;

    private PlayerState _state;
    private string _streamUrl = string.Empty;

    public string MainButtonText =>
    State == PlayerState.Playing ? "STOP" : "PLAY";

    public string? FacebookUrl { get; private set; }
    public string? InstagramUrl { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public string? TikTokUrl { get; private set; }
    public string? WhatsAppUrl { get; private set; }

    public ICommand OpenUrlCommand { get; }

    public string FacebookIcon => FaIcons.Facebook;
    public string InstagramIcon => FaIcons.Instagram;
    public string WebsiteIcon => FaIcons.Globe;
    public string TikTokIcon => FaIcons.Tiktok;
    public string WhatsappIcon => FaIcons.Whatsapp;

    public string InfoIcon => FaIcons.Info;

    public string MainButtonIcon =>
        State == PlayerState.Playing
            ? FaIcons.Stop
            : FaIcons.Play;



    public ICommand MainButtonCommand =>
        State == PlayerState.Playing ? StopCommand : PlayCommand;


    public ICommand GoToPlayerCommand => new Command(async () =>
{
    await Shell.Current.GoToAsync("PlayerPage", true);
});

    public ICommand GoToAboutCommand => new Command(async () =>
{
    // await Shell.Current.GoToAsync(nameof(AboutPage));
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

    public PlayerState State
    {
        get => _state;
        private set
        {
            _state = value;
            OnPropertyChanged();

            OnPropertyChanged(nameof(MainButtonIcon));

            OnPropertyChanged(nameof(IsPlaying));
            OnPropertyChanged(nameof(IsConnecting));
            OnPropertyChanged(nameof(StatusText));

            OnPropertyChanged(nameof(MainButtonText));
            OnPropertyChanged(nameof(MainButtonCommand));
        }
    }

    public bool IsPlaying => State == PlayerState.Playing;
    public bool IsConnecting => State == PlayerState.Connecting || State == PlayerState.Reconnecting;

    public string StatusText => State switch
    {
        PlayerState.Stopped => "Conectate con Frontera 95.1FM",
        PlayerState.Connecting => "Sintonizando señal...",
        PlayerState.Playing => "En Vivo: La Ley del FM",
        PlayerState.Reconnecting => "Señal débil, recuperando...",
        PlayerState.Error => "Opps! Revisa tu internet",
        _ => ""
    };

    public ICommand PlayCommand { get; }
    public ICommand StopCommand { get; }

    public PlayerViewModel(
        IAudioService audioService,
        IAppConfigService configService)
    {
        _audioService = audioService;
        _configService = configService;

        PlayCommand = new Command(async () => await PlayAsync());
        StopCommand = new Command(async () => await StopAsync());

        _audioService.StateChanged += OnPlayerStateChanged;

        _ = InitializeAsync();

        OpenUrlCommand = new Command<string>(async (url) =>
    {
        if (string.IsNullOrWhiteSpace(url))
            return;

        await Launcher.Default.OpenAsync(url);
    });


    }

    private async Task InitializeAsync()
    {
        var config = await _configService.GetConfigAsync();

        // Force all UI property updates to the main thread!
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _streamUrl = config.StreamUrl;
            State = PlayerState.Stopped;

            FacebookUrl = config.Social.Facebook;
            InstagramUrl = config.Social.Instagram;
            WebsiteUrl = config.Social.Website;
            TikTokUrl = config.Social.TikTok;
            WhatsAppUrl = config.Social.Whatsapp;

            OnPropertyChanged(nameof(FacebookUrl));
            OnPropertyChanged(nameof(InstagramUrl));
            OnPropertyChanged(nameof(WebsiteUrl));
            OnPropertyChanged(nameof(TikTokUrl));
            OnPropertyChanged(nameof(WhatsAppUrl));
        });
    }

    private async Task PlayAsync()
    {
        if (string.IsNullOrWhiteSpace(_streamUrl))
            return;

        await _audioService.PlayAsync(_streamUrl);
    }

    private async Task StopAsync()
    {
        await _audioService.StopAsync();
    }

    private void OnPlayerStateChanged(PlayerState newState)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            State = newState;
        });
    }
}