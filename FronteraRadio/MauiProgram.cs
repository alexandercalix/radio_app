using CommunityToolkit.Maui;
using FronteraRadio.Core.Interfaces;
using FronteraRadio.Services;
using FronteraRadio.Services.Configuration;
using FronteraRadio.ViewModels;
using FronteraRadio.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Analytics;

namespace FronteraRadio;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder
	.UseMauiApp<App>()
	.UseMauiCommunityToolkit()
	.UseMauiCommunityToolkitMediaElement(true)
	.ConfigureFonts(fonts =>
	{
		fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
		fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

		fonts.AddFont("fa-solid-900.ttf", "FASolid");
		fonts.AddFont("fa-brands-400.ttf", "FABrands");
	})
	.RegisterFirebase();


		// 1. REGISTRAR SERVICIOS (Lógica)
		builder.Services.AddSingleton<HttpClient>();
		builder.Services.AddSingleton<IAppConfigService, AppConfigService>();
		builder.Services.AddSingleton<IAudioService, AudioService>();
		builder.Services.AddSingleton<IMetadataService, MetadataService>();

		builder.Services.AddSingleton(_ => CrossFirebaseAnalytics.Current);
		builder.Services.AddSingleton<IFirebaseService, FirebaseService>();

		// 2. REGISTRAR VIEWMODELS (Músculos)
		builder.Services.AddSingleton<PlayerViewModel>();
		builder.Services.AddSingleton<AboutViewModel>();


		// 3. REGISTRAR VIEWS (Páginas)
		// Es vital registrar la página para que el Dependency Injection funcione
		builder.Services.AddSingleton<PlayerPage>();
		builder.Services.AddSingleton<AboutPage>();

		builder.Services.AddSingleton(_ => CrossFirebaseAnalytics.Current);

#if DEBUG
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}

	private static MauiAppBuilder RegisterFirebase(this MauiAppBuilder builder)
	{
		builder.ConfigureLifecycleEvents(events =>
		{
#if IOS
			events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) =>
			{
				// Fully qualified name prevents the "Missing Namespace" error [cite: 2026-03-03]
				//Plugin.Firebase.Core.Platforms.iOS.CrossFirebase.Initialize();
				return true;
			}));
#elif ANDROID
        events.AddAndroid(android => android.OnCreate((activity, bundle) =>
		{
			Plugin.Firebase.Core.Platforms.Android.CrossFirebase.Initialize(activity);
		}));
#endif
		});

		return builder;
	}
}