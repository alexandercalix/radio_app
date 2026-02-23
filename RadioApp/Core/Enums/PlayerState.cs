using System;

namespace RadioApp.Core.Enums;

public enum PlayerState
{
    Stopped,
    Connecting,
    Playing,
    Reconnecting,
    Error
}