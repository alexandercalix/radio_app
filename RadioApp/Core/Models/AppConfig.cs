using System;

namespace RadioApp.Core.Models;

public class AppConfig
{
    public string RadioName { get; set; }
    public string StreamUrl { get; set; }

    public SocialLinks Social { get; set; }

    public AboutInfo About { get; set; } = new();

    public List<Announcement> Announcements { get; set; }

    public DateTime LastUpdated { get; set; }
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

public class RemoteConfig
{
    public RadioSection Radio { get; set; }
    public LinksSection Links { get; set; }
    public AboutSection About { get; set; }
}

public class RadioSection
{
    public string Name { get; set; }
    public List<StreamItem> Streams { get; set; }
}

public class StreamItem
{
    public string Url { get; set; }
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