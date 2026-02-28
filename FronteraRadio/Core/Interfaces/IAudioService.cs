using System;
using CommunityToolkit.Maui.Views;
using FronteraRadio.Core.Enums;

namespace FronteraRadio.Core.Interfaces;

public interface IAudioService
{
    PlayerState State { get; }

    event Action<PlayerState>? StateChanged;

    Task PlayAsync(string url);
    Task StopAsync();
    void AttachPlayer(MediaElement mediaElement);
}
