using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using FronteraRadio.Core.Enums;
using FronteraRadio.Core.Interfaces;

namespace FronteraRadio.Services;

public class AudioService : IAudioService
{
    private MediaElement? _mediaElement;

    private readonly object _playLock = new();
    private bool _isStartingPlayback;

    private int _retryCount;
    private const int MaxRetries = 5;

    public PlayerState State { get; private set; } = PlayerState.Stopped;

    public event Action<PlayerState>? StateChanged;

    public void AttachPlayer(MediaElement mediaElement)
    {
        _mediaElement = mediaElement;

        _mediaElement.MediaOpened += OnMediaOpened;
        _mediaElement.MediaFailed += OnMediaFailed;
        _mediaElement.MediaEnded += OnMediaEnded;
    }

    private void OnMediaOpened(object? sender, EventArgs e)
    {
        Console.WriteLine("MEDIA OPENED");

        _retryCount = 0;

        if (State == PlayerState.Playing)
            return;

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

        lock (_playLock)
        {
            if (_isStartingPlayback)
                return;

            if (State == PlayerState.Playing ||
                State == PlayerState.Connecting ||
                State == PlayerState.Reconnecting)
                return;

            _isStartingPlayback = true;
        }

        try
        {
            _retryCount = 0;

            SetState(PlayerState.Connecting);

            // HARD RESET to guarantee previous stream is fully stopped
            _mediaElement.Stop();
            _mediaElement.Source = null;

            _mediaElement.Source = MediaSource.FromUri(new Uri(url));
            _mediaElement.Play();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PLAY ERROR: {ex.Message}");
            SetState(PlayerState.Error);
        }
        finally
        {
            lock (_playLock)
            {
                _isStartingPlayback = false;
            }
        }
    }

    public Task StopAsync()
    {
        if (_mediaElement == null)
            return Task.CompletedTask;

        try
        {
            _mediaElement.Stop();

            // EXTREME RADIO FIX
            // guarantees network socket closes
            _mediaElement.Source = null;

            SetState(PlayerState.Stopped);
            _retryCount = 0;
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

        // Prevent reconnect if user stopped playback
        if (State == PlayerState.Stopped)
            return;

        if (_retryCount >= MaxRetries)
        {
            SetState(PlayerState.Error);
            return;
        }

        _retryCount++;

        SetState(PlayerState.Reconnecting);

        await Task.Delay(2000 * _retryCount);

        // Safety check again before reconnect
        if (State == PlayerState.Stopped)
            return;

        _mediaElement.Play();
    }

    private void SetState(PlayerState state)
    {
        State = state;
        StateChanged?.Invoke(state);
    }
}