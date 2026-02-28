using System;
using System.Collections.Generic;

namespace RadioApp.Core.Models;

// --- MODELO DE DOMINIO (El que usa tu UI/ViewModels) ---

public class AppConfig
{
    public string RadioName { get; set; }
    public string StreamUrl { get; set; }
    public SocialLinks Social { get; set; }
    public AboutInfo About { get; set; } = new();
    public AppUpdateInfo Update { get; set; } = new(); // Nueva sección
    public List<Announcement> Announcements { get; set; } = new();
    public DateTime LastUpdated { get; set; }
}

public class AppUpdateInfo
{
    public string Version { get; set; } = "1.0.0";
    public string MinimumVersion { get; set; } = "1.0.0";
    public bool ForceUpdate { get; set; }
    public string UpdateMessage { get; set; } = "";
    public string PlayStoreUrl { get; set; } = "";
    public string AppStoreUrl { get; set; } = "";
}

public class SocialLinks
{
    public string Instagram { get; set; }
    public string Facebook { get; set; }
    public string Website { get; set; }
    public string TikTok { get; set; }
    public string Whatsapp { get; set; }
}

public class AboutInfo
{
    public string? Description { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? DeveloperName { get; set; }
    public string? DeveloperLinkedIn { get; set; }
}

// --- MODELO DTO / REMOTE (El que mapea directamente el JSON) ---

public class RemoteConfig
{
    public AppSection App { get; set; } // Nueva sección
    public RadioSection Radio { get; set; }
    public LinksSection Links { get; set; }
    public AboutSection About { get; set; }
}

public class AppSection
{
    public string Version { get; set; }
    public string MinVersion { get; set; }
    public bool ForceUpdate { get; set; }
    public string Message { get; set; }
    public string PlayStoreUrl { get; set; }
    public string AppStoreUrl { get; set; }
}

public class RadioSection
{
    public string Name { get; set; }
    public int RetryDelayMs { get; set; }
    public int MaxRetriesPerStream { get; set; }
    public List<StreamItem> Streams { get; set; }
}

public class StreamItem
{
    public string Url { get; set; }
    public string Label { get; set; }
}

public class LinksSection
{
    public string Website { get; set; }
    public string Facebook { get; set; }
    public string Instagram { get; set; }
    public string Tiktok { get; set; }
    public string Whatsapp { get; set; }
}

public class AboutSection
{
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string DeveloperName { get; set; }
    public string DeveloperLinkedIn { get; set; }
}

