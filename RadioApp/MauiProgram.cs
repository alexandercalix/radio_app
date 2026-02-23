using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using RadioApp.Services.Configuration;
using RadioApp.Core.Interfaces;
using RadioApp.Services;
using RadioApp.ViewModels;
using RadioApp.Views;
#if IOS
using UIKit;
#endif
namespace RadioApp;

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



		//Services
		builder.Services.AddSingleton<IAppConfigService, AppConfigService>();
		builder.Services.AddSingleton<IAudioService, AudioService>();


		//ViewModels
		builder.Services.AddSingleton<PlayerViewModel>();
		builder.Services.AddSingleton<AboutViewModel>();

		//Pages
		builder.Services.AddSingleton<PlayerPage>();
		builder.Services.AddSingleton<AboutPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}