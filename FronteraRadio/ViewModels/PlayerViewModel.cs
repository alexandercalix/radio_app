using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using FronteraRadio.Core.Enums;
using FronteraRadio.Core.Icons;
using FronteraRadio.Core.Interfaces;
using FronteraRadio.Views;

namespace FronteraRadio.ViewModels;



public class PlayerViewModel : BaseViewModel
{
    private readonly IAudioService _audioService;
    private readonly IAppConfigService _configService;
    private readonly IFirebaseService _firebaseService;
    public bool IsStopped => State == PlayerState.Stopped;
    private readonly IMetadataService _metadataService;
    private DateTime? _radioStartTime;

    private PlayerState _state;
    private bool _isStartingPlayback;
    private bool _isStoppingPlayback;
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
    await Shell.Current.GoToAsync(nameof(AboutPage));
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

    private string _metadataTitle = "Frontera 95.1 FM";
    private string _metadataArtist = "La Ley del FM";
    private string _metadataArtwork = "radio_logo.png"; // Stored in Resources/Images

    // Add these public properties
    public string MetadataTitle
    {
        get => _metadataTitle;
        set { _metadataTitle = value; OnPropertyChanged(); }
    }

    public string MetadataArtist
    {
        get => _metadataArtist;
        set { _metadataArtist = value; OnPropertyChanged(); }
    }

    public string MetadataArtwork
    {
        get => _metadataArtwork;
        set { _metadataArtwork = value; OnPropertyChanged(); }
    }

    public PlayerState State
    {
        get => _state;
        set
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
     IAppConfigService configService,
     IMetadataService metadataService,
     IFirebaseService firebaseService)
    {
        _audioService = audioService;
        _configService = configService;
        _metadataService = metadataService;
        _firebaseService = firebaseService;

        _state = PlayerState.Stopped;

        PlayCommand = new Command(async () => await PlayAsync());
        StopCommand = new Command(async () => await StopAsync());

        _audioService.StateChanged += OnPlayerStateChanged;

        _ = InitializeAsync();

        OpenUrlCommand = new Command<string>(async (url) =>
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            await Launcher.Default.OpenAsync(url);
            string socialPlatform =
                url.Contains("facebook", StringComparison.OrdinalIgnoreCase) ? "facebook" :
                url.Contains("instagram", StringComparison.OrdinalIgnoreCase) ? "instagram" :
                url.Contains("tiktok", StringComparison.OrdinalIgnoreCase) ? "tiktok" :
                url.Contains("whatsapp", StringComparison.OrdinalIgnoreCase) ? "whatsapp" :
                "website";
            _firebaseService.LogEvent("social_click", new Dictionary<string, object>
{
    { "platform", socialPlatform }
});

        });


    }

    public void UpdateMetadata()
    {
        // Update the properties; the UI will update via Data Binding
        MetadataTitle = "Frontera 95.1 FM";
        MetadataArtist = "La Ley del FM";
        MetadataArtwork = "logo";
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

        if (_isStartingPlayback || State == PlayerState.Playing)
            return;

        _isStartingPlayback = true;

        try
        {
            await _audioService.PlayAsync(_streamUrl);

            await Task.Delay(1000);
            _metadataService.UpdateMetadata("Frontera 95.1 FM", "La Ley del FM", "");

            await Task.Delay(4000);
            _metadataService.UpdateMetadata("Frontera 95.1 FM", "La Ley del FM", "");

            _radioStartTime = DateTime.UtcNow;

            _firebaseService.LogEvent("radio_play_started", new Dictionary<string, object>
{
    { "station", "Frontera 95.1 FM" },
    { "platform", DeviceInfo.Platform.ToString() }
});
        }
        finally
        {
            _isStartingPlayback = false;
        }
    }



    private async Task StopAsync()
    {
        if (_isStoppingPlayback || State == PlayerState.Stopped)
            return;

        _isStoppingPlayback = true;

        try
        {
            await _audioService.StopAsync();

            if (_radioStartTime != null)
            {
                var duration = (DateTime.UtcNow - _radioStartTime.Value).TotalSeconds;

                _firebaseService.LogEvent("radio_session_end", new Dictionary<string, object>
{
    { "station", "Frontera 95.1 FM" },
    { "platform", DeviceInfo.Platform.ToString() },
    { "duration_sec", duration }
});
            }

            _radioStartTime = null;
        }
        finally
        {
            _isStoppingPlayback = false;
        }
    }

    private void OnPlayerStateChanged(PlayerState newState)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            State = newState;
        });

        if (newState == PlayerState.Reconnecting)
        {
            _firebaseService.LogEvent("radio_buffering", new Dictionary<string, object>
{
    { "station", "Frontera 95.1 FM" },
    { "platform", DeviceInfo.Platform.ToString() }
});
        }

        if (newState == PlayerState.Error)
        {
            _firebaseService.LogEvent("radio_stream_error", new Dictionary<string, object>
{
    { "station", "Frontera 95.1 FM" },
    { "platform", DeviceInfo.Platform.ToString() }
});
        }
    }


}