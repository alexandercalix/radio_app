using System;
using CommunityToolkit.Maui.Views;
using RadioApp.Core.Enums;

namespace RadioApp.Core.Interfaces;

public interface IAudioService
{
    PlayerState State { get; }

    event Action<PlayerState>? StateChanged;

    Task PlayAsync(string url);
    Task StopAsync();
    void AttachPlayer(MediaElement mediaElement);
}
