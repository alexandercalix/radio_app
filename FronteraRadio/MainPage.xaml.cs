using FronteraRadio.Views;
using FronteraRadio.Core.Interfaces;
using RadioApp.Core.Models;
using Microsoft.Maui.ApplicationModel;
// ESTE ES EL USING CLAVE PARA QUE FUNCIONE EL POPUP
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Extensions;

namespace FronteraRadio;

public partial class MainPage : ContentPage
{
	private readonly IAppConfigService _configService;

	public MainPage(IAppConfigService configService)
	{
		InitializeComponent();
		_configService = configService;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		// 1. Iniciamos la animación y la carga en paralelo
		var animationTask = Task.WhenAll(
			IntroLogo.FadeTo(1, 800, Easing.CubicOut),
			IntroLogo.ScaleTo(1, 800, Easing.CubicOut),
			IntroText.FadeTo(0.8, 1000, Easing.Linear)
		);

		var configTask = _configService.GetConfigAsync();

		await Task.WhenAll(animationTask, configTask);
		var config = await configTask;

		// 2. Verificamos actualización con el nuevo MODAL
		bool updateRequired = await CheckForUpdates(config);

		if (updateRequired)
		{
			// Bloqueamos el flujo. El usuario se queda viendo el Logo + el Popup.
			return;
		}

		// 3. Si todo está bien, terminamos la intro y vamos al Player
		await Task.Delay(1000);

		await Task.WhenAll(
			IntroLogo.FadeTo(0, 400),
			IntroText.FadeTo(0, 400),
			LoadingIndicator.FadeTo(0, 400)
		);

		await Shell.Current.GoToAsync("//PlayerPage", animate: false);
	}

	private async Task<bool> CheckForUpdates(AppConfig? config)
	{
		if (config?.Update == null || !config.Update.ForceUpdate)
			return false;

		try
		{
			// 1. Obtenemos la versión y la forzamos a 3 dígitos (ej: 1.0 -> 1.0.0)
			var current = AppInfo.Current.Version;
			var currentNormalized = new Version(current.Major, current.Minor, Math.Max(current.Build, 0));

			if (Version.TryParse(config.Update.MinimumVersion, out var minRequired))
			{
				// 2. Hacemos lo mismo con la versión del JSON
				var minNormalized = new Version(minRequired.Major, minRequired.Minor, Math.Max(minRequired.Build, 0));


				if (currentNormalized < minNormalized)
				{
					// 🚀 LANZAMOS EL POPUP PERSONALIZADO
					string url = DeviceInfo.Platform == DevicePlatform.Android
								 ? config.Update.PlayStoreUrl
								 : config.Update.AppStoreUrl;

					// Instanciamos nuestro popup con el mensaje y la URL del JSON
					var popup = new UpdatePopup(config.Update.UpdateMessage, url);

					// Lo mostramos. Este método es del Community Toolkit.
					await this.ShowPopupAsync(popup);

					return true; // Bloqueamos la navegación al PlayerPage
				}
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"[UPDATE CHECK ERROR] {ex.Message}");
		}

		return false;
	}
}