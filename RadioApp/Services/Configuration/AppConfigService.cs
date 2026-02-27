using System;
using System.Net.Http.Json;
using System.Text.Json;
using RadioApp.Core.Interfaces;
using RadioApp.Core.Models;

namespace RadioApp.Services.Configuration;

public class AppConfigService : IAppConfigService
{
    private readonly HttpClient _httpClient;

    private const string ConfigUrl =
        "https://alexandercalix.github.io/radio_app/remote-config/config.json";

    private const string ConfigCacheKey = "remote_config_cache";
    private const string ConfigLastFetchKey = "remote_config_last_fetch";

    public AppConfigService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AppConfig> GetConfigAsync()
    {
        var shouldRefresh = ShouldRefresh();

        if (shouldRefresh)
        {
            Console.WriteLine("Attempting to fetch remote config...");
            var remote = await TryFetchRemote();
            Console.WriteLine(remote != null
                ? "Remote config fetched successfully."
                : "Failed to fetch remote config.");
            if (remote != null)
            {
                SaveCache(remote);
                return MapToDomain(remote);
            }
        }

        // If not refreshing OR fetch failed
        var cached = LoadFromCache();
        if (cached != null)
            return MapToDomain(cached);

        // Final fallback
        return GetDefaultConfig();
    }

    private bool ShouldRefresh()
    {
        var lastFetch = Preferences.Get(ConfigLastFetchKey, DateTime.MinValue);
        return DateTime.UtcNow - lastFetch > TimeSpan.FromDays(1);
    }

    private async Task<RemoteConfig?> TryFetchRemote()
    {
        try
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
            return await _httpClient.GetFromJsonAsync<RemoteConfig>(ConfigUrl);
        }
        catch
        {
            return null;
        }
    }

    private void SaveCache(RemoteConfig config)
    {
        var json = JsonSerializer.Serialize(config);
        Preferences.Set(ConfigCacheKey, json);
        Preferences.Set(ConfigLastFetchKey, DateTime.UtcNow);
    }

    private RemoteConfig? LoadFromCache()
    {
        var json = Preferences.Get(ConfigCacheKey, null);
        if (string.IsNullOrWhiteSpace(json))
            return null;

        return JsonSerializer.Deserialize<RemoteConfig>(json);
    }

    private AppConfig GetDefaultConfig()
    {
        return new AppConfig
        {
            RadioName = "La Ley del FM",
            StreamUrl = "https://stream-176.zeno.fm/0uwm0hb5u0hvv",
            LastUpdated = DateTime.UtcNow,
            Social = new SocialLinks
            {
                Instagram = "https://www.instagram.com/frontera_95.1fm",
                Facebook = "https://www.facebook.com/Frontera95.1FM",
                Website = "https://www.fronterahn.com",
                TikTok = "https://www.tiktok.com/@frontera95.1fm",
                Whatsapp = "https://wa.me/50432352141"
            },
            About = new AboutInfo
            {
                Description = "La Ley del FM is your 24/7 station with the best Latin hits.",
                Latitude = 14.3833,
                Longitude = -88.5500,
                DeveloperName = "Oscar Calix",
                DeveloperLinkedIn = "https://www.linkedin.com/in/oscarcalixnolasco/"
            }
        };
    }

    private static AppConfig MapToDomain(RemoteConfig remote)
    {
        return new AppConfig
        {
            RadioName = remote.Radio.Name,
            StreamUrl = remote.Radio.Streams.First().Url,
            LastUpdated = DateTime.UtcNow,
            Social = new SocialLinks
            {
                Website = remote.Links.Website,
                Facebook = remote.Links.Facebook,
                Instagram = remote.Links.Instagram,
                TikTok = remote.Links.Tiktok,
                Whatsapp = remote.Links.Whatsapp
            },
            About = new AboutInfo
            {
                Description = remote.About.Description,
                Latitude = remote.About.Latitude,
                Longitude = remote.About.Longitude,
                DeveloperName = remote.About.DeveloperName,
                DeveloperLinkedIn = remote.About.DeveloperLinkedIn
            }
        };
    }
}