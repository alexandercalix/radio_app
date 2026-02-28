using System;
using System.Net.Http.Json;
using System.Text.Json;
using FronteraRadio.Core.Interfaces;
using RadioApp.Core.Models;

namespace FronteraRadio.Services.Configuration;

public class AppConfigService : IAppConfigService
{
    private readonly HttpClient _httpClient;

    // --- CONFIGURACIÓN DE FRECUENCIA ---
    // 0 = Siempre (cada apertura)
    // 1 = Una vez al día
    // 7 = Una vez por semana
    private const int RefreshDays = 1;

    private const string ConfigUrl = "https://alexandercalix.github.io/radio_app/remote-config/config.json";
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

            if (remote != null)
            {
                Console.WriteLine("Remote config fetched successfully.");
                SaveCache(remote);
                return MapToDomain(remote);
            }
            Console.WriteLine("Failed to fetch remote config. Falling back to cache or default.");
        }

        // Si no toca refrescar O el fetch falló, intentamos cargar de Cache
        var cached = LoadFromCache();
        if (cached != null)
            return MapToDomain(cached);

        // Si no hay nada en cache, usamos el default "quemado"
        return GetDefaultConfig();
    }

    private bool ShouldRefresh()
    {
        // Si la frecuencia es 0, siempre devuelve true
        if (RefreshDays <= 0) return true;

        var lastFetch = Preferences.Get(ConfigLastFetchKey, DateTime.MinValue);
        var timeElapsed = DateTime.UtcNow - lastFetch;

        return timeElapsed.TotalDays >= RefreshDays;
    }

    private async Task<RemoteConfig?> TryFetchRemote()
    {
        try
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
            // Esto mapea el JSON directamente a tu clase RemoteConfig
            return await _httpClient.GetFromJsonAsync<RemoteConfig>(ConfigUrl);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching remote config: {ex.Message}");
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
        if (string.IsNullOrWhiteSpace(json)) return null;

        try
        {
            return JsonSerializer.Deserialize<RemoteConfig>(json);
        }
        catch
        {
            return null;
        }
    }

    private AppConfig GetDefaultConfig()
    {
        return new AppConfig
        {
            RadioName = "La Ley del FM",
            StreamUrl = "https://stream-176.zeno.fm/0uwm0hb5u0hvv",
            LastUpdated = DateTime.UtcNow,
            Update = new AppUpdateInfo // Default para evitar nulls
            {
                Version = "1.0.0",
                ForceUpdate = false
            },
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
        // Usamos el operador ?. y ?? para que si el JSON viene incompleto no truene la app
        return new AppConfig
        {
            RadioName = remote.Radio?.Name ?? "Frontera Radio",
            StreamUrl = remote.Radio?.Streams?.FirstOrDefault()?.Url ?? "",
            LastUpdated = DateTime.UtcNow,

            // --- NUEVO MAPEO DE VERSIÓN ---
            Update = new AppUpdateInfo
            {
                Version = remote.App?.Version ?? "1.0.0",
                MinimumVersion = remote.App?.MinVersion ?? "1.0.0",
                ForceUpdate = remote.App?.ForceUpdate ?? false,
                UpdateMessage = remote.App?.Message ?? "Nueva versión disponible",
                PlayStoreUrl = remote.App?.PlayStoreUrl ?? "",
                AppStoreUrl = remote.App?.AppStoreUrl ?? ""
            },

            Social = new SocialLinks
            {
                Website = remote.Links?.Website ?? "",
                Facebook = remote.Links?.Facebook ?? "",
                Instagram = remote.Links?.Instagram ?? "",
                TikTok = remote.Links?.Tiktok ?? "",
                Whatsapp = remote.Links?.Whatsapp ?? ""
            },
            About = new AboutInfo
            {
                Description = remote.About?.Description,
                Latitude = remote.About?.Latitude,
                Longitude = remote.About?.Longitude,
                DeveloperName = remote.About?.DeveloperName,
                DeveloperLinkedIn = remote.About?.DeveloperLinkedIn
            }
        };
    }
}