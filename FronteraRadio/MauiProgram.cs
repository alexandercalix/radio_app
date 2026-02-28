
using CommunityToolkit.Maui;
using FronteraRadio.Core.Interfaces;
using FronteraRadio.Services;
using FronteraRadio.Services.Configuration;
using FronteraRadio.ViewModels;
using FronteraRadio.Views;
using Microsoft.Extensions.Logging;

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
	});

		// 1. REGISTRAR SERVICIOS (Lógica)
		builder.Services.AddSingleton<HttpClient>();
		builder.Services.AddSingleton<IAppConfigService, AppConfigService>();
		builder.Services.AddSingleton<IAudioService, AudioService>();

		// 2. REGISTRAR VIEWMODELS (Músculos)
		builder.Services.AddSingleton<PlayerViewModel>();

		// 3. REGISTRAR VIEWS (Páginas)
		// Es vital registrar la página para que el Dependency Injection funcione
		builder.Services.AddSingleton<PlayerPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}