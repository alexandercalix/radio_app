using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using FronteraRadio.Core.Enums;
using FronteraRadio.Core.Interfaces;
namespace FronteraRadio.Services;

public class AudioService : IAudioService
{
    private MediaElement? _mediaElement;

    private int _retryCount;
    private const int MaxRetries = 5;

    public PlayerState State { get; private set; } = PlayerState.Stopped;

    public event Action<PlayerState>? StateChanged;

    public void AttachPlayer(MediaElement mediaElement)
    {
        _mediaElement = mediaElement;

        // Attach events AFTER player exists
        _mediaElement.MediaOpened += OnMediaOpened;
        _mediaElement.MediaFailed += OnMediaFailed;
        _mediaElement.MediaEnded += OnMediaEnded;
    }

    private void OnMediaOpened(object? sender, EventArgs e)
    {
        Console.WriteLine("MEDIA OPENED");
        _retryCount = 0;
        SetState(PlayerState.Playing);
    }

    private void OnMediaFailed(object? sender, MediaFailedEventArgs e)
    {
        Console.WriteLine($"MEDIA FAILED: {e.ErrorMessage}");
        _ = HandleReconnectAsync();
    }

    private void OnMediaEnded(object? sender, EventArgs e)
    {
        Console.WriteLine("MEDIA ENDED");
        _ = HandleReconnectAsync();
    }

    public async Task PlayAsync(string url)
    {
        if (_mediaElement == null)
            return;

        // BLOQUEO CRÍTICO: Si ya está sonando O está intentando conectar, no hagas nada.
        if (State == PlayerState.Playing || State == PlayerState.Connecting || State == PlayerState.Reconnecting)
            return;

        _retryCount = 0;
        SetState(PlayerState.Connecting);

        try
        {
            // Limpiamos cualquier rastro anterior antes de asignar el nuevo
            _mediaElement.Source = null;

            _mediaElement.Source = MediaSource.FromUri(new Uri(url));
            _mediaElement.Play();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PLAY ERROR: {ex.Message}");
            SetState(PlayerState.Error);
        }
    }

    public Task StopAsync()
    {
        if (_mediaElement == null)
            return Task.CompletedTask;

        try
        {
            _mediaElement.Stop();

            // ESTO ES LO MÁS IMPORTANTE PARA RADIOS ONLINE:
            // Forzamos la liberación de la conexión y del buffer
            _mediaElement.Source = null;

            SetState(PlayerState.Stopped);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"STOP ERROR: {ex.Message}");
        }

        return Task.CompletedTask;
    }

    private async Task HandleReconnectAsync()
    {
        if (_mediaElement == null)
            return;

        if (_retryCount >= MaxRetries)
        {
            SetState(PlayerState.Error);
            return;
        }

        _retryCount++;

        SetState(PlayerState.Reconnecting);

        await Task.Delay(2000 * _retryCount);

        _mediaElement.Play();
    }

    private void SetState(PlayerState state)
    {
        State = state;
        StateChanged?.Invoke(state);
    }
}