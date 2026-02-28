using FronteraRadio.ViewModels;
using FronteraRadio.Core.Interfaces;
using CommunityToolkit.Maui.Views;

namespace FronteraRadio.Views;

public partial class PlayerPage : ContentPage
{
	private readonly IAudioService _audioService;
	private bool _isAnimating = false;
	public PlayerPage(ViewModels.PlayerViewModel vm, IAudioService audioService)
	{
		InitializeComponent();
		BindingContext = vm;
		_audioService = audioService;

		// Escuchamos cuando la página termina de cargarse
		this.Loaded += OnPageLoaded;
	}

	private async void OnPageLoaded(object sender, EventArgs e)
	{
		// Un respiro final para asegurar estabilidad
		await Task.Delay(500);

		// Creamos el MediaElement dinámicamente
		var player = new MediaElement
		{
			WidthRequest = 1,
			HeightRequest = 1,
			ShouldAutoPlay = false
		};

		// Lo metemos en el contenedor oculto y lo conectamos al servicio
		PlayerContainer.Content = player;
		_audioService.AttachPlayer(player);

		Console.WriteLine("RADIO: Player inyectado y listo.");
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		// Escuchamos el cambio de propiedad del ViewModel
		if (BindingContext is PlayerViewModel vm)
		{
			vm.PropertyChanged += OnViewModelPropertyChanged;
		}
	}
	private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(PlayerViewModel.IsPlaying))
		{
			var vm = (PlayerViewModel)sender;
			if (vm.IsPlaying)
			{
				StartBounceAnimation();
			}
		}
	}

	private async void StartBounceAnimation()
	{
		// Si ya está animando, no iniciamos otro bucle
		if (_isAnimating) return;
		_isAnimating = true;

		// LogoImage ahora ya es reconocido gracias al x:Name
		while (BindingContext is PlayerViewModel vm && vm.IsPlaying)
		{
			try
			{
				// Efecto de respiración suave (1.05 es un 5% más grande)
				await LogoImage.ScaleTo(1.05, 1000, Easing.CubicInOut);
				await LogoImage.ScaleTo(1.00, 1000, Easing.CubicInOut);
			}
			catch (Exception)
			{
				// Por si la página se destruye mientras anima
				break;
			}
		}

		_isAnimating = false;
	}
}