using System.Diagnostics;
using FronteraRadio.Core.Interfaces;
using Plugin.Firebase.Analytics;

namespace FronteraRadio.Services;

public class FirebaseService : IFirebaseService
{
    private bool _initialized;

    private void EnsureInitialized()
    {
        if (_initialized)
            return;

        if (CrossFirebaseAnalytics.Current != null)
        {
            Debug.WriteLine("[Firebase] Analytics initialized");
            _initialized = true;
        }
        else
        {
            Debug.WriteLine("[Firebase] Analytics NOT ready yet");
        }
    }

    public void LogEvent(string eventName)
    {
        try
        {
            EnsureInitialized();

            if (!_initialized)
                return;

            CrossFirebaseAnalytics.Current.LogEvent(eventName);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Firebase] LogEvent('{eventName}') failed: {ex.Message}");
        }
    }

    public void LogEvent(string eventName, Dictionary<string, object> parameters)
    {
        try
        {
            EnsureInitialized();

            if (!_initialized)
                return;

            CrossFirebaseAnalytics.Current.LogEvent(eventName, parameters);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Firebase] LogEvent('{eventName}', params) failed: {ex.Message}");
        }
    }
}